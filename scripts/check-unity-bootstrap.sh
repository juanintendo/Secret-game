#!/usr/bin/env bash
set -euo pipefail

project="unity/SecretGame"

jq -e '.sdk.version == "8.0.100" and .sdk.rollForward == "latestPatch" and .sdk.allowPrerelease == false' \
  global.json >/dev/null
rg -q '<LangVersion>10\.0</LangVersion>' Directory.Build.props

jq -e '.dependencies["com.unity.render-pipelines.universal"] == "17.3.0"' \
  "$project/Packages/manifest.json" >/dev/null
jq -e '.dependencies["com.unity.test-framework"] == "1.5.1"' \
  "$project/Packages/manifest.json" >/dev/null
rg -qx 'm_EditorVersion: 6000\.3\.0f1' "$project/ProjectSettings/ProjectVersion.txt"
rg -qx -- '-langversion:10\.0' "$project/Assets/csc.rsp"
rg -qx -- '-nullable:enable' "$project/Assets/csc.rsp"
rg -q 'UnityIsExternalInit\.cs' scripts/sync-unity-kernel.ps1
rg -q 'namespace System\.Runtime\.CompilerServices' scripts/sync-unity-kernel.ps1
rg -q -- '--burst-disable-compilation' scripts/run-unity-bootstrap-windows.ps1

while IFS= read -r -d '' assembly_definition; do
  jq empty "$assembly_definition"
done < <(find "$project/Assets" -name '*.asmdef' -print0)

while IFS= read -r asset; do
  if [[ ! -f "$asset.meta" ]]; then
    echo "Missing Unity metadata: $asset.meta" >&2
    exit 1
  fi
done < <(find "$project/Assets" -type f ! -name '*.meta')

if rg -n '\bCombatState\b' "$project/Assets/Game/Presentation"; then
  echo "Presentation API references authoritative CombatState." >&2
  exit 1
fi

if ! git check-ignore -q "$project/Assets/Generated/probe.cs"; then
  echo "Generated Unity kernel mirror is not ignored." >&2
  exit 1
fi

echo "Unity bootstrap structure clean."

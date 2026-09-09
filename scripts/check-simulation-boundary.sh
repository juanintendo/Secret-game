#!/usr/bin/env bash
set -euo pipefail

simulation_dir="${1:-src/SecretGame.Simulation}"
forbidden='UnityEngine|Vector3|worldPos|Meters|\bfloat\b|\bdouble\b|[0-9]+\.[0-9]+[fFdD]?'
unsupported_unity_framework_api='ArgumentNullException\.ThrowIfNull|SHA256\.HashData|Convert\.ToHexString'

if rg -n --glob '*.cs' "$forbidden" "$simulation_dir"; then
  echo "Simulation boundary violation found." >&2
  exit 1
fi

if rg -n --glob '*.cs' "$unsupported_unity_framework_api" "$simulation_dir"; then
  echo "Unity framework compatibility violation found." >&2
  exit 1
fi

echo "Simulation boundary clean."

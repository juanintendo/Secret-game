#!/usr/bin/env bash
set -euo pipefail

simulation_dir="${1:-src/SecretGame.Simulation}"
forbidden='UnityEngine|Vector3|worldPos|Meters|\bfloat\b|\bdouble\b|[0-9]+\.[0-9]+[fFdD]?'

if rg -n --glob '*.cs' "$forbidden" "$simulation_dir"; then
  echo "Simulation boundary violation found." >&2
  exit 1
fi

echo "Simulation boundary clean."

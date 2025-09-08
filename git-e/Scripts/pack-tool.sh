#!/usr/bin/env bash
set -euo pipefail

PROJECT_REL="${PROJECT_REL:-git-e}"      # path to .csproj
CONFIG="${CONFIG:-Release}"

# Optional: publish on NuGet
PUBLISH="${PUBLISH:-0}"                        # 1 = push
NUGET_API_KEY="${NUGET_API_KEY:-}"             # key for nuget.org
NUGET_SOURCE="${NUGET_SOURCE:-https://api.nuget.org/v3/index.json}"

# Optional: local test of installing as a dotnet tool
INSTALL_LOCAL="${INSTALL_LOCAL:-0}"            # 1 = install from local folder
PACKAGE_ID="${PACKAGE_ID:-}"                   # necessary if INSTALL_LOCAL=1


SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" >/dev/null 2>&1 && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
PROJECT_DIR="$ROOT_DIR/$PROJECT_REL"

echo ">> Packing tool from: $PROJECT_DIR"
pushd "$PROJECT_DIR" >/dev/null

dotnet clean -c "$CONFIG"
dotnet restore
dotnet pack -c "$CONFIG"

# Find latest .nupkg
PKG_PATH="$(ls -1 bin/"$CONFIG"/*.nupkg | sort -V | tail -n1)"
echo ">> Package ready: $PKG_PATH"

if [[ "$PUBLISH" == "1" ]]; then
  if [[ -z "$NUGET_API_KEY" ]]; then
    echo "!! NUGET_API_KEY is empty; cannot push" >&2
    exit 1
  fi
  echo ">> Pushing to NuGet: $NUGET_SOURCE"
  dotnet nuget push "$PKG_PATH" --api-key "$NUGET_API_KEY" --source "$NUGET_SOURCE"
fi

if [[ "$INSTALL_LOCAL" == "1" ]]; then
  if [[ -z "$PACKAGE_ID" ]]; then
    echo "!! Set PACKAGE_ID to install locally (e.g. MyCompany.MyTool)" >&2
    exit 1
  fi
  echo ">> Installing as local tool from ./bin/$CONFIG"
  dotnet tool uninstall --global "$PACKAGE_ID" >/dev/null 2>&1 || true
  dotnet tool install --global "$PACKAGE_ID" --add-source "bin/$CONFIG"
  echo ">> Installed. Try: $(grep -oPm1 '(?<=<ToolCommandName>)[^<]+' *.csproj || echo 'your-command') --help"
fi

popd >/dev/null

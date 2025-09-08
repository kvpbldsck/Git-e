#!/usr/bin/env bash
set -euo pipefail

PROJECT_REL="${PROJECT_REL:-git-e}"      # path to .csproj
CONFIG="${CONFIG:-Release}"
RID="${RID:-win-x64}"                      # win-x64 | linux-x64 | osx-arm64 | osx-x64
USE_PROFILE="${USE_PROFILE:-1}"            # 1 = try pubxml Aot-<RID>, else flags
STRIP_SYMBOLS="${STRIP_SYMBOLS:-true}"

SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" >/dev/null 2>&1 && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
PROJECT_DIR="$ROOT_DIR/$PROJECT_REL"
PROFILE_PATH="$PROJECT_DIR/Properties/PublishProfiles/Aot-${RID}.pubxml"

echo ">> AOT publish for $RID from: $PROJECT_DIR"
pushd "$PROJECT_DIR" >/dev/null

if [[ "$USE_PROFILE" == "1" && -f "$PROFILE_PATH" ]]; then
  echo ">> Using publish profile: $PROFILE_PATH"
  dotnet publish -p:PublishProfile="Aot-${RID}"
else
  echo ">> Using inline flags"
  dotnet publish -c "$CONFIG" -r "$RID" \
    -p:PublishAot=true \
    -p:SelfContained=true \
    -p:PublishSingleFile=true \
    -p:StripSymbols="$STRIP_SYMBOLS" \
    -p:DebugType=none
fi

# Name of target file ~ name of project
APP_NAME="$(basename "$PROJECT_DIR")"
OUT_DIR="bin/$CONFIG/net8.0/$RID/publish"
EXT=""
[[ "$RID" == win-* ]] && EXT=".exe"

echo ">> Output: $OUT_DIR/${APP_NAME}${EXT}"
ls -lah "$OUT_DIR" || true

popd >/dev/null

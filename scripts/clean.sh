#!/usr/bin/env bash

CurrWorkDir=$(pwd)
ScriptRootDir=$(dirname "$0")
cd "$ScriptRootDir" || exit 1
cd ..

FILES=$(find . -type f -name '*~')

for FILE in $FILES; do
  [ -f "$FILE" ] && echo "$FILE" && rm "$FILE"
done

cd "$CurrWorkDir" || exit 1

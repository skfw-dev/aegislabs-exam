#!/usr/bin/env bash

CurrWorkDir=$(pwd)
ScriptRootDir=$(dirname "$0")
cd "$ScriptRootDir" || exit 1
cd ..

FILES=$(find . -type f -name '*.cs')

for FILE in $FILES; do
  echo "$FILE"
  grep -Ei "$1" "$FILE"
done

cd "$CurrWorkDir" || exit 1

#!/bin/bash
set -euo pipefail

for file in $(find . -name "gleam.toml")
do
    dir=$(dirname $file)
    # Skip the testbase directory and all directories containing "build" in the path
    if [[ "$dir" == *"/testbase" ]] || [[ "$dir" == *"/build/"* ]]; then
        continue
    fi
    echo "Running tests in $dir"
    cd "$dir"
    gleam deps download
    gleam test
    gleam format --check src test
    cd -
    echo ""
done
echo "Success!"

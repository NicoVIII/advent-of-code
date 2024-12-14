#!/bin/bash
set -euo pipefail

for file in $(find . -name "main.roc")
do
    dir=$(dirname $file)
    echo "Running tests in $dir"
    cd "$dir"
    roc test
    cd -
    echo ""
done
echo "Success!"

#!/bin/bash
set -e

for file in $(find . -name "*.Test.fsproj")
do
    echo "Running tests in $file"
    dotnet run --project $file
    echo ""
done

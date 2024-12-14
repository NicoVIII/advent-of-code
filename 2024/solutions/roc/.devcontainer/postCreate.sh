#!/bin/bash
set -euo pipefail
IFS=$'\n\t'

cd /opt
# Download and unpack roc
sudo wget https://github.com/roc-lang/roc/releases/download/nightly/roc_nightly-linux_x86_64-latest.tar.gz
sudo mkdir roc
sudo tar -xf roc_nightly-linux_x86_64-latest.tar.gz -C roc --strip-components 1
sudo rm roc_nightly-linux_x86_64-latest.tar.gz

# Add roc to PATH
echo "export PATH=\$PATH:/opt/roc" >> ~/.bashrc
echo "export PATH=\$PATH:/opt/roc" >> ~/.zshrc

cd -

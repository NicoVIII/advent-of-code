#!/bin/bash
set -euo pipefail
IFS=$'\n\t'

cd /opt
# Download and unpack roc
sudo wget https://github.com/roc-lang/roc/releases/download/alpha3-rolling/roc-linux_x86_64-alpha3-rolling.tar.gz
sudo mkdir roc
sudo tar -xf roc-linux_x86_64-alpha3-rolling.tar.gz -C roc --strip-components 1
sudo rm roc-linux_x86_64-alpha3-rolling.tar.gz

# Add roc to PATH
echo "export PATH=\$PATH:/opt/roc" >> ~/.bashrc
echo "export PATH=\$PATH:/opt/roc" >> ~/.zshrc

cd -

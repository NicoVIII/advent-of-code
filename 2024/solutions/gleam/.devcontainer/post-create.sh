#!/bin/bash
set -euo pipefail

echo "deb http://binaries2.erlang-solutions.com/ubuntu/ jammy-esl-erlang-25 contrib" | sudo tee -a /etc/apt/sources.list
wget https://binaries2.erlang-solutions.com/GPG-KEY-pmanager.asc
sudo apt-key add GPG-KEY-pmanager.asc
rm GPG-KEY-pmanager.asc
sudo apt update
sudo apt install -y esl-erlang

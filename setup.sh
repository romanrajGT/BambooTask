#!/bin/bash

# Step 1: Clone the NopCommerce source code
# Replace this URL with the actual public GitHub URL of your NopCommerce repo
GIT_REPO="https://github.com/romanrajGT/BambooTask.git"
CLONE_DIR="nopcommerce"

# Delete the folder if it already exists
if [ -d "$CLONE_DIR" ]; then
  echo "Removing existing directory $CLONE_DIR..."
  rm -rf "$CLONE_DIR"
fi

echo "Cloning NopCommerce source from $GIT_REPO..."
git clone $GIT_REPO $CLONE_DIR

cd "$CLONE_DIR"

docker compose up
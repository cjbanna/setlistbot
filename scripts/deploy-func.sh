#!/bin/bash

if [ -n "$1" ]; then
  echo "resource group name: $1"
else
  echo "resource group name not supplied."
  exit 1
fi

if [ -n "$2" ]; then
  echo "func name: $2"
else
  echo "func name not supplied."
  exit 1
fi

rg_name=$1
func_name=$2

echo "Creating publish.zip"
cd publish
zip -r ../publish.zip *
cd ..

echo "Deploying publish.zip to Azure"
az functionapp deployment source config-zip -g $rg_name -n $func_name --src publish.zip
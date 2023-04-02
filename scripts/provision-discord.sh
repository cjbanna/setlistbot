#!/bin/bash

if [ -n "$1" ]; then
  echo "environmentName: $1"
else
  echo "environmentName not supplied."
  exit 1
fi

environment_name=$1

echo "Getting secrets from Key Vault kv-setlistbot-$environment_name..."

array=($(eval $"az keyvault secret list --vault-name kv-setlistbot-$environment_name | grep id | cut -d '\"' -f4"))

json="{"
for i in "${array[@]}"
do
    key=$(echo $i | cut -d '/' -f5 | sed -e 's/\-\-/__/g')
    value=$(az keyvault secret show --id ${i} | grep value | cut -d '"' -f4)
    json+="\"${key}\": \"${value}\", "
done

json=${json::-2}
json+="}"

echo "Provisioning Azure resources..."

az deployment sub create \
    --name deploy-$environment_name \
    --location centralus \
    --template-file discord-main.bicep \
    --parameters environmentName=$environment_name funcAppSettings="$json"

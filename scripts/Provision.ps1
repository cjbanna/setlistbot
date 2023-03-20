param (
    [string] $environmentName,
    [string] $artistId
)

# Provision Azure resources
Write-Output "Provisioning Azure resources..."
$provisionCommand = "az deployment sub create --name deploy-$environmentName --location centralus --template-file main.bicep --parameters environmentName=$environmentName artistId=$artistId"
Write-Output $provisionCommand
Invoke-Expression $provisionCommand | Write-Output
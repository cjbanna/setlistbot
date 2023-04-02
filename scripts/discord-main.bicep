targetScope = 'subscription'

@description('The name of the environment')
param environmentName string

@description('The location of the resource group')
param location string = 'centralus'

@description('The function app settings')
@secure()
param funcAppSettings object = {}

resource rgSetlistbot 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: 'rg-setlistbot-${environmentName}'
  location: location
}

module resources 'discord-resources.bicep' = {
  name: 'resourcesModule'
  scope: rgSetlistbot
  params: {
    location: location
    environmentName: environmentName
    funcAppSettings: funcAppSettings
  }
}

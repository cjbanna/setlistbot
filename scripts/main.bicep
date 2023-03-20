targetScope = 'subscription'

@description('The name of the environment')
param environmentName string

@description('The location of the resource group')
param location string = 'centralus'

@description('The artist identifier')
param artistId string

@description('The function app settings')
param funcAppSettings object = {}

// var funcAppSettings = {
//   'Bot:ArtistId': 'kglw'
//   'Bot:Subreddit': 'bottesting'
//   'Bot:MaxSetlistCount': '50'
//   'Bot:RequireMention': 'true'
//   'KglwNet:BaseUrl': 'https://kglw.songfishapp.com/api/v1'
// }

resource newRG 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: 'rg-setlistbot-${environmentName}'
  location: location
}

module resources 'resources.bicep' = {
  name: 'resourcesModule'
  scope: newRG
  params: {
    location: location
    environmentName: environmentName
    artistId: artistId
    funcAppSettings: funcAppSettings
  }
}

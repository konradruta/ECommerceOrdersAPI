param location string = resourceGroup().location

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: 'ecommerceorders-plan'
  location: location
  sku: {
    name: 'F1'
    tier: 'Free'
  }
}

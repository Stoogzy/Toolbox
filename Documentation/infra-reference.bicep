// =============================================================================
// REFERENCE FILE — Infrastructure as Code with Bicep
// =============================================================================
// Bicep is Azure's declarative Infrastructure as Code (IaC) language.
// It compiles down to ARM (Azure Resource Manager) JSON templates.
//
// This file describes the Azure resources needed to host the Toolbox API.
// When VM quota is available, deploy with:
//
//   az deployment group create \
//     --resource-group rg-toolbox-dev \
//     --template-file infra/main.bicep \
//     --parameters appName=toolbox-api-stoogzy
//
// Or via the pipeline using AzureResourceManagerTemplateDeployment@3
// (see Documentation/pipeline-deploy-reference.yml for the full pipeline step).
// =============================================================================

// ─────────────────────────────────────────────────────────────────────────────
// PARAMETERS
// Inputs to the template — can be overridden at deployment time.
// This means the same Bicep file can deploy to dev AND prod by passing
// different parameter values.
// ─────────────────────────────────────────────────────────────────────────────

@description('The name of the App Service. Must be globally unique.')
param appName string = 'toolbox-api-stoogzy'

@description('Azure region for all resources. Defaults to the resource group location.')
param location string = resourceGroup().location

@description('The App Service pricing tier. B1 = Basic (smallest paid tier).')
@allowed(['F1', 'B1', 'B2', 'S1'])
param sku string = 'B1'

// ─────────────────────────────────────────────────────────────────────────────
// APP SERVICE PLAN
// The underlying compute that your App Service runs on.
// Think of it as the "server" — App Service is the "app" running on that server.
// Multiple App Services can share one Plan to save cost.
// ─────────────────────────────────────────────────────────────────────────────
resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${appName}-plan'
  location: location
  // 'kind: linux' combined with reserved: true = Linux hosting
  kind: 'linux'
  sku: {
    name: sku
  }
  properties: {
    reserved: true  // required for Linux
  }
}

// ─────────────────────────────────────────────────────────────────────────────
// APP SERVICE (Web App)
// The managed host for your .NET API.
// URL will be: https://<appName>.azurewebsites.net
// ─────────────────────────────────────────────────────────────────────────────
resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: appName
  location: location
  properties: {
    serverFarmId: appServicePlan.id   // links to the Plan above
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|10.0'  // the runtime stack
      alwaysOn: sku != 'F1'              // AlwaysOn not available on Free tier
    }
    httpsOnly: true  // redirect all HTTP traffic to HTTPS automatically
  }
  // System-assigned Managed Identity — gives this App Service an Azure AD
  // identity so it can authenticate to Key Vault without any credentials.
  // See Module 12 for how this is used with Key Vault references.
  identity: {
    type: 'SystemAssigned'
  }
}

// ─────────────────────────────────────────────────────────────────────────────
// KEY VAULT
// Secure store for secrets, certificates, and keys.
// The App Service's managed identity is granted access below.
// ─────────────────────────────────────────────────────────────────────────────
resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: 'kv-toolbox-dev'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    // tenantId must match the Azure AD tenant of the App Service's identity
    tenantId: subscription().tenantId
    enableRbacAuthorization: false  // using Access Policies (simpler for learning)
    accessPolicies: [
      {
        // Grant the App Service's managed identity permission to read secrets.
        // principalId comes from the App Service's identity output.
        tenantId: subscription().tenantId
        objectId: appService.identity.principalId
        permissions: {
          secrets: ['get', 'list']  // read-only — app cannot create/delete secrets
        }
      }
    ]
  }
}

// ─────────────────────────────────────────────────────────────────────────────
// APPLICATION INSIGHTS
// Telemetry, logging, and monitoring for the running API.
// The connection string is output below so it can be passed as a parameter
// to the App Service configuration.
// ─────────────────────────────────────────────────────────────────────────────
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${appName}-insights'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    RetentionInDays: 30  // how long to keep telemetry data (free = 90 days max)
  }
}

// ─────────────────────────────────────────────────────────────────────────────
// OUTPUTS
// Values returned after deployment — useful for piping into subsequent steps,
// e.g. outputting the App Insights connection string so the pipeline can store
// it as a variable for the next deployment step.
// ─────────────────────────────────────────────────────────────────────────────
output appServiceUrl string = 'https://${appService.properties.defaultHostName}'
output appInsightsConnectionString string = appInsights.properties.ConnectionString
output keyVaultUri string = keyVault.properties.vaultUri

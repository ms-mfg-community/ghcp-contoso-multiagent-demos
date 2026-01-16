---
name: bicep-best-practices
description: Provides Bicep Infrastructure as Code best practices including file organization, naming conventions, parameter decorators, Azure Verified Modules (AVM), and security patterns. Use when creating Bicep templates, reviewing .bicep files, working with deployment stacks, or asking Stratus (Azure Infrastructure agent) about IaC patterns.
---

# Bicep Best Practices

## MCP-First: Query Current Documentation

**Before using the static reference below, query these MCP tools for current information:**

### Microsoft Learn MCP

```
1. microsoft_docs_search: query="Bicep best practices Azure"
2. microsoft_code_sample_search: query="Bicep module examples", language="bicep"
3. microsoft_docs_fetch: url from search results for complete content
```

Recommended queries:
- "Bicep file structure best practices"
- "Azure Verified Modules usage"
- "Bicep parameter decorators"
- "Bicep security patterns Key Vault"

### Context7 MCP

```
1. resolve-library-id: libraryName="bicep", query="infrastructure as code templates"
2. query-docs: libraryId from step 1, query="module patterns"
```

---

## Static Reference (Fallback)

Use this section only when MCP tools are unavailable or return no results.

### File Organization

```
infra/
├── main.bicep              # Entry point
├── main.bicepparam         # Parameter file
├── modules/                # Reusable modules
│   ├── networking/
│   ├── compute/
│   └── storage/
└── environments/           # Environment-specific params
    ├── dev.bicepparam
    ├── test.bicepparam
    └── prod.bicepparam
```

### Naming Conventions

- Use `camelCase` for parameters and variables
- Use descriptive names: `storageAccountName` not `sa`
- Prefix module outputs clearly: `storageAccountId`, `storageAccountName`
- Use consistent resource naming with `resourceGroup().location` awareness

### Parameter Best Practices

```bicep
// Always include descriptions
@description('The name of the storage account')
@minLength(3)
@maxLength(24)
param storageAccountName string

// Use allowed values for constrained options
@description('The SKU of the storage account')
@allowed(['Standard_LRS', 'Standard_GRS', 'Standard_ZRS'])
param storageAccountSku string = 'Standard_LRS'

// Use secure decorator for sensitive values
@secure()
@description('The administrator password')
param adminPassword string
```

### Azure Verified Modules

Prefer Azure Verified Modules (AVM) over custom implementations:

```bicep
// Use AVM modules for battle-tested patterns
module storageAccount 'br/public:avm/res/storage/storage-account:0.9.0' = {
  name: 'storageAccountDeployment'
  params: {
    name: storageAccountName
    // AVM handles best practices automatically
  }
}
```

- AVM modules include built-in security, diagnostics, and tagging support
- Pin module versions explicitly for reproducibility

### Security Patterns

```bicep
// Never hardcode secrets
param keyVaultName string
resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

// Reference secrets from Key Vault
module appService 'modules/appService.bicep' = {
  params: {
    connectionString: keyVault.getSecret('connectionString')
  }
}

// Use managed identities
resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'mi-${workloadName}'
  location: location
}
```

### Deployment Patterns

- Use deployment stacks for lifecycle management
- Implement what-if checks before deployment
- Use `dependsOn` sparingly - prefer implicit dependencies
- Set appropriate `scope` for subscription or management group deployments

### Validation

Before deployment:
1. Run `az bicep build` to catch syntax errors
2. Use `az deployment group what-if` to preview changes
3. Run PSRule for Azure compliance checks
4. Validate parameter files match environment requirements

---

## Fallback Reference Links

Only provide these if MCP returned no results and user needs persistent links:

- [Bicep Documentation](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/)
- [Azure Verified Modules](https://azure.github.io/Azure-Verified-Modules/)
- [Bicep Linter Rules](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/linter)

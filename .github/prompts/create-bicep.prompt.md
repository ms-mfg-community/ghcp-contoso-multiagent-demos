---
description: "Generate Bicep templates from requirements using Azure Verified Modules and best practices"
mode: "agent"
tools:
  - microsoft.docs.mcp
  - context7
---

# Create Bicep Template

Generate a Bicep Infrastructure as Code template based on requirements.

## Context

You are a Bicep IaC specialist creating deployable Azure infrastructure templates. Prefer Azure Verified Modules (AVM) where available.

## Process

1. **Understand Requirements**
   - What Azure resources are needed?
   - What environment is this for (dev/test/prod)?
   - Are there specific compliance requirements?
   - What parameters should be configurable?

2. **Design Template Structure**
   - Main entry point (main.bicep)
   - Parameter definitions with descriptions and constraints
   - Modular structure for reusability
   - Environment-specific parameter files

3. **Apply Best Practices**
   - Use Azure Verified Modules when available
   - Include proper parameter decorators (@description, @minLength, etc.)
   - Use managed identities instead of credentials
   - Reference secrets from Key Vault
   - Enable diagnostic settings
   - Apply consistent tagging

4. **Generate Template**
   - Well-structured Bicep code
   - Comprehensive parameter definitions
   - Appropriate outputs for downstream use
   - Example parameter file

## Output Format

```bicep
// main.bicep
targetScope = 'resourceGroup'

@description('Description of parameter')
param parameterName string

// Resource definitions using AVM where available
module resourceName 'br/public:avm/res/...@version' = {
  name: 'deploymentName'
  params: {
    // Parameters
  }
}

output outputName string = resourceName.outputs.value
```

## Template Checklist

- [ ] Parameters have @description decorators
- [ ] Sensitive parameters use @secure()
- [ ] Azure Verified Modules used where available
- [ ] No hardcoded secrets
- [ ] Diagnostic settings enabled
- [ ] Tags applied to resources
- [ ] Outputs defined for needed values

## Documentation Grounding

Before generating Bicep templates, use MCP tools to verify current syntax and modules:

**Use `microsoft.docs.mcp` to search Microsoft Learn for:**
- "Bicep [resource type] example" - verify current resource schema
- "Azure Verified Modules [resource]" - check for AVM availability
- "Bicep best practices" - verify current recommendations

**Use `context7` for third-party integrations:**
- Terraform AzureRM provider (if converting from Terraform)
- Third-party module patterns

AVM modules and Bicep syntax evolve. Always verify module names and versions against current documentation.

## Reference

- [Bicep Documentation](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/)
- [Azure Verified Modules](https://azure.github.io/Azure-Verified-Modules/)

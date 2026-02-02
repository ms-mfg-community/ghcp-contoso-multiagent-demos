---
mode: agent
model: Claude Sonnet 4
tools: ['codebase', 'usages', 'vscodeAPI', 'think', 'problems', 'changes', 'testFailure', 'terminalSelection', 'terminalLastCommand', 'openSimpleBrowser', 'fetch', 'findTestFiles', 'searchResults', 'githubRepo', 'extensions', 'runTests', 'editFiles', 'runNotebooks', 'search', 'new', 'runCommands', 'runTasks', 'Microsoft Docs', 'Azure MCP']
---
Implement Playwright end-to-end testing for the web application.

Start by reading the project status report at `reports/Report-Status.md` to understand the current technology stack and application state.

# Rules for Playwright Testing

- Read the status report first to understand the current project configuration
- Use `semantic_search` to identify application features that need testing
- Create tests for core user workflows (authentication, CRUD operations, navigation)
- Use Page Object Model pattern for maintainable tests
- Set up proper test data management and cleanup
- Configure tests to work with the current authentication method
- Test across multiple browsers and devices as needed

- Use `file_search` to locate existing test files and understand current testing structure.

- Use `semantic_search` to identify application features that require testing coverage.This prompt provides comprehensive guidance for implementing Playwright end-to-end testing for the Contoso University web application. Use this prompt to ensure consistent, reliable, and maintainable test automation.tools: ['codebase', 'usages', 'vscodeAPI', 'think', 'problems', 'changes', 'testFailure', 'terminalSelection', 'terminalLastCommand', 'openSimpleBrowser', 'fetch', 'findTestFiles', 'searchResults', 'githubRepo', 'extensions', 'runTests', 'editFiles', 'runNotebooks', 'search', 'new', 'runCommands', 'runTasks', 'Microsoft Docs', 'Azure MCP']

- Read the assessment report in the 'reports' folder to understand the application architecture and technology stack.

- Read the status report at 'reports/Report-Status.md' to determine the current migration phase and configuration.**Dynamic Context**: Before implementing Playwright tests, review the current project status report at `reports/Report-Status.md` to understand:

- Ensure Playwright tests align with the current application state and technology choices.

---

## Pre-Implementation Analysis

- **Technology Stack**: Check the migration status and current framework versions

### Required Information Gathering:

1. **Technology Stack Detection**: Read status report to identify:- **Authentication Strategy**: Verify the current authentication implementation (cookie-based vs Azure AD)## Application Context

   - .NET version and framework type

   - Database provider and configuration- **Project Structure**: Understand the Clean Architecture layers (Core, Infrastructure, Web)

   - Authentication method (cookie-based vs Azure AD)

   - Current hosting configuration (local vs Azure)- **Database Configuration**: Confirm Entity Framework setup and database provider- **Technology Stack**: ASP.NET Core MVC (.NET 8), Entity Framework Core, SQL Server LocalDBAsk the user for a screenshot of the page they want to build PlayWright testing for



2. **Application Architecture Analysis**: Read assessment report to understand:- **Testing Status**: Review existing test coverage and patterns- **Authentication**: Cookie-based authentication (development), Azure AD (production)

   - Application structure and project layout

   - Key features and user workflows- **Main Features**: Student management, Course management, Instructor management, Department management

   - Existing test patterns and frameworks

   - Security and authorization requirements**Key Information to Extract from Status Report**:- **Base URL**: `https://localhost:52379` (HTTPS), `http://localhost:52380` (HTTP)



3. **Testing Environment Setup**: Determine based on current state:```

   - Base URLs for testing (development vs production)

   - Authentication strategies for test execution

   - Database seeding and cleanup requirements

   - Browser and device testing requirements2. Database provider and connection strategy



## Always Use PlayWright MCP
- Use PlayWright MCP Server for updates to documentation and best practices, examples
- Use MS Learn Documentation via MCP where necessary for additional context and exapmles




## Troubleshooting Common IssuesRemember: Focus on testing user workflows, not implementation details. Write tests that provide value and catch real issues that users would encounter.

### Issue: Tests fail due to timing
**Solution**: Use proper wait strategies instead of fixed delays
```typescript
// Don't use arbitrary waits
await page.waitForTimeout(3000); // ❌

// Use specific wait conditions
await page.waitForSelector('[data-testid="content-loaded"]'); // ✅
await page.waitForLoadState('networkidle'); // ✅
```

### Issue: Flaky authentication tests
**Solution**: Implement robust authentication state management
```typescript
// Create reusable authentication state
await page.context().storageState({ path: 'auth.json' });
```

### Issue: Database state pollution
**Solution**: Implement proper test isolation
```typescript
test.beforeEach(async () => {
  await DatabaseFixture.resetToCleanState();
});
```

## Success Metrics

- **Test Coverage**: >80% of critical user journeys
- **Test Reliability**: <5% flaky test rate
- **Execution Time**: Full test suite completes in <15 minutes
- **Defect Detection**: Tests catch >90% of regression issues

## Quick Start Commands

### Install Playwright
```bash
npm init playwright@latest
```

### Run Tests
```bash
# Run all tests
npx playwright test

# Run specific test file
npx playwright test students.create.success.spec.ts

# Run with UI mode
npx playwright test --ui

# Debug mode
npx playwright test --debug
```

### Generate Tests
```bash
# Record new test
npx playwright codegen https://localhost:52379

# Generate page object
npx playwright codegen --target javascript-page-object
```

## Status Report Integration

**Always start with**: "Let me check the current project status to understand the technology stack and configuration..."

```
1. @workspace /file reports/Report-Status.md
2. Identify current migration phase
3. Extract technology stack details
4. Determine authentication strategy
5. Adapt test implementation accordingly
```

Remember: This prompt adapts to your project's current state. Always consult the status report first to ensure your Playwright implementation matches the actual project configuration.
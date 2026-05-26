---
name: backend
description: Backend engineering standards for coding, security, performance and documentation.
license: Complete terms in LICENSE.txt
---

# Backend Engineering Standards

## Architecture

- MUST follow Clean or Layered Architecture
- MUST follow the layers structure
  - API / Presentation
  - Application / Service
  - Domain / Business
  - Infrastructure / Persistence
- MUST follow the rules:
  - Business logic only in Application/Domain
  - API MUST strictly handles request/response orchestration 
  - Infrastructure handles DB, external APIs, file systems
  - MUST Maintain loose coupling
  - MUST Follow RESTful API principles
- MUST configure ALL CORS origins from appsettings.json

---

## Coding Standards

### General

- MUST Use clear meaningful naming
- MUST follow language naming conventions
- MUST AVOID magic strings / hardcoded values
- MUST follow Single Responsibility Principle
- MUST generate readable, maintainable code
- Every generated API request MUST include:
  ```http
  Content-Type: application/json
  Accept: application/json
  ```
### Core Enum Serialization Rules

- MUST return enum INTEGER IDs in ALL:
  - DTOs
  - API responses
  - command/query responses
  - websocket payloads
  - exported data contracts
- MUST cast enums using (int) before mapping to DTOs.
- MUST use foreign key IDs for relationships.
- MUST ensure frontend receives numeric identifiers only.
- MUST keep enum integer IDs stable across releases.

### Data

- DO NOT expose database entities
- Use DTOs / response models
- MUST follow the code structure
  - Controllers / Routes
  - Services
  - Repositories
  - Models / DTOs

### Async

- MUST use async for I/O operations
- MUST AVOID blocking operations
- MUST generate async database access

### Validation

- NEVER trust client input
- MUST validate all requests
- MUST use framework validation when available
- MUST reject invalid inputs with proper errors
- MUST prevent:
  - SQL Injection
  - XSS
  - Command Injection

### Response Format

- MUST follow the API Response Format
  {
    "success": true,
    "statusCode": 200,
    "data": {},
    "message": "Operation successful",
    "errors": []
  }

### Error Handling

- MUST handle global/centralized
- DO NOT use try/catch in controllers
- MUST log errors
- DO NOT expose internal details

---

## Security Standards

### Authentication

- JWT / OAuth2 / Session tokens
- MUST Validate token expiration
- MUST use secure signing keys
- MUST store secrets securely

### Authorization

- MUST use role-based or policy-based authorization
- MUST protect all endpoints unless specified
- DO NOT rely on frontend authorization

### Secure Headers

- MUST follow the standard policies
  - Content-Security-Policy
  - X-Content-Type-Options
  - X-Frame-Options
  - Referrer-Policy
  - Strict-Transport-Security

### Data Protection

- Use HTTPS
- MUST strictly encrypt sensitive data when required
- NEVER log:
  - passwords
  - tokens
  - secret keys

### OWASP Compliance

- MUST implement OWASP Top 10
- MUST implement OWASP API Security Top 10
- MUST follow framework/vendor secure coding practices

---

## Performance

- MUST use pagination for large datasets
- MUST AVOID N+1 queries
- MUST use proper database indexing
- Apply caching where appropriate
- Avoid unnecessary data loading

---

## Documentation

- MUST provide API documentation
- Prefer OpenAPI / Swagger
- MUST document all endpoints
- MUST include sample request/response

---

## Post-Generation Verification

- Codex MUST
  - Run build verification
  - Ensure no compile/runtime errors
  - Fix detected issues
  - Generate production-quality code

---
  
## Build & Execution Files

- MUST create `build.bat` in root folder.
- MUST create `run.bat` in root folder.
- MUST create `publish.bat` in root folder.
- MUST ensure all batch files work directly from solution root.
- MUST use relative paths only.
- MUST stop execution when build/publish fails.
- MUST include basic console logs using `echo`.

## launchSettings.json Rules

- MUST NOT use random localhost ports.
- MUST use predefined ports based on project type and tech stack.
- MUST keep port consistency across all environments.

## Restrictions

- Codex MUST NOT
  - Put business logic in controllers
  - Access DB directly from API layer
  - Hardcode credentials/secrets
  - Use blocking DB calls when async exists
  - Expose internal system details in API responses
  - Testcase generation
---
name: architecture
description: A reusable, structured skill that defines the standard architecture stack for modern application development. This skill guides teams in selecting, organizing, and implementing frontend, backend, database, and infrastructure components based on project requirements.
license: Complete terms in LICENSE.txt
---

## Backend Stack

Language: C#
Framework: .NET 10
API Style: REST (Minimal API) 
API Port: 5000 
ORM: Entity Framework Core
Mediator Pattern: MediatR
Validation: Fluent Validator
Test Framework: xUnit
Mock dependencies: Moq
Assertions: Fluent Assertion
Architecture: Clean Architecture
Authentication Type: JWT Bearer Authentication
API Documentation: Swagger
Logging Framework: Serilog 
Root Folder Name: Api

### Rules

- MUST follow short project name for root folders
   Example
   - Project name Gravience Management System, structure should be like:
     Api/
      ├── GMS.Api/
      ├── GMS.Application/
      └── GMS.Application.Tests/
- MUST strictly follow the API Response Format
  {
    "success": true,
    "statusCode": 200,
    "data": {},
    "message": "Operation successful",
    "errors": []
  }
        
## Frontend Stack

Language: TypeScript 
Application Port: 5001 
Framework: React With Functional Components 
Style Sheet: Tailwind CSS Build 
Tool: Vite 
Root Folder Name: App

## Database

Database Type:  MySql

## Figma MCP Integration

### Purpose

- These Figma designs are the SINGLE SOURCE OF TRUTH for:
  - UI layouts
  - page structures
  - spacing
  - typography
  - colors
  - component behavior
  - responsive behavior
  - design consistency

- AI agents MUST fetch and analyze Figma designs BEFORE generating frontend code.

- MUST read .codex/rules/mcp-rules.md
- MUST read .codex/rules/figma-rules

### REQUIRED FRONTEND GENERATION FLOW

- STEP 1 — Fetch Figma Design
  - Before generating ANY form/page:
    - MUST call MCP Figma tool.
    - MUST read the referenced Figma node.
    - MUST analyze:
      - layout structure
      - component hierarchy
      - auto-layout
      - spacing
      - typography
      - colors
      - form fields
      - action buttons
      - responsive rules

- STEP 2 — Create UI Component Plan
  - Generate:
    - reusable components
    - form sections
    - validation mapping
    - responsive layout plan
    - state handling structure

- STEP 3 — Generate React Code
  - Generated React code MUST:
    - visually match Figma.
    - preserve spacing.
    - preserve typography.
    - preserve responsive behavior.
    - reuse shared components.
    - follow Tailwind utility conventions.
    - follow design system hierarchy.

### Figma References

#### Form Create / Update
- Primary Figma Design: https://www.figma.com/design/S82vVfxbLhCNe5a026LZ02/Pan-ERP-DS?node-id=6-68

### Important

- MUST maintain migration history properly
- MUST generate migration for every schema change
- MUST create system admin user: (username: sysadmin, password: Assyst@123)
- MUST create necessary migration scripts with seed data
- MUST automate migration in application start
- MUST Incorporate the necessary database migration procedure and provide seed data whenever any new domains or entities are created, changed, or eliminated

# OUTPUT RULES

- DO NOT invent output paths.
- DO NOT create additional folders.

## Read

- MUST read .codex/rules/artifact-rules.md
- MUST read .codex/rules/enum-governance.md
- Generate ONLY:
   - architecture.md content
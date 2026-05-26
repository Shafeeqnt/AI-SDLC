---
name: frontend
description: Enforces frontend coding, security, performance and documentation when generating frontend code.
license: Complete terms in LICENSE.txt
---

Codex MUST follow ./ux-ui-rules.md the appropriate baseline ui guide when generating frontend.

# Frontend Engineering Standards

These rules MUST be followed when generating frontend applications.

---

# 1. Architecture Principles

- Frontend systems MUST separate:
  - Presentation (UI rendering)
  - Business Logic
  - Data Access
  - Configuration

- UI code MUST never be tightly coupled with backend communication.

---

# 2. Mandatory Function Pattern (Bouncer Pattern)

- Every function MUST follow this order:
  1. Validate input
  2. Type validation
  3. Business rule validation
  4. Main logic

## Rules

- Validation MUST be ALWAYS at the top
- MUST USE early return
- MUST AVOID nested if/else

---

# 3. Layered Frontend Architecture

- Required architecture: Presentation → Application → Service → Data

## Presentation Layer

- MUST be responsible for:
  - UI rendering
  - layout
  - user interaction
- MUST restrict:
  - NO API calls
  - NO business logic

## Application Layer

- MUST be responsible for:
  - UI state
  - event handling
  - workflow orchestration
  - navigation logic
  - form submissions

## Service Layer

- MUST be responsible for:
  - API communication
  - request/response mapping
  - error handling

### Rule

- UI MUST NOT call APIs directly

## Data Layer

- MUST be responsible for:
  - data models
  - API response mapping
  - caching strategies

---

# 4. Domain-Based Project Structure

- Applications MUST be organized by business domains, not technical layers.
- MUST follow the structure
  application
  ├ authentication
  ├ users
  ├ orders
  └ reports
- Each domain MUST contains:
  module
  ├ components
  ├ services
  ├ models
  ├ validations
  └ constants
  └ shared

---

# 5. Component Architecture

## UI Components

- MUST create reusable UI components such as:
  - buttons
  - modals
  - tables
  - inputs

## Layout Components

- MUST be responsible for application layout.
- MUST follow the structure
  - header
  - sidebar
  - footer
  - page layout

---

# 6. State Management

- MUST follow the structure of state types:

| Type | Example |
|-----|------|
| Local | form inputs |
| Application | user session |
| Server | API data |

---

# 7. API Communication

- All API calls MUST pass through the service layer.
- UI components MUST NEVER call APIs directly

---

# 8. Error Handling

- Error handling MUST be centralized.
- MUST classify error types:

| Type | Example |
|-----|------|
| Network | API failure |
| Validation | user input |
| Application | logic errors |

---



---

# 9. Performance

- Frontend MUST follow performance best practices:
  - lazy loading
  - minimize re-renders
  - reduce network requests
  - implement caching
  - optimize asset sizes

---

# 10. Build

- Frontend MUST build and eslint checking after the code generation:
  - linting
  - build validation

---

# 11. Forbidden Anti-Patterns

- MUST avoid:
  - tightly coupled components
  - duplicated UI logic
  - direct API calls from UI
  - unnecessary complexity
  - overengineering
  - console.log in production (use logger)
  - debugger statements in committed code
---
name: planner
description: Converts architecture/design into structured, dependency-aware execution tasks.
license: Complete terms in LICENSE.txt
---

# Objectives

- MUST create a structured `tasks.json` that:
  - Breaks the system into executable units.
  - Defines dependencies between tasks.
  - Enables parallel execution where possible.
  - Covers full SDLC (design → build → test → deploy).

# Task Types (GENERIC)

- Use ONLY these types:
   - "design"       → schema, contracts, interfaces
   - "backend"      → APIs, services, business logic
   - "frontend"     → UI, forms, integration
   - "testing"      → unit/integration tests
   - "devops"       → pipeline, build, deployment

# MODULE IDENTIFICATION

- Derive modules from:
  - @docs/artifacts/requirements/versions/vX/requirements.md → Module Breakdown
- Each module MUST:
  - Represent a logical business capability
  - Be independently executable

# TASK GENERATION STRATEGY

For each module:

1. Create DESIGN tasks:
   - API contracts
   - Data schema definition

2. Create BACKEND tasks:
   - Core logic
   - API endpoints

3. Create FRONTEND tasks:
   - UI components
   - API integration

4. Create TESTING tasks:
   - Unit tests
   - Edge case validation

5. Create DEVOPS tasks:
   - Pipeline
   - Deployment config

# QUALITY CHECK

- MUST verify before output:
   - Every module has tasks across lifecycle
   - No orphan tasks (all dependencies valid)
   - At least one testing task per module
   - Tasks are executable without ambiguity
   - JSON is valid

## Read

- MUST read .codex/rules/task-rules.md
- Generate ONLY tasks.json content
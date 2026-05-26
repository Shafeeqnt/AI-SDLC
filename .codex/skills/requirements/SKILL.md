---
name: requirements
description: Requirements generation and business analysis specialist
---

# REQUIREMENTS GENERATION SKILL

## PURPOSE
Transform unstructured or semi-structured input into complete, structured, implementation-ready requirements.

---

## INPUT

- The input may contain:
   - Project recovery document
   - User description
   - Partial module definition
   - Business notes
   - Screenshots (described in text)

Input may be incomplete or ambiguous.

---

## OUTPUT FORMAT (STRICT)

You MUST return structured markdown in the following format:

### Module Overview

- Module Name:
- Business Purpose:
- Primary Users:

### User Stories

- For each story:
  - As a <user>
  - I want to <action>
  - So that <benefit>

### Functional Requirements

- List each requirement clearly:
   - FR1:
   - FR2:
   - FR3:

### Non-Functional Requirements

- Include:
   - Performance
   - Security
   - Scalability
   - Validation rules

### Backend Expectations

- For each API:
   - Name:
   - Method: (GET/POST/PUT/DELETE)
   - Endpoint:
   - Request Fields:
   - Response Fields:

### Data Model (Initial Draft)

- List entities and fields:
   - Entity Name:
      - Field Name
      - Type (infer intelligently)
      - Required/Optional

### Edge Cases

- MUST list:
   - Invalid inputs
   - Empty states
   - Failure scenarios

### Assumptions

- MUST list inferred assumptions:
   - Assumption 1:
   - Assumption 2:

### Open Questions / Missing Info

- MUST list gaps that need clarification:
   - Question 1:
   - Question 2:

---

## BEHAVIOR RULES

1. NEVER leave sections empty  
   → If unknown, write: "TBD"

2. INFER intelligently:
   - "Id" → integer / GUID
   - "Date" → datetime
   - "Name" → string
   - "Is/Has" → boolean

3. DO NOT hallucinate business logic
   → If unclear, move to "Open Questions"

4. MUST ALWAYS expand vague input into:
   - Concrete actions
   - Measurable outcomes

5. MUST NORMALIZE terminology
   → Use consistent naming across:
      - User stories
      - APIs
      - Data models

6. MUST BREAK DOWN complex features
   → One user story = one clear action

7. MUST INCLUDE validation thinking:
   - Required fields
   - Length constraints
   - Format rules

8. MUST THINK LIKE:
   - Product Owner
   - Business Analyst
   - API Designer

---

## QUALITY CHECKLIST (MANDATORY)

- Before output, MUST ensure:
   - At least 3 user stories per module (if possible)
   - APIs align with user actions
   - Data model supports APIs
   - Edge cases are realistic
   - Open questions exist (if ambiguity present)

---

## EXAMPLE TRANSFORMATION

### Input:

"User can register and login"

### Output (partial):

User Stories:
- As a user, I want to register an account so that I can access the system
- As a user, I want to log in so that I can use my account

Functional Requirements:
- FR1: System must allow user registration
- FR2: System must validate email uniqueness
- FR3: System must allow user login with credentials

API:
- POST /api/auth/register
- POST /api/auth/login

Data Model:
User:
- Id (GUID)
- Email (string, required)
- Password (string, required)

---

## FAILURE CONDITIONS

The output is INVALID if:
   - Sections are missing
   - Only summary is provided
   - NO APIs are defined
   - NO edge cases listed
   - NO open questions included

# OUTPUT RULES

- DO NOT invent output paths.
- DO NOT create additional folders.

## Read

- MUST read .codex/rules/artifact-rules.md
- Generate ONLY:
   - requirements.md content
   - requirement-history.json content
   - CHANGELOG.md content
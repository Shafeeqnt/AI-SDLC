This file defines **project-specific development rules, architecture decisions, and technology stack** for this repository.
Codex must follow these instructions when generating or modifying code in this project.

# 1. Project Overview

**Project Name:** Gravience Management System
**Project Type:** REST API Backend using .NET Core + SPA application built with React 
**Architecture:** Clean Architecture (Backend)

**Purpose:**  
The system should be able to track the entire life cycle of a gravience from submission till closure.

# 2. Instructions for Codex

Codex **must** Do:

- Verify that the application has no build errors
- Ensure that the necessary test cases are included
- Entity level changes needs to be incorporate in related migration files also.

Codex **must NOT** Do:

- Bypass architecture layers
- Hardcode configuration values
- Ignore validation rules

# 3. Important

- Copyright information **must reference PanApps** and use the **current calendar year**.
- Author information **must be set as `gms@panapps.com`** in all package/configuration metadata.
- Client logo **must be sourced from @doc/reference**.
- If a client logo is not provided, refer the existing content in the **Project Requirement** and create a Client logo according to it.
- MUST read .codex/rules/code-generation-enforcement-rules.md
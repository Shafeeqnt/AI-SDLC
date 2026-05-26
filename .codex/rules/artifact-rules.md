# Artifact Storage Rules

All generated outputs MUST follow centralized version structure.

Agents MUST NEVER invent folder paths.

---

# Standard Structure

/docs/artifacts/
    requirements/
        versions/vX/requirements.md

    history/requirement-history.json
    CHANGELOG.md

    plans/
        versions/vX/task.json

    architecture/
        versions/vX/architecture.md

---

# Rules

1. Use ONLY orchestrator-provided version
2. NEVER create custom folders
3. NEVER duplicate version structures
4. NEVER mix:
   - version
   - versions
   - v1
   - version1

---

# Path Resolution Rules

IF current_version = v4

THEN:
- requirements → /docs/artifacts/requirements/versions/v4/requirements.md
- plans → /docs/artifacts/plans/versions/v4/task.json
- architecture → /docs/artifacts/architecture/versions/v4/architecture.md

# CHANGELOG RULES:

- MUST append an entry to `/docs/artifacts/CHANGELOG.md`
- MUST include:
    - timestamp
    - planner name
    - summary of generated tasks
- MUST update changelog on every execution
- MUST NOT overwrite existing entries
- MUST ask confirmation before changelog was updated
- MUST return confirmation that changelog was updated
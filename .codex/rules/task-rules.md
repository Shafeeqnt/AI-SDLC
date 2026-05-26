# Task Json Rules

All generated outputs MUST follow centralized version structure.

Agents MUST follow the strcture.

# Output Structure

- You MUST return ONLY valid JSON
- DO NOT:
   - add markdown
   - add explanations
   - add comments
   - add trailing commas
   - add additional root properties
   - add unknown fields
- ROOT FORMAT:
   {
      "backend": [],
      "frontend": [],
      "testing": [],
      "devops": []
   }

- TASK OBJECT FORMAT:
   {
      "id": "BKD-001",
      "name": "Create authentication service",
      "module": "Authentication",
      "type": "backend",
      "depends_on": ["DSN-001"],
      "parallelizable": false,
      "artifacts": []
   }

- FIELD RULES:
   - id → string
   - name → string
   - module → string
   - type → enum:
      - backend
      - frontend
      - testing
      - devops
      - design
      - data
- depends_on → string[]
- parallelizable → boolean
- artifacts → string[]

- FORBIDDEN:
   - estimated_hours
   - priority
   - status
   - description
   - owner
   - notes
   - comments

### Example

{
  "backend": [
    {
      "id": "BKD-001",
      "name": "Create user entity",
      "module": "User Management",
      "type": "backend",
      "depends_on": [],
      "parallelizable": true,
      "artifacts": []
    }
  ],
  "frontend": [
    {
      "id": "FRD-001",
      "name": "Create user listing page",
      "module": "User Management",
      "type": "frontend",
      "depends_on": ["BKD-001"],
      "parallelizable": false,
      "artifacts": []
    }
  ],
  "testing": [],
  "devops": []
}

# RULES

- DO NOT invent output paths
- DO NOT create additional folders
- DO NOT create task estimation hours in json

# FAILURE CONDITIONS

- Output is INVALID if:
   - Not valid JSON
   - Missing dependencies
   - Tasks are too broad
   - No testing or devops tasks included
   - Not aligned with architecture
  
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
# Enum Governance Rules

This document defines mandatory enterprise enumeration standards for all AI agents, generators, templates, backend services, frontend applications, migrations, and database structures.

The purpose of these rules is to ensure:

- Stable enum persistence
- AI regeneration consistency
- Localization readiness
- Database integrity
- Frontend flexibility
- Backward compatibility
- Cross-agent consistency

---

# Core Principle

- Enumerations are immutable domain identifiers.
- Display labels are presentation-only values.
- AI agents MUST NEVER persist enum labels as strings.

---

# Enumeration Architecture

- All enumerations MUST follow:
  - VALID:
    public enum Status
    {
        Open = 1,
        Submitted = 2,
        InProgress = 3,
        Completed = 4
    }
  - INVALID:
    public enum Status
    {
        Open,
        Submitted
    }

# Rules

- New enum values MUST only append
- System controlled value comparison MUST be done with IDs 
  (Eg: if(Status.Open == 1) {})
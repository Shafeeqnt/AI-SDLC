# Code Generation Enforcement Rules

All generated outputs MUST follow rules.

- ALWAYS search for existing enums before creating new ones.
- ALWAYS reuse centralized enums.
- NEVER generate hardcoded status IDs.
- NEVER generate magic numbers.
- NEVER generate duplicated enum definitions.
- ALWAYS validate DTO property names before generating components.
- ALWAYS verify backend contracts before creating frontend state models.

## Comment Placement Rules

- MUST place comments ABOVE the related code block.
- MUST explain:
  - purpose
  - business reason
  - expected behavior
  - important conditions
- MUST use multi-line comments for complex logic.
- MUST keep comments synchronized with implementation.
- MUST update comments when logic changes.
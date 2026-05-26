# Figma Implementation Rules

- MUST use Figma designs as the primary UI reference.
- MUST follow spacing, typography, and layout hierarchy from Figma.
- MUST reuse design-system components before creating new components.
- MUST match responsive behavior defined in Figma.
- MUST NOT invent layouts when Figma exists.
- MUST NOT change UI structure without explicit approval.
- MUST extract:
  - colors
  - typography
  - spacing
  - component variants
  - icons
  - interaction states
  from Figma before implementation.

# REQUIRED NODE RULE

- MUST use the exact node-id from the Figma URL.
- MUST NOT use parent page fallback.
- MUST fetch node 6:68 directly.
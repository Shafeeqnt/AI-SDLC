# IMPORTANT MCP EXECUTION RULES

- MUST use MCP Figma tools BEFORE frontend generation.
- MUST fetch Figma node metadata before creating React components.
- MUST extract:
  - spacing
  - typography
  - colors
  - variants
  - component hierarchy
  - auto-layout behavior
  - responsive constraints
- MUST map Figma components to reusable React components.
- MUST NOT approximate UI when Figma exists.
- MUST NOT skip MCP fetching.
- MUST regenerate the UI if MCP fetch fails.
- MUST prioritize Figma structure over assumptions.

## MCP Failure Handling

- If MCP fetch fails:
  - MUST stop frontend generation.
  - MUST NOT hallucinate UI.
  - MUST request correct Figma access or node-id.
  - MUST log the MCP failure reason.
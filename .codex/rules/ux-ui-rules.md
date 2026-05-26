This file defines **project-specific UI/UX rules** for this repository.
Codex must follow these requirement rules when generating or modifying ui code in this project.

# 1. UI/UX Design Standards

- The application MUST resemble a professionally designed enterprise product, not an AI-generated prototype.
- UI MUST follow modern SaaS design principles with clean layout, strong hierarchy, and consistent spacing.
- **Should** avoid overly flashy, inconsistent, or template-like designs.

## Typography (MANDATORY)

- Font Family: Primary: Inter, sans-serif
- Font Usage Rules: 
    - Headings: 600–700 weight
    - Body text: 400–500 weight
    - MUST maintain clear typographic hierarchy (H1 → H6 → body → caption)
- MUST avoid mixing multiple font families.

## Color System (MANDATORY)

- MUST use minimal and professional color palette:
    - Primary color MUST use: muted corporate tone (eg: button color: #1960a4, selected action color: #1960a4, Panel color: #ebebeb, label color: #242424 etc...)
    - Navigator Panel Color: #f5f5f5
    - Body Color: #FFFFFF
    - Secondary color: subtle neutral (gray scale)
    - Accent color: limited and purposeful usage
- Ensure WCAG contrast compliance for readability.
- MUST AVOID: Neon colors, Random gradients, Over-saturated palettes

## Layout & Spacing

- MUST follow consistent spacing system (<Eg: 8px grid>).
- MUST avoid cluttered UI or cramped layouts.

CRITICAL Use: Adequate padding (<Eg:16px–24px>), Clear section separation, Proper alignment and whitespace

## Components & Styling

- MUST USE ant Design components as base, enhanced with tailwind CSS.
- Custom styling MUST: Be minimal and purposeful, Not override ant Design behavior unnecessarily
- MUST USE: Rounded corners 6px max, Soft shadows (not heavy drop shadows)
- MUST USE component background color #fff and light gray thin border
- MUST USE form input content color #212121 and fontsize 13px
- MUST USE form label content color #00000073 and fontsize 13px
- MUST USE table th, td content color #212121 and fontsize 13px
- MUST USE table th font-weight 600
- MUST USE outline icons with thin border and 6px border-radius and meaning full colors

## Visual Elements

- Images and icons MUST: Be high-quality and relevant, Follow consistent style (flat / minimal / outline)
- MUST avoid: Generic stock images that look artificial, Irrelevant illustrations
- MUST use icons from ant design only.

## Interaction & UX

- MUST ensure: Clear hover, focus, and active states, Smooth transitions (<Eg:150–250ms>)
- Maintain predictable UX patterns aligned with enterprise apps.

## Responsiveness

- UI MUST be fully responsive: Desktop-first but mobile-compatible
- MUST use tailwind responsive utilities (sm, md, lg, xl).

## Anti-AI Design Constraints

- UI MUST NOT: 
    - Look like a template or auto-generated layout
    - Use inconsistent spacing or typography
    - Mix too many colors or styles
    - Include placeholder-like content in final UI
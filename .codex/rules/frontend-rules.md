# Frontend Rules

All generated frontend outputs MUST follow centralized version structure.

Agents MUST follow the core rules.

---

# Core Rules

- Enable **strict** typing
- Every module/component/service/function MUST have ONLY one responsibility
- MUST follow maximum 30 lines per function
- MUST create reusable components over duplication
- Applications MUST be modular and domain-based
- Example domains:
  - authentication
  - users
  - dashboard
  - reports
- Each domain MUST be independently maintainable.
- NEVER hardcode configurations
- MUST use environment variables wherever required
- NEVER show data identity values in UI (eg: primary key)
- MUST handle global/centralized api and error handling interceptor 
- Code MUST be separated using:
  - Application level
  - Module/Area level
  - Feature level
- Each module MUST contain isolated implementation files.
- Routing MUST be separated:
  - Application-wise
  - Module-wise
  - Feature-wise
- MUST display values instead of ids for dropdowns.
- MUST use Async Arrow Function, Async with Parameters, One-Line Async Arrow Function
- MUST use latest ECMAScript features
- MUST reduce initial page load time
- MUST reduce JavaScript execution cost
- UI Components:
  - MUST be reusable
  - MUST NOT contain business logic
- State Management:
  - shared state MUST be centralized
  - local state remains inside components
  - MUST avoid unnecessary global state
- Routing:
  - Routing MUST be centralized.
  - MUST centralized route definitions
  - MUST implement role-based access support
  - MUST implement unauthorized route protection
- Error Handling:
  - MUST provide meaningful user feedback
  - MUST NOT expose sensitive information 
- Responsiveness & Accessibility:
  - UI MUST be:
    - responsive
    - mobile compatible
    - readable with proper contrast
    - accessible via keyboard navigation

# Enum & Role Identity Rules

- MUST define ALL application enumerations in a single centralized location
- MUST organize enums by domain/module.
- MUST keep enum IDs immutable once released.
- MUST maintain identical enum IDs between:
  - backend
  - frontend
  - database seed data
  - authorization logic
  - workflow engines
  - reporting systems

# API Response Field Alignment Rules

- MUST ensure frontend and backend enum IDs are synchronized
- MUST expose enum metadata APIs when frontend dropdowns depend on backend enums
- MUST verify frontend type definitions EXACTLY match backend DTO contracts before implementing UI logic.
- MUST validate:
  - field names
  - nullability
  - array types
  - enum types
  - nested objects
- MUST update shared types FIRST when backend APIs change
- MUST refactor consuming code ONLY AFTER updating type definitions
- MUST never assume alternative field names
  
# Security Standards And Rules

- Frontend MUST implement:
  - XSS protection
  - CSRF protection
  - secure authentication handling
  - secure API communication
  - client-side input validation
  - sensitive data protection
  - dependency security
  - secure build configuration
  - Content Security Policy
  - clickjacking protection
  - UI security policies
  - logging and monitoring
- MUST NEVER hardcode secrets

# OWASP Compliance And Rules

- All frontend MUST comply with OWASP Top 10, including:
  - XSS
  - broken access control
  - sensitive data exposure
  - CSRF
  - clickjacking
  - CORS misuse awareness
  - dependency vulnerabilities
  - insecure third-party scripts
  - DOM-based attacks
  - secure build configuration

# Design Rules

- If a **Figma URL** is provided → follow it exactly
- If a **style guide** is provided → override default styling
- DO NOT invent UI styles beyond provided designs
- Styling MUST use component-level inline styles

# Webpack Rules

- MUST enable splitChunks optimization.
- MUST configure cache groups properly.
- MUST use runtime chunk separation.

# Build Tool Rules

- MUST configure manualChunks for large dependencies.
- MUST optimize dependency pre-bundling.
- MUST use build compression where applicable.

## Read

- MUST read .codex/rules/frontend-rules.md
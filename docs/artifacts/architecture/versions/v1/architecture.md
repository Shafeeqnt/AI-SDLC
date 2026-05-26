# Grievance Management Application Architecture v1

## Solution Overview

The Grievance Management Application will be implemented as a web-based system with a React frontend and a .NET 10 backend exposing REST APIs. The solution will support secure access for end users, staff members, and system administrators to submit, track, review, and administer grievances in a controlled and auditable workflow.

The architecture follows Clean Architecture to isolate domain rules from infrastructure concerns, support testability, and keep business workflows stable as the application grows.

## Technology Stack

### Backend

- Language: C#
- Framework: .NET 10
- API Style: REST Minimal API
- API Port: 5000
- Architecture: Clean Architecture
- ORM: Entity Framework Core
- Mediator Pattern: MediatR
- Validation: FluentValidation
- Authentication: JWT Bearer Authentication
- API Documentation: Swagger
- Logging: Serilog
- Testing: xUnit, Moq, FluentAssertions
- Root Folder: `Api`

### Frontend

- Language: TypeScript
- Framework: React with functional components
- Build Tool: Vite
- Styling: Tailwind CSS
- Application Port: 5001
- Root Folder: `App`

### Database

- Database Type: MySQL

## Proposed Solution Structure

```text
Api/
  GMS.Api/
  GMS.Application/
  GMS.Domain/
  GMS.Infrastructure/
  GMS.Application.Tests/

App/
  src/
    app/
    components/
    features/
      auth/
      grievances/
      users/
      notifications/
      settings/
    services/
    hooks/
    types/
```

## Backend Architecture

### Layer Responsibilities

- `GMS.Api`: Minimal API endpoints, authentication wiring, middleware, Swagger, exception handling, and dependency registration.
- `GMS.Application`: Use cases, commands, queries, validators, DTOs, response contracts, and notification orchestration.
- `GMS.Domain`: Core entities, enums, domain rules, and business invariants.
- `GMS.Infrastructure`: EF Core persistence, JWT services, password hashing, notification providers, email integration, migrations, and repository implementations.
- `GMS.Application.Tests`: Unit tests for handlers, validators, and application services.

### API Response Standard

All APIs must return the mandated envelope:

```json
{
  "success": true,
  "statusCode": 200,
  "data": {},
  "message": "Operation successful",
  "errors": []
}
```

### Cross-Cutting Components

- Authentication middleware for JWT validation
- Authorization policies by role
- Global exception handling middleware
- Request validation pipeline using FluentValidation + MediatR behaviors
- Structured logging using Serilog
- Audit logging for authentication, user administration, and grievance lifecycle changes

## Core Domains

### Identity and Access

Responsible for:

- User authentication
- Password reset by email token or recovery questions
- Password expiry enforcement
- Role-based access control
- Internal and external user management

Primary entities:

- User
- Role
- UserRole
- RecoveryQuestion

### Grievance Management

Responsible for:

- Grievance submission
- Unique reference generation
- User grievance tracking
- Staff grievance review visibility
- Status lifecycle management

Primary entities:

- Grievance
- GrievanceStatusHistory

### Notifications

Responsible for:

- New grievance alerts for relevant staff
- In-app notification storage
- Optional email notification extension point

Primary entities:

- Notification

### Administration

Responsible for:

- User administration
- Role assignment
- System setting maintenance

Primary entities:

- SystemSetting

## Domain Model Design

### Key Entities

- `User`
  - `UserId`
  - `Username`
  - `Email`
  - `UserType`
  - `IsActive`
  - `PasswordHash`
  - `PasswordLastChangedDate`

- `Role`
  - `RoleId`
  - `RoleName`
  - `RoleDescription`

- `RecoveryQuestion`
  - `RecoveryQuestionId`
  - `UserId`
  - `QuestionText`
  - `AnswerHash`

- `Grievance`
  - `GrievanceId`
  - `ReferenceNumber`
  - `ComplainerName`
  - `OrganizationName`
  - `ContactNumber`
  - `EmailAddress`
  - `GrievanceDescription`
  - `ProjectName`
  - `ProjectId`
  - `StatusId`
  - `CreatedByUserId`
  - `CreatedDate`
  - `UpdatedDate`
  - `ClosedDate`

- `Notification`
  - `NotificationId`
  - `RecipientUserId`
  - `GrievanceId`
  - `Message`
  - `IsRead`
  - `CreatedDate`

- `SystemSetting`
  - `SettingId`
  - `SettingKey`
  - `SettingValue`
  - `UpdatedByUserId`
  - `UpdatedDate`

- `GrievanceStatusHistory`
  - `GrievanceStatusHistoryId`
  - `GrievanceId`
  - `FromStatusId`
  - `ToStatusId`
  - `ChangedByUserId`
  - `ChangedDate`
  - `Comment`

### Enum Governance

Enums must use explicit integer values and IDs must be persisted rather than labels.

Example:

```csharp
public enum GrievanceStatus
{
    Open = 1,
    Submitted = 2,
    InProgress = 3,
    Resolved = 4,
    Closed = 5
}
```

Notes:

- Final grievance statuses remain business-confirmation dependent.
- New enum values must append only.
- Frontend must consume IDs plus labels from API or a controlled mapping, not persist labels as system state.

## Database Architecture

### Persistence Strategy

- EF Core Code First with MySQL provider
- Migration history maintained in source control
- Automatic migration execution at application startup
- Seed data execution during startup after successful migration

### Initial Tables

- `Users`
- `Roles`
- `UserRoles`
- `RecoveryQuestions`
- `Grievances`
- `GrievanceStatusHistories`
- `Notifications`
- `SystemSettings`

### Constraints and Indexes

- Unique index on `Users.Username`
- Unique index on `Users.Email`
- Unique index on `Grievances.ReferenceNumber`
- Index on `Grievances.StatusId`
- Index on `Grievances.ProjectId`
- Index on `Notifications.RecipientUserId`
- Foreign key constraints across all relationship tables

### Seed Data

Mandatory initial seed:

- System administrator account
  - Username: `sysadmin`
  - Password: `Assyst@123`
- Default roles
  - `SystemAdministrator`
  - `StaffMember`
  - `EndUser`
- Baseline system settings required for password policy and notification behavior

Implementation note:

- Seed password must be hashed before persistence.
- Production rollout should force the initial sysadmin password change at first login.

## API Architecture

### Authentication Endpoints

- `POST /api/auth/login`
- `POST /api/auth/forgot-password`
- `POST /api/auth/reset-password`
- `POST /api/auth/reset-password/recovery`

### User Administration Endpoints

- `GET /api/users`
- `POST /api/users`
- `PUT /api/users/{userId}`

### Grievance Endpoints

- `POST /api/grievances`
- `GET /api/grievances/my`
- `GET /api/grievances/{grievanceId}`
- `GET /api/grievances`

### Notification Endpoints

- `GET /api/notifications`

### Endpoint Design Rules

- Minimal APIs grouped by feature
- Handlers delegated to MediatR commands and queries
- Input validation before handler execution
- Role checks enforced at endpoint or policy layer
- Paginated list endpoints for user, grievance, and notification collections

## Frontend Architecture

### Application Structure

- `app/`: routing, shell, providers, auth bootstrap
- `components/`: reusable UI components
- `features/auth/`: login, forgot password, reset password
- `features/grievances/`: submit grievance, grievance list, grievance detail, grievance status tracking
- `features/users/`: user list, create user, update user, role management
- `features/notifications/`: notification center
- `features/settings/`: administrative settings
- `services/`: API client, auth token handling, API wrappers
- `types/`: shared frontend DTOs and enums

### UI Modules

- Login page
- Forgot password page
- Reset password page
- Dashboard shell
- Grievance submission form
- My grievances list
- Grievance detail page
- User management page
- Notification panel
- System settings page

### State Management

- React local state for simple forms and screen interactions
- Shared auth/session context for current user and role claims
- Query/mutation service layer for backend integration
- Client-side route guards based on authenticated roles

### Validation Strategy

- Frontend validation aligned with backend FluentValidation rules
- Required-field validation for grievance submission
- Email format validation
- Password strength validation hints
- Controlled display of API validation errors from the standard response envelope

## Security Architecture

- JWT bearer tokens for authenticated API access
- Password hashing with a strong one-way algorithm in infrastructure
- Role-based authorization for all sensitive routes
- Password reset tokens must expire
- Recovery question answers must be stored as hashes, not plaintext
- Rate limiting for authentication and password reset endpoints is recommended
- Audit trail for login attempts, password resets, user updates, and grievance status changes

## Notification Architecture

- Notification creation triggered when a grievance is submitted
- Store notifications in the database for in-app consumption
- Support extensible notifier interface for future email delivery
- Notification failure must not roll back successful grievance creation unless business rules later require strict coupling

## Reporting Architecture

- Initial reporting is query-based record viewing only
- Reporting filters should support status, reference number, and project ID
- Exporting, analytics, and external BI integration remain out of scope for v1

## Deployment View

### Runtime Components

- React SPA served on port `5001`
- .NET API served on port `5000`
- MySQL database instance

### Environment Configuration

- JWT secret / signing configuration
- Database connection string
- Email configuration for password reset notifications
- Password policy settings
- Allowed frontend origin configuration
- Serilog sink configuration

## Testing Strategy

- Unit tests for validators, commands, queries, and business rules
- Integration tests for API endpoints and persistence behavior
- Authentication tests for role restrictions and reset flows
- Migration tests to verify schema creation and seed data
- Frontend component and form-behavior tests for key user workflows

## Key Architectural Decisions

- Clean Architecture is used to keep grievance business rules independent from transport and persistence concerns.
- Minimal APIs plus MediatR keep endpoint definitions concise while preserving clear application use cases.
- MySQL with EF Core migrations supports a straightforward operational model and mandatory seeded startup behavior.
- Notifications are persisted as first-class records to support in-app visibility and future channel expansion.
- Role-based authorization is central because the product requirements distinguish system administrators, staff members, and end users.

## Risks and Deferred Decisions

- Exact grievance status model and transition rules are still TBD.
- Notification routing rules for “relevant staff members” are not yet defined.
- Exact system settings catalog is not yet defined.
- Final password policy values need business confirmation.
- Figma-driven screen implementation remains pending because this artifact defines architecture only, not frontend code generation.

### Module Overview

- Module Name: Grievance Management Application
- Business Purpose: Provide a structured system to receive, track, manage, and resolve grievances with transparent workflows, controlled access, and timely communication.
- Primary Users: End users, staff members, system administrators

### User Stories

- As an end user
- I want to log in securely
- So that I can access only the grievance features available to my account

- As an end user
- I want to submit a grievance with complainant, contact, project, and issue details
- So that my issue is formally recorded and can be tracked

- As an end user
- I want to track the status of grievances I raised
- So that I can understand progress and resolution state

- As a staff member
- I want to receive notifications for newly submitted grievances
- So that I can begin review and follow-up promptly

- As a system administrator
- I want to create and manage user accounts and roles
- So that only approved users can access the system with the correct permissions

- As a user
- I want to reset my password through email or recovery questions
- So that I can regain account access without manual intervention

### Functional Requirements

- FR1: The system must allow approved users to log in using a username and password.
- FR2: The system must support password reset using an email link or saved recovery questions.
- FR3: The system must guide users to create strong passwords.
- FR4: The system must enforce periodic password changes for all users except system administrators.
- FR5: The system must restrict system access to authorized users only.
- FR6: The system must support role-based access for system administrators, staff members, and end users.
- FR7: The system must allow only system administrators to create, update, activate, deactivate, and assign roles to users.
- FR8: The system must require each user account to have a valid email address and at least one assigned role.
- FR9: The system must support internal and external user types.
- FR10: The system must allow end users to submit grievances with complainant details, project information, and grievance description.
- FR11: The system must automatically assign a unique reference number to each grievance.
- FR12: The system must store key grievance information including contact number, email address, grievance description, project name, and project ID as mandatory fields.
- FR13: The system must allow users to view the status of grievances they raised.
- FR14: The system must show open grievances by default and provide an option to view closed grievances.
- FR15: The system must notify relevant staff members when a new grievance is submitted.
- FR16: The system must allow authorized users to view grievance records filtered by status and basic details.
- FR17: The system must provide configurable system-level settings appropriate to authorized administrative users.

### Non-Functional Requirements

- Performance: The system should load login, grievance submission, and grievance tracking screens within acceptable business response times under normal concurrent usage. Exact thresholds are TBD.
- Security: Passwords must be handled securely, access must be role-based, only authorized users may sign in, and password reset flows must use verified email or recovery-question checks.
- Scalability: The application should support growth in users, grievances, and reporting volume without redesign of the core grievance lifecycle. Exact capacity targets are TBD.
- Validation rules: Username and password are required for login; user email must be valid in format; each user must have at least one role; grievance submission must require contact number, email address, grievance description, project name, and project ID; unique grievance reference numbers must not duplicate; password rules must enforce strength requirements; required fields must reject blank values; email fields must follow valid email format; contact number should enforce numeric and length constraints TBD.

### Backend Expectations

- Name: User Login
- Method: POST
- Endpoint: /api/auth/login
- Request Fields: username (string, required), password (string, required)
- Response Fields: accessToken (string), refreshToken (string/TBD), userId (GUID/integer), displayName (string), roles (string array), passwordChangeRequired (boolean)

- Name: Forgot Password
- Method: POST
- Endpoint: /api/auth/forgot-password
- Request Fields: usernameOrEmail (string, required)
- Response Fields: requestAccepted (boolean), deliveryChannel (string), message (string)

- Name: Reset Password by Token
- Method: POST
- Endpoint: /api/auth/reset-password
- Request Fields: resetToken (string, required), newPassword (string, required)
- Response Fields: resetSuccess (boolean), message (string)

- Name: Reset Password by Recovery Questions
- Method: POST
- Endpoint: /api/auth/reset-password/recovery
- Request Fields: username (string, required), recoveryAnswers (object/array, required), newPassword (string, required)
- Response Fields: resetSuccess (boolean), message (string)

- Name: List Users
- Method: GET
- Endpoint: /api/users
- Request Fields: role (string, optional), userType (string, optional), status (string, optional), page (integer, optional), pageSize (integer, optional)
- Response Fields: users (array), totalCount (integer)

- Name: Create User
- Method: POST
- Endpoint: /api/users
- Request Fields: username (string, required), email (string, required), userType (string, required), roles (string array, required), isActive (boolean, optional)
- Response Fields: userId (GUID/integer), username (string), email (string), userType (string), roles (string array), isActive (boolean)

- Name: Update User
- Method: PUT
- Endpoint: /api/users/{userId}
- Request Fields: email (string, optional), userType (string, optional), roles (string array, optional), isActive (boolean, optional)
- Response Fields: userId (GUID/integer), updated (boolean), message (string)

- Name: Submit Grievance
- Method: POST
- Endpoint: /api/grievances
- Request Fields: complainerName (string, optional), organizationName (string, optional), contactNumber (string, required), emailAddress (string, required), grievanceDescription (string, required), projectName (string, required), projectId (string, required)
- Response Fields: grievanceId (GUID/integer), referenceNumber (string), status (string), createdDate (datetime), message (string)

- Name: List My Grievances
- Method: GET
- Endpoint: /api/grievances/my
- Request Fields: status (string, optional, default=open), includeClosed (boolean, optional), page (integer, optional), pageSize (integer, optional)
- Response Fields: grievances (array), totalCount (integer)

- Name: Get Grievance Detail
- Method: GET
- Endpoint: /api/grievances/{grievanceId}
- Request Fields: grievanceId (path, required)
- Response Fields: grievanceId (GUID/integer), referenceNumber (string), complainerName (string), organizationName (string), contactNumber (string), emailAddress (string), grievanceDescription (string), projectName (string), projectId (string), status (string), createdDate (datetime), updatedDate (datetime/TBD)

- Name: List Grievance Records
- Method: GET
- Endpoint: /api/grievances
- Request Fields: status (string, optional), referenceNumber (string, optional), projectId (string, optional), page (integer, optional), pageSize (integer, optional)
- Response Fields: grievances (array), totalCount (integer)

- Name: List Notifications
- Method: GET
- Endpoint: /api/notifications
- Request Fields: unreadOnly (boolean, optional), page (integer, optional), pageSize (integer, optional)
- Response Fields: notifications (array), totalCount (integer)

### Data Model (Initial Draft)

- Entity Name: User
- Field Name: userId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: username
- Type: string
- Required/Optional: Required
- Field Name: email
- Type: string
- Required/Optional: Required
- Field Name: userType
- Type: string
- Required/Optional: Required
- Field Name: isActive
- Type: boolean
- Required/Optional: Required
- Field Name: passwordHash
- Type: string
- Required/Optional: Required
- Field Name: passwordLastChangedDate
- Type: datetime
- Required/Optional: Optional

- Entity Name: Role
- Field Name: roleId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: roleName
- Type: string
- Required/Optional: Required
- Field Name: roleDescription
- Type: string
- Required/Optional: Optional

- Entity Name: UserRole
- Field Name: userRoleId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: userId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: roleId
- Type: GUID/integer
- Required/Optional: Required

- Entity Name: RecoveryQuestion
- Field Name: recoveryQuestionId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: userId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: questionText
- Type: string
- Required/Optional: Required
- Field Name: answerHash
- Type: string
- Required/Optional: Required

- Entity Name: Grievance
- Field Name: grievanceId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: referenceNumber
- Type: string
- Required/Optional: Required
- Field Name: complainerName
- Type: string
- Required/Optional: Optional
- Field Name: organizationName
- Type: string
- Required/Optional: Optional
- Field Name: contactNumber
- Type: string
- Required/Optional: Required
- Field Name: emailAddress
- Type: string
- Required/Optional: Required
- Field Name: grievanceDescription
- Type: string
- Required/Optional: Required
- Field Name: projectName
- Type: string
- Required/Optional: Required
- Field Name: projectId
- Type: string
- Required/Optional: Required
- Field Name: status
- Type: string
- Required/Optional: Required
- Field Name: createdByUserId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: createdDate
- Type: datetime
- Required/Optional: Required
- Field Name: updatedDate
- Type: datetime
- Required/Optional: Optional
- Field Name: closedDate
- Type: datetime
- Required/Optional: Optional

- Entity Name: Notification
- Field Name: notificationId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: grievanceId
- Type: GUID/integer
- Required/Optional: Optional
- Field Name: recipientUserId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: message
- Type: string
- Required/Optional: Required
- Field Name: isRead
- Type: boolean
- Required/Optional: Required
- Field Name: createdDate
- Type: datetime
- Required/Optional: Required

- Entity Name: SystemSetting
- Field Name: settingId
- Type: GUID/integer
- Required/Optional: Required
- Field Name: settingKey
- Type: string
- Required/Optional: Required
- Field Name: settingValue
- Type: string
- Required/Optional: Required
- Field Name: updatedByUserId
- Type: GUID/integer
- Required/Optional: Optional
- Field Name: updatedDate
- Type: datetime
- Required/Optional: Optional

### Edge Cases

- Invalid inputs: Login attempted with empty username or password.
- Invalid inputs: User creation attempted without a valid email address or without any assigned role.
- Invalid inputs: Grievance submission attempted without contact number, email address, grievance description, project name, or project ID.
- Invalid inputs: Contact number contains non-numeric characters or unsupported length.
- Invalid inputs: Email address format is invalid.
- Invalid inputs: Password does not meet strength policy or is reused when rotation is enforced.
- Empty states: User has no open grievances to display on the default tracking screen.
- Empty states: No grievance records match the selected reporting filters.
- Empty states: Staff user has no unread notifications.
- Failure scenarios: Password reset token is expired, invalid, or already used.
- Failure scenarios: Recovery question answers do not match stored answers.
- Failure scenarios: Notification delivery fails after grievance creation.
- Failure scenarios: Duplicate grievance reference number generation attempt must be rejected and regenerated.
- Failure scenarios: Unauthorized user attempts to access administrative user management APIs.

### Assumptions

- Assumption 1: This version covers a single web-based application used by authorized internal and external users.
- Assumption 2: Grievance workflow statuses exist, but the exact status list and transition rules are TBD.
- Assumption 3: Relevant staff members for notifications are determined by internal business assignment rules not defined in the source document.
- Assumption 4: Reporting in the current phase is limited to record viewing and filtering rather than exports or advanced dashboards.
- Assumption 5: System-level settings exist, but individual setting keys and behavior are TBD.

### Open Questions / Missing Info

- Question 1: What are the exact grievance statuses and allowed transitions between them?
- Question 2: Which user roles can view all grievance records versus only their own records?
- Question 3: What specific system-level settings must administrators manage in phase one?
- Question 4: What are the password policy details, including minimum length, complexity, history, and expiry interval?
- Question 5: Are usernames unique across the system, and what format constraints apply to them?
- Question 6: Should end users be allowed to edit or withdraw grievances after submission?
- Question 7: What channels should notifications use beyond in-app behavior, such as email or SMS?
- Question 8: What business details are included in reporting beyond status and basic grievance details?
- Question 9: What audit trail requirements apply to user management, password resets, and grievance status changes?
- Question 10: What are the exact validation constraints for contact number, project ID, and grievance description length?

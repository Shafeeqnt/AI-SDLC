# Requirement Classification Rules

IF no existing project exists:
    classification = "initial_project"

ELSE IF request introduces new functionality:
    classification = "enhancement"

ELSE IF request fixes broken behavior:
    classification = "hotfix"

ELSE IF request modifies existing workflow:
    classification = "change_request"

ELSE IF request changes architecture:
    classification = "refactor"

ELSE IF request changes platform/database:
    classification = "migration"
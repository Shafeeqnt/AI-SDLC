import type {
  ForgotPasswordValues,
  LoginValues,
  ResetPasswordByRecoveryValues,
  ResetPasswordValues,
} from '../models/authModels';

export const validateLoginValues = (values: LoginValues): string | null => {
  if (!values.username.trim()) {
    return 'Username is required.';
  }

  if (!values.password.trim()) {
    return 'Password is required.';
  }

  return null;
};

export const validateForgotPasswordValues = (values: ForgotPasswordValues): string | null => {
  if (!values.usernameOrEmail.trim()) {
    return 'Username or email is required.';
  }

  return null;
};

export const validateResetPasswordValues = (values: ResetPasswordValues): string | null => {
  if (!values.resetToken.trim()) {
    return 'Reset token is required.';
  }

  if (!values.newPassword.trim()) {
    return 'New password is required.';
  }

  if (values.newPassword.length < 8) {
    return 'New password must be at least 8 characters.';
  }

  return null;
};

export const validateRecoveryResetValues = (values: ResetPasswordByRecoveryValues): string | null => {
  if (!values.username.trim()) {
    return 'Username is required.';
  }

  if (values.recoveryAnswers.some((answer) => !answer.trim())) {
    return 'All recovery answers are required.';
  }

  if (!values.newPassword.trim()) {
    return 'New password is required.';
  }

  if (values.newPassword.length < 8) {
    return 'New password must be at least 8 characters.';
  }

  return null;
};

export type AuthResult = {
  accessToken: string;
  userId: number;
  username: string;
  email: string;
  roleIds: number[];
  roles: string[];
  passwordChangeRequired: boolean;
};

export type Session = AuthResult;

export type LoginValues = {
  username: string;
  password: string;
};

export type ForgotPasswordValues = {
  usernameOrEmail: string;
};

export type ResetPasswordValues = {
  resetToken: string;
  newPassword: string;
};

export type ResetPasswordByRecoveryValues = {
  username: string;
  recoveryAnswers: string[];
  newPassword: string;
};

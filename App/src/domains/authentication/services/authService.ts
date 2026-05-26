import type { ApiEnvelope } from '../../../shared/models/api';
import { httpClient } from '../../../shared/data/httpClient';
import type {
  AuthResult,
  ForgotPasswordValues,
  LoginValues,
  ResetPasswordByRecoveryValues,
  ResetPasswordValues,
} from '../models/authModels';

export const authService = {
  async login(values: LoginValues) {
    if (!values.username || !values.password) {
      throw new Error('Username and password are required.');
    }

    const response = await httpClient.post<ApiEnvelope<AuthResult>>('/api/auth/login', values);
    return response.data.data;
  },
  async forgotPassword(values: ForgotPasswordValues) {
    if (!values.usernameOrEmail) {
      throw new Error('Username or email is required.');
    }

    await httpClient.post('/api/auth/forgot-password', values);
  },
  async resetPassword(values: ResetPasswordValues) {
    if (!values.resetToken || !values.newPassword) {
      throw new Error('Reset token and new password are required.');
    }

    await httpClient.post('/api/auth/reset-password', values);
  },
  async resetPasswordByRecovery(values: ResetPasswordByRecoveryValues) {
    if (!values.username || values.recoveryAnswers.length === 0 || !values.newPassword) {
      throw new Error('Recovery reset fields are required.');
    }

    await httpClient.post('/api/auth/reset-password/recovery', values);
  },
};

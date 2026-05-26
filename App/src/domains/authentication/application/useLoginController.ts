import { App, Form } from 'antd';
import { useNavigate } from 'react-router-dom';
import { useAuth } from './useAuth';
import type {
  ForgotPasswordValues,
  LoginValues,
  ResetPasswordByRecoveryValues,
  ResetPasswordValues,
} from '../models/authModels';
import {
  validateForgotPasswordValues,
  validateLoginValues,
  validateRecoveryResetValues,
  validateResetPasswordValues,
} from '../validations/authValidation';
import { authService } from '../services/authService';
import { getErrorMessage } from '../../../shared/utils/errors';

export const useLoginController = () => {
  const navigate = useNavigate();
  const { message } = App.useApp();
  const { signIn } = useAuth();
  const [loginForm] = Form.useForm<LoginValues>();
  const [forgotForm] = Form.useForm<ForgotPasswordValues>();
  const [tokenResetForm] = Form.useForm<ResetPasswordValues>();
  const [recoveryForm] = Form.useForm<ResetPasswordByRecoveryValues>();

  const handleLogin = async (values: LoginValues) => {
    const validationMessage = validateLoginValues(values);
    if (validationMessage) {
      message.error(validationMessage);
      return;
    }

    try {
      await signIn(values);
      message.success('Signed in successfully.');
      navigate('/grievances', { replace: true });
    } catch (error) {
      message.error(getErrorMessage(error));
    }
  };

  const handleForgotPassword = async (values: ForgotPasswordValues) => {
    const validationMessage = validateForgotPasswordValues(values);
    if (validationMessage) {
      message.error(validationMessage);
      return;
    }

    try {
      await authService.forgotPassword(values);
      message.success('Password reset instructions were queued.');
      forgotForm.resetFields();
    } catch (error) {
      message.error(getErrorMessage(error));
    }
  };

  const handleTokenReset = async (values: ResetPasswordValues) => {
    const validationMessage = validateResetPasswordValues(values);
    if (validationMessage) {
      message.error(validationMessage);
      return;
    }

    try {
      await authService.resetPassword(values);
      message.success('Password updated successfully.');
      tokenResetForm.resetFields();
    } catch (error) {
      message.error(getErrorMessage(error));
    }
  };

  const handleRecoveryReset = async (values: ResetPasswordByRecoveryValues) => {
    const validationMessage = validateRecoveryResetValues(values);
    if (validationMessage) {
      message.error(validationMessage);
      return;
    }

    try {
      await authService.resetPasswordByRecovery(values);
      message.success('Password updated successfully.');
      recoveryForm.resetFields();
    } catch (error) {
      message.error(getErrorMessage(error));
    }
  };

  return {
    loginForm,
    forgotForm,
    tokenResetForm,
    recoveryForm,
    handleLogin,
    handleForgotPassword,
    handleTokenReset,
    handleRecoveryReset,
  };
};

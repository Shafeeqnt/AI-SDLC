import type { UpdateUserPayload, UserPayload } from '../models/userModels';

export const validateCreateUser = (values: UserPayload): string | null => {
  if (!values.username.trim()) {
    return 'Username is required.';
  }

  if (!values.email.trim()) {
    return 'Email is required.';
  }

  if (!values.password.trim()) {
    return 'Password is required.';
  }

  if (values.roleIds.length === 0) {
    return 'At least one role is required.';
  }

  return null;
};

export const validateUpdateUser = (values: UpdateUserPayload): string | null => {
  if (!values.email.trim()) {
    return 'Email is required.';
  }

  if (values.roleIds.length === 0) {
    return 'At least one role is required.';
  }

  return null;
};

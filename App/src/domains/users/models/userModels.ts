import type { PagedResult } from '../../../shared/models/api';

export type User = {
  userId: number;
  username: string;
  email: string;
  userTypeId: number;
  isActive: boolean;
  roleIds: number[];
  roles: string[];
};

export type UserPayload = {
  username: string;
  email: string;
  userTypeId: number;
  isActive: boolean;
  password: string;
  roleIds: number[];
};

export type UpdateUserPayload = Omit<UserPayload, 'username' | 'password'>;

export type UserList = PagedResult<User>;

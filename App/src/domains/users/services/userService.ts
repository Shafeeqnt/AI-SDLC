import type { ApiEnvelope } from '../../../shared/models/api';
import { httpClient } from '../../../shared/data/httpClient';
import type { User, UserList, UserPayload, UpdateUserPayload } from '../models/userModels';

export const userService = {
  async list() {
    const response = await httpClient.get<ApiEnvelope<UserList>>('/api/users', {
      params: { page: 1, pageSize: 50 },
    });
    return response.data.data;
  },
  async create(values: UserPayload) {
    const response = await httpClient.post<ApiEnvelope<User>>('/api/users', values);
    return response.data.data;
  },
  async update(userId: number, values: UpdateUserPayload) {
    const response = await httpClient.put<ApiEnvelope<User>>(`/api/users/${userId}`, values);
    return response.data.data;
  },
};

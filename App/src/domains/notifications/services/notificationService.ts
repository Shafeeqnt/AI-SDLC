import type { ApiEnvelope } from '../../../shared/models/api';
import { httpClient } from '../../../shared/data/httpClient';
import type { NotificationList } from '../models/notificationModels';

export const notificationService = {
  async list(unreadOnly = false) {
    const response = await httpClient.get<ApiEnvelope<NotificationList>>('/api/notifications', {
      params: {
        unreadOnly,
        page: 1,
        pageSize: 50,
      },
    });

    return response.data.data;
  },
};

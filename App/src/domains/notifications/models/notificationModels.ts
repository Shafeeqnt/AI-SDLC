import type { PagedResult } from '../../../shared/models/api';

export type NotificationItem = {
  notificationId: number;
  grievanceId?: number | null;
  message: string;
  isRead: boolean;
  createdDateUtc: string;
};

export type NotificationList = PagedResult<NotificationItem>;

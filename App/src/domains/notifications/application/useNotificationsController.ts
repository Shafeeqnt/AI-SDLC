import { useQuery } from '@tanstack/react-query';
import { notificationService } from '../services/notificationService';

export const useNotificationsController = () =>
  useQuery({
    queryKey: ['notifications'],
    queryFn: () => notificationService.list(false),
  });

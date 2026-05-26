import { App } from 'antd';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { settingsService } from '../services/settingsService';
import { getErrorMessage } from '../../../shared/utils/errors';
import { validateSettingValue } from '../validations/settingsValidation';

export const useSettingsController = () => {
  const queryClient = useQueryClient();
  const { message } = App.useApp();

  const settingsQuery = useQuery({
    queryKey: ['settings'],
    queryFn: settingsService.list,
  });

  const updateMutation = useMutation({
    mutationFn: ({ settingKey, settingValue }: { settingKey: string; settingValue: string }) =>
      settingsService.update(settingKey, settingValue),
    onSuccess: async () => {
      message.success('Setting saved.');
      await queryClient.invalidateQueries({ queryKey: ['settings'] });
    },
    onError: (error) => {
      message.error(getErrorMessage(error));
    },
  });

  const handleSave = (settingKey: string, settingValue: string) => {
    if (!settingKey.trim()) {
      message.error('Setting key is required.');
      return;
    }

    const validationMessage = validateSettingValue(settingValue);
    if (validationMessage) {
      message.error(validationMessage);
      return;
    }

    updateMutation.mutate({ settingKey, settingValue });
  };

  return {
    settingsQuery,
    handleSave,
    isSaving: updateMutation.isPending,
  };
};

import type { ApiEnvelope } from '../../../shared/models/api';
import { httpClient } from '../../../shared/data/httpClient';
import type { SettingItem } from '../models/settingsModels';

export const settingsService = {
  async list() {
    const response = await httpClient.get<ApiEnvelope<SettingItem[]>>('/api/settings');
    return response.data.data;
  },
  async update(settingKey: string, settingValue: string) {
    const response = await httpClient.put<ApiEnvelope<SettingItem>>(`/api/settings/${settingKey}`, { settingValue });
    return response.data.data;
  },
};

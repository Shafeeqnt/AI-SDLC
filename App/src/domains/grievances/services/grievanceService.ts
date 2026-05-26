import type { ApiEnvelope } from '../../../shared/models/api';
import { httpClient } from '../../../shared/data/httpClient';
import type { Grievance, GrievanceList, SubmitGrievanceValues } from '../models/grievanceModels';

export const grievanceService = {
  async listMine(statusId?: number) {
    const response = await httpClient.get<ApiEnvelope<GrievanceList>>('/api/grievances/my', {
      params: {
        statusId,
        page: 1,
        pageSize: 20,
      },
    });

    return response.data.data;
  },
  async listQueue(statusId?: number) {
    const response = await httpClient.get<ApiEnvelope<GrievanceList>>('/api/grievances', {
      params: {
        statusId,
        page: 1,
        pageSize: 20,
      },
    });

    return response.data.data;
  },
  async submit(values: SubmitGrievanceValues) {
    const response = await httpClient.post<ApiEnvelope<Grievance>>('/api/grievances', values);
    return response.data.data;
  },
};

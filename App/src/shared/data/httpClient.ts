import axios from 'axios';
import { appConfig } from '../../app/config';
import { getStoredSession } from '../../domains/authentication/application/sessionStorage';

export const httpClient = axios.create({
  baseURL: appConfig.apiBaseUrl,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
  },
});

httpClient.interceptors.request.use((config) => {
  if (!config) {
    return config;
  }

  const session = getStoredSession();
  if (!session?.accessToken) {
    return config;
  }

  config.headers.Authorization = `Bearer ${session.accessToken}`;
  return config;
});

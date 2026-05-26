import { appConfig } from '../../../app/config';
import type { Session } from '../models/authModels';

export const getStoredSession = (): Session | null => {
  const rawValue = window.localStorage.getItem(appConfig.storageKey);
  if (!rawValue) {
    return null;
  }

  try {
    return JSON.parse(rawValue) as Session;
  } catch {
    window.localStorage.removeItem(appConfig.storageKey);
    return null;
  }
};

export const storeSession = (session: Session | null) => {
  if (!session) {
    window.localStorage.removeItem(appConfig.storageKey);
    return;
  }

  window.localStorage.setItem(appConfig.storageKey, JSON.stringify(session));
};

import { useMemo, useState, type PropsWithChildren } from 'react';
import { authService } from '../services/authService';
import { getStoredSession, storeSession } from './sessionStorage';
import type { Session } from '../models/authModels';
import { AuthContext, type AuthContextValue } from './auth-context';

export const AuthProvider = ({ children }: PropsWithChildren) => {
  const [session, setSession] = useState<Session | null>(() => getStoredSession());

  const value = useMemo<AuthContextValue>(
    () => ({
      session,
      async signIn(values) {
        const nextSession = await authService.login(values);
        storeSession(nextSession);
        setSession(nextSession);
        return nextSession;
      },
      signOut() {
        storeSession(null);
        setSession(null);
      },
    }),
    [session],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

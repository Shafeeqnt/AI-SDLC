import { createContext } from 'react';
import type { LoginValues, Session } from '../models/authModels';

export type AuthContextValue = {
  session: Session | null;
  signIn: (values: LoginValues) => Promise<Session>;
  signOut: () => void;
};

export const AuthContext = createContext<AuthContextValue | undefined>(undefined);

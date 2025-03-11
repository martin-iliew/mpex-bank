import { createContext } from "react";

export interface AuthContextType {
  token: string | null;
  userRole: string | null;
  logout: () => void;
  isLoading: boolean;
  setTokenState: (token: string | null) => void;
  isAuthenticated: boolean;
}

export const AuthContext = createContext<AuthContextType | null>(null);

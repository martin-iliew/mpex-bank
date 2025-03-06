import { createContext } from "react";

export interface AuthContextType {
  token: string | null;
  userRole: string | null;
  isAuthenticated: boolean;
  setToken: (token: string | null) => void;
  logout: () => void;
}
export const AuthContext = createContext<AuthContextType | null>(null);

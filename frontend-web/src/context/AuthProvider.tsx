import { useState, useEffect, ReactNode } from "react";
import { AuthContext, AuthContextType } from "./AuthContext";
import { setToken, getToken, decodeToken } from "@/services/authService";

interface AuthProviderProps {
  children: ReactNode;
}

export default function AuthProvider({ children }: AuthProviderProps) {
  // eslint-disable-next-line
  debugger;

  const [token, setTokenState] = useState<string | null>(null);
  const [userRole, setUserRole] = useState<string | null>(null);

  const isAuthenticated = Boolean(token);

  useEffect(() => {
    if (getToken()) {
      const decoded = decodeToken();
      if (decoded) {
        setUserRole(decoded.userRole);
      }
    } else {
      setUserRole(null);
    }
  }, [token, userRole]);

  const handleSetToken = (newToken: string | null) => {
    setTokenState(newToken);
    setToken(newToken);
    if (newToken) {
      const decoded = decodeToken();
      if (decoded) {
        setUserRole(decoded.userRole);
      }
    } else {
      setUserRole(null);
    }
  };

  const logout = () => {
    handleSetToken(null);
  };

  const authContextValue: AuthContextType = {
    token,
    userRole,
    isAuthenticated,
    setToken: handleSetToken,
    logout,
  };

  return (
    <AuthContext.Provider value={authContextValue}>
      {children}
    </AuthContext.Provider>
  );
}

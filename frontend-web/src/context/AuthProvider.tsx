import React, { useState, useEffect, ReactNode } from "react";
import { AuthContext, AuthContextType } from "./AuthContext";
import { setToken, getToken, decodeToken } from "@/services/authService";

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [token, setTokenState] = useState<string | null>(getToken());
  const [userRole, setUserRole] = useState<string | null>(null);

  useEffect(() => {
    if (token) {
      const decoded = decodeToken();
      if (decoded) {
        setUserRole(decoded.userRole);
      }
    } else {
      setUserRole(null);
    }
  }, [token]);

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

  const isAuthenticated = Boolean(token);

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
};

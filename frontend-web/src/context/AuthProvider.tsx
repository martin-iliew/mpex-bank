import React, { useState, useEffect } from "react";
import { AuthContext } from "./AuthContext";
import {
  setToken as storeToken,
  getToken,
  decodeToken,
} from "@/services/authService";

interface AuthProviderProps {
  children: React.ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [token, setTokenState] = useState<string | null>(getToken());
  const [userRole, setUserRole] = useState<string | null>(null);

  const isAuthenticated = token !== null;

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

  const setToken = (newToken: string | null) => {
    setTokenState(newToken);
    storeToken(newToken);
    if (newToken) {
      const decoded = decodeToken();
      if (decoded) {
        setUserRole(decoded.userRole);
      } else {
        setUserRole(null);
      }
    } else {
      setUserRole(null);
    }
  };

  const logout = () => {
    setToken(null);
  };

  return (
    <AuthContext.Provider
      value={{ isAuthenticated, token, userRole, setToken, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};

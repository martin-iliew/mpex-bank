import { useState, useEffect, ReactNode } from "react";
import { AuthContext, AuthContextType } from "./AuthContext";
import { setToken, getToken, decodeToken } from "@/lib/utils";
import { refreshToken } from "@/api/auth";
import { logoutUser } from "@/api/auth";
interface AuthProviderProps {
  children: ReactNode;
}

export default function AuthProvider({ children }: AuthProviderProps) {
  const [token, setTokenState] = useState<string | null>(null);
  const [userRole, setUserRole] = useState<string | null>(null);

  const setUserRoleFromToken = () => {
    const decoded = decodeToken();
    if (decoded) {
      setUserRole(decoded.role);
      console.log(decoded.role);
    }
  };

  useEffect(() => {
    const initializeAuth = async () => {
      try {
        const storedToken = getToken();
        if (!storedToken) {
          const newAccessToken = await refreshToken();
          if (newAccessToken) {
            setTokenState(newAccessToken);
            setUserRoleFromToken();
          }
        } else {
          setTokenState(storedToken);
          setUserRoleFromToken();
        }
      } catch (error) {
        console.error("Token refresh failed", error);
        setUserRole(null);
        setTokenState(null);
      }
    };

    initializeAuth();
  }, []);

  const logout = () => {
    setToken(null);
    setUserRole(null);
    setTokenState(null);
    logoutUser();
  };

  const authContextValue: AuthContextType = {
    token,
    userRole,
    logout,
    setTokenState,
    isAuthenticated: !!token,
  };

  return (
    <AuthContext.Provider value={authContextValue}>
      {children}
    </AuthContext.Provider>
  );
}

import { useState, useEffect, ReactNode, useCallback } from "react";
import { AuthContext } from "./AuthContext";
import apiClient from "@/api/axios";

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [userRole, setUserRole] = useState<string | null>(null);

  function getCookie(name: string) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop()?.split(";").shift();
    return null;
  }

  function isTokenExpired(token: string) {
    const payload = JSON.parse(atob(token.split(".")[1]));
    const expirationTime = payload.exp * 1000;
    return Date.now() >= expirationTime;
  }

  const refreshAuthToken = useCallback(async () => {
    const refreshToken = getCookie("refreshToken");
    if (refreshToken) {
      try {
        const response = await apiClient.post("/api/Auth/refresh-token");
        const { accessToken } = response.data;
        localStorage.setItem("accessToken", accessToken);
        apiClient.defaults.headers["Authorization"] = `Bearer ${accessToken}`;
        return true;
      } catch (error) {
        console.error("Token refresh failed:", error);
        document.cookie = "refreshToken=; Max-Age=-99999999; path=/";
        return false;
      }
    }
    return false;
  }, []);

  useEffect(() => {
    const accessToken =
      localStorage.getItem("accessToken") || getCookie("accessToken");

    if (accessToken && !isTokenExpired(accessToken)) {
      apiClient.defaults.headers["Authorization"] = `Bearer ${accessToken}`;
      setIsAuthenticated(true);
      setUserRole("user");
    } else {
      const refreshToken = getCookie("refreshToken");
      if (refreshToken) {
        refreshAuthToken()
          .then((success) => {
            if (success) {
              setIsAuthenticated(true);
              setUserRole("user");
            } else {
              setIsAuthenticated(false);
              setUserRole(null);
            }
          })
          .catch(() => {
            setIsAuthenticated(false);
            setUserRole(null);
          });
      } else {
        setIsAuthenticated(false);
        setUserRole(null);
      }
    }
  }, [refreshAuthToken]);

  const login = (role: string) => {
    setIsAuthenticated(true);
    setUserRole(role);
  };

  const logout = () => {
    setIsAuthenticated(false);
    setUserRole(null);
    localStorage.removeItem("accessToken");
    document.cookie = "refreshToken=; Max-Age=-99999999; path=/";
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, userRole, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

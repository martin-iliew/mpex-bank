import { useState, useEffect, ReactNode, useCallback } from "react";
import apiClient from "@/api/axios";
import { AuthContext } from "@/context/AuthContext";

const decodeToken = (token: string) => {
  const payload = token.split(".")[1];
  const decoded = JSON.parse(atob(payload));
  return decoded;
};

const isTokenExpired = (token: string) => {
  const decoded = decodeToken(token);
  return decoded.exp * 1000 < Date.now();
};

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

  const fetchUserRole = async () => {
    try {
      const response = await apiClient.get("/api/User/info");
      setUserRole(response.data.role);
    } catch (error) {
      console.error("Failed to fetch user role:", error);
      setUserRole("Guest");
    }
  };

  const refreshAuthToken = useCallback(async () => {
    const refreshToken = getCookie("refreshToken");
    if (refreshToken) {
      try {
        const response = await apiClient.post("/api/Auth/refreshToken");
        const { accessToken } = response.data;
        apiClient.defaults.headers["Authorization"] = `Bearer ${accessToken}`;
        return true;
      } catch (error) {
        console.error("Token refresh failed:", error);
        return false;
      }
    }
    return false;
  }, []);

  useEffect(() => {
    const accessToken = getCookie("accessToken");

    if (accessToken && !isTokenExpired(accessToken)) {
      apiClient.defaults.headers["Authorization"] = `Bearer ${accessToken}`;
      setIsAuthenticated(true);
      fetchUserRole();
    } else {
      const refreshToken = getCookie("refreshToken");
      if (refreshToken) {
        refreshAuthToken()
          .then((success) => {
            if (success) {
              setIsAuthenticated(true);
              fetchUserRole();
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
    document.cookie =
      "accessToken=; Max-Age=-99999999; path=/; Secure; HttpOnly; SameSite=Strict";
    document.cookie =
      "refreshToken=; Max-Age=-99999999; path=/; Secure; HttpOnly; SameSite=Strict";
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, userRole, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

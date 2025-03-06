import { useEffect } from "react";
import { refreshToken } from "@/services/authService";

const useTokenRefresh = () => {
  useEffect(() => {
    const refreshInterval = async () => {
      try {
        await refreshToken();
      } catch (error) {
        console.error("Auto-refresh failed, logging out.", error);
      }
    };

    const interval = setInterval(refreshInterval, 14 * 60 * 1000);

    return () => clearInterval(interval);
  }, []);

  return null;
};

export default useTokenRefresh;

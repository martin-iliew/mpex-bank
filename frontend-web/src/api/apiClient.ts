import axios from "axios";
import { getToken } from "@/lib/utils";
import { refreshToken } from "@/api/auth";
import { setToken } from "@/lib/utils";
const apiClient = axios.create({
  baseURL: "https://localhost:5187/",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

apiClient.interceptors.request.use(
  (config) => {
    const token = getToken();
    if (token && !config.headers.Authorization) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  },
);

apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (
      error.response?.status === 401 &&
      !originalRequest._retry &&
      !originalRequest.url.includes("/api/Auth/refresh-token")
    ) {
      originalRequest._retry = true;
      try {
        const newAccessToken = await refreshToken();
        setToken(newAccessToken);
        originalRequest.headers["Authorization"] = `Bearer ${newAccessToken}`;
        return apiClient(originalRequest);
      } catch (tokenRefreshError) {
        return Promise.reject(tokenRefreshError);
      }
    }
    return Promise.reject(error);
  },
);

export default apiClient;

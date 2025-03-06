import axios from "axios";
import { jwtDecode } from "jwt-decode";

interface DecodedToken {
  userRole: string;
  userId: string;
}

let accessToken: string | null = null;
let isRefreshing = false;
let refreshQueue: ((token: string) => void)[] = [];

const refreshClient = axios.create({
  baseURL: "https://localhost:5187/",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

export const setToken = (token: string | null): void => {
  if (accessToken !== token) {
    accessToken = token;
  }
};

export const getToken = (): string | null => {
  return accessToken;
};

export const refreshToken = async (): Promise<string> => {
  if (isRefreshing) {
    return new Promise((resolve) => refreshQueue.push(resolve));
  }

  isRefreshing = true;
  try {
    const response = await refreshClient.post("/api/Auth/refresh-token", null, {
      withCredentials: true,
    });
    const newAccessToken = response.data.token;
    setToken(newAccessToken);
    refreshQueue.forEach((resolve) => resolve(newAccessToken));
    refreshQueue = [];
    return newAccessToken;
  } catch (error) {
    console.error("Session expired. Redirecting to login.");
    setToken(null);
    throw error;
  } finally {
    isRefreshing = false;
  }
};

export const decodeToken = (): DecodedToken | null => {
  const token = getToken();
  if (token) {
    try {
      const decoded: DecodedToken = jwtDecode(token);
      return decoded;
    } catch (error) {
      console.error("Error decoding token", error);
      return null;
    }
  }
  return null;
};

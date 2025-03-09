import axios from "axios";
import { jwtDecode } from "jwt-decode";

interface DecodedToken {
  userId: string;
  userRole: string;
}

let accessToken: string | null = null;

const refreshClient = axios.create({
  baseURL: "https://localhost:5187/",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

export const setToken = (token: string | null): void => {
  accessToken = token;
};

export const getToken = (): string | null => {
  return accessToken;
};

export const refreshToken = async (): Promise<string> => {
  try {
    const response = await refreshClient.post("/api/Auth/refresh-token");
    const newAccessToken = response.data.token;
    setToken(newAccessToken);
    return newAccessToken;
  } catch (error) {
    console.error("Session expired. Redirecting to login.");
    setToken(null);
    throw error;
  }
};

export const decodeToken = (): DecodedToken | null => {
  const token = getToken();
  if (token) {
    try {
      const decoded: DecodedToken = jwtDecode(token);
      console.log("Decoded token", decoded);
      return decoded;
    } catch (error) {
      console.error("Error decoding token", error);
      return null;
    }
  }
  return null;
};

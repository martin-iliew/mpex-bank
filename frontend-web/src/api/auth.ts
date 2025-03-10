import apiClient from "@/api/apiClient";
import axios from "axios";

export interface LoginPayload {
  email: string;
  password: string;
}

export async function loginUser(data: LoginPayload): Promise<string> {
  try {
    const response = await apiClient.post("/api/Auth/login", data);
    if (response.status === 200 && response.data.token) {
      const accessToken = response.data.token;
      return accessToken;
    } else {
      throw new Error("No token received or login failed");
    }
  } catch (error) {
    console.error("Login failed:", error);
    throw new Error("Login request failed. Please try again.");
  }
}

export async function refreshToken(): Promise<string> {
  try {
    const response = await apiClient.post("/api/Auth/refresh-token");
    if (response.status === 200 && response.data.token) {
      return response.data.token;
    } else {
      throw new Error("No token received during refresh");
    }
  } catch (error) {
    console.error("Session expired or user logged out", error);
    throw error;
  }
}

interface RegisterPayload {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}
export async function logoutUser(): Promise<void> {
  try {
    await apiClient.post("/api/Auth/logout");
  } catch (error) {
    console.error("Logout failed:", error);
    throw new Error("Logout request failed. Please try again.");
  }
}

export async function registerUser(data: RegisterPayload): Promise<void> {
  try {
    await apiClient.post("/api/Auth/register", data);
  } catch (error: unknown) {
    if (axios.isAxiosError(error)) {
      throw error.response?.data || error.message;
    } else {
      throw "An unexpected error occurred.";
    }
  }
}

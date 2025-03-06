import apiClient from "@/api/apiClient";
import axios from "axios";

export interface LoginPayload {
  email: string;
  password: string;
}

export async function loginUser(data: LoginPayload): Promise<string> {
  try {
    const response = await apiClient.post<{ token: string }>(
      "/api/Auth/login",
      data,
    );
    return response.data.token;
  } catch (error) {
    console.error("Login failed:", error);
    throw error;
  }
}

interface RegisterPayload {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
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

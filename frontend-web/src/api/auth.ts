import apiClient from "@/api/axios";
import axios from "axios";

interface LoginPayload {
  email: string;
  password: string;
}

export async function loginUser(data: LoginPayload): Promise<void> {
  try {
    await apiClient.post("/api/Auth/login", data);
  } catch (error: unknown) {
    if (axios.isAxiosError(error) && error.response) {
      throw error.response.data.message || "Login failed.";
    } else {
      throw "An unexpected error occurred.";
    }
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

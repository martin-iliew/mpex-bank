import apiClient from "@/api/axios";
import axios from "axios";

interface RegisterPayload {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  imageUrl: string;
}

export async function registerUser(
  data: RegisterPayload,
): Promise<{
  message: string;
  user: { email: string; firstName: string; lastName: string };
}> {
  try {
    const response = await apiClient.post("/api/Auth/register", data);
    return response.data;
  } catch (error: unknown) {
    if (axios.isAxiosError(error)) {
      throw error.response?.data || error.message;
    } else {
      throw "An unexpected error occurred.";
    }
  }
}

interface LoginPayload {
  email: string;
  password: string;
}

export async function loginUser(data: LoginPayload): Promise<string> {
  try {
    const response = await apiClient.post("/api/Auth/login", data);
    return response.data.accessToken;
  } catch (error: unknown) {
    if (axios.isAxiosError(error) && error.response) {
      throw error.response.data?.message || "Login failed.";
    } else {
      throw "An unexpected error occurred.";
    }
  }
}

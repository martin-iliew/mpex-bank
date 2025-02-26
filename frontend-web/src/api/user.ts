import apiClient from "@/api/axios";
import axios from "axios";

export interface UserProfile {
  firstName: string;
  lastName: string;
  imageUrl: string | null;
}

export async function fetchUserProfile(): Promise<UserProfile> {
    try {
        const response = await apiClient.get("/api/User/profile");
        return response.data;
    } catch (error: unknown) {
      if (axios.isAxiosError(error)) {
        throw error.response?.data || error.message;
      } else {
        throw "An unexpected error occurred.";
      }
    }
  }

import apiClient from "@/api/axios";
import axios from "axios";

export interface Account {
  id: string;
  userId: string;
  accountType: string;
  accountPlan: string;
  iban: string;
  balance: number;
  cards: number;
}

export async function fetchBankAccountInfo(): Promise<Account[]> {
  try {
    const response = await apiClient.get<Account[]>("/api/BankAccount"); 
    return response.data;
  } catch (error: unknown) {
    console.error("Error fetching bank account info:", error);

    if (axios.isAxiosError(error) && error.response) {
      return Promise.reject(
        error.response.data.message || "Failed to fetch bank account info.",
      );
    }

    return Promise.reject("An unexpected error occurred.");
  }
}

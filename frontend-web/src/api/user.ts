import apiClient from "@/api/apiClient";
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
export interface Card {
  id: string;
  userId: string;
  cardNumber: string;
  cvv: string;
  expiaryDate: string;
  ownerName: string;
  cardStatus: string;
}

export interface AccountById {
  id: string;
  userId: string;
  accountType: string;
  accountPlan: string;
  iban: string;
  balance: number;
  cards: Card[];
}

export async function fetchBankAccountInfo(): Promise<Account[]> {
  try {
    const response = await apiClient.get<Account[]>("/api/BankAccounts");
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

export async function fetchBankAccountById(id: string): Promise<AccountById> {
  try {
    const response = await apiClient.get<AccountById>(`/api/BankAccount/${id}`);
    return response.data;
  } catch (error: unknown) {
    console.error("Error fetching bank account by ID:", error);

    if (axios.isAxiosError(error) && error.response) {
      return Promise.reject(
        error.response.data.message || "Failed to fetch bank account details.",
      );
    }

    return Promise.reject("An unexpected error occurred.");
  }
}

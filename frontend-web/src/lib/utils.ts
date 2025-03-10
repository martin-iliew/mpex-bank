import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";
import { jwtDecode, JwtPayload } from "jwt-decode";

export interface DecodedToken extends JwtPayload {
  role: string;
}
let accessToken: string | null = null;

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const setToken = (token: string | null): void => {
  accessToken = token;
};

export const getToken = (): string | null => {
  return accessToken;
};

export const decodeToken = (): DecodedToken | null => {
  const token = getToken();
  if (token) {
    try {
      const decoded: DecodedToken = jwtDecode<DecodedToken>(token);
      return decoded;
    } catch (error) {
      console.error("Error decoding token", error);
      return null;
    }
  }
  return null;
};

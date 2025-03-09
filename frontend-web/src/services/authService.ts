// import axios from "axios";
// import jwtDecode from "jwt-decode";

// interface DecodedToken {
//   userRole: string;
//   userId: string;
// }

// // In-memory token storage
// let accessToken: string | null = null;
// let isRefreshing = false;
// let refreshQueue: ((token: string) => void)[] = [];

// // Axios instance for token refresh requests
// const refreshClient = axios.create({
//   baseURL: "https://localhost:5187/",
//   headers: {
//     "Content-Type": "application/json",
//   },
//   withCredentials: true,
// });

// // Update the in-memory token
// export const setToken = (token: string | null): void => {
//   if (accessToken !== token) {
//     accessToken = token;
//   }
// };

// // Retrieve the current token from memory
// export const getToken = (): string | null => {
//   return accessToken;
// };

// // Attempt to refresh the token. If a refresh is already in progress, queue the request.
// export const refreshToken = async (): Promise<string> => {
//   if (isRefreshing) {
//     return new Promise((resolve) => refreshQueue.push(resolve));
//   }

//   isRefreshing = true;
//   try {
//     const response = await refreshClient.post("/api/Auth/refresh-token", null, {
//       withCredentials: true,
//     });
//     const newAccessToken = response.data.token;
//     setToken(newAccessToken);
//     // Resolve any queued refresh calls
//     refreshQueue.forEach((resolve) => resolve(newAccessToken));
//     refreshQueue = [];
//     return newAccessToken;
//   } catch (error) {
//     console.error("Session expired. Redirecting to login.");
//     setToken(null);
//     throw error;
//   } finally {
//     isRefreshing = false;
//   }
// };

// // Decode the current token using jwt-decode
// export const decodeToken = (): DecodedToken | null => {
//   const token = getToken();
//   if (token) {
//     try {
//       const decoded: DecodedToken = jwtDecode(token);
//       return decoded;
//     } catch (error) {
//       console.error("Error decoding token", error);
//       return null;
//     }
//   }
//   return null;
// };

import axios from "axios";
import jwtDecode from "jwt-decode";

interface DecodedToken {
  userRole: string;
  userId: string;
}

// In-memory token storage
let accessToken: string | null = null;
let isRefreshing = false;
let refreshQueue: ((token: string) => void)[] = [];

// Listeners for token changes
let tokenListeners: ((token: string | null) => void)[] = [];

// Axios instance for token refresh requests
const refreshClient = axios.create({
  baseURL: "https://localhost:5187/",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

// Subscribe to token changes so that consumers (e.g., AuthProvider) can react to updates
export const subscribeToTokenChanges = (
  callback: (token: string | null) => void
): (() => void) => {
  tokenListeners.push(callback);
  // Return an unsubscribe function
  return () => {
    tokenListeners = tokenListeners.filter((cb) => cb !== callback);
  };
};

// Update the in-memory token and notify subscribers if it changes
export const setToken = (token: string | null): void => {
  if (accessToken !== token) {
    accessToken = token;
    tokenListeners.forEach((callback) => callback(token));
  }
};

// Retrieve the current token from memory
export const getToken = (): string | null => {
  return accessToken;
};

// Attempt to refresh the token. If a refresh is already in progress, queue the request.
export const refreshToken = async (): Promise<string> => {
  if (isRefreshing) {
    return new Promise((resolve) => refreshQueue.push(resolve));
  }

  isRefreshing = true;
  try {
    const response = await refreshClient.post(
      "/api/Auth/refresh-token",
      null,
      { withCredentials: true }
    );
    const newAccessToken = response.data.token;
    setToken(newAccessToken);
    // Resolve any queued refresh calls
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

// Decode the current token using jwt-decode
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

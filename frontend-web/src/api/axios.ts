import axios from "axios";

// Function to get the JWT token from cookies
const getTokenFromCookie = () => {
  const cookie = document.cookie
    .split("; ")
    .find((row) => row.startsWith("Token="));
  return cookie ? cookie.split("=")[1] : null; // Extracts the value of the Token cookie
};

// Set up the axios client
const apiClient = axios.create({
  baseURL: "https://localhost:5187/", // Base URL for your API
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true, // Ensure cookies are sent with requests
});

// Add an interceptor to ensure the token is sent with every request
apiClient.interceptors.request.use(
  (config) => {
    const token = getTokenFromCookie(); // Get the token from the cookie
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`; // Add the token to the Authorization header
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  },
);

// Add response interceptor to handle errors
apiClient.interceptors.response.use(
  (response) => response, // If the response is successful, return it
  (error) => {
    console.log("Full error", error);
    console.error("Axios Response Error:", error.response);

    if (error.response?.status === 401) {
      // Handle unauthorized error, such as token expiry or invalid token
      return Promise.reject(new Error("Unauthorized: Please log in again."));
    }

    return Promise.reject(error); // For all other errors, reject the promise
  },
);

export default apiClient;

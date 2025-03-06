import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import LoginPage from "./pages/(auth)/Login";
import DashboardPage from "./pages/(dashboard)/Dashboard";
import HomePage from "./pages/Home";
// import AuthGuard from "./guards/AuthGuard";
// import GuestGuard from "@/guards/GuestGuard";
import useTokenRefresh from "@/hooks/useTokenRefresh";

const App = () => {
  useTokenRefresh();
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/dashboard" element={<DashboardPage />} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;

import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import LoginPage from "./pages/(auth)/Login";
import DashboardPage from "./pages/(dashboard)/Dashboard";
import GuestGuard from "@/guards/GuestGuard";
import AuthLayout from "./layouts/AuthLayout";

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<div>Home Page</div>} />
        <Route path="/dashboard" element={<DashboardPage />} />
        <Route
          path="/login"
          element={
            <GuestGuard>
              <AuthLayout>
                <LoginPage />
              </AuthLayout>
            </GuestGuard>
          }
        />
      </Routes>
    </BrowserRouter>
  );
};

export default App;

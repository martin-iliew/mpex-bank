import { Route, Routes } from "react-router-dom";
import DashboardLayout from "@/layouts/DashboardLayout";
import AuthLayout from "@/layouts/AuthLayout";
import LandingLayout from "@/layouts/LandingLayout";
import DashboardPage from "@/pages/(dashboard)/Dashboard";
import LoginPage from "@/pages/(auth)/Login";
import RegisterPage from "@/pages/(auth)/Register";
import HomePage from "@/pages/(landing)/Home";
import GuestGuard from "@/guards/GuestGuard";
import AuthGuard from "@/guards/AuthGuard";
import { Navigate } from "react-router-dom";
import "./App.css";
const App = () => {
  return (
    <Routes>
      {/* Landing Page Routes */}
      <Route
        path="/home"
        element={
          <GuestGuard>
            <LandingLayout>
              <HomePage />
            </LandingLayout>
          </GuestGuard>
        }
      />
      <Route path="/" element={<Navigate to="/home" replace />} />

      {/* Authentication Routes */}
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
      <Route
        path="/register"
        element={
          <GuestGuard>
            <AuthLayout>
              <RegisterPage />
            </AuthLayout>
          </GuestGuard>
        }
      />

      {/* Dashboard Routes */}
      <Route
        path="/dashboard"
        element={
          <AuthGuard>
            <DashboardLayout>
              <DashboardPage />
            </DashboardLayout>
          </AuthGuard>
        }
      />
    </Routes>
  );
};

export default App;

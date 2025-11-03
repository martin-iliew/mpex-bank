import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import LoginPage from "./pages/(auth)/login/Page";
import DashboardPage from "./pages/(dashboard)/Dashboard";
import RegisterPage from "./pages/(auth)/register/Page";
import HomePage from "./pages/Home";
import AuthGuard from "./guards/AuthGuard";
import GuestGuard from "@/guards/GuestGuard";
import DashboardLayout from "@/layouts/DashboardLayout";

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route
          path="/login"
          element={
            <GuestGuard>
              <LoginPage />
            </GuestGuard>
          }
        />
        <Route
          path="/register"
          element={
            <GuestGuard>
              <RegisterPage />
            </GuestGuard>
          }
        />
        <Route
          path="/dashboard"
          element={
            <AuthGuard requiredRole="User">
              <DashboardLayout>
                <DashboardPage />
              </DashboardLayout>
            </AuthGuard>
          }
        />
      </Routes>
    </BrowserRouter>
  );
};

export default App;

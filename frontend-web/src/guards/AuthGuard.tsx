// import { Navigate, Outlet } from "react-router-dom";
// import { useAuth } from "@/hooks/useAuth";

// interface AuthGuardProps {
//   children: React.ReactNode;
//   requiredRole?: string;
// }

// const AuthGuard = ({ children, requiredRole }: AuthGuardProps) => {
//   // const { isAuthenticated, userRole } = useAuth();
//   const { userRole } = useAuth();
//   // if (!isAuthenticated) {
//   //   return <Navigate to="/login" />;
//   // }

//   if (requiredRole && userRole !== requiredRole) {
//     return <Navigate to="/unauthorized" />;
//   }

//   return children || <Outlet />;
// };

// export default AuthGuard;

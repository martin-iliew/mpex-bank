import { Link } from "react-router-dom";
import { Button } from "@/components/ui/button";
import Logo from "./ui/logo";
import { useAuth } from "@/hooks/useAuth";
import { useState } from "react";

const items = [
  {
    title: "Home",
    url: "/",
  },
  {
    title: "Dashboard",
    url: "/dashboard",
  },
];

const Header = () => {
  const { isAuthenticated } = useAuth();
  const [menuOpen, setMenuOpen] = useState(false);

  return (
    <header className="sticky top-0 z-50 w-full bg-background shadow-sm">
      <div className="nav-inner flex items-center justify-between w-full h-[var(--nav-height)]">
        {/* Logo */}
        <div>
          <Link to="/">
            <Logo size="large" varient="full" color="dark" />
          </Link>
        </div>
        {/* Desktop Navigation */}
        <nav
          role="navigation"
          aria-label="Main navigation"
          className="hidden md:flex gap-8 items-center"
        >
          <ul className="flex gap-8">
            {items.map((item) => (
              <li key={item.url}>
                <Link
                  to={item.url}
                  className="text-gray-600 hover:text-gray-900 font-medium transition-colors"
                >
                  {item.title}
                </Link>
              </li>
            ))}
          </ul>
          {!isAuthenticated ? (
            <div className="flex gap-4">
              <Link to="/login">
                <Button size="lg">Login</Button>
              </Link>
              <Link to="/register">
                <Button variant="outlinePrimary" size="lg">
                  Register
                </Button>
              </Link>
            </div>
          ) : (
            <Button variant="destructive" size="lg">
              Logout
            </Button>
          )}
        </nav>
        {/* Mobile Menu Button */}
        <button
          aria-label="Toggle menu"
          onClick={() => setMenuOpen((prev) => !prev)}
          className="md:hidden flex items-center justify-center w-10 h-10 bg-gray-200 rounded-full"
        >
          <span className="material-icons">{menuOpen ? "close" : "menu"}</span>
        </button>
      </div>

      {/* Mobile Navigation */}
      {menuOpen && (
        <nav
          role="navigation"
          aria-label="Mobile navigation"
          className="flex flex-col items-center gap-4 border-t border-border/40 bg-background p-4 md:hidden"
        >
          <ul className="flex flex-col items-center gap-4">
            {items.map((item) => (
              <li key={item.url}>
                <Link
                  to={item.url}
                  className="text-gray-600 hover:text-gray-900 font-medium transition-colors"
                >
                  {item.title}
                </Link>
              </li>
            ))}
          </ul>
          {!isAuthenticated ? (
            <div className="flex flex-col gap-4 w-full items-center">
              <Link to="/login" className="w-full">
                <Button size="lg" className="w-full">
                  Login
                </Button>
              </Link>
              <Link to="/register" className="w-full">
                <Button variant="outlinePrimary" size="lg" className="w-full">
                  Register
                </Button>
              </Link>
            </div>
          ) : (
            <Button variant="destructive" size="lg" className="w-full">
              Logout
            </Button>
          )}
        </nav>
      )}
    </header>
  );
};

export { Header };

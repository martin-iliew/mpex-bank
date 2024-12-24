import React from "react";
import { Link } from "react-router-dom";
import icon from "@/assets/logo-icon.svg";
import text from "@/assets/logo-text.svg";

interface LogoProps {
  size?: "small" | "medium" | "large";
  varient?: "icon" | "full";
  color?: "light" | "dark" | "white";
}

const Logo: React.FC<LogoProps> = ({
  size = "medium",
  varient = "full",
  // color = "dark",
}) => {
  const sizes = {
    small: "h-6",
    medium: "h-8",
    large: "h-10",
  };

  // const colors = {
  //   light: "text-white",
  //   dark: "text-gray-800",
  //   white: "text-white",
  // };

  const logoSize = sizes[size];
  // const iconColor = colors[color];

  return (
    <div className="flex items-center w-auto space-x-3 ">
      {varient === "full" && (
        <Link to="/">
          <img src={text} alt="Logo Text" className={`${logoSize}`} />
        </Link>
      )}
      <Link to="/">
        <img src={icon} alt="Logo Icon" className={`${logoSize}` } />
      </Link>
    </div>
  );
};

export default Logo;

import { ReactNode } from "react";
import clsx from "clsx";

interface TypographyProps {
  children: ReactNode;
  className?: string;
}

const DisplayLarge = ({ children, className }: TypographyProps) => {
  return (
    <h1
      className={clsx(
        "font-title text-9xl leading-tight font-black uppercase sm:text-5xl lg:text-6xl",
        className,
      )}
    >
      {children}
    </h1>
  );
};

const TitlePage = ({ children, className }: TypographyProps) => {
  return (
    <h2
      className={clsx(
        "font-title text-9xl font-black uppercase sm:text-5xl lg:text-6xl",
        className,
      )}
    >
      {children}
    </h2>
  );
};

const BodyBase = ({ children, className }: TypographyProps) => {
  return (
    <p className={clsx("font-body text-sm sm:text-base", className)}>
      {children}
    </p>
  );
};

const BodyExtraSmall = ({ children, className }: TypographyProps) => {
  return (
    <p className={clsx("font-body text-xs sm:text-sm", className)}>
      {children}
    </p>
  );
};

export { DisplayLarge, TitlePage, BodyBase, BodyExtraSmall };

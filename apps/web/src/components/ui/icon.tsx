import * as React from "react";
import { Slot } from "@radix-ui/react-slot";
import { cva, type VariantProps } from "class-variance-authority";
import { cn } from "@/lib/utils.ts";

import { IconType } from "react-icons";
import {
  HiChevronDown,
  HiChevronRight,
  HiChevronLeft,
  HiOutlineArrowPath,
  HiCheck,
  HiXMark,
  HiMagnifyingGlass,
  HiQuestionMarkCircle,
  HiPlus,
  HiMinus,
  HiPencilSquare,
  HiTrash,
  HiEye,
  HiEyeSlash,
  HiArrowUpRight,
  HiArrowDownLeft,
} from "react-icons/hi2";

export const iconLibrary: Record<string, IconType> = {
  chevronDown: HiChevronDown,
  chevronRight: HiChevronRight,
  chevronLeft: HiChevronLeft,
  refresh: HiOutlineArrowPath,
  check: HiCheck,
  close: HiXMark,
  search: HiMagnifyingGlass,
  helpCircle: HiQuestionMarkCircle,
  plus: HiPlus,
  minus: HiMinus,
  edit: HiPencilSquare,
  delete: HiTrash,
  view: HiEye,
  hide: HiEyeSlash,
  arrowUpRight: HiArrowUpRight,
  arrowDownLeft: HiArrowDownLeft,
};

const iconVariants = cva(
  "inline-flex items-center justify-center shrink-0 select-none transition-colors",
  {
    variants: {
      size: {
        sm: "[&>svg]:size-3",
        md: "[&>svg]:size-4",
        lg: "[&>svg]:size-5",
        xl: "[&>svg]:size-6",
      },
    },
    defaultVariants: {
      size: "md",
    },
  },
);

type IconProps = Omit<React.HTMLAttributes<HTMLSpanElement>, "color"> & {
  name: keyof typeof iconLibrary;
  size?: VariantProps<typeof iconVariants>["size"];
  asChild?: boolean;
};

const Icon = React.forwardRef<HTMLSpanElement, IconProps>(
  ({ name, size, className, asChild = false, ...props }, ref) => {
    const Comp = asChild ? Slot : "span";
    const IconComponent = iconLibrary[name];

    if (!IconComponent) {
      console.warn(`Icon "${name}" not found in iconLibrary.`);
      return null;
    }

    return (
      <Comp
        ref={ref}
        data-slot="icon"
        className={cn(iconVariants({ size }), className)}
        {...props}
      >
        <IconComponent aria-hidden="true" focusable="false" />
      </Comp>
    );
  },
);

Icon.displayName = "Icon";

export { Icon, iconVariants };

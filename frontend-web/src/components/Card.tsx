import { motion } from "framer-motion";
import { useState } from "react";

interface CardProps {
  card: {
    cardNumber: string;
    ownerName: string;
    expiaryDate: string;
    cvv: string;
  };
}

export function CardComponent({ card }: CardProps) {
  const [isFlipped, setIsFlipped] = useState(false);

  const handleFlip = () => {
    setIsFlipped(!isFlipped);
  };

  return (
    <div
      className="relative h-56 w-full max-w-sm cursor-pointer"
      onClick={handleFlip}
      style={{ perspective: "1000px" }}
    >
      <motion.div
        className="relative h-full w-full"
        initial={false}
        animate={{ rotateY: isFlipped ? 180 : 0 }}
        transition={{ duration: 0.6 }}
        style={{ transformStyle: "preserve-3d" }}
      >
        <div
          className="absolute inset-0 rounded-xl bg-gradient-to-br from-red-700 to-red-600 p-6 shadow-xl backface-hidden"
          style={{ backfaceVisibility: "hidden" }}
        >
          <div className="flex h-full flex-col justify-between">
            <div className="flex items-start justify-between">
              <div className="text-lg font-semibold text-white/90">Mpex</div>
            </div>

            <div className="space-y-6">
              <div className="text-xl font-medium tracking-wider text-white">
                <strong>Card Number:</strong> **** **** ****{" "}
                {card.cardNumber.slice(-4)}
              </div>

              <div className="flex justify-between">
                <div className="space-y-1">
                  <div className="text-xs text-white/70">CARD HOLDER</div>
                  <div className="text-sm font-medium tracking-wide text-white">
                    {card.ownerName}
                  </div>
                </div>
                <div className="space-y-1">
                  <div className="text-xs text-white/70">EXPIRES</div>
                  <div className="text-sm font-medium tracking-wide text-white">
                    {card.expiaryDate}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div
          className="absolute inset-0 rounded-xl bg-gradient-to-br from-violet-500 to-purple-700 p-6 shadow-xl backface-hidden"
          style={{ backfaceVisibility: "hidden", transform: "rotateY(180deg)" }}
        >
          <div className="flex h-full flex-col justify-between">
            <div className="mt-4 h-10 w-full bg-black/80" />

            <div className="space-y-4">
              <div className="flex justify-end">
                <div className="relative flex h-10 w-16 items-center justify-center rounded bg-white/90">
                  <span className="text-sm font-medium text-gray-800">
                    {card.cvv}
                  </span>
                </div>
              </div>

              <div className="h-10 w-full bg-gray-200/20 m-0.5 font-se" />

              <div className="text-right text-xs text-white/80">
                For customer service, call the number on the back of your card.
              </div>
            </div>
          </div>
        </div>
      </motion.div>
    </div>
  );
}

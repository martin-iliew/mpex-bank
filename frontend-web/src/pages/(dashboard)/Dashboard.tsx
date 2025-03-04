import { useQuery } from "react-query";
import { fetchBankAccountInfo, Account } from "@/api/user";
import { Skeleton } from "@/components/ui/skeleton";

export default function DashboardPage() {
  const { data, isLoading, error } = useQuery<Account[] | null>(
    "bankAccountInfo",
    fetchBankAccountInfo,
  );

  if (isLoading) {
    return (
      <div className="mt-11 ml-10">
        <Skeleton className="h-10 w-[360px]" />
      </div>
    );
  }

  if (error) {
    return (
      <div>Error: {error instanceof Error ? error.message : String(error)}</div>
    );
  }

  if (!data || data.length === 0) {
    return <div>No bank account information found.</div>;
  }

  const bankAccount = data[0];

  return (
    <div className="mt-11 ml-10">
      <h1 className="text-4xl font-normal">
        Welcome,{" "}
        <span className="font-semibold">{bankAccount.userId || "User"}</span>
      </h1>
      <div className="mt-6">
        <h2 className="text-2xl">Bank Account Details</h2>
        <div className="mt-3">
          <p>
            <strong>Account Type:</strong> {bankAccount.accountType || "N/A"}
          </p>
          <p>
            <strong>Account Plan:</strong> {bankAccount.accountPlan || "N/A"}
          </p>
          <p>
            <strong>IBAN:</strong> {bankAccount.iban || "N/A"}
          </p>
          <p>
            <strong>Balance:</strong>{" "}
            {typeof bankAccount.balance === "number"
              ? bankAccount.balance.toFixed(2)
              : "0.00"}{" "}
            BGN
          </p>
          <p>
            <strong>Cards:</strong> {bankAccount.cards || 0}
          </p>
        </div>
      </div>
    </div>
  );
}

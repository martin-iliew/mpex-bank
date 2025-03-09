import { useQuery } from "react-query";
import {
  fetchBankAccountInfo,
  fetchBankAccountById,
  Account,
  AccountById,
} from "@/api/account";
import { Skeleton } from "@/components/ui/skeleton";
import { CardComponent } from "@/components/Card";
import { useState } from "react";

export default function DashboardPage() {
  const [selectedAccountId, setSelectedAccountId] = useState<string>("");
  const {
    data: accounts,
    isLoading,
    error,
  } = useQuery<Account[] | null>("bankAccountInfo", fetchBankAccountInfo);

  const {
    data: bankAccountDetails,
    isLoading: isDetailsLoading,
    error: detailsError,
  } = useQuery<AccountById | null>(
    ["bankAccountDetails", selectedAccountId],
    () =>
      selectedAccountId
        ? fetchBankAccountById(selectedAccountId)
        : Promise.resolve(null),
    {
      enabled: !!selectedAccountId,
      refetchInterval: 5000, 
      refetchOnWindowFocus: true,
    },
  );

  if (isLoading || isDetailsLoading) {
    return (
      <div className="mt-11 ml-10">
        <Skeleton className="h-10 w-[360px]" />
      </div>
    );
  }

  if (error || detailsError) {
    return (
      <div>
        Error:{" "}
        {error instanceof Error
          ? error.message
          : detailsError instanceof Error
            ? detailsError.message
            : "Unknown error"}
      </div>
    );
  }

  return (
    <div className="mt-11 ml-10">
      <h1 className="text-4xl font-normal">
        Welcome, <span className="font-semibold">Martin Iliev</span>
      </h1>
      <div className="mt-6">
        <h2 className="text-2xl">Select Bank Account</h2>
        <select
          value={selectedAccountId}
          onChange={(e) => setSelectedAccountId(e.target.value)}
          className="mt-2 rounded border p-2"
        >
          <option value="">Select an account</option>
          {accounts?.map((account) => (
            <option key={account.id} value={account.id}>
              {account.accountType} - {account.id}
            </option>
          ))}
        </select>
      </div>
      {bankAccountDetails && (
        <>
          <div className="mt-6">
            <h2 className="text-2xl">Bank Account Details</h2>
            <div className="mt-3">
              <p>
                <strong>Account Id:</strong> {bankAccountDetails.id || "N/A"}
              </p>
              <p>
                <strong>Account Type:</strong>{" "}
                {bankAccountDetails.accountType || "N/A"}
              </p>
              <p>
                <strong>IBAN:</strong> {bankAccountDetails.iban || "N/A"}
              </p>
              <p>
                <strong>Balance:</strong>{" "}
                {typeof bankAccountDetails.balance === "number"
                  ? bankAccountDetails.balance.toFixed(2)
                  : "0.00"}{" "}
                BGN
              </p>
            </div>
          </div>
          <div className="mt-6">
            <h2 className="text-2xl">Cards</h2>
            {bankAccountDetails.cards.length > 0 ? (
              <div className="mt-3 space-y-4">
                {bankAccountDetails.cards.map((card) => (
                  <CardComponent key={card.cardNumber} card={card} />
                ))}
              </div>
            ) : (
              <p>No cards associated with this account.</p>
            )}
          </div>
        </>
      )}
    </div>
  );
}

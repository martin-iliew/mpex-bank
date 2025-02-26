import { useQuery } from "react-query";
import { fetchUserProfile, UserProfile } from "@/api/user";
import { Skeleton } from "@/components/ui/skeleton";

export default function DashboardPage() {
  const { data, isLoading, error } = useQuery<UserProfile>(
    "userProfile",
    fetchUserProfile,
  );

  if (isLoading) {
    return (
      <div className="mt-11 ml-10">
        <Skeleton className="h-10 w-[360px]" />
      </div>
    );
  }
  if (error instanceof Error) {
    return <div>Error: {error.message}</div>;
  }

  const userFirstName = data?.firstName;
  const userLastName = data?.lastName;
  return (
    <div className="mt-11 ml-10">
      <h1 className="text-4xl font-normal">
        Welcome,{" "}
        <span className="font-semibold">
          {userFirstName} {userLastName}
        </span>
      </h1>
    </div>
  );
}

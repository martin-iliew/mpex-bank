import { DisplayLarge } from "@/components/Typography";
import { Button } from "@/components/ui/button";
import { useNavigate } from "react-router-dom";
export default function HomePage() {
  const navigate = useNavigate();
  return (
    <>
      <div className="bg-background container mx-auto">
        <DisplayLarge className="text-heading">Home Page</DisplayLarge>
        <Button onClick={() => navigate("/login")}>Login</Button>
      </div>
    </>
  );
}

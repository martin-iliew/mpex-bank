import {
  Sidebar,
  SidebarHeader,
  SidebarContent,
  SidebarGroup,
  SidebarGroupLabel,
  SidebarGroupContent,
  SidebarMenu,
  SidebarMenuItem,
  SidebarMenuButton,
  SidebarFooter,
} from "@/components/ui/sidebar";
import { Link } from "react-router-dom";
import { Button } from "../ui/button";
import { useAuth } from "@/hooks/useAuth";
import { BodyExtraSmall } from "../Typography";

const SidebarComponent = ({ children }: { children?: React.ReactNode }) => {
  const { logout } = useAuth();
  const handleLogout = () => {
    logout();
  };
  return (
    <Sidebar>
      <SidebarHeader className="flex h-16 justify-center pl-4">
        <img
          src="/Horizontallogo.svg"
          alt="Logo"
          width={60}
          height={32}
          className="h-12 w-44"
        />
      </SidebarHeader>

      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupLabel>
            <BodyExtraSmall>Projects</BodyExtraSmall>
          </SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              <SidebarMenuItem>
                <SidebarMenuButton asChild variant={"outline"}>
                  <Link to="/dashboard">
                    <BodyExtraSmall>Home</BodyExtraSmall>
                  </Link>
                </SidebarMenuButton>
              </SidebarMenuItem>
              <SidebarMenuItem>
                <SidebarMenuButton asChild variant={"outline"}>
                  <Link to="/cards">
                    <BodyExtraSmall>Home</BodyExtraSmall>
                  </Link>
                </SidebarMenuButton>
              </SidebarMenuItem>
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>

      <SidebarFooter>
        <SidebarMenu>
          <SidebarMenuItem>
            <Button size={"lg"} className="w-full" onClick={handleLogout}>
              Log out
            </Button>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarFooter>
      {children}
    </Sidebar>
  );
};

export default SidebarComponent;

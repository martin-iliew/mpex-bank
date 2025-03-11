import SideBar from "@/components/SideBar";
import { SidebarProvider, SidebarInset } from "@/components/ui/sidebar";
export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div>
      <SidebarProvider>
        <SideBar />
        <SidebarInset className="bg-neutral-100">
          <main>{children}</main>
        </SidebarInset>
      </SidebarProvider>
    </div>
  );
}

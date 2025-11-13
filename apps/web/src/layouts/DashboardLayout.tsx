import SideBar from "@/components/layout/SideBar";
import {
  SidebarProvider,
  SidebarInset,
  SidebarTrigger,
} from "@/components/ui/sidebar";
export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div>
      <SidebarProvider className="bg-[#F7F7F7]">
        <SideBar />
        <SidebarInset className="m-3 rounded-2xl border-[#E7E8E7] bg-white">
          <main>
            <SidebarTrigger />
            {children}
          </main>
        </SidebarInset>
      </SidebarProvider>
    </div>
  );
}

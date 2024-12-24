import { Header } from "@/components/Header";

export default function LandingLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="landing-layout">
      <Header />
      <main>{children}</main>
    </div>
  );
}

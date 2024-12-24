import { Link } from "react-router-dom";
import { Button } from "@/components/ui/button";
export default function HomePage() {
  return (
    <main id="content" tabIndex={-1}>
      <section
        className="hero min-h-screen flex items-center justify-center bg-primary-foreground"
        data-sublocation="Hero"
        aria-hidden="false"
      >
        <div className="inner">
          <h1 className="mb-5 text-5xl text-background-text font-semibold uppercase text-center">
            Welcome to Mpex
          </h1>
          <div className="">
            <p className="mb-10 text-lg text-background-subtext text-center ">
              Securing your future today
            </p>
            <div className="flex flex-col justify-center gap-4 sm:flex-row">
              <Link to="/rent">
                <Button className="mt-4" variant={"default"} size={"lg"}>
                  Start Now
                </Button>
              </Link>
              <Link to="/rent">
                <Button className="mt-4" variant={"outlinePrimary"} size={"lg"}>
                  Browse
                </Button>
              </Link>
            </div>
          </div>
        </div>
      </section>
    </main>
  );
}

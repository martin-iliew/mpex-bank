import path from "path";
import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";
import tailwindcss from "@tailwindcss/vite";
import fs from "fs";

export default defineConfig({
  server: {
    https: {
      key: fs.readFileSync(
        "C:\\Users\\Martin\\Desktop\\Projects\\StarterKit\\Certificates\\key.pem",
      ),
      cert: fs.readFileSync(
        "C:\\Users\\Martin\\Desktop\\Projects\\StarterKit\\Certificates\\cert.pem",
      ),
    },
    port: 5173,
    proxy: {
      "/api": {
        target: "https://localhost:5187",
        changeOrigin: true,
        secure: false,
      },
    },
  },
  plugins: [react(), tailwindcss()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
});

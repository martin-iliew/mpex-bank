import path from "path";
import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";
import tailwindcss from "@tailwindcss/vite";
import fs from "fs";
import { visualizer } from "rollup-plugin-visualizer";

export default defineConfig({
  server: {
    https: {
      key: fs.readFileSync(
        "C:\\Users\\Martin\\Desktop\\Resources\\Certificates\\key.pem",
      ),
      cert: fs.readFileSync(
        "C:\\Users\\Martin\\Desktop\\Resources\\Certificates\\cert.pem",
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
  plugins: [
    react(),
    tailwindcss(),
    visualizer({
      open: true,
      filename: "bundle-visualizer.html",
      gzipSize: true,
    }),
  ],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
});

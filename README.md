# 💳 MPEX Bank

MPEX Bank is a **modern banking playground** built as a monorepo.

The goal is to experiment with:

- real-world **authentication & identity**
- a shared **design system & design tokens**
- multiple **frontends** (web + mobile) talking to the same backend

---

## 🧱 Monorepo structure

This repository is managed with **Turborepo** + **pnpm workspaces**.

```text
.
├── apps/
│   ├── web/            # Web app (React + Vite)
│   └── mobile/         # Mobile app (Expo / React Native)
│
├── packages/
│   ├── design-tokens/  # Shared design tokens (colors, typography, spacing, etc.)
│   ├── ui/             # Shared UI components library
│   └── config/         # Shared tooling configs (lint, tsconfig, etc.) [optional/coming soon]
│
├── .github/            # GitHub workflows (CI/CD) [if present]
├── .turbo/             # Turborepo cache folder (local)
├── .gitignore
├── pnpm-workspace.yaml
├── pnpm-lock.yaml
├── package.json
├── turbo.json
└── README.md
```

## 🛠 Tech stack

### Core

- 🟦 **TypeScript**
- 📦 **pnpm workspaces**
- ⚡ **Turborepo** for task orchestration & caching

### Frontend – Web (`apps/web`)

- ⚛️ **React**
- ⚡ **Vite** dev server & bundler
- 🎨 Shared design tokens & UI components from `packages/`

### Frontend – Mobile (`apps/mobile`)

- 📱 **Expo / React Native**
- Shared business logic and design tokens where possible

### Backend

- 🌐 **ASP.NET Web API (MpexWebApi)**
- 🔐 Integrated **Identity** for user management & authentication

### Tooling

- 🧹 **ESLint**, **Prettier** (through shared config package)
- 🧪 (Testing stack to be added / documented later)

---

## 🚀 Getting started

### 1️⃣ Prerequisites

Make sure you have:

- **Node.js** (recommended: LTS 18+)
- **pnpm** (v8+)

### 2️⃣ Install dependencies

From the repository root:

```bash
pnpm install
```

This installs dependencies for all apps and packages in the workspace.

---

### 3️⃣ Environment variables

Each app has its own `.env` (or `.env.local`) file.

Typical examples (adjust to your actual setup):

```bash
# apps/web/.env
VITE_API_URL=http://localhost:5000

# apps/mobile/.env
EXPO_PUBLIC_API_URL=http://localhost:5000

# backend/.env or appsettings.Development.json (if in this repo)
# Connection strings, Identity config, etc.
```

> 🔐 **Important:** Never commit secrets, real API keys or production connection strings.

---

### 4️⃣ Run the development servers

From the repo root:

```bash
pnpm dev
```

This uses Turborepo to run the `dev` script in relevant apps in parallel  
(for example `apps/web` and `apps/mobile`).

Common patterns (depending on how your `package.json` is set up):

```bash
# Run only web
pnpm dev --filter web

# Run only mobile
pnpm dev --filter mobile

# Run backend (if it lives in this repo and has scripts wired)
pnpm dev --filter backend
```

Adjust filter names if your actual package names differ  
(you can check them in each app’s `package.json`).

---

## 🏗 Scripts

In the root `package.json` you’ll typically have something like:

```jsonc
{
  "scripts": {
    "dev": "turbo run dev",
    "build": "turbo run build",
    "lint": "turbo run lint",
    "check-types": "turbo run check-types",
  },
}
```

### Usage

```bash
# Start all dev servers
pnpm dev

# Build all apps & packages
pnpm build

# Lint all packages/apps
pnpm lint

# Run type-checks (if configured)
pnpm check-types
```

Individual apps/packages can also have their own scripts, e.g.:

```bash
pnpm --filter web dev
pnpm --filter mobile build
```

---

## 🧩 Architecture highlights

### Monorepo first

All apps and shared libraries live in a single repo for easier refactoring and reuse.

### Shared design tokens & UI

The `packages/design-tokens` and `packages/ui` packages are the “source of truth”
for visual language across web & mobile.

### Authentication / Identity

The backend (**MpexWebApi**) uses Identity for user accounts and auth flows.  
Frontends consume this via API calls and share auth logic where possible.

### Turborepo orchestration

- Caches builds across apps and packages
- Runs tasks in parallel
- Makes the project scalable as it grows

---

## 🌱 Development guidelines

Some suggested conventions for this repo:

### Commits

Follow a conventional style when possible:

- `feat: ...`
- `fix: ...`
- `refactor: ...`
- `chore: ...`

### Branches

- `main` – stable
- `feature/<name>` – new features
- `fix/<name>` – bug fixes

### PRs

- Keep them focused (one feature / refactor at a time)
- Include screenshots / GIFs for UI changes when possible

---

## 🧭 Roadmap / Ideas

Some potential next steps for **MPEX Bank**:

- Document full API endpoints for the backend
- Add tests (unit & integration) for critical flows
- Improve design system docs and Storybook integration
- Add CI workflow (lint, types, tests) on every PR
- Docker setup for backend + frontends (optional)

---

## 📜 License

Specify your license here (MIT, GPL, proprietary, etc.).

Example:

```text
MIT License – see LICENSE file for details.
```

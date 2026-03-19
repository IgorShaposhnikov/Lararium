# 🏡 Lararium — Digital Home Hub

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Svelte 5](https://img.shields.io/badge/Svelte-5-FF3E00?logo=svelte&logoColor=white)](https://svelte.dev/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)

**Lararium** is a unified, secure, and extensible home server (hub) designed for families or small groups of friends. 

At some point, I got tired of using a dozen disconnected services: Google Photos for snapshots, Telegram for family chats, Excel for tracking expenses, and Trello for shared tasks. But the real catalyst was the growing unreliability of these external platforms. With constant regional network restrictions and unpredictable blocks of popular messengers by local regulators (like RKN), relying on third-party clouds became incredibly frustrating. 

I wanted a system that I fully control, hosted on my own hardware, which stays completely functional and accessible no matter what happens to the global internet. The primary idea is to host Lararium on your local home network (LAN) and access it securely from anywhere via a personal VPN like WireGuard or Tailscale. However, nothing prevents you from deploying it on a standard cloud VPS if you prefer.

*(The name refers to "lararia" — domestic shrines in Ancient Rome where the guardian spirits of the household were kept and honored).*

## ✨ Features (Modules)

The system is built around a core Role-Based Access Control (RBAC) model. The planned modules include:

- 📸 **Media Library:** A single vault for family archives. It extracts EXIF/GPS data and serves videos using HTTP Live Streaming (HLS) for adaptive playback via a custom HTML5 player.
- 📁 **Drive (File Manager):** A personal cloud storage space similar to Google Drive or Yandex Disk. I wanted a place to just drop documents, PDFs, and random archives that don't belong in the photo/video library. Supports folder hierarchies and file sharing.
- 💬 **Chat:** Direct messages and group rooms with file attachments, infinite scroll, and live typing indicators.
- 💰 **Finance:** Collaborative income and expense tracking. Multiple accounts, hierarchical categories, and budget limits.
- ✅ **Productivity:** Kanban boards, shopping lists, calendar events, and reminders.
- 🛡️ **Admin Panel:** Fine-grained access control. You can make certain photo albums public while keeping financial reports completely hidden.

### 🔐 A Note on Registration
Because Lararium is meant to be a closed, private space, you won't find a standard open registration page out of the box. On the very first launch, you'll be prompted to create the primary `superadmin` account. Through this account, you can manually set up profiles for the rest of the household. 

If you prefer, you can flip a switch in the settings to allow people to register themselves. However, to keep random users out, any newly registered account will just sit in a waiting list until the `superadmin` hits approve.


## 🛠 Tech Stack & Architecture

I'm building this using a strict **Modular Monolith** architecture. The backend is cleanly sliced into independent feature modules (`Video`, `Media`, `Authorization`). Each module handles its own dependency injection and controllers via a custom `IModuleInitializer` system, keeping the codebase clean, highly cohesive, and easy to maintain.

**Backend:**
* **C# / ASP.NET Core (.NET 10)** — Modular Monolith design.
* **PostgreSQL + EF Core** — Primary relational database using a unified `AppDbContext` and generic `IDataStore` repository patterns. Configured with automated snake_case naming conventions.
* **Garnet (Redis)** — Microsoft's ultra-fast cache store. Used for distributed caching and session state.
* **FFmpeg (via FFMpegCore)** — Instead of basic progressive download, the backend implements an `HlsEncoder` that processes uploaded videos into HTTP Live Streaming (HLS) formats (`.m3u8` playlists with `.ts` segments) for smooth adaptive playback in the browser.
* **Advanced API Design:** Fully versioned REST API (v1.0, v2.0) with Swagger integration.

**Real-Time Communication:**
I split the real-time protocols to keep the server footprint minimal:
* **Server-Sent Events (SSE):** Used for unidirectional, lightweight server-to-client updates (global notifications, dashboard widgets, and WebRTC signaling).
* **SignalR (WebSockets):** Reserved strictly for bi-directional, high-frequency communication like the Chat module.

**Frontend:**
* **Svelte 5 (with runes 🪄)** — Built as a pure Client-Side Rendered (CSR) SPA application using `@sveltejs/adapter-static`.
* **Tailwind CSS + Bits UI** — For fast, accessible components.

**Infrastructure & Deployment:**
* Docker / Docker Compose.
* S3-compatible storage (MinIO) or local file system via `VideoS3Options`.

## 🚀 Quick Start (Docker)

The easiest way to spin up the project locally is via `docker-compose`.

1. Clone the repository:
   ```bash
   git clone https://github.com/IgorShaposhnikov/lararium.git
   cd lararium
   ```
2. Copy the environment variables file and tweak it:
   ```bash
   cp .env.example .env
   ```
3. Start the containers:
   ```bash
   docker-compose up -d
   ```
4. Open `http://localhost:3000` in your browser.

## 🗺 Roadmap

> ⚠️ **Note:** This project is currently in the early stages of development. I'm building it in my free time.

I've already laid down the core backend architecture, and right now I'm piecing together the Svelte frontend. Here is the current progress:

### Phase 1: Core Backend MVP
- [x] Project architecture setup (Modular Monolith, ASP.NET Core, DI).
- [x] Database schema & EF Core migrations (PostgreSQL).
- [x] **Auth API:** User registration, login, basic RBAC, and **Garnet** integration for sessions.
- [x] **Video API:** Uploading, metadata extraction, and **HLS** encoding via FFmpeg.
- [ ] **Background Jobs:** Initial **Hangfire** setup for async tasks.
- [ ] **Photo API:** Image uploading, thumbnail generation, and EXIF extraction.
- [ ] **Admin API:** Basic CRUD for user management.

### Phase 2: Frontend Foundation & MVP Integration (Current Status: In Progress 🚧)
- [x] Initialize Svelte 5 SPA (CSR + adapter-static) with Tailwind and Bits UI.
- [x] Setup state management and REST API client.
- [x] **Auth UI:** Login, Registration, and session handling.
- [ ] Build the main app shell (Sidebar, Navigation, Theme switcher).
- [ ] **Media UI:** Video upload interface and custom HTML5 HLS player integration.
- [ ] Fully dockerize the MVP via `docker-compose`.

### Phase 3: Communication & Real-time
- [ ] Backend: Configure **SSE** for lightweight unidirectional data.
- [ ] Backend: Configure **SignalR** Hubs for bi-directional chat events.
- [ ] Chat Module: Direct 1-on-1 messaging API & UI.
- [ ] Chat Module: Group rooms, roles, and message moderation.
- [ ] Chat Module: Typing indicators and file attachments.
- [ ] Global in-app notification system (via SSE).

### Phase 4: Productivity & Storage
- [ ] **Drive (File Manager):** Folder hierarchies, drag-and-drop uploads, and file sharing UI.
- [ ] **Finance Tracker:** APIs and UI for accounts and transactions.
- [ ] **Task Manager (ToDo):** Kanban boards and deadlines.
- [ ] **Calendar:** Shared events and reminders.
- [ ] **Dashboard:** Customizable widget grid for the home page.

### Phase 5: Polish & Advanced Features
- [ ] Full S3/MinIO integration for scalable media/file storage.
- [ ] Advanced RBAC UI (fine-grained permissions).
- [ ] Cross-module integrations (e.g., linking a photo receipt to an expense).
- [ ] Global search across all modules.

### Phase 6: WebRTC & Advanced Communication 📞
- [ ] Infrastructure: Setup STUN/TURN servers for NAT traversal (e.g., Coturn).
- [ ] Backend: Build a lightweight WebRTC signaling server using REST API + **SSE**.
- [ ] **Video & Audio Calls:** P2P real-time communication inside the browser.

## 🤝 Contributing

Pull Requests are welcome! If you find a bug or have an idea for a new module, feel free to open an Issue. Since I'm developing this mostly solo right now, any help is appreciated.

Before writing code, please check out the [Frontend Architecture](docs/FRONTEND.md) and [Backend API Structure](docs/BACKEND.md) docs.

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
```

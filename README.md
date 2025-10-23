# 🧩 Project Management Platform

A scalable, cloud-native **Project Management Platform** built with **.NET 8**, **Angular 17**, and **Azure**, designed for teams and enterprises to collaborate through projects, boards, and tasks.

---

## 🚀 Key Features

- ✅ Task, Board & Project Management  
- 🔐 Authentication (JWT + Google OAuth 2.0)  
- 📨 Microservices communication via RabbitMQ (MassTransit)  
- 🧠 Caching with Redis  
- ⚙️ Containerized & deployable on Azure Kubernetes Service (AKS)  
- 📦 CI/CD pipelines using Azure DevOps  

---

## 🏗️ Architecture Overview

flowchart TD
    A[Frontend<br>Angular App] --> B[YARP API Gateway<br>.NET 8]
    B --> C1[Auth Service<br>JWT + Google OAuth]
    B --> C2[Board Service<br>Project Logic]
    B --> C3[Task Service<br>Workflow Management]
    B --> C4[Notification Service<br>SignalR, Emails]
    C1 & C2 & C3 & C4 --> D[RabbitMQ Event Bus<br>(Pub/Sub)]

---

## 🧱 Tech Stack

| Layer | Technology |
|-------|-------------|
| Frontend | Angular 17, PrimeNG |
| Backend | .NET 8 Web APIs |
| Gateway | YARP |
| Messaging | RabbitMQ + MassTransit |
| Database | SQL Server |
| Caching | Redis |
| Hosting | Azure AKS |
| CI/CD | Azure Pipelines |
| Auth | JWT + Google OAuth 2.0 |

---

## 🧰 Quick Start

### Prerequisites
- Node.js 20+  
- .NET 8 SDK  
- Docker Desktop  
- RabbitMQ & Redis  

### Local Setup
```bash
# Clone the repo
git clone https://github.com/yourusername/project-management-platform.git
cd project-management-platform

# Run services
docker-compose up --build

# Run frontend
cd frontend
npm install
ng serve


☁️ Deployment

Docker images pushed via Azure DevOps

Ingress NGINX routes external traffic to YARP Gateway

AKS manifests deployed via pipeline stages (Dev → QA → Prod)

📄 Documentation

Full technical documentation (architecture, CI/CD, API endpoints, and environments):

📘 /docs/README_DETAILED.md
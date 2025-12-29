# CleanFoodViet Backend API

![.NET](https://img.shields.io/badge/.NET-9-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-9-512BD4?logo=dotnet&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?logo=mysql&logoColor=white)
![Azure](https://img.shields.io/badge/Azure-0078D4?logo=microsoftazure&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)

**A production-ready REST API backend** for the CleanFoodViet platform, built with modern .NET 9 and ASP.NET Core. This system powers a marketplace connecting local gardeners directly with retailers of clean, sustainable food.

## ✨ Features

- **🧭 Advanced Geospatial Search**: Find gardeners and products within a specific geographic radius using latitude/longitude coordinates, integrated with Geocoding APIs. (Currently disabled)
- **⚙️ Complete Marketplace Core**: Manage the full lifecycle of Orders, Appointments, Products, and User Posts in a multi-role system.
- **📊 Data Insights for Gardeners**: Automated statistics engine providing yearly order analytics and subscription quota tracking.
- **☁️ Cloud-Native & DevOps Ready**: Fully configured for deployment on Microsoft Azure with CI/CD pipelines using GitHub Actions and Docker containerization.
- **🔒 Security & Validation**: Robust input validation, constraint management, and secure database connection handling.

## 🏗️ System Architecture

This project follows a **N-Layer Architecture** pattern, separating concerns into distinct layers for maintainability and testability.
CleanFoodVietBE/
├── FoodVietAPI.Presentation/ # API Controllers, DTOs, HTTP layer
├── FoodVietAPI.Application/ # Business logic, services, use cases
├── FoodVietAPI.Data/ # Data access, Entity Framework, Migrations
├── CleanFoodVietAPI.Functions/ # Azure Functions for background jobs
├── .github/workflows/ # CI/CD pipeline definitions (GitHub Actions)
├── Dockerfile # Container configuration
└── CleanFoodVietAPI.sln # Solution file


## 📦 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MySQL Server](https://dev.mysql.com/downloads/) (or a compatible database)
- [Git](https://git-scm.com/)
- (Optional) [Docker](https://www.docker.com/get-started) and [Docker Compose](https://docs.docker.com/compose/install/)

### Installation & Local Run

1.  **Clone the repository**
    ```bash
    git clone https://github.com/CleanFoodViet/CleanFoodVietBE.git
    cd CleanFoodVietBE
    ```

2.  **Configure the application settings**
    - Navigate to the `FoodVietAPI.Presentation` project.
    - Update the `appsettings.Development.json` file with your local database connection string and any required API keys (e.g., Geocoding service).

3.  **Apply database migrations**
    Ensure your MySQL server is running, then from the solution root:
    ```bash
    dotnet ef database update --project FoodVietAPI.Data --startup-project FoodVietAPI.Presentation
    ```

4.  **Run the application**
    ```bash
    dotnet run --project FoodVietAPI.Presentation
    ```
    The API will start, typically at `https://localhost:5001` and `http://localhost:5000`. An interactive Swagger/OpenAPI documentation page will be available at `/swagger`.

## 🤝 Contributions & Team

This project was developed collaboratively. Major contributions include:

- **Huynh Nguyen Thai Duong ([@hntduong12345](https://github.com/hntduong12345))**:
    - Designed and implemented the **core system architecture** (Order, Appointment, Product/Post systems).
    - Developed the **geospatial search engine** with radius filtering.
    - Refactored data mapping and pagination for **high-performance API responses**.
    - Managed database schema migrations and **cloud deployment** configuration.

- **Urapacito ([@Urapacito](https://github.com/Urapacito))**:
    - Architected and implemented the complete Stripe payment gateway, including webhooks and currency (VND) handling.
    - Developed the core subscription service system with contract management, expiry logic, and admin endpoints.
    - Built the review & rating system for products, including validation and API endpoints.
    - Enhanced service packages and features with improved DTOs, error handling, and business logic.

We welcome issues and constructive feedback. Please feel free to **fork** the repository and submit **Pull Requests**.

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details (you may need to create this file).

---
*If this API powers your connection to fresh, local food, give it a ⭐ on GitHub!*

# 🍽️ OrderWebsiteASP

> A food ordering web application where users can browse restaurants, explore menus, and place orders — with a full admin panel for managing content.

![.NET Version](https://img.shields.io/badge/.NET-6.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-blue)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Features](#features)
- [Usage](#usage)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## 📖 About the Project

OrderWebsiteASP is a restaurant ordering platform built with ASP.NET Core MVC. Users can browse restaurants, view food menus with images and prices, explore active promotions, and place orders. The application includes a role-based admin panel that allows administrators to manage all content through a clean CRUD interface.

---

## 🛠️ Technologies Used

| Technology            | Version  | Purpose                          |
|-----------------------|----------|----------------------------------|
| ASP.NET Core MVC      | 6.0+     | Web framework                    |
| Entity Framework Core | 6.0+     | ORM / Database access            |
| SQL Server            | -        | Database                         |
| ASP.NET Core Identity | -        | Authentication & authorization   |
| Bootstrap             | 5.1      | Frontend styling                 |
| Razor Views           | -        | Server-side HTML rendering       |

---

## ✅ Prerequisites

Make sure you have the following installed before running the project:

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or SQL Server LocalDB
- [Git](https://git-scm.com/)

---

## 🚀 Getting Started

Follow these steps to get the project running locally.

### 1. Clone the repository

```bash
git clone https://github.com/JustMargata/OrderWebsiteASP.git
cd OrderWebsiteASP
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Apply database migrations

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run --project OrderWebsiteASP
```

The app will be available at `https://localhost:5001` or `http://localhost:5000`.

---

## 📁 Project Structure

```
OrderWebsiteASP/
│
├── OrderWebsiteASP/                   # Main web application
│   ├── Controllers/                   # MVC Controllers (Home, FoodItems, Promotions, ...)
│   ├── Views/                         # Razor Views (.cshtml)
│   │   ├── FoodItems/
│   │   ├── Orders/
│   │   ├── Promotions/
│   │   ├── Restaurants/
│   │   └── Shared/                    # Layout, login partial, error page
│   ├── wwwroot/                       # Static files (CSS, JS, Bootstrap)
│   ├── appsettings.json               # App configuration
│   └── Program.cs                     # App entry point and middleware setup
│
├── OrderWebsiteASP.Data/              # DbContext and migrations
├── OrderWebsiteASP.Data.Models/       # Domain / entity models
├── OrderWebsiteASP.Services.Core/     # Business logic / service layer
│   └── Contracts/                     # Service interfaces
├── OrderWebsiteASP.ViewModels/        # ViewModels for type-safe views
└── OrderWebsiteASP.GCommon/           # Shared validation constants
```

---

## ✨ Features

- [x] User registration and login (ASP.NET Core Identity)
- [x] Role-based access control (Admin / User)
- [x] Browse restaurants with images and addresses
- [x] View restaurant menus with food items, prices, and images
- [x] Active promotions with discount percentages and validity dates
- [x] Full CRUD for restaurants, food items, promotions, and orders (Admin only)
- [x] Input validation (server-side with anti-forgery tokens)
- [x] Responsive UI with Bootstrap 5
- [x] Service layer architecture with interface contracts
- [x] Dedicated ViewModels for each action (Create, Edit, Delete)

---

## 💻 Usage

After launching the app:

```
1. Navigate to /Register to create an account.
2. Log in at /Login.
3. Browse restaurants on the home page.
4. Click a restaurant to see its full menu.
5. Visit /Promotions to explore active deals.
```

**Admin access** — assign the `Admin` role to a user to unlock:

```
/Restaurants/Create                        → Add a new restaurant
/FoodItems/Create?restaurantId={id}        → Add food items to a restaurant
/Promotions/Create                         → Create a promotional campaign
```

> 💡 **Tip:** You can seed an admin user directly in the database or add a seeder in `Program.cs` on first run.

---

## 🗄️ Database Setup

The project uses **Entity Framework Core** with a Code-First approach.

Connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OrderWebsiteASP;Trusted_Connection=True;"
}
```

To create and apply the database:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

The migrations include both the ASP.NET Core Identity schema and the application tables (Restaurants, FoodItems, Promotions, Orders).

---

## ⚙️ Configuration

Key settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string-here"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

> ⚠️ **Never commit sensitive data** (passwords, API keys) to source control. Use `appsettings.Development.json` or environment variables for local secrets.

---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a new branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "Add some feature"`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## 📬 Contact

**JustMargata** – [@JustMargata](https://github.com/JustMargata)

Project Link: [https://github.com/JustMargata/OrderWebsiteASP](https://github.com/JustMargata/OrderWebsiteASP)

---

*Built as part of the **ASP.NET Fundamentals** course.*

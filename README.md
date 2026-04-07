# 🍽️ OrderWebsiteASP

> A food ordering web application where users can browse restaurants, explore menus, place orders, and discover promotions — with an admin panel for managing application content.

![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-MVC-blue)
![Database](https://img.shields.io/badge/Database-SQL_Server-red)
![Tests](https://img.shields.io/badge/Tests-xUnit-green)

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Technologies Used](#technologies-used)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Features](#features)
- [Validation and Security](#validation-and-security)
- [Pagination, Search and Filtering](#pagination-search-and-filtering)
- [Error Handling](#error-handling)
- [Data Seeding](#data-seeding)
- [Testing](#testing)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Usage](#usage)
- [Future Improvements](#future-improvements)
- [License](#license)
- [Contact](#contact)

---

## 📖 About the Project

OrderWebsiteASP is a restaurant ordering platform built with ASP.NET Core MVC.  
Users can browse restaurants, view food menus with images and prices, explore active promotions, and place orders.  
The application includes role-based functionality, where regular users can manage their own orders, while administrators can manage restaurants, food items, and promotions through protected CRUD operations.

The project was developed as part of an ASP.NET Advanced course and demonstrates the use of ASP.NET Core MVC, Razor views, Entity Framework Core, Identity, service-layer architecture, validation, pagination, filtering, error handling, and unit testing.

---

## 🛠️ Technologies Used

| Technology | Version | Purpose |
|-----------|---------|---------|
| ASP.NET Core MVC | 8.0 | Web framework |
| Entity Framework Core | 8.0 | ORM / database access |
| SQL Server | - | Main relational database |
| ASP.NET Core Identity | - | Authentication and authorization |
| Razor Views | - | Server-side HTML rendering |
| Bootstrap | 5.x | Responsive frontend styling |
| xUnit | - | Unit testing |
| EF Core InMemory | - | Testing database provider |

---

## 🏗️ Architecture

The project follows a **layered architecture** in order to keep responsibilities separated and the code easier to maintain and test.

### Application Layers

- **Presentation Layer**  
  Contains MVC controllers, Razor views, user interaction logic, and routing.

- **Service Layer**  
  Contains the core business logic of the application, including restaurant pagination and search, promotion filtering, order/cart logic, and protected CRUD operations.

- **Data Layer**  
  Contains Entity Framework Core configuration, DbContext, migrations, and persistence logic.

- **ViewModel Layer**  
  Contains dedicated models used for form binding, validation, and rendering views.

### Design Decisions

- MVC was used to keep controllers, views, and business logic clearly separated.
- A service layer was introduced to keep controllers lightweight and focused on request handling.
- Dedicated ViewModels were used for Create/Edit/Delete operations to keep UI binding separate from entity models.
- Pagination was added to restaurant and promotion listings to improve performance and usability.
- Search and filtering were added to make the application easier to navigate.
- Ownership validation was added to order operations so that users can only access and modify their own orders.
- Role-based authorization was used to protect administrative actions.

---

## 📁 Project Structure

```text
OrderWebsiteASP/
│
├── OrderWebsiteASP/                   # Main web application
│   ├── Controllers/                   # MVC Controllers
│   ├── Views/                         # Razor Views (.cshtml)
│   │   ├── FoodItems/
│   │   ├── Orders/
│   │   ├── Promotions/
│   │   ├── Restaurants/
│   │   └── Shared/
│   ├── wwwroot/                       # Static files (CSS, JS, images)
│   ├── appsettings.json               # Application configuration
│   └── Program.cs                     # App entry point and middleware
│
├── OrderWebsiteASP.Data/              # DbContext and migrations
├── OrderWebsiteASP.Data.Models/       # Entity models
├── OrderWebsiteASP.Services.Core/     # Business logic / services
│   └── Contracts/                     # Service interfaces
├── OrderWebsiteASP.ViewModels/        # ViewModels for form binding and UI
├── OrderWebsiteASP.GCommon/           # Shared constants and validation helpers
└── OrderWebsiteASP.Tests/             # Unit tests
```

---

## ✨ Features

- [x] User registration and login with ASP.NET Core Identity
- [x] Role-based access control (Admin / User)
- [x] Browse restaurants with images and addresses
- [x] View restaurant menus with food items, prices, and images
- [x] Browse active promotions with discount percentages and validity dates
- [x] Add items to cart/order
- [x] Increase, decrease, and remove order items
- [x] View personal orders
- [x] Cancel personal orders
- [x] Full CRUD for restaurants, food items, and promotions (Admin only)
- [x] Search restaurants by name and address
- [x] Filter promotions by restaurant and minimum discount
- [x] Pagination for restaurant and promotion listings
- [x] Custom 404 and 500 error pages
- [x] Service layer architecture with interface contracts
- [x] Dedicated ViewModels for Create, Edit, and Delete actions
- [x] Unit tests for service logic

---

## 🔒 Validation and Security

### Validation
The project uses both **server-side** and **client-side** validation.

#### Server-Side Validation
- Data annotations on models and view models
- `ModelState` validation in controller actions
- null checks before using entities
- safe ID validation in controller actions
- existence checks before edit and delete operations

#### Client-Side Validation
- Razor validation helpers
- validation scripts partial for form validation

### Security
The application includes the following security measures:

- ASP.NET Core Identity for authentication
- role-based authorization for administrative functionality
- anti-forgery token validation for POST actions
- ownership validation for order operations
- safe ID checks to avoid invalid or manipulated requests
- Entity Framework Core LINQ queries to reduce SQL injection risk
- separation between public actions and admin-only actions

### Roles
The application supports at least the following roles:
- **Admin**
- **User**

---

## 🔎 Pagination, Search and Filtering

### Pagination
Pagination is implemented for:
- restaurant listing
- promotion listing

This prevents loading too many records on a single page and improves usability.

### Restaurant Search
Users can search restaurants by:
- restaurant name
- restaurant address

### Promotion Filtering
Users can filter promotions by:
- restaurant
- minimum discount percentage

These features improve navigation and make the application easier to use.

---

## ⚠️ Error Handling

The application includes:
- global exception handling
- custom **404 Not Found** page
- custom **500 Internal Server Error** page
- shared error page

This improves reliability and provides a better user experience when something goes wrong.

---

## 🌱 Data Seeding

The database is seeded with initial data so the application can be used immediately after setup.

Seeded data includes:
- sample restaurants
- sample food items
- sample promotions
- predefined roles
- default administrator/user accounts if configured in startup logic

This makes the project easier to test and demonstrate after initial setup.

---

## 🧪 Testing

The project contains a dedicated test project:

- **OrderWebsiteASP.Tests**

Unit tests are written for the **service layer** and cover:
- restaurant pagination
- restaurant search
- promotion filtering
- active and expired promotion logic
- order ownership validation
- order/cart edge cases

### Testing Tools
- xUnit
- EF Core InMemory Provider

### Running Tests

#### Through Visual Studio
- Open **Test Explorer**
- Click **Run All Tests**

#### Through command line
```bash
dotnet test
```

---

## ✅ Prerequisites

Make sure you have the following installed before running the project:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or SQL Server LocalDB
- [Git](https://git-scm.com/)

---

## 🚀 Getting Started

Follow these steps to run the project locally.

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

The application will be available locally after startup.

---

## 🗄️ Database Setup

The project uses **Entity Framework Core** with a **Code-First** approach.

The connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OrderWebsiteASP;Trusted_Connection=True;"
}
```

To create and update the database:

```bash
dotnet ef database update
```

The database includes:
- ASP.NET Core Identity tables
- Restaurants
- FoodItems
- Promotions
- Orders
- OrderItems

---

## ⚙️ Configuration

Key settings are stored in `appsettings.json`.

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

> Do not commit sensitive data such as passwords, API keys, or private secrets to source control. Use development configuration or environment variables when needed.

---

## 💻 Usage

After launching the app:

1. Register a new account
2. Log in with your account
3. Browse restaurants
4. Open restaurant details to view food items
5. Visit the promotions page to explore active offers
6. Add items to your order
7. Manage your order through the order pages

### Admin Usage
Users with the **Admin** role can additionally:
- create restaurants
- edit restaurants
- delete restaurants
- create food items
- edit food items
- delete food items
- create promotions
- edit promotions
- delete promotions

---

## 🔮 Future Improvements

Possible future improvements include:
- order status tracking
- payment integration
- improved admin dashboard
- richer filtering and sorting
- better cart workflow
- more unit and integration tests

---

## 📄 License

This project is licensed under the **MIT License**.

---

## 📬 Contact

**JustMargata**  
GitHub: [https://github.com/JustMargata](https://github.com/JustMargata)

Project Link: [https://github.com/JustMargata/OrderWebsiteASP](https://github.com/JustMargata/OrderWebsiteASP)

---

*Built as part of the ASP.NET Advanced course.*

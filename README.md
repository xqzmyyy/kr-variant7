
---

# Poultry Farm Management System

Welcome to the **Poultry Farm Management System** project! This is a console application for managing a poultry farm system. It allows users to work with information about employees, chickens, and cages. The project is built using **.NET 8.0**, **Entity Framework Core**, and **SQLite**.

---

## 🔧 Prerequisites

Ensure you have the following installed on your system:

- **.NET 8.0 SDK**  
- **Git**

---

## 🚀 Getting Started

Follow these steps to set up and run the project locally:

### 1. Clone the Repository

```bash
git clone git@github.com:xqzmyyy/poultry-farm-management-system.git
cd poultry-farm-management-system
```

### 2. Restore Dependencies

Restore all project dependencies:

```bash
cd poultry-farm-management-system
dotnet restore
```

### 3. Install Required Libraries

Make sure all necessary libraries are installed:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package System.Data.SQLite
```

For testing dependencies:

```bash
cd ../Tests
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
```

### 4. Create Database Migrations

If this is the first setup or after any model changes, create migrations:

```bash
cd poultry-farm-management-system
dotnet ef migrations add InitialCreate
```

### 5. Apply Migrations to Create Database

To create and apply the database schema, run:

```bash
dotnet ef database update
```

### 6. Build the Project

Build the project to ensure all dependencies and configurations are correct:

```bash
dotnet build
```

### 7. Run the Application

To start the application, use:

```bash
dotnet run
```

---

## 📋 Database Management

The project uses **Entity Framework Core** and **SQLite** for database management.

- **Create a new migration**:
```bash
dotnet ef migrations add <MigrationName>
```

- **Apply migrations**:
```bash
dotnet ef database update
```

- **Recreate the database**:
```bash
rm -rf poultry-farm-management-system/Data/database.db
dotnet ef database update
```

---

## 🧪 Running Tests

To run unit tests, navigate to the `Tests` directory:

```bash
cd Tests
dotnet test
```

Test results and coverage will be displayed in the console.

---

## 📦 **Database Models**

### 1. **Chicken** (Model for chickens)
Stores information about each chicken on the poultry farm.

#### Fields:
- **Id** (int): Unique identifier for the chicken.
- **Weight** (double): Weight of the chicken.
- **Age** (int): Age of the chicken in months.
- **EggsPerMonth** (int): Number of eggs laid by the chicken per month.
- **Breed** (string): Breed of the chicken.
- **CageId** (int): Reference to the cage where the chicken is located.
- **Cage** (Cage): Navigation property linking the chicken to its cage.

---

### 2. **Employee** (Model for employees)
Stores data about employees on the poultry farm.

#### Fields:
- **Id** (int): Unique identifier for the employee.
- **Name** (string): Name of the employee.
- **Salary** (decimal): Salary of the employee.
- **Cages** (List<Cage>): List of cages assigned to the employee.

---

### 3. **Cage** (Model for cages)
Stores information about cages where chickens are kept.

#### Fields:
- **Id** (int): Unique identifier for the cage.
- **EmployeeId** (int): Reference to the employee responsible for the cage.
- **ChickenId** (int?): Reference to the chicken currently in the cage (can be null).
- **Employee** (Employee): Navigation property linking the cage to its employee.
- **Chicken** (Chicken): Navigation property linking the cage to its chicken.
- **Date** (DateTime): The date when the cage was last used.
- **IsEggLaid** (bool): Indicates whether an egg was laid.

---

## 🔨 **Main Methods**

### Chicken Methods
- **AddChicken**: Adds a new chicken to the database.
- **DeleteChicken**: Removes a chicken from the database by its ID.
- **ListAllChickens**: Displays all chickens with their details.
- **CalculateAverageEggs**: Calculates the average number of eggs based on weight and age.
- **CageWithTopChicken**: Finds the cage with the most productive chicken.

---

### Employee Methods
- **ListAllEmployees**: Retrieves all employees and their details.
- **EggsCollectedByEmployees**: Shows the number of eggs collected by each employee.

---

### Cage Methods
- **ListAllCages**: Retrieves all cages with their associated chickens and employees.

---

## 🧪 **Testing Methods**
Tests for all main functionalities are implemented in the `Tests` directory using xUnit:

### ChickenTests
- Validates adding, deleting, and calculating chicken statistics.

### EmployeeTests
- Validates assigning cages to employees.

### CageTests
- Validates cage assignment and chicken-cage relationships.

---

## 📄 License

This project is licensed under the MIT License.

---

## 🛡️ Maintainers

- **kk**

Developed by the student as a coursework.

---
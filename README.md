
---

# Poultry Farm Management System

Welcome to the **Poultry Farm Management System** project! This is a console application for managing a poultry farm system. It allows users to work with information about employees, chickens, and cages. The project is built using **.NET 8.0**, **Entity Framework Core**, and **SQLite**.

---

## 🔧 Prerequisites

Ensure you have the following installed on your system:

- .NET 8.0 SDK  
- Git  

---

## 🚀 Getting Started

Follow these steps to set up and run the project locally:

### 1. Clone the Repository

```
git clone git@github.com:xqzmyyy/poultry-farm-management-system.git  
cd poultry-farm-management-system
```

### 2. Restore Dependencies

Make sure you have `.NET 8.0 SDK` installed, then restore all dependencies:

```
dotnet restore
```

### 3. Create Migrations (if needed)

If you make changes to the models or are setting up for the first time:

```
dotnet ef migrations add InitialCreate
```

### 4. Apply Migrations

To apply the migrations and create the database:

```
dotnet ef database update
```

### 5. Build the Project

Build the project to ensure all dependencies and configurations are correct:

```
dotnet build
``` 

### 6. Run the Application

To start the application, run:

```
dotnet run
``` 

---

## 📋 Database Management

The project uses **Entity Framework Core** and **SQLite** for database management. Here are some common operations:

- **Create a new migration**:
```  
dotnet ef migrations add <MigrationName> 
```

- **Apply migrations to update the database**:
```
dotnet ef database update
```

- **Recreate the database (optional)**:
```
rm kr-v7/Data/database.db  
dotnet ef database update  
```

---

## 🧪 Running Tests

To open the folder containing the test files:

```
cd Tests/
```

The project includes unit tests for all core functionalities. To run the tests:

```
dotnet test
```

Test files are located in the `Tests/` directory.

---

## 📦 **Database Models**

### 1. **Chicken** (Model for chickens)
Stores information about each chicken on the poultry farm.

#### Fields:
- **Id** (int): Unique identifier for the chicken.
- **Weight** (double): Weight of the chicken.
- **Age** (int): Age of the chicken in months.
- **EggsPerMonth** (int): Number of eggs laid by the chicken per month.
- **CageId** (int): Reference to the cage where the chicken is located.

---

### 2. **Employee** (Model for employees)
Stores data about employees on the poultry farm.

#### Fields:
- **Id** (int): Unique identifier for the employee.
- **Name** (string): Name of the employee.
- **Salary** (double): Salary of the employee.
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

---

## 🔨 **Main Methods**

### 1. **Chicken Methods**
- **AddChicken**: Adds a new chicken to the database.
- **DeleteChicken**: Removes a chicken from the database by its ID.
- **GetChickenWithMostEggs**: Retrieves the chicken that lays the most eggs.
- **CalculateAverageEggs**: Calculates the average number of eggs for chickens based on weight and age.

### 2. **Employee Methods**
- **GetEmployeeChickenCount**: Returns the number of chickens assigned to an employee.
- **ListAllEmployees**: Retrieves all employees and their details.

### 3. **Cage Methods**
- **AssignChickenToCage**: Assigns a chicken to a cage.
- **GetAllCages**: Retrieves all cages, including their assigned chickens and employees.

---

## 🧪 **Testing Methods**
Tests for all main functionalities are implemented in the `Tests` directory using xUnit:
- **ChickenTests**: Validates adding, deleting, and calculating chicken statistics.
- **EmployeeTests**: Validates assigning cages to employees.
- **CageTests**: Validates cage assignment and chicken-cage relationships.

---

## 📄 License

This project is licensed under the MIT License.

---

## 🛡️ Maintainers

- **kk**

Developed by the student as a coursework.
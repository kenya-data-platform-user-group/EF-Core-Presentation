# EF Core: Simplifying Database Interactions in .NET

## Table of Contents
1. [Why EF Core?](#why-ef-core)
2. [What is EF Core?](#what-is-ef-core)
3. [Why Use EF Core?](#why-use-ef-core)
4. [New Features in EF Core (.NET 9)](#new-features-in-ef-core-net-9)
5. [Setting Up EF Core](#setting-up-ef-core)
6. [Defining Models and Relationships](#defining-models-and-relationships)
7. [Data Annotations vs Fluent API](#data-annotations-vs-fluent-api)
8. [Migrations and Database Operations](#migrations-and-database-operations)
9. [Basic CRUD Operations](#basic-crud-operations)
10. [Performance and Best Practices](#performance-and-best-practices)
11. [Handling Concurrency in EF Core](#handling-concurrency-in-ef-core)
12. [Closing](#closing)

---

# Why EF Core?

## Quick Story/Example
Imagine you're building a .NET application and need to interact with a database. How do you do it efficiently? You could write raw SQL queries, but that can be error-prone and hard to maintain. This is where an ORM (Object-Relational Mapper) like EF Core comes in handy.

## How Do We Interact with Databases in .NET?
Before Entity Framework Core (EF Core), developers interacted with databases using **ADO.NET**, **Dapper**, or raw SQL queries. While these approaches provided control and performance, they often required writing a lot of boilerplate code for CRUD operations.

- Open a database connection.
- Write SQL queries manually.
- Handle result mappings to objects.
- Manage transactions and exceptions explicitly.

This process is repetitive and error-prone. This is where **EF Core** comes in.

---

# What is EF Core?
EF Core is a modern, open-source, and cross-platform **Object-Relational Mapper (ORM)** for .NET that eliminates the need to write complex SQL queries manually. It allows developers to work with databases using **C# classes and LINQ** instead of SQL.

EF Core acts as a bridge between **.NET applications** and **databases**, allowing developers to perform operations using object-oriented techniques.

---

# Why Use EF Core?

### 🚀 **Simplifies Data Access**
EF Core abstracts database interactions, allowing developers to use **C# objects** instead of SQL queries.

### ⚡ **Boosts Productivity**
- Eliminates the need to write repetitive SQL queries.
- Supports **automatic migrations** to handle database schema changes.
- Works seamlessly with **LINQ queries** for data retrieval.

### 🔄 **Supports Multiple Databases**
EF Core is database-agnostic and supports multiple database providers, including:
- **SQL Server**
- **PostgreSQL**
- **MySQL**
- **SQLite**
- **Azure Cosmos DB**

---

# New Features in EF Core (.NET 9)

## 2️⃣ Enhanced Raw SQL Queries
- Enables safer and more efficient execution of raw SQL statements.
- Improves mapping results directly to entity models.

### Example: Executing Raw SQL in EF Core 9
```csharp
var users = await context.Users
    .FromSql($"SELECT * FROM Users WHERE IsActive = 1")
    .ToListAsync();
```

## Better LINQ Translation
- Supports more complex expressions and nested queries.
- Reduces unnecessary SQL statements for better performance.

### Example: Improved LINQ Translation
```csharp
var highValueOrders = await context.Orders
    .Where(o => o.TotalAmount > 100)
    .Select(o => new { o.Id, o.TotalAmount })
    .ToListAsync();
```

---

# Setting Up EF Core

## Installing EF Core
To install EF Core in your .NET project, run the following command in your terminal:
```bash
dotnet add package Microsoft.EntityFrameworkCore
```

---

# Defining Models and Relationships

## Simple Model Example: User and Order
In this example, we'll define a **User** and **Order** model, where each user can have many orders.

### Example: User and Order Models
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Order> Orders { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
```

---

# Data Annotations vs Fluent API

## Data Annotations
- [Required]: Ensures the property is not null.
- [MaxLength]: Sets the maximum length of a string property.
- [Key]: Marks a property as the primary key.

### Example: Data Annotations
```csharp
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
}
```

## Fluent API
Fluent API provides more flexibility and is typically used when data annotations are insufficient or not possible.

### Example: Fluent API Configuration for Constraints
```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
```

---

# Migrations and Database Operations

## 1️⃣ EF Core Migrations
Migrations are a way to apply changes to the database schema based on your model classes.

### Adding a Migration
```bash
dotnet ef migrations add InitialCreate
```

### Applying Migrations to the Database
```bash
dotnet ef database update
```

---

# Basic CRUD Operations

## Create (Add)
```csharp
var user = new User { Name = "John Doe", IsActive = true };
context.Users.Add(user);
await context.SaveChangesAsync();
```

## Read (Query)
```csharp
var users = await context.Users.ToListAsync();
```

## Update
```csharp
var user = await context.Users.FirstOrDefaultAsync(u => u.Id == 1);
if (user != null)
{
    user.Name = "Updated Name";
    await context.SaveChangesAsync();
}
```

## Delete
```csharp
var user = await context.Users.FirstOrDefaultAsync(u => u.Id == 1);
if (user != null)
{
    context.Users.Remove(user);
    await context.SaveChangesAsync();
}
```

---

# Performance and Best Practices

## 1️⃣ AsNoTracking
Use **AsNoTracking** for read-only operations to improve performance.
```csharp
var users = await context.Users.AsNoTracking().ToListAsync();
```

## 2️⃣ Optimizing Queries with Projections
```csharp
var userNames = await context.Users
    .Where(u => u.IsActive)
    .Select(u => new { u.Name })
    .ToListAsync();
```

---

# Handling Concurrency in EF Core

## Example: Using a Timestamp for Concurrency
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; } // RowVersion column
}

try
{
    await context.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException)
{
    // Handle concurrency conflict (e.g., notify user)
}
```

---

# Closing

## 1️⃣ Recap Key Takeaways
- EF Core simplifies database interactions in .NET applications.
- New features in EF Core 9 improve performance, raw SQL execution, and LINQ translation.
- You can easily define models, relationships, and apply migrations.
- CRUD operations are straightforward with async support.
- Best practices like **AsNoTracking** and projections improve query performance.
- Concurrency handling in EF Core helps prevent conflicts in multi-user scenarios.

## 2️⃣ Useful Resources
- **Microsoft Docs**:  
  [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)  
  [Getting Started with EF Core](https://docs.microsoft.com/en-us/ef/core/get-started/)

- **EF Core GitHub Repository**:  
  [EF Core GitHub](https://github.com/dotnet/efcore)

## 3️⃣ Q&A
Feel free to ask any questions about EF Core, .NET, or anything covered in today’s talk!

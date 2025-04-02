# Opening: Why EF Core?

## Quick Story/Example
Imagine you're building a .NET application and need to interact with a database. How do you do it efficiently? You could write raw SQL queries, but that can be error-prone and hard to maintain. This is where an ORM (Object-Relational Mapper) like EF Core comes in handy

## How Do We Interact with Databases in .NET?
Before Entity Framework Core (EF Core), developers interacted with databases using **ADO.NET**, **Dapper**, or raw SQL queries. While these approaches provided control and performance, they often required writing a lot of boilerplate code for CRUD operations.

Imagine building an application where you need to:
- Open a database connection.
- Write SQL queries manually.
- Handle result mappings to objects.
- Manage transactions and exceptions explicitly.

This process is repetitive and error-prone. This is where **EF Core** comes in.

## What is EF Core?
EF Core is a is a modern, open-source, and cross-platform **Object-Relational Mapper (ORM)** for .NET that eliminates the need to write complex SQL queries manually. It allows developers to work with databases using **C# classes and LINQ** instead of SQL.

EF Core acts as a bridge between **.NET applications** and **databases**, allowing developers to perform operations using object-oriented techniques.

## Why Use EF Core?
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

## Key Takeaways
- EF Core reduces the amount of **manual database code**.
- It enhances maintainability by allowing developers to work with **strongly-typed models**.
- It supports various databases, making it a flexible ORM choice for .NET developers.


# New Features in EF Core (.NET 9)

## 2️⃣ Enhanced Raw SQL Queries  

### Enables safer and more efficient execution of raw SQL statements.  

### Improves mapping results directly to entity models.  

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


## Why These Features Matter
✅ Faster Queries → Improved database performance.

✅ More Flexibility → Greater control over data retrieval.

✅ Better Developer Experience → Write cleaner, more efficient queries.

### Example: Improved LINQ Translation

```csharp
var highValueOrders = await context.Orders
    .Where(o => o.TotalAmount > 100)
    .Select(o => new { o.Id, o.TotalAmount })
    .ToListAsync();
```


# Setting Up EF Core (5 minutes)

## Installing EF Core  

To install EF Core in your .NET project, run the following command in your terminal:  

```bash
dotnet add package Microsoft.EntityFrameworkCore
```

# Basic DbContext and Entity Class

### A DbContext is the primary class for interacting with the database in EF Core. Here's how to define a basic DbContext and an entity class:

### Example: DbContext and Entity Class

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Your_Connection_String_Here");
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}

```

# OnConfiguring and Setting Up Database Providers

### You can use the OnConfiguring method to set up different database providers. Here’s how to configure a few popular ones:

### Example: Configuring SQL Server

```csharp
options.UseSqlServer("Your_Connection_String_Here");

```

### Example: Configuring SQLite

```csharp
options.UseSqlite("Data Source=mydatabase.db");

```

# Defining Models and Relationships (7 minutes)

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

# One-to-Many Relationship with Fluent API
## In this relationship, one User can have many Orders.

### Example: Fluent API Configuration for One-to-Many

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);
    }
}

```


# Many-to-Many Relationship with Fluent API
## In a many-to-many relationship, a User can have many Products, and a Product can belong to many Users. Here's how we configure this using the Fluent API.

### Example: Many-to-Many Relationship with Fluent API

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<User> Users { get; set; }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Products)
            .WithMany(p => p.Users)
            .UsingEntity(j => j.ToTable("UserProducts"));
    }
}

```


# Data Annotations vs Fluent API for Defining Constraints
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

# Fluent API
### Fluent API provides more flexibility and is typically used when data annotations are insufficient or not possible.

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


# Migrations and Database Operations (8 minutes)

## 1️⃣ EF Core Migrations  

Migrations are a way to apply changes to the database schema based on your model classes. 

### Adding a Migration  

To create an initial migration, use the following command:

In Visual studio code 
```bash
dotnet ef migrations add InitialCreate
```

In Visual studio  
```bash
dotnet add migration InitialCreate
```

This will generate migration files that include changes to the database schema.


## Applying Migrations to the Database
After adding the migration, apply it to the database using:
In Visual studio code 
```bash
dotnet ef database update

```

In Visual studio  
```bash
dotnet Database-Update
```


# 2️⃣ Basic CRUD Operations
EF Core makes it easy to perform basic CRUD (Create, Read, Update, Delete) operations on your database.

## Create (Add)
To add a new record to the database:

```csharp
var user = new User { Name = "John Doe", IsActive = true };
context.Users.Add(user);
await context.SaveChangesAsync();

```

## Read (Query)
To read data from the database:

```csharp
var users = await context.Users.ToListAsync();

```

## Update
To update an existing record:

```csharp
var user = await context.Users.FirstOrDefaultAsync(u => u.Id == 1);
if (user != null)
{
    user.Name = "Updated Name";
    await context.SaveChangesAsync();
}

```

## Delete
To delete a record:

```csharp
var user = await context.Users.FirstOrDefaultAsync(u => u.Id == 1);
if (user != null)
{
    context.Users.Remove(user);
    await context.SaveChangesAsync();
}

```

# 3️⃣ Asynchronous Queries
- EF Core supports asynchronous queries, which can improve performance by freeing up threads while waiting for database operations.
- Using await ensures that the query is executed asynchronously, allowing the application to remain responsive.


```csharp
var users = await context.Users.ToListAsync();
```


# Performance and Best Practices (5 minutes)

## 1️⃣ AsNoTracking  

EF Core provides **AsNoTracking** to optimize read-only queries. It tells EF Core not to track changes for the retrieved entities, which improves performance.

### When to Use AsNoTracking  

- Use **AsNoTracking** for read-only operations where you do not need to modify the data.
- It reduces memory usage and improves performance, especially when querying large datasets.

### Example: Using AsNoTracking

```csharp
var users = await context.Users.AsNoTracking().ToListAsync();
```

# 2️⃣ Optimizing Queries with Projections
Projections allow you to shape the data into a more efficient format, reducing unnecessary data retrieval and improving performance.

## Using Select to Project Data
Instead of loading entire entities, use Select to only retrieve the necessary data:

```csharp
var userNames = await context.Users
    .Where(u => u.IsActive)
    .Select(u => new { u.Name })
    .ToListAsync();

```


# 3️⃣ Handling Concurrency in EF Core
Concurrency issues occur when multiple users or processes try to modify the same data simultaneously. EF Core provides mechanisms to handle these conflicts.

## Optimistic Concurrency
EF Core uses optimistic concurrency to manage conflicts. You can use a timestamp or version number to track changes.

## Example: Using a Timestamp for Concurrency

1. Add a RowVersion column to the model:

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; } // RowVersion column
}

```

2. Handle concurrency exception during update:

```csharp
try
{
    await context.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException)
{
    // Handle concurrency conflict (e.g., notify user)
}


```

# Closing (5 minutes)

## 1️⃣ Recap Key Takeaways  

- EF Core simplifies database interactions in .NET applications.
- New features in EF Core 9 improve performance, raw SQL execution, and LINQ translation.
- You can easily define models, relationships, and apply migrations.
- CRUD operations are straightforward with async support.
- Best practices like **AsNoTracking** and projections improve query performance.
- Concurrency handling in EF Core helps prevent conflicts in multi-user scenarios.

---

## 2️⃣ Useful Resources  

- **Microsoft Docs**:  
  [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)  
  [Getting Started with EF Core](https://docs.microsoft.com/en-us/ef/core/get-started/)

- **EF Core GitHub Repository**:  
  [EF Core GitHub](https://github.com/dotnet/efcore)

---

## 3️⃣ Q&A  

Feel free to ask any questions about EF Core, .NET, or anything covered in today’s talk!

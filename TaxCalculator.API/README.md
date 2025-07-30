# TaxCalculator.API

A simple RESTful API that calculates income tax based on progressive tax bands.

---

## 🚀 Overview

`TaxCalculator.API` provides endpoints for computing the income tax of individuals based on configurable tax bands. The API is built with ASP.NET Core and follows clean architecture principles. It supports unit testing and easy extension of tax rules.

---

## 📦 Features

- Calculates tax based on progressive bands
- Configurable tax bands via seed data
- Simple REST API using ASP.NET Core
- Fully unit-tested logic with xUnit and FluentAssertions
- Swagger/OpenAPI support for interactive testing

---

## 🛠️ Tech Stack

- ASP.NET Core 6
- Entity Framework Core (InMemory)
- xUnit for unit testing
- FluentAssertions for readable test assertions
- Swagger (Swashbuckle)

---

## 📚 Example Tax Bands

```csharp
new TaxBand { Min = 0, Max = 5000, Rate = 0.00m },     // 0% for 0–5000
new TaxBand { Min = 5001, Max = 20000, Rate = 0.20m }, // 20% for 5001–20000
new TaxBand { Min = 20001, Max = null, Rate = 0.40m }  // 40% for 20001+

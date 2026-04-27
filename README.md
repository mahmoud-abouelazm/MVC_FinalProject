# The Editorial Archive — Library Management System

An ASP.NET Core MVC web application for managing a physical library's book inventory, copies, rentals, users, and reporting.

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| ORM | Entity Framework Core (Code-First) |
| Database | SQL Server |
| Identity | ASP.NET Core Identity (integer PKs) |
| External Auth | Google OAuth 2.0 |
| Email | SMTP via `IEmailSender` |
| Frontend | Razor Views, Bootstrap, libman (client-side libs) |

---

## Business Decisions

### 1. Book & Copy Separation

A **Book** represents a title (metadata: title, description, price, category, authors).  
A **Copy** is a distinct physical instance of a book that can be rented independently.

- Each book can have **3–5 copies** (seeded; adjustable by admin).
- Copies have an `AllowToRental` flag that can be toggled at any time by an admin.
- This allows the library to remove a damaged copy from circulation without deleting its history.

### 2. Soft Delete for Books

Books are never hard-deleted from the database. Instead, a boolean `IsDeleted` flag is set to `true`.

**Reason:** Preserves the referential integrity of historical rental records and prevents orphaned data in copies and rental history.

### 3. Many-to-Many Book–Author Relationship

A book can have multiple authors, and an author can be associated with multiple books, via a `BookAuthor` join table.

**Reason:** Accurately reflects real-world publishing (co-authored works), without duplicating author records per book.

### 4. Rental Pricing Calculated at Return Time

Rental `Amount` is **not set when a rental is created** — it is calculated when the book is returned.

The formula is:

```
daysRented   = floor((returnedAt - rentedAt).TotalDays) + 1   (minimum 1)
daysOnTime   = daysRented - daysLate
baseAmount   = daysOnTime × book.Price
lateFee      = daysLate × $5 × numberOfBooks
totalAmount  = baseAmount + lateFee
```

**Reason:** The actual return date is unknown at rental creation. Pricing on return ensures accuracy and simplifies the create-rental flow.

### 5. Late Fee: Flat $5 Per Day Per Book

Overdue penalties are charged as **$5 per overdue day per book** — a fixed fee independent of the book's price.

**Reason:** A flat fee is predictable for users, easy to communicate, and avoids penalising users more for renting expensive books.

### 6. Rental States: Active / Returned / Overdue

Three `RentalState` enum values track the lifecycle of every rental:

| State | Meaning |
|---|---|
| `Active` | Currently rented, within due date |
| `Overdue` | Due date passed, not yet returned |
| `Returned` | Copy handed back; amount finalised |

**Revenue is only counted from `Returned` rentals** (unpaid active/overdue rentals are excluded from financial summaries).

### 7. Copy Cannot Be Deleted If It Has Rental History

Attempting to delete a copy that has any `CopyRental` records is blocked with a user-friendly error message, suggesting the admin restricts it from rental instead.

**Reason:** Hard-deleting a copy with history would break the audit trail and orphan rental records.

### 8. Role-Based Access: Admin vs. User

The system uses two roles seeded at startup:

| Role | Access |
|---|---|
| `Admin` | Full management of books, copies, authors, categories, users, rentals, reports, settings |
| `User` | Regular library member; visible on rental records |

Admins can change any user's role at any time via the user management panel.

### 9. Admin Controls User Email Confirmation

Admins can manually **confirm** or **deactivate** a user's email through the admin panel.

**Reason:** Allows the library desk staff to onboard walk-in members directly without requiring self-service email verification flows.

### 10. Return Invoice Sent by Email — Failure Is Non-Blocking

When a rental is returned, the system attempts to send a formatted HTML invoice email to the user.  
If the email send fails (network/SMTP error), the return is still committed and the error is logged — **it does not roll back the return transaction**.

**Reason:** Email delivery is a best-effort notification; the core business operation (returning a book) must not be blocked by a transient third-party failure.

### 11. Cookie Security Settings

Authentication cookies are configured with:

| Setting | Value | Reason |
|---|---|---|
| `HttpOnly` | `true` | Prevents JavaScript access (mitigates XSS) |
| `SecurePolicy` | `Always` | Cookies only sent over HTTPS |
| `SameSite` | `Strict` | Prevents CSRF via cross-origin requests |
| `ExpireTimeSpan` | 60 minutes | Limits session window |
| `SlidingExpiration` | `true` | Resets timer on activity |

### 12. Google OAuth as Optional External Login

Google OAuth is conditionally registered **only if** `Authentication:Google:ClientId` and `ClientSecret` are present in configuration.

**Reason:** Makes the application deployable without Google credentials configured (e.g., on a fresh dev machine) while supporting it in production.

### 13. Global Exception Handling Middleware

A custom `GlobalExceptionHandlingMiddleware` intercepts all unhandled exceptions and HTTP error status codes (4xx / 5xx) and routes them to a unified `/Home/Error` page.

**Reason:** Centralises error presentation, ensures no raw stack traces leak to end users in production, and logs all errors consistently.

### 14. Pagination Defaults to 8 Items Per Page

All list views (books, copies, rentals, users) are paginated with a default page size of **8**.

**Reason:** Balances readability on standard screen sizes with minimising database query load.

### 15. Repository Pattern with Generic + Specialised Repositories

A generic `IRepository<T>` provides basic CRUD. Domain-specific interfaces (`IBookRepository`, `IRentalRepository`, etc.) extend it with business-specific queries.

**Reason:** Decouples data access from business logic, improves testability, and avoids fat service classes containing raw EF queries.

### 16. Admin Analytics Dashboard (Reports)

A dedicated `ReportsController` (Admin-only) provides:

- **KPI summary**: total users, books, authors, and all-time revenue.
- **Monthly charts**: rentals per month, revenue per month, unique users per month, unique authors per month.
- **Category distribution**: pie/bar chart of book count per category.

All data is scoped to the **current calendar year** for trends, and lifetime totals for KPIs.

---

## Data Model Overview

```
Category ──< Book >── BookAuthor >── Author
               │
              Copy
               │
           CopyRental
               │
            Rental ──── ApplicationUser
```

- `BookAuthor` — join table (many-to-many Books ↔ Authors)
- `CopyRental` — join table (many-to-many Copies ↔ Rentals)
- `ApplicationUser` extends `IdentityUser<int>` with `FullName`

---

## Getting Started

1. Set your SQL Server connection string in `appsettings.Development.json` under `ConnectionStrings:DefaultConnection`.
2. (Optional) Add Google OAuth credentials under `Authentication:Google:ClientId` and `Authentication:Google:ClientSecret`.
3. Run EF migrations: `dotnet ef database update --project Library.Web`
4. Launch the app. Navigate to `/Settings` (as Admin) to seed demo data.

Default seeded admin credentials:

| Field | Value |
|---|---|
| Email | `admin@library.com` |
| Password | `Password@123` |

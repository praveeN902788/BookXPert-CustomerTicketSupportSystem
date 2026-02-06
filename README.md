# BookXPert-CustomerTicketSupportSystem
# Customer Support Ticket System

## Project Overview
A complete Customer Support Ticket System with C# WinForms Desktop Application frontend and ASP.NET Web API backend using MySQL database.

## Technology Stack
- **Frontend**: C# WinForms Desktop Application
- **Backend**: ASP.NET Web API (.NET 8)
- **Database**: Sql
- **ORM**: Entity Framework Core
- **Authentication**: JWT Token-based
- **Communication**: JSON over HTTP

## Project Structure
```
CustomerSupportSystem/
├── CustomerSupport.API/              # ASP.NET Web API Project
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Services/
│   ├── Data/
│   └── Program.cs
├── CustomerSupport.Desktop/          # WinForms Desktop Application
│   ├── Forms/
│   ├── Models/
│   ├── Services/
│   └── Program.cs
├── CustomerSupport.Shared/           # Shared Models/DTOs
│   └── Models/
└── Database/
    └── Scripts/
```

## Setup Instructions

### Prerequisites
1. Visual Studio 2022 or later
2. .NET 6/7/8 SDK
3. MySQL Server 8.0+
4. MySQL Workbench (optional)

### Database Setup
1. Install MySQL Server
2. Create database: `customer_support_db`
3. Run the provided SQL scripts
4. Update connection strings in both projects

### API Setup
1. Open `CustomerSupport.API` project
2. Update `appsettings.json` with your MySQL connection string
3. Run `dotnet ef database update` to apply migrations
4. Start the API project (default: https://localhost:7001)

### Desktop App Setup
1. Open `CustomerSupport.Desktop` project
2. Update API base URL in configuration
3. Build and run the desktop application

## Features Implemented
- ✅ User Authentication (Login/Logout)
- ✅ Role-based Access Control (User/Admin)
- ✅ Ticket Creation and Management
- ✅ Ticket Assignment (Admin only)
- ✅ Status Updates with History Tracking
- ✅ Comment System (Internal/External)
- ✅ Real-time Data Refresh
- ✅ Input Validation and Error Handling

## Business Rules
- Ticket numbers are auto-generated
- Status flow: Open → In Progress → Closed
- Users can only view/modify their own tickets
- Admins can view/modify all tickets
- All status changes are logged in history
- Server time is used for all timestamps

## Default Users
- **Admin**: username: `admin`, password: `admin123`
- **User**: username: `user1`, password: `user123`

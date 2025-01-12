# 2C2P Assignment Project

This project consists of two main parts: a Frontend Client and a Backend Service.

## 1. Frontend Client

- **Technology Stack**: React with Vite.js
- **Project Setup**: Mostly scaffolding from Project Initialize when creating the project using Visual Studio's create new project function.
- **Pages**:
  - A single index page containing two components:
    1. **File Uploader**
    2. **Table** displaying transaction data

## 2. Backend Service

- **Technology Stack**: .NET 9 using ASP.NET
- **Features**:
  - Single Controller: `TransactionController` with two routes:
    - `GET transaction`: For retrieving all transactions with filters.
    - `POST transaction/upload`: For uploading files.
- **Database**: PostgreSQL

### Project Setup Instructions

1. **Database Setup**:
   - Ensure PostgreSQL is installed.
   - Create a new database named `assignment`:
     ```sql
     CREATE DATABASE assignment;
     ```

2. **Configure the Solution**:
   - Open the solution.
   - Navigate to `Tools` -> `NuGet Package Manager` -> `Package Manager Console`.
   - In the Console, select `WebAssignment.Server` as the Default project.
   - Run the command:
     ```
     Update-Database
     ```

3. **Running the Project**:
   - Start the project. Both Frontend and Backend should be running concurrently.
   - To view Swagger documentation, change the path to `/swagger` to access the SwaggerUI.

## Notes

- The frontend is set up to interact with the backend seamlessly, ensuring that file uploads and data retrieval are handled efficiently.
- The backend setup involves setting up the database schema and connectivity to ensure smooth operation with the PostgreSQL database.


# MediTrack - Pharmacy Management System 🏥💊

MediTrack is a comprehensive, full-stack Pharmacy Management System designed to streamline the operations between Admins, Doctors, and Pharmaceutical Suppliers. It handles drug inventory management, order processing, and role-based access control.

## 🚀 Tech Stack

### Frontend
- **Angular 18** - Single Page Application framework
- **TypeScript** - Strongly typed programming language
- **HTML5/CSS3** - Structure and styling

### Backend
- **ASP.NET Core 8 Web API** - Robust backend framework
- **C#** - Backend programming language
- **Entity Framework Core** - ORM for database operations
- **JWT (JSON Web Tokens)** - Secure Authentication & Authorization
- **Swagger** - API Documentation

### Database & Integrations
- **MySQL** - Relational database management system
- **Razorpay** - Payment Gateway Integration
- **MailKit** - Email notifications

---

## 🌟 Key Features

* **🔐 Role-Based Access Control (RBAC):** Three distinct roles with unique dashboards and permissions:
  * **Admin:** Manages suppliers, adds drugs, views overall sales reports, and verifies orders.
  * **Doctor:** Browses drug catalog, places orders, and tracks order status.
  * **Supplier:** Views assigned orders and updates fulfillment status.
* **📦 Inventory Management:** Add, edit, and delete drug inventory. Tracks expired drugs.
* **🛒 Order Processing Lifecycle:** Doctors request orders -> Admins verify -> Suppliers fulfill.
* **💳 Payment Integration:** Secure checkout process powered by Razorpay.

---

## 🛠️ Installation & Setup Guide

### Prerequisites
Make sure you have the following installed on your machine:
* [Node.js & npm](https://nodejs.org/)
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [MySQL Server](https://dev.mysql.com/downloads/installer/)

### 1. Database Setup
1. Open MySQL and ensure your root password is set to `Admin@123`.
2. Open `PharmacyBackend/appsettings.json` and verify the connection string:
   ```json
   "DefaultConnection": "Server=localhost;Port=0203;User Id=root;Password=xyx555;Database=PharmacyDB;"
   ```

### 2. Backend Setup (.NET Core)
1. Open a terminal and navigate to the backend folder:
   ```bash
   cd PharmacyBackend
   ```
2. Apply the Entity Framework migrations to create the database:
   ```bash
   dotnet ef database update
   ```
3. Run the backend server:
   ```bash
   dotnet run
   ```
   *The API will start running (usually on `http://localhost:5118`). You can visit `http://localhost:5118/swagger` to view the API documentation.*

### 3. Frontend Setup (Angular)
1. Open a new terminal and navigate to the frontend folder:
   ```bash
   cd PharmacyFrontend
   ```
2. Install the required Node packages:
   ```bash
   npm install
   ```
3. Start the Angular development server:
   ```bash
   npm start
   ```
4. Open your browser and navigate to `http://localhost:4200`.

---

## 👨‍💻 Default Login Credentials
After setting up the database, a default Admin account is automatically seeded.
* **Email:** `xyz@pharmacy.com`
* **Password:** `xyz@123`

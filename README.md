# Men's Accessories E-Commerce Store

A full-featured e-commerce platform built with ASP.NET Core MVC and .NET 10, specializing in men's accessories. The application provides a seamless shopping experience for customers and comprehensive inventory management tools for administrators.

## 📋 Table of Contents

- [Overview](#overview)
- [Key Features](#key-features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Installation & Setup](#installation--setup)
- [Database](#database)
- [API Endpoints](#api-endpoints)
- [Authentication & Authorization](#authentication--authorization)

## 🎯 Overview

Men's Accessories is a modern, responsive e-commerce application designed for selling premium men's accessories including watches, belts, sunglasses, wallets, bags, and more. The platform features a role-based architecture separating customer and admin experiences, integrated payment processing with Stripe, and an intuitive product discovery system.

## ✨ Key Features

### Customer Features
- ✅ **User Authentication** - Secure registration and login with ASP.NET Identity
- ✅ **Social Login** - Google and Facebook authentication integration
- ✅ **Product Browsing** - Advanced filtering by category, price range, and search
- ✅ **Multi-Image Products** - Products with multiple images and carousel view with circular navigation
- ✅ **Product Details** - Comprehensive product information with ratings and reviews
- ✅ **Rating & Reviews** - 5-star rating system with customer comments
- ✅ **Shopping Cart** - Add/remove items with real-time quantity management
- ✅ **Wishlist/Favorites** - Save favorite products for later
- ✅ **Secure Checkout** - Stripe-integrated payment processing
- ✅ **Order History** - View past orders and order details
- ✅ **Responsive Design** - Optimized for mobile, tablet, and desktop
- ✅ **Featured Products** - Special showcase page for trending items

### Admin Features
- ✅ **Product Management** - Create, read, update, and delete products
- ✅ **Multi-Image Upload** - Add multiple images per product with preview carousel
- ✅ **Category Management** - Create, update, and delete product categories
- ✅ **Cascade Delete** - Deleting a category automatically deletes all products in it
- ✅ **Inventory Management** - Track stock levels with real-time status badges
- ✅ **Discount Pricing** - Set percentage-based product discounts
- ✅ **Featured Products** - Mark products for homepage promotion
- ✅ **Order Monitoring** - View all customer orders with payment and fulfillment status
- ✅ **Admin Dashboard** - Centralized control panel for store management

## 🛠️ Tech Stack

### Backend
- **Framework:** ASP.NET Core MVC 10.0
- **Language:** C# 14.0
- **ORM:** Entity Framework Core 10.0.7 with SQL Server
- **Authentication:** ASP.NET Core Identity + OAuth (Google, Facebook)
- **Payment:** Stripe API
- **Database:** SQL Server

### Frontend
- **Markup:** Razor HTML5
- **Styling:** CSS3 with custom design system and CSS variables
- **JavaScript:** Vanilla ES6+ (no frameworks)
- **Animations:** CSS transitions and keyframe animations

### Architecture Patterns
- **Repository Pattern** - Generic `IBaseRepository<T>` with specific implementations
- **Service Layer** - Business logic abstraction (`ICartService`, `IOrderService`)
- **Dependency Injection** - ASP.NET Core built-in DI container
- **Code-First Migrations** - Database schema versioning with EF Core

## 🏗️ Architecture

The project follows a layered architecture with clear separation of concerns:

```
Controllers (Presentation Layer)
    ↓
Services (Business Logic)
    ↓
Repositories (Data Access)
    ↓
Entity Framework Core (ORM)
    ↓
SQL Server Database
```

### Key Components

**Models**
- Product, ProductImage, Category
- Order, OrderItem, Cart, CartItem
- Customer, Rate (Reviews)
- ApplicationUser (ASP.NET Identity)

**Repositories**
- `IBaseRepository<T>` - Generic interface for CRUD operations
- `BaseRepository<T>` - Generic implementation with sync and async methods
- `ProductRepository` - Custom product queries (GetByCategory, GetByPriceRange, Search, etc.)
- `CartRepository` - Shopping cart operations
- `OrderRepository` - Order creation and management

**Services**
- `ICartService` - Shopping cart business logic
- `IOrderService` - Order processing
- `IProductRepository` - Product queries and filtering

**Controllers**
- `ProductController` - Admin CRUD for products
- `CategoryController` - Admin category management
- `HomeController` - Public product browsing with filtering
- `CartController` - Shopping cart operations
- `OrderController` - Order management (customer and admin)
- `FavoritesController` - Wishlist management
- `AdminController` - Admin dashboard

## 🚀 Installation & Setup

### Prerequisites
- .NET 10 SDK or later
- SQL Server 2019 or higher
- Visual Studio 2026 (or VS Code with C# extension)
- Git

### Steps

1. **Clone the repository**
```
git clone https://github.com/0Abdelrahman1/Men-Accessories-ITI.git
cd "Men Accessories"
```

2. **Update database connection string**
Edit `appsettings.json`:
```json
"ConnectionStrings": {
    "CS": "Server=YOUR_SERVER;Database=MenAccessories;User Id=YOUR_USER;Password=YOUR_PASSWORD;Encrypt=True;TrustServerCertificate=True;"
}
```

3. **Configure third-party services in `appsettings.json`**
- Stripe API keys
- Google OAuth credentials
- Facebook OAuth credentials
- Email/SMTP settings

4. **Restore NuGet packages**
```bash
dotnet restore
```

5. **Apply database migrations**
```bash
dotnet ef database update
```

6. **Run the application**
```bash
dotnet run
```

7. **Access the application**
- Navigate to: `https://localhost:5001`
- Admin dashboard: `/Admin/Index`
- Default admin credentials: Username: `admin` | Password: `Admin@123`

## 💾 Database

### Entities & Relationships

```
Categories (1) ──[CASCADE DELETE]──> (Many) Products
Products (1) ──[CASCADE DELETE]──> (Many) ProductImages
Products (1) ──[RESTRICT]──> (Many) OrderItems
Products (1) ──[RESTRICT]──> (Many) CartItems
Customers (1) ──[CASCADE DELETE]──> (1) Cart
Customers (1) ──[RESTRICT]──> (Many) Orders
Orders (1) ──[CASCADE DELETE]──> (Many) OrderItems
Carts (1) ──[CASCADE DELETE]──> (Many) CartItems
Products (1) ──[One-to-Many]──> (Many) Rates
```

**Cascade Delete Strategy:**
- When a category is deleted → All products in it are deleted
- When a product is deleted → All product images are deleted
- When a customer is deleted → Their cart is deleted
- When a cart is deleted → All cart items are deleted
- When an order is deleted → All order items are deleted

## 🔌 API Endpoints

### Public Endpoints
- `GET /` - Homepage with product listing
- `GET /Home/Features` - Featured products page
- `GET /Home/FilterAndSort` - AJAX filtering/sorting endpoint
- `GET /Product/Details/{id}` - Product details page

### Customer Endpoints (Requires [Authorize])
- `GET /Cart/Index` - View shopping cart
- `POST /Cart/AddToCart` - Add product to cart
- `POST /Cart/UpdateQuantity` - Update item quantity
- `POST /Cart/RemoveFromCart` - Remove item from cart
- `GET /Favorites/Index` - View wishlist
- `POST /Favorites/Toggle` - Add/remove from favorites
- `GET /Order/MyOrders` - Customer order history
- `GET /Order/Details/{id}` - Order details
- `GET /Order/Success/{sessionId}` - Checkout success page

### Admin Endpoints (Requires [Authorize(Roles = "Admin")])
- `GET /Product/Index` - List all products
- `GET /Product/Create` - Product creation form
- `POST /Product/Create` - Create product with images
- `GET /Product/Edit/{id}` - Edit product form
- `POST /Product/Edit/{id}` - Update product
- `GET /Product/Delete/{id}` - Delete confirmation
- `POST /Product/Delete/{id}` - Confirm product deletion
- `GET /Category/Index` - List categories
- `GET /Category/Create` - Category creation form
- `POST /Category/Create` - Create category
- `GET /Category/Edit/{id}` - Edit category
- `POST /Category/Edit/{id}` - Update category
- `GET /Category/Delete/{id}` - Delete category (cascades to products)
- `POST /Category/Delete/{id}` - Confirm category deletion
- `GET /Order/Index` - List all orders
- `GET /Order/Details/{id}` - Order details (admin view)
- `GET /Admin/Index` - Admin dashboard

## 🔐 Authentication & Authorization

### User Roles
- **Admin** - Full access to product, category, and order management
- **Customer** - Browse products, manage cart, place orders

### Protected Resources
```
[Authorize(Roles = "Admin")]           // Admin-only endpoints
[Authorize]                             // Any authenticated user
[AllowAnonymous]                        // Public endpoints
```

### Authentication Methods
- Local username/password (ASP.NET Identity)
- Google OAuth 2.0
- Facebook OAuth 2.0

## 🎨 UI/UX Features

### Responsive Design
- Mobile-first CSS approach
- Grid-based layouts with CSS media queries
- Touch-friendly buttons and forms
- Optimized for all screen sizes

### Product Browsing
- Advanced filtering (category, price range, stock status)
- Full-text search
- Sorting options (price, newest, ratings)
- Pagination with customizable page size
- Product cards with hover effects

### Admin Interface
- Modern gradient headers
- Table-based layouts with hover states
- Action buttons (Edit, Delete, View)
- Status badges for inventory and orders
- Form validation with error messages
- Animation effects (fade-up, hover transitions)

### Shopping Experience
- Product carousel with circular navigation (keyboard and mouse support)
- Real-time cart updates
- Stripe Checkout integration
- Order tracking with status badges
- Favorites/Wishlist management

## 🔄 Advanced Features

### Multi-Image Products
- Add multiple images to a single product
- Circular carousel navigation (loops to first/last image)
- Keyboard support (Arrow keys)
- Dot indicators for image navigation

### Inventory Management
- Real-time stock tracking
- Out-of-stock status badges
- Prevent purchases of unavailable items

### Rating & Review System
- 5-star rating scale
- Customer comments/feedback
- Average rating calculation
- Aggregate rating tracking

### Favorites/Wishlist
- Persistent wishlist per authenticated user
- Toggle favorite with single click
- AJAX-based without page reload
- Quick access from favorites to cart

### Payment Processing
- Secure Stripe Checkout integration
- Session tracking for orders
- Payment status monitoring (Pending, Paid, Succeeded)
- Order fulfillment status tracking

### Search & Filter
- Multi-criteria filtering
- Keyword search across products
- Category filtering
- Price range filtering
- Stock availability filtering
- Sort options (default, price low-to-high, price high-to-low)

## 🧪 Testing

The application includes built-in data seeding for testing:
- Default admin account created on startup
- Sample categories and products can be added manually

## 📝 Code Quality

### Best Practices Implemented
- ✅ SOLID principles
- ✅ DRY (Don't Repeat Yourself)
- ✅ Clean code conventions
- ✅ Error handling with try-catch
- ✅ Input validation on models
- ✅ SQL injection prevention (parameterized queries via EF Core)
- ✅ CSRF token validation on forms
- ✅ Async/await for database operations

### Error Handling
- Model state validation
- Custom error messages
- Graceful error pages (404, 500)
- Try-catch blocks in critical sections

## 🚀 Future Enhancements

- [ ] Admin order status updates (not just read-only)
- [ ] Email notifications on order status changes
- [ ] Product reviews moderation panel
- [ ] Low inventory alerts
- [ ] Advanced analytics dashboard
- [ ] Product recommendations engine
- [ ] Discount codes/coupon system
- [ ] Real-time shipping integration
- [ ] Customer support live chat
- [ ] Social media sharing buttons
- [ ] Product comparison tool
- [ ] Gift cards system

## 📞 Contact & Support

For questions, issues, or suggestions:
- **Email:** dinaalaraby9503@gmail.com
- **GitHub:** [0Abdelrahman1](https://github.com/0Abdelrahman1)

## 📄 License

This project is open source and available for educational purposes.

---

**Built with ❤️ using ASP.NET Core MVC and .NET 10**
**Last Updated:** June 2026

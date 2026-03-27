# Authentication & Authorization System Summary

## Overview
Your Construction Project Management System has a complete JWT-based authentication system with role-based access control (RBAC). This document provides a quick reference for all authentication, login, and admin functionality.

---

## 🔐 User Roles

| Role | ID | Key Responsibilities | Key Permissions |
|------|----|--------------------|-----------------|
| **Admin** | 0 | System administration | Manage all users, all endpoints |
| **ProjectManager** | 1 | Project management | Create projects, assign tasks, manage teams |
| **SiteEngineer** | 2 | Field operations | Execute tasks, manage inventory, report progress |
| **Contractor** | 3 | Contracted work | Execute assigned tasks only |
| **SafetyOfficer** | 4 | Safety compliance | Record inspections, track compliance |

---

## 🔑 Login Flow

### Step 1: Login to Get JWT Token
```bash
POST /api/user/login
Content-Type: application/json

{
  "email": "admin@construction.com",
  "password": "Admin@123"
}
```

**Response (200 OK):**
```json
{
  "userId": 1,
  "name": "Admin",
  "email": "admin@construction.com",
  "role": "Admin",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Step 2: Use Token in All Subsequent Requests
```bash
Authorization: Bearer <token>
```

**Token Contains:**
- User ID
- User Name
- Email
- Role
- Issued at & Expiration (60 minutes default)

---

## 👥 User Management API (Admin Only)

### 1. Register New User
```
POST /api/user/register
Authorization: Bearer <admin_token>
Content-Type: application/json

{
  "name": "John Smith",
  "email": "john@construction.com",
  "password": "SecurePass123!",
  "role": 1  // 0=Admin, 1=ProjectManager, 2=SiteEngineer, 3=Contractor, 4=SafetyOfficer
}
```

**Response (201 Created):**
```json
{
  "userId": 5,
  "name": "John Smith",
  "email": "john@construction.com",
  "role": "ProjectManager",
  "isActive": true,
  "createdAt": "2025-03-25T12:00:00Z"
}
```

### 2. Get All Users
```
GET /api/user
Authorization: Bearer <admin_token>
```

### 3. Get Single User
```
GET /api/user/{userId}
Authorization: Bearer <admin_token>
```

### 4. Get Users by Role
```
GET /api/user/role/{role}
Authorization: Bearer <admin_or_projectmanager_token>

Roles: Admin, ProjectManager, SiteEngineer, Contractor, SafetyOfficer
```

### 5. Update User Role
```
PUT /api/user/{userId}/role
Authorization: Bearer <admin_token>
Content-Type: application/json

{
  "newRole": 2  // 0=Admin, 1=ProjectManager, 2=SiteEngineer, 3=Contractor, 4=SafetyOfficer
}
```

### 6. Deactivate User
```
PUT /api/user/{userId}/deactivate
Authorization: Bearer <admin_token>
```
Users marked as inactive cannot login.

### 7. Activate User
```
PUT /api/user/{userId}/activate
Authorization: Bearer <admin_token>
```

### 8. Delete User
```
DELETE /api/user/{userId}
Authorization: Bearer <admin_token>
```
Permanently removes user from system.

---

## 🛡️ Authorization Policies

### Built-in Policies in Program.cs

| Policy | Required Roles | Use Case |
|--------|----------------|----------|
| `AdminOnly` | Admin | Admin-only operations |
| `ProjectManagerAndAbove` | Admin, ProjectManager | Project creation/editing |
| `SiteEngineersAndAbove` | Admin, ProjectManager, SiteEngineer | Field operations |
| `CanManageSafety` | Admin, SafetyOfficer, SiteEngineer | Safety inspections |

### Using Policies in Controllers
```csharp
[Authorize(Policy = "AdminOnly")]
public async Task<IActionResult> ManageUsers() { }

[Authorize(Roles = "Admin,ProjectManager")]
public async Task<IActionResult> CreateProject() { }
```

---

## 🔍 Role Authorization Helper

The `RoleAuthorizationHelper` class provides utility methods for checking permissions in code:

### In Controllers
```csharp
// Get current user's info
int? userId = RoleAuthorizationHelper.GetUserId(User);
string? userRole = RoleAuthorizationHelper.GetUserRole(User);
string? userEmail = RoleAuthorizationHelper.GetUserEmail(User);

// Check permissions
if (RoleAuthorizationHelper.CanManageUsers(User)) { }
if (RoleAuthorizationHelper.CanManageInventory(User)) { }
if (RoleAuthorizationHelper.CanRecordSafetyInspection(User)) { }
if (RoleAuthorizationHelper.CanManageTasks(User)) { }
if (RoleAuthorizationHelper.CanViewProject(User)) { }
```

### In Razor Pages
```csharp
@using ConstructionProject.Helpers
@using ConstructionProject.Models

@if (RoleAuthorizationHelper.HasRole(User, UserRole.Admin))
{
    <button class="btn btn-danger">Delete User</button>
}

@if (RoleAuthorizationHelper.CanManageInventory(User))
{
    <div class="inventory-management">
        <!-- Inventory UI -->
    </div>
}
```

---

## 📋 Permission Matrix

```
                    | Admin | PM   | SE   | Contractor | Safety |
--------------------|-------|------|------|------------|--------|
Login               | ✓     | ✓    | ✓    | ✓          | ✓      |
Register Users      | ✓     | ✗    | ✗    | ✗          | ✗      |
View All Users      | ✓     | ✗    | ✗    | ✗          | ✗      |
Update User Role    | ✓     | ✗    | ✗    | ✗          | ✗      |
Deactivate User     | ✓     | ✗    | ✗    | ✗          | ✗      |
View Projects       | ✓     | ✓    | ✓    | ✗          | ✓      |
Create Project      | ✓     | ✓    | ✗    | ✗          | ✗      |
Manage Inventory    | ✓     | ✓    | ✓    | ✗          | ✗      |
Record Safety       | ✓     | ✗    | ✓    | ✗          | ✓      |
Manage Tasks        | ✓     | ✓    | ✓    | ✗          | ✗      |
```

---

## 🔐 JWT Configuration (appsettings.json)

```json
"JwtSettings": {
  "SecretKey": "your-super-secret-jwt-key-must-be-at-least-32-characters-long-for-hs256",
  "Issuer": "ConstructionProjectAPI",
  "Audience": "ConstructionProjectUsers",
  "ExpiryMinutes": 60
}
```

### Security Checklist
- [ ] Change `SecretKey` to a unique, strong value (32+ characters)
- [ ] Never commit real secrets; use user secrets or environment variables in production
- [ ] Tokens expire after 60 minutes (configurable)
- [ ] Token validation checks:
  - Signature verification
  - Issuer validation
  - Audience validation
  - Expiration check
  - Clock skew tolerance: 0 seconds (strict)

---

## 🔐 Password Security

- Passwords are hashed using **BCrypt.Net**
- Plain passwords are **never stored** in the database
- On login, BCrypt verifies the provided password against the stored hash
- Only hashed values in `PasswordHash` field

---

## 🚀 Authentication Flow Diagram

```
┌──────────────┐
│ Login Request│
│ Email/Pass   │
└──────┬───────┘
       │
       ▼
┌─────────────────────┐
│ Validate Credentials│
│ (Check DB)          │
└────────┬────────────┘
         │ (Valid?)
         ├─► (No) ──► 401 Unauthorized
         │
         ▼ (Yes)
┌──────────────────────┐
│ Generate JWT Token   │
│ (with claims)        │
└────────┬─────────────┘
         │
         ▼
┌──────────────────────┐
│ Return Token to User │
└────────┬─────────────┘
         │
         ▼ (Use token)
┌────────────────────────────┐
│ Future API Requests        │
│ Authorization: Bearer TOKEN│
└────────┬───────────────────┘
         │
         ▼
┌──────────────────────┐
│ Validate Token       │
│ - Signature check    │
│ - Expiration check   │
│ - Role verification  │
└────────┬─────────────┘
         │ (Valid?)
         ├─► (No) ──► 401 Unauthorized / 403 Forbidden
         │
         ▼ (Yes)
┌──────────────────────┐
│ Process Request      │
└──────────────────────┘
```

---

## ⚠️ Common Issues & Solutions

| Issue | Cause | Solution |
|-------|-------|----------|
| Login returns 401 | Invalid email/password | Verify credentials, ensure user is Active |
| Token rejected | Token expired | Login again to get new token |
| 403 Forbidden | Insufficient role | User role doesn't have permission |
| CORS errors | Frontend on different domain | Configure CORS in Program.cs |
| Token invalid | Wrong secret key | Verify SecretKey matches in appsettings.json |
| User can't login | Account deactivated | Admin must activate user with PUT /api/user/{id}/activate |

---

## 🧪 Testing Authentication

### Using Swagger UI
1. Run the application
2. Navigate to `/swagger`
3. Click **Authorize** button
4. Paste your JWT token (with `Bearer ` prefix)
5. Test protected endpoints

### Using cURL
```bash
# Get token
TOKEN=$(curl -s -X POST http://localhost:5000/api/user/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@construction.com","password":"Admin@123"}' \
  | jq -r '.token')

# Use token
curl -X GET http://localhost:5000/api/user \
  -H "Authorization: Bearer $TOKEN"
```

### Using Postman
1. Login endpoint returns token in `token` field
2. Set token in Authorization tab: Type = Bearer Token
3. Paste the token value
4. Make requests to protected endpoints

---

## 📁 Key Files Reference

| File | Purpose |
|------|---------|
| `Models/AppUser.cs` | User entity definition |
| `Models/UserRole.cs` | Role enum (Admin, ProjectManager, etc.) |
| `Services/UserService.cs` | User CRUD operations |
| `Services/JwtTokenService.cs` | JWT token generation |
| `Controllers/UserController.cs` | Authentication endpoints |
| `Helpers/RoleAuthorizationHelper.cs` | Authorization utility methods |
| `Program.cs` | JWT & authorization configuration |
| `appsettings.json` | JWT settings |
| `DTOs/UserDtos.cs` | Request/response DTOs |

---

## 🎯 Next Steps

1. **Change JWT Secret**: Update `appsettings.json` with a unique secret key
2. **Initialize Database**: Run migrations to create the database
3. **Create Admin User**: Register the first admin account
4. **Assign Roles**: Create users with appropriate roles
5. **Enable Swagger**: Visit `/swagger` to test endpoints
6. **Integrate Frontend**: Use the token in Authorization headers

---

## 📞 Support References

- **JWT Documentation**: https://jwt.io/
- **ASP.NET Core Authentication**: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/
- **BCrypt.NET**: https://github.com/BcryptNet/bcrypt.net
- **OpenAPI/Swagger**: https://swagger.io/


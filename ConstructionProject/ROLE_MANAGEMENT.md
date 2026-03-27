# User Role Management System

## Overview

The Construction Project Management System implements a comprehensive role-based access control (RBAC) system with JWT authentication. This document outlines how to manage user roles and permissions.

## User Roles

The system supports five distinct roles:

| Role | Description | Primary Responsibilities |
|------|-------------|--------------------------|
| **Admin** | System administrator | Manage users, assign roles, system configuration |
| **ProjectManager** | Project lead | Create/edit projects, assign tasks, manage teams |
| **SiteEngineer** | Field engineer | Monitor site activities, manage inventory, report progress |
| **Contractor** | External contractor | Execute assigned tasks, report progress |
| **SafetyOfficer** | Safety compliance officer | Record safety inspections, compliance checks, issue enforcement |

## API Endpoints

### Authentication

#### Login
```
POST /api/user/login
Content-Type: application/json

{
  "email": "user@construction.com",
  "password": "password123"
}

Response (200 OK):
{
  "userId": 1,
  "name": "John Doe",
  "email": "user@construction.com",
  "role": "ProjectManager",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Note:** The token must be included in all subsequent requests as a Bearer token in the Authorization header:
```
Authorization: Bearer <token>
```

### User Management (Admin Only)

#### Register New User
```
POST /api/user/register
Authorization: Bearer <admin_token>
Content-Type: application/json

{
  "name": "Jane Doe",
  "email": "jane@construction.com",
  "password": "SecurePassword123!",
  "role": 2  // 0=Admin, 1=ProjectManager, 2=SiteEngineer, 3=Contractor, 4=SafetyOfficer
}
```

#### Get All Users
```
GET /api/user
Authorization: Bearer <admin_token>
```

#### Get User by ID
```
GET /api/user/{userId}
Authorization: Bearer <admin_token>
```

#### Get Users by Role
```
GET /api/user/role/{role}
Authorization: Bearer <admin_or_project_manager_token>

{role} values: Admin, ProjectManager, SiteEngineer, Contractor, SafetyOfficer
```

#### Update User Role
```
PUT /api/user/{userId}/role
Authorization: Bearer <admin_token>
Content-Type: application/json

{
  "newRole": 1  // 0=Admin, 1=ProjectManager, 2=SiteEngineer, 3=Contractor, 4=SafetyOfficer
}
```

#### Deactivate User
```
PUT /api/user/{userId}/deactivate
Authorization: Bearer <admin_token>
```

#### Activate User
```
PUT /api/user/{userId}/activate
Authorization: Bearer <admin_token>
```

#### Delete User
```
DELETE /api/user/{userId}
Authorization: Bearer <admin_token>
```

## Authorization Policies

The system includes built-in authorization policies:

| Policy | Required Roles | Use Case |
|--------|---|----------|
| `AdminOnly` | Admin | System administration operations |
| `ProjectManagerAndAbove` | Admin, ProjectManager | Project management operations |
| `SiteEngineersAndAbove` | Admin, ProjectManager, SiteEngineer | Site operations |
| `CanManageSafety` | Admin, SafetyOfficer, SiteEngineer | Safety and compliance operations |

### Using Policies in Controllers

```csharp
[Authorize(Policy = "AdminOnly")]
public async Task<IActionResult> AdminOnlyEndpoint()
{
    // Only admins can access this
}

[Authorize(Policy = "ProjectManagerAndAbove")]
public async Task<IActionResult> ProjectManagementEndpoint()
{
    // Admins and project managers can access
}
```

## Role Authorization Helper

The `RoleAuthorizationHelper` class provides utility methods for role-based authorization in your code:

```csharp
using ConstructionProject.Helpers;

// Check if user has specific role
bool isAdmin = RoleAuthorizationHelper.HasRole(User, UserRole.Admin);

// Check if user has any of multiple roles
bool canManage = RoleAuthorizationHelper.HasRole(User, "Admin", "ProjectManager");

// Get user claims
int? userId = RoleAuthorizationHelper.GetUserId(User);
string? role = RoleAuthorizationHelper.GetUserRole(User);
string? email = RoleAuthorizationHelper.GetUserEmail(User);

// Check specific permissions
bool canManageUsers = RoleAuthorizationHelper.CanManageUsers(User);
bool canViewProject = RoleAuthorizationHelper.CanViewProject(User);
bool canManageInventory = RoleAuthorizationHelper.CanManageInventory(User);
bool canRecordSafety = RoleAuthorizationHelper.CanRecordSafetyInspection(User);
bool canManageTasks = RoleAuthorizationHelper.CanManageTasks(User);
```

## Permission Matrix

| Feature | Admin | ProjectManager | SiteEngineer | Contractor | SafetyOfficer |
|---------|:---:|:---:|:---:|:---:|:---:|
| Manage Users | ✓ | | | | |
| Create Project | ✓ | ✓ | | | |
| Assign Tasks | ✓ | ✓ | ✓ | | |
| Manage Inventory | ✓ | ✓ | ✓ | | |
| View Projects | ✓ | ✓ | ✓ | ✓ | ✓ |
| View Assigned Tasks | ✓ | ✓ | ✓ | ✓ | |
| Record Progress | ✓ | ✓ | ✓ | ✓ | |
| Record Safety Inspection | ✓ | | ✓ | | ✓ |
| View Reports | ✓ | ✓ | ✓ | | |

## JWT Token Configuration

JWT tokens are configured in `appsettings.json`:

```json
"JwtSettings": {
  "SecretKey": "your-super-secret-jwt-key-must-be-at-least-32-characters-long",
  "Issuer": "ConstructionProjectAPI",
  "Audience": "ConstructionProjectUsers",
  "ExpiryMinutes": 60
}
```

### Important Security Notes

⚠️ **CHANGE THE SECRET KEY BEFORE PRODUCTION**
- The default secret key in appsettings.json should be replaced with a strong, random key
- Store the secret key securely (use Azure Key Vault or similar for production)
- Never commit production secrets to version control

### Token Expiration

- Default expiration: 60 minutes
- Adjust `ExpiryMinutes` in appsettings.json to change the duration
- Implement token refresh logic for long-lived sessions

## Implementation Examples

### Example 1: Protecting a Controller Action

```csharp
[HttpPost("createTask")]
[Authorize(Roles = "Admin,ProjectManager")]
public async Task<IActionResult> CreateTask([FromBody] ProjectTaskDto dto)
{
    if (!RoleAuthorizationHelper.CanManageTasks(User))
        return Forbid("You don't have permission to manage tasks");
    
    var task = await _service.CreateTaskAsync(dto);
    return Ok(task);
}
```

### Example 2: Role-Based Logic in Service

```csharp
public async Task<SafetyInspection> RecordInspectionAsync(SafetyInspection inspection, ClaimsPrincipal user)
{
    if (!RoleAuthorizationHelper.CanRecordSafetyInspection(user))
        throw new UnauthorizedAccessException("Not authorized to record inspections");
    
    _db.SafetyInspections.Add(inspection);
    await _db.SaveChangesAsync();
    return inspection;
}
```

### Example 3: Getting Current User Information

```csharp
public IActionResult GetCurrentUserInfo()
{
    var userId = RoleAuthorizationHelper.GetUserId(User);
    var role = RoleAuthorizationHelper.GetUserRole(User);
    var email = RoleAuthorizationHelper.GetUserEmail(User);
    
    return Ok(new { userId, role, email });
}
```

## Default Admin Credentials

After the first migration, a default admin user is seeded into the database:

- **Email:** `admin@construction.com`
- **Password:** `Admin@123`
- **Role:** Admin

⚠️ **IMPORTANT:** Change the admin password immediately after first login!

## Password Security

- Passwords are hashed using BCrypt.NET
- Plain text passwords are never stored
- Use strong passwords:
  - Minimum 6 characters
  - Mix of uppercase, lowercase, numbers, and special characters recommended
  - Example: `SecurePass123!`

## Best Practices

1. **Always validate user roles** before sensitive operations
2. **Use authorization policies** rather than manual role checks when possible
3. **Log authentication events** for audit trails
4. **Refresh tokens** periodically for long-running sessions
5. **Protect the JWT secret key** - store in secure configuration
6. **Implement role-based auditing** to track who accessed what
7. **Regularly review user permissions** and remove unnecessary access
8. **Use HTTPS** for all API communications to protect tokens in transit

## Troubleshooting

### Token Expired
- Request a new token by logging in again
- Implement token refresh endpoint for better UX

### Unauthorized (401)
- Ensure token is included in Authorization header with "Bearer " prefix
- Verify token hasn't expired
- Check that the user is active (not deactivated)

### Forbidden (403)
- User is authenticated but lacks required role
- Contact an admin to update user role

### Invalid Token
- Token may be malformed or tampered with
- Clear auth state and login again

## Future Enhancements

- [ ] Implement token refresh tokens
- [ ] Add multi-factor authentication (MFA)
- [ ] Implement role-based audit logging
- [ ] Add dynamic permissions per role
- [ ] Implement project-level permissions
- [ ] Add session management and concurrent login limits

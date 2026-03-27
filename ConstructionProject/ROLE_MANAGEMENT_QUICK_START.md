# User Role Management - Quick Reference

## 5-Minute Setup

### 1. Login to Get JWT Token
```bash
curl -X POST https://yourapi.com/api/user/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@construction.com",
    "password": "Admin@123"
  }'
```

**Save the token from response for all subsequent requests**

### 2. Create New Users (Admin Only)
```bash
curl -X POST https://yourapi.com/api/user/register \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Smith",
    "email": "john@construction.com",
    "password": "SecurePass123!",
    "role": 1
  }'
```

Role Numbers:
- 0 = Admin
- 1 = ProjectManager
- 2 = SiteEngineer
- 3 = Contractor
- 4 = SafetyOfficer

### 3. Manage Existing Users

#### Get All Users
```bash
curl -X GET https://yourapi.com/api/user \
  -H "Authorization: Bearer YOUR_TOKEN"
```

#### Get Users by Role
```bash
curl -X GET "https://yourapi.com/api/user/role/SiteEngineer" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

#### Update User Role
```bash
curl -X PUT https://yourapi.com/api/user/5/role \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"newRole": 2}'
```

#### Deactivate User
```bash
curl -X PUT https://yourapi.com/api/user/5/deactivate \
  -H "Authorization: Bearer YOUR_TOKEN"
```

#### Activate User
```bash
curl -X PUT https://yourapi.com/api/user/5/activate \
  -H "Authorization: Bearer YOUR_TOKEN"
```

#### Delete User
```bash
curl -X DELETE https://yourapi.com/api/user/5 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

## Common API Responses

### Successful Login (200 OK)
```json
{
  "userId": 1,
  "name": "Admin",
  "email": "admin@construction.com",
  "role": "Admin",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### User Created (201 Created)
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

### Error Responses

#### Invalid Credentials (401 Unauthorized)
```json
{
  "message": "Invalid email or password."
}
```

#### Email Already Exists (409 Conflict)
```json
{
  "message": "Email already exists."
}
```

#### Insufficient Permissions (403 Forbidden)
```json
{
  "message": "You are not authorized to perform this action."
}
```

#### Not Found (404 Not Found)
```json
{
  "message": "User 999 not found."
}
```

## Role Capabilities at a Glance

```
Admin ──────────────────────────
├─ Manage all users
├─ Create projects
├─ Assign tasks & equipment
├─ Manage inventory
└─ View all reports

ProjectManager ─────────────────
├─ Create & manage projects
├─ Assign site engineers
├─ Monitor progress
└─ Manage team inventory

SiteEngineer ───────────────────
├─ Execute assigned tasks
├─ Manage field inventory
├─ Record safety inspections
└─ Update task progress

SafetyOfficer ──────────────────
├─ Record safety inspections
├─ Track compliance status
└─ Generate compliance reports

Contractor ─────────────────────
├─ View assigned tasks
└─ Report task progress
```

## Code Usage Examples

### In Razor Pages
```csharp
@using ConstructionProject.Helpers
@using ConstructionProject.Models

@if (RoleAuthorizationHelper.HasRole(User, UserRole.Admin))
{
    <button class="btn btn-danger">Delete User</button>
}
```

### In Controllers
```csharp
[Authorize(Roles = "Admin,ProjectManager")]
public async Task<IActionResult> CreateProject([FromBody] ProjectDto dto)
{
    var userId = RoleAuthorizationHelper.GetUserId(User);
    var role = RoleAuthorizationHelper.GetUserRole(User);
    
    // Your logic here
}
```

### Checking Permissions
```csharp
if (RoleAuthorizationHelper.CanManageUsers(User))
{
    // Admin only operations
}

if (RoleAuthorizationHelper.CanManageInventory(User))
{
    // Inventory operations
}

if (RoleAuthorizationHelper.CanRecordSafetyInspection(User))
{
    // Safety operations
}
```

## Configuration Checklist

- [ ] Change JWT secret key in appsettings.json
- [ ] Update JWT issuer/audience if needed
- [ ] Set appropriate token expiry (default: 60 minutes)
- [ ] Update default admin password after first login
- [ ] Configure HTTPS for production
- [ ] Enable CORS if frontend is on different domain
- [ ] Set up audit logging for user management operations
- [ ] Create backup/recovery procedures for admin account

## Swagger Documentation

Visit `/swagger` when running locally to:
- See all available endpoints
- Test API calls directly
- View request/response schemas
- Generate Bearer token for testing

Click "Authorize" button and paste your token to test protected endpoints.

## Secure Token Handling

### For Frontend Developers
1. Get token from login endpoint
2. Store in secure storage (not localStorage for sensitive apps):
   ```javascript
   // Store token
   sessionStorage.setItem('authToken', response.token);
   
   // Retrieve for requests
   const token = sessionStorage.getItem('authToken');
   ```
3. Include in all API requests:
   ```javascript
   fetch('/api/user', {
     headers: {
       'Authorization': `Bearer ${token}`
     }
   })
   ```
4. Handle token expiration and refresh

### For Backend Developers
1. Never log tokens
2. Always use HTTPS in production
3. Validate token signature on every request
4. Implement token refresh mechanism
5. Log access attempts (especially failures)
6. Implement rate limiting on login endpoint

## Troubleshooting Checklist

| Issue | Solution |
|-------|----------|
| Login fails with valid credentials | Check user is Active status |
| Token shows as invalid | Verify token not expired; re-login |
| No permission despite having role | Check authorization policy; verify role name |
| CORS errors with frontend | Configure CORS in Program.cs |
| Token not being recognized | Verify Bearer prefix in header |
| JWT secret not working | Check appsettings.json for typos |

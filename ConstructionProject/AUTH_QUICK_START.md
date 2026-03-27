# 🔐 AUTHENTICATION & ROLES - QUICK START

## What's New ✨

Your application now has:
- ✅ User login/registration
- ✅ Role-based access control
- ✅ Profile management
- ✅ Role-specific dashboards

---

## 🚀 Quick Start

### Step 1: Run Application
```bash
dotnet run
```

### Step 2: Visit Application
```
https://localhost:7272/
```

### Step 3: You'll See
```
🏗️ Construction Site Management

         [ Login ]  [ Sign Up ]
```

---

## 📝 Registration Form

### Fill Out:
```
👤 Full Name:        John Smith
📧 Email:           john@company.com
🔒 Password:        SecurePass123 (min 6 chars)
👔 Role:            Select from dropdown ↓
```

### Role Options:
```
0️⃣  Admin - Full system access
1️⃣  ProjectManager - Manage projects & contractors
2️⃣  SiteEngineer - Field operations & progress logging
3️⃣  Contractor - Limited contractor-specific access
4️⃣  SafetyOfficer - Safety inspections & compliance
```

### Click: "Create My Account"

---

## 🔓 Login

### Step 1: On Login Page
```
📧 Email:    john@company.com
🔒 Password: SecurePass123
```

### Step 2: Click "Login to Your Account"

### Step 3: Dashboard Opens with Your Role!

---

## 👥 Dashboard by Role

### 🟢 ADMIN Dashboard
```
📋 Projects          [View All] [+ New]
👷 Contractors       [View All] [+ New]
📊 Progress          [View All]
🔧 Inventory         [View All] [+ Add]
⚠️ Safety            [View All] [+ New]
👥 User Management   [Access]
```

### 🟠 PROJECT MANAGER Dashboard
```
📋 Projects          [View All] [+ New]
👷 Contractors       [View All] [+ New]
📊 Progress          [View All]
🔧 Inventory         [View All] [+ Add]
⚠️ Safety            [View All]
```

### 🔵 SITE ENGINEER Dashboard
```
📊 Progress          [View All] [+ Log]
🔧 Inventory         [View All] [+ Add]
⚠️ Safety            [View All]
```

### 🟣 SAFETY OFFICER Dashboard
```
⚠️ Safety            [View All] [+ New]
📊 Progress          [View]
```

### 🟡 CONTRACTOR Dashboard
```
📋 Projects          [View Assigned]
(Limited access to contractor-specific data)
```

---

## 👤 Your Profile

### Access:
1. Look at navbar (top-right)
2. Click your email with role badge
3. Select "My Profile"

### You'll See:
```
👤 User Information
   Name:     John Smith
   Email:    john@company.com
   Role:     ProjectManager
   Created:  March 25, 2026

📋 Role Permissions
   ✓ Create projects
   ✓ Manage contractors
   ✓ View progress
   ✓ Manage inventory
   ✗ Cannot manage users

✓ Account Status: Active
```

---

## 🚪 Logout

### Click Dropdown (top-right)
```
👤 john@company.com [ProjectManager] ▼
├─ My Profile
├─────────────────
└─ Logout  ← Click here
```

### You'll Be Redirected to Login Page

---

## 🧪 Test Users to Create

### Test Admin Account
```
Name:     Admin User
Email:    admin@company.com
Password: admin123
Role:     Admin (0)
```

### Test ProjectManager Account
```
Name:     Project Manager
Email:    manager@company.com
Password: manager123
Role:     ProjectManager (1)
```

### Test SiteEngineer Account
```
Name:     Site Engineer
Email:    engineer@company.com
Password: engineer123
Role:     SiteEngineer (2)
```

### Test SafetyOfficer Account
```
Name:     Safety Officer
Email:    safety@company.com
Password: safety123
Role:     SafetyOfficer (4)
```

### Test Contractor Account
```
Name:     Contractor Name
Email:    contractor@company.com
Password: contractor123
Role:     Contractor (3)
```

---

## 🔐 Security Features

✅ Passwords hashed with BCrypt
✅ JWT tokens for authentication
✅ HttpOnly cookies (no JavaScript access)
✅ HTTPS Secure flag
✅ CSRF protection with SameSite cookies
✅ Email validation
✅ Role-based access control

---

## 📊 How It Works

### Registration:
```
User fills form → Email unique check → Password hashed 
→ User saved to database → Redirected to login
```

### Login:
```
Email & password submitted → Verified against database 
→ JWT token generated → Cookie set → Dashboard shown
```

### Dashboard:
```
System reads user role → Shows only role-specific sections
→ User can interact with allowed features
```

---

## 💡 Pro Tips

### Tip 1: Remember Your Passwords
All test accounts use simple passwords for easy testing.

### Tip 2: Different Roles = Different Views
Try logging in with different roles to see different dashboards.

### Tip 3: Check Profile
Visit your profile to see what you can access with your role.

### Tip 4: Role Badge
Your role is displayed in the navbar next to your email.

---

## ❓ Common Questions

### Q: Can I change roles after registration?
**A:** Not yet, but admin can modify roles in future updates.

### Q: What if I forget my password?
**A:** Password reset feature can be added later.

### Q: Are passwords stored securely?
**A:** Yes! Hashed with BCrypt + salted for security.

### Q: Can users see each other's data?
**A:** Only based on their role permissions.

### Q: How long does login session last?
**A:** 1 hour. After that, you'll need to login again.

---

## 🎯 Role Permission Summary

| Feature | Admin | PM | Engineer | Safety | Contractor |
|---------|-------|----|---------|---------|----|
| Create Projects | ✅ | ✅ | ❌ | ❌ | ❌ |
| Edit Projects | ✅ | ✅ | ❌ | ❌ | ❌ |
| Manage Contractors | ✅ | ✅ | ❌ | ❌ | ❌ |
| View Projects | ✅ | ✅ | ✅ | ✅ | ✅ |
| Log Progress | ✅ | ✅ | ✅ | ✅ | ❌ |
| Create Safety Reports | ✅ | ✅ | ✅ | ✅ | ❌ |
| Manage Inventory | ✅ | ✅ | ✅ | ❌ | ❌ |

---

## 🎉 You're All Set!

**Your application now has:**
- Complete user authentication
- Role-based access control
- Professional login/signup interface
- User profiles with permissions
- Secure password handling
- Session management

**Ready to use!** 🚀

---

*For detailed implementation info, see: AUTHENTICATION_AND_ROLES_IMPLEMENTATION.md*

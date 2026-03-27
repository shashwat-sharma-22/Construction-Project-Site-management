# Frontend Views Quick Reference

## File Structure
```
ConstructionProject/
├── Views/
│   ├── Home/
│   │   └── Index.cshtml          # Dashboard
│   ├── Project/
│   │   ├── Index.cshtml          # List all projects
│   │   ├── Create.cshtml         # Create new project
│   │   ├── Edit.cshtml           # Edit project
│   │   └── Details.cshtml        # View project details
│   ├── Contractor/
│   │   ├── Index.cshtml          # List all contractors
│   │   ├── Create.cshtml         # Add new contractor
│   │   ├── Edit.cshtml           # Edit contractor
│   │   └── Details.cshtml        # View contractor details
│   ├── Progress/
│   │   ├── Index.cshtml          # List progress reports
│   │   ├── Create.cshtml         # Log new progress
│   │   ├── Edit.cshtml           # Edit progress
│   │   └── Details.cshtml        # View progress details
│   ├── Inventory/
│   │   ├── Index.cshtml          # List equipment & materials
│   │   └── Create.cshtml         # Add inventory item
│   ├── Safety/
│   │   ├── Index.cshtml          # List safety inspections
│   │   ├── Create.cshtml         # Create inspection report
│   │   ├── Edit.cshtml           # Edit inspection
│   │   └── Details.cshtml        # View inspection details
│   └── Shared/
│       └── _Layout.cshtml        # Updated navigation
```

## Controller Action Requirements

### ProjectController
- `Index()` - Return `IEnumerable<Project>`
- `Create(Project)` - POST action
- `Edit(int id, Project)` - POST action
- `Details(int id)` - Return single `Project`
- `Delete(int id)` - DELETE action

### ContractorController
- `Index()` - Return `IEnumerable<Contractor>`
- `Create(Contractor)` - POST action
- `Edit(int id, Contractor)` - POST action
- `Details(int id)` - Return single `Contractor`
- `Delete(int id)` - DELETE action

### ProgressController
- `Index()` - Return `IEnumerable<Progress>`
- `Create(Progress)` - POST action
- `Edit(int id, Progress)` - POST action
- `Details(int id)` - Return single `Progress`
- `Delete(int id)` - DELETE action

### InventoryController
- `Index()` - Return equipment & material lists
- `Create()` - GET & POST for adding items

### SafetyController
- `Index()` - Return `IEnumerable<SafetyInspection>`
- `Create(SafetyInspection)` - POST action
- `Edit(int id, SafetyInspection)` - POST action
- `Details(int id)` - Return single `SafetyInspection`
- `Delete(int id)` - DELETE action

## URL Patterns
```
GET  /                              → Home Dashboard
GET  /Project                       → Projects List
GET  /Project/Create               → Create Project Form
POST /Project/Create               → Submit New Project
GET  /Project/Details/1            → View Project #1
GET  /Project/Edit/1               → Edit Project #1
POST /Project/Edit/1               → Submit Project #1 Changes
POST /Project/Delete/1             → Delete Project #1

GET  /Contractor                   → Contractors List
GET  /Contractor/Create            → Add Contractor Form
POST /Contractor/Create            → Submit New Contractor
GET  /Contractor/Details/1         → View Contractor #1
GET  /Contractor/Edit/1            → Edit Contractor #1
POST /Contractor/Edit/1            → Submit Contractor #1 Changes
POST /Contractor/Delete/1          → Delete Contractor #1

GET  /Progress                     → Progress Reports List
GET  /Progress/Create              → Log Progress Form
POST /Progress/Create              → Submit New Progress
GET  /Progress/Details/1           → View Progress #1
GET  /Progress/Edit/1              → Edit Progress #1
POST /Progress/Edit/1              → Submit Progress #1 Changes
POST /Progress/Delete/1            → Delete Progress #1

GET  /Inventory                    → Inventory List
GET  /Inventory/Create             → Add Item Form
POST /Inventory/Create             → Submit New Item

GET  /Safety                       → Safety Inspections List
GET  /Safety/Create                → Create Inspection Form
POST /Safety/Create                → Submit New Inspection
GET  /Safety/Details/1             → View Inspection #1
GET  /Safety/Edit/1                → Edit Inspection #1
POST /Safety/Edit/1                → Submit Inspection #1 Changes
POST /Safety/Delete/1              → Delete Inspection #1
```

## Quick Start

1. **Run the Application**
   ```bash
   dotnet run
   ```

2. **Navigate to Home**
   - Visit: `https://localhost:7XXX/`

3. **Explore Modules**
   - Click cards on dashboard or use navigation bar
   - Try creating new records in each module

4. **Test Features**
   - Create → View → Edit → Delete workflow

## Styling Classes Used

| Class | Purpose |
|-------|---------|
| `btn-primary` | Main action buttons |
| `btn-warning` | Edit buttons |
| `btn-danger` | Delete buttons |
| `btn-info` | View/Details buttons |
| `btn-secondary` | Cancel buttons |
| `badge bg-success` | Success status |
| `badge bg-danger` | Error/Non-compliant status |
| `table-hover` | Interactive table rows |
| `form-control` | Input fields |
| `form-select` | Dropdown fields |
| `card` | Content containers |

## Color Scheme

| Module | Colors |
|--------|--------|
| Projects | Purple to Pink gradient |
| Contractors | Pink to Red gradient |
| Progress | Light Blue gradient |
| Inventory | Orange to Yellow gradient |
| Safety | Light Blue to Pink gradient |

## Tips & Tricks

✅ All forms include validation
✅ Tables are responsive on mobile
✅ Confirmation dialogs on delete actions
✅ Breadcrumb-style button navigation
✅ Empty state messages for better UX
✅ Status badges for quick visual reference
✅ Date pickers for date fields
✅ Tab-based interface for Inventory

## Need to Modify?

- **Layout**: Edit `Views/Shared/_Layout.cshtml`
- **Styles**: Add CSS to `wwwroot/css/site.css`
- **Colors**: Modify Bootstrap classes in view files
- **Icons**: Replace emoji with Font Awesome or Bootstrap Icons
- **Forms**: Add/remove fields in Create/Edit views

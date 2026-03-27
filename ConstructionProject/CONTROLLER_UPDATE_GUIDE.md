# Controller Update Guide - Converting API to View-Based

Your project was originally set up as an API with controllers returning JSON. To use the new frontend views, you need to update your controllers to return Views instead of JSON responses. This guide shows how.

## Basic Pattern Conversion

### From API to MVC

**BEFORE (API Style):**
```csharp
[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    [HttpGet("getProjectDetails/{id}")]
    public async Task<IActionResult> GetProjectDetails(int id)
    {
        return Ok(project);  // Returns JSON
    }
}
```

**AFTER (MVC Style):**
```csharp
[Route("[controller]")]
public class ProjectController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var projects = await _service.GetAllProjectsAsync();
        return View(projects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var project = await _service.GetProjectDetailsAsync(id);
        if (project == null) return NotFound();
        return View(project);
    }
}
```

---

## ProjectController - Complete Example

```csharp
using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _service;

        public ProjectController(ProjectService service)
        {
            _service = service;
        }

        // GET: /Project
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await _service.GetAllProjectsAsync();
            return View(projects);
        }

        // GET: /Project/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _service.GetProjectDetailsAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        // GET: /Project/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Project/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Project project)
        {
            if (ModelState.IsValid)
            {
                var created = await _service.CreateProjectAsync(project);
                return RedirectToAction(nameof(Details), new { id = created.ProjectId });
            }
            return View(project);
        }

        // GET: /Project/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _service.GetProjectDetailsAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        // POST: /Project/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] Project project)
        {
            if (id != project.ProjectId) return BadRequest();

            if (ModelState.IsValid)
            {
                var success = await _service.UpdateProjectPlanAsync(id, project);
                if (!success) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // POST: /Project/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteProjectAsync(id);
            if (!success) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
```

---

## ContractorController - Complete Example

```csharp
using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    public class ContractorController : Controller
    {
        private readonly ContractorService _service;

        public ContractorController(ContractorService service)
        {
            _service = service;
        }

        // GET: /Contractor
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contractors = await _service.GetAllContractorsAsync();
            return View(contractors);
        }

        // GET: /Contractor/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var contractor = await _service.GetContractorDetailsAsync(id);
            if (contractor == null) return NotFound();
            return View(contractor);
        }

        // GET: /Contractor/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Contractor/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                var created = await _service.AddContractorAsync(contractor);
                return RedirectToAction(nameof(Details), new { id = created.ContractorId });
            }
            return View(contractor);
        }

        // GET: /Contractor/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var contractor = await _service.GetContractorDetailsAsync(id);
            if (contractor == null) return NotFound();
            return View(contractor);
        }

        // POST: /Contractor/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] Contractor contractor)
        {
            if (id != contractor.ContractorId) return BadRequest();

            if (ModelState.IsValid)
            {
                var success = await _service.UpdateContractorAsync(id, contractor);
                if (!success) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(contractor);
        }

        // POST: /Contractor/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteContractorAsync(id);
            if (!success) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
```

---

## ProgressController - Complete Example

```csharp
using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    public class ProgressController : Controller
    {
        private readonly ProgressService _service;

        public ProgressController(ProgressService service)
        {
            _service = service;
        }

        // GET: /Progress
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var progressReports = await _service.GetAllProgressAsync();
            return View(progressReports);
        }

        // GET: /Progress/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var progress = await _service.GetProgressDetailsAsync(id);
            if (progress == null) return NotFound();
            return View(progress);
        }

        // GET: /Progress/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Progress/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Progress progress)
        {
            if (ModelState.IsValid)
            {
                var created = await _service.LogProgressAsync(progress);
                return RedirectToAction(nameof(Details), new { id = created.ProgressId });
            }
            return View(progress);
        }

        // GET: /Progress/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var progress = await _service.GetProgressDetailsAsync(id);
            if (progress == null) return NotFound();
            return View(progress);
        }

        // POST: /Progress/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] Progress progress)
        {
            if (id != progress.ProgressId) return BadRequest();

            if (ModelState.IsValid)
            {
                var success = await _service.UpdateProgressAsync(id, progress);
                if (!success) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(progress);
        }

        // POST: /Progress/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteProgressAsync(id);
            if (!success) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
```

---

## SafetyController - Complete Example

```csharp
using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    public class SafetyController : Controller
    {
        private readonly SafetyService _service;

        public SafetyController(SafetyService service)
        {
            _service = service;
        }

        // GET: /Safety
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var inspections = await _service.GetAllInspectionsAsync();
            return View(inspections);
        }

        // GET: /Safety/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var inspection = await _service.GetInspectionDetailsAsync(id);
            if (inspection == null) return NotFound();
            return View(inspection);
        }

        // GET: /Safety/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Safety/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SafetyInspection inspection)
        {
            if (ModelState.IsValid)
            {
                var created = await _service.CreateInspectionAsync(inspection);
                return RedirectToAction(nameof(Details), new { id = created.InspectionId });
            }
            return View(inspection);
        }

        // GET: /Safety/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var inspection = await _service.GetInspectionDetailsAsync(id);
            if (inspection == null) return NotFound();
            return View(inspection);
        }

        // POST: /Safety/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] SafetyInspection inspection)
        {
            if (id != inspection.InspectionId) return BadRequest();

            if (ModelState.IsValid)
            {
                var success = await _service.UpdateInspectionAsync(id, inspection);
                if (!success) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(inspection);
        }

        // POST: /Safety/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteInspectionAsync(id);
            if (!success) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
```

---

## InventoryController - Complete Example

```csharp
using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    public class InventoryController : Controller
    {
        private readonly InventoryService _service;

        public InventoryController(InventoryService service)
        {
            _service = service;
        }

        // GET: /Inventory
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var equipment = await _service.GetAllEquipmentAsync();
            var materials = await _service.GetAllMaterialsAsync();
            
            ViewBag.Equipment = equipment;
            ViewBag.Materials = materials;
            
            return View();
        }

        // GET: /Inventory/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Inventory/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string itemType, [FromForm] Equipment equipment, [FromForm] Material material)
        {
            if (itemType == "equipment" && equipment != null)
            {
                await _service.AddEquipmentAsync(equipment);
            }
            else if (itemType == "material" && material != null)
            {
                await _service.AddMaterialAsync(material);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
```

---

## Required Service Methods

Make sure your services have these methods. If they don't exist, add them:

### ProjectService
```csharp
public async Task<IEnumerable<Project>> GetAllProjectsAsync()
public async Task<bool> DeleteProjectAsync(int id)
```

### ContractorService
```csharp
public async Task<IEnumerable<Contractor>> GetAllContractorsAsync()
public async Task<bool> UpdateContractorAsync(int id, Contractor contractor)
public async Task<bool> DeleteContractorAsync(int id)
```

### ProgressService
```csharp
public async Task<IEnumerable<Progress>> GetAllProgressAsync()
public async Task<Progress> GetProgressDetailsAsync(int id)
public async Task<Progress> LogProgressAsync(Progress progress)
public async Task<bool> UpdateProgressAsync(int id, Progress progress)
public async Task<bool> DeleteProgressAsync(int id)
```

### SafetyService
```csharp
public async Task<IEnumerable<SafetyInspection>> GetAllInspectionsAsync()
public async Task<SafetyInspection> GetInspectionDetailsAsync(int id)
public async Task<SafetyInspection> CreateInspectionAsync(SafetyInspection inspection)
public async Task<bool> UpdateInspectionAsync(int id, SafetyInspection inspection)
public async Task<bool> DeleteInspectionAsync(int id)
```

### InventoryService
```csharp
public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
public async Task<IEnumerable<Material>> GetAllMaterialsAsync()
public async Task AddEquipmentAsync(Equipment equipment)
public async Task AddMaterialAsync(Material material)
```

---

## Key Differences

| Aspect | API | MVC |
|--------|-----|-----|
| Route | `[Route("api/[controller]")]` | `[Route("[controller]")]` |
| Base Class | `ControllerBase` | `Controller` |
| Parameter Binding | `[FromBody]` | `[FromForm]` |
| Return Type | `Ok()`, `Json()` | `View()`, `RedirectToAction()` |
| HTTP Verbs | POST/GET/PUT/DELETE | GET/POST |
| Model Binding | JSON body | Form data |

---

## Testing Your Views

1. **Start the Application**
   ```bash
   dotnet run
   ```

2. **Navigate to** `https://localhost:7XXX/`

3. **Test Each Module**
   - Click dashboard cards or use navigation
   - Create new records
   - Edit existing records
   - Delete records

---

## Common Issues & Solutions

❌ **Issue**: Views not found (404)
✅ **Solution**: Ensure controller names match folder names (e.g., `ProjectController` → `/Views/Project/`)

❌ **Issue**: Model binding fails
✅ **Solution**: Use `[FromForm]` instead of `[FromBody]` in controller methods

❌ **Issue**: Service methods don't exist
✅ **Solution**: Add missing methods to your Service classes

❌ **Issue**: Redirect loops
✅ **Solution**: Check that your service methods return correct values

---

Have questions? Check the `FRONTEND_VIEWS_GUIDE.md` for more details!

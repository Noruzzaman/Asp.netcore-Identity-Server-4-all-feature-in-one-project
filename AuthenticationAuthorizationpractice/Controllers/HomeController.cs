using AuthenticationAuthorizationpractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;



namespace AuthenticationAuthorizationpractice.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IEmployeetRepository _employeetRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly AppDbContext _context;


        public HomeController(ILogger<HomeController> logger,
            IEmployeetRepository employeetRepository,
            RoleManager<IdentityRole> roleManager, 
            UserManager<IdentityUser> userManager,
            AppDbContext context)
        {

            _logger = logger;
            _employeetRepository = employeetRepository;
            _context = context;
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        // GET: Home/index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _employeetRepository.GetEmployees();
            if (posts == null)
            {
                return NotFound();
            }
            return View(posts);
        }



        // GET: Home/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var posts = await _employeetRepository.GetEmployeesByID(id);
            if (posts == null)
            {
                return NotFound();
            }
            var res1 = new EmployeeDTO();
            res1.Id = posts.Id;
            res1.Name = posts.Name;
            res1.LastName = posts.LastName;
            res1.Email = posts.Email;
            res1.Gender = posts.Gender;
            res1.IsActive = posts.IsActive;
            res1.Age = posts.Age;
            res1.IsMarried = posts.IsMarried;
            res1.Doj = posts.Doj;
            res1.UserID = posts.UserID;
            res1.UserNameOrEmail = posts.UserNameOrEmail;
            var user = await _userManager.FindByIdAsync(res1.UserID);
            var rolest = await _userManager.GetRolesAsync(user);
            res1.DesignationID = posts.DesignationID;

            string RoleNameF = "";
            foreach (var from in rolest)
            {
                RoleNameF = from;
            }
            var role = await _roleManager.FindByNameAsync(RoleNameF);
            res1.RoleID = role.Id;
            var roles = _roleManager.Roles;
            ViewBag.RoleViewModelList = new SelectList(roles, "Id", "Name");
            var model = new EmployeeDTO();
            model.DesignationsList = await _employeetRepository.GetDesignations();
            ViewBag.DeptList = new SelectList(model.DesignationsList, "Id", "Designations");
            return View(res1);

        }

        // GET: Home/Create
        public async Task<ActionResult> Create()
        {

            var roles = _roleManager.Roles;
            ViewBag.RoleViewModelList = new SelectList(roles, "Id", "Name");
            var model = new EmployeeDTO();
            model.DesignationsList = await _employeetRepository.GetDesignations();
            ViewBag.DeptList = new SelectList(model.DesignationsList, "Id", "Designations");
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmployeeDTO oEmployeeDTO)
        {
            try
            {
                int x = 0;
                if (oEmployeeDTO != null)
                {
                    var UserList = _employeetRepository.GetUserID(oEmployeeDTO.Email);
                    x = _employeetRepository.GetDuplicateEmployees(oEmployeeDTO);



                }
                if (x == 0)
                {
                    if (oEmployeeDTO != null)
                    {
                        var val = new Employee();
                        //  val.Id = oEmployeeDTO.Id;
                        val.Name = oEmployeeDTO.Name;
                        val.LastName = oEmployeeDTO.LastName;
                        val.Email = oEmployeeDTO.Email;
                        val.Age = oEmployeeDTO.Age;
                        val.Doj = oEmployeeDTO.Doj;
                        val.Gender = oEmployeeDTO.Gender;
                        val.IsMarried = oEmployeeDTO.IsMarried;
                        val.IsActive = oEmployeeDTO.IsActive;
                        val.DesignationID = oEmployeeDTO.DesignationID; 
                        await _employeetRepository.InsertEmployees(val);

                        string roleId = oEmployeeDTO.RoleID;
                        var role = await _roleManager.FindByIdAsync(roleId);
                        var IdValueUser = _employeetRepository.GetUserIDValue(oEmployeeDTO.Email);
                        string UserName = oEmployeeDTO.Email;

                        List<UserRole> authors = new List<UserRole>
                        {
                            new UserRole { UserId = IdValueUser, UserName =UserName }

                        };

                        for (int i = 0; i < authors.Count; i++)
                        {
                            var user = await _userManager.FindByIdAsync(authors[i].UserId);
                            IdentityResult result = null;
                            result = await _userManager.AddToRoleAsync(user, role.Name);
                        }


                    }

                   
                }
                else
                {
                    ViewBag.Message = "Email is already exists !";
                    var model = new EmployeeDTO();
                    model.DesignationsList = await _employeetRepository.GetDesignations();
                    ViewBag.DeptList = new SelectList(model.DesignationsList, "Id", "Designations");
                    return View();
                }



            }
            catch (Exception ex)
            {
                if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                {
                    return NotFound();
                }

                return BadRequest();
            }

            return RedirectToAction(nameof(Index));

        }

        // GET: Home/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            var posts = await _employeetRepository.GetEmployeesByID(id);
            if (posts == null)
            {
                return NotFound();
            }
            var res1=new EmployeeDTO();
            res1.Id = posts.Id;
            res1.Name = posts.Name;
            res1.LastName = posts.LastName;
            res1.Email = posts.Email;
            res1.Gender = posts.Gender;
            res1.IsActive = posts.IsActive;
            res1.Age = posts.Age;
            res1.IsMarried = posts.IsMarried;
            res1.Doj = posts.Doj;
            res1.UserID = posts.UserID;
            res1.UserNameOrEmail = posts.UserNameOrEmail;
            var user = await _userManager.FindByIdAsync(res1.UserID);
            var rolest = await _userManager.GetRolesAsync(user);
            res1.DesignationID = posts.DesignationID;

            string RoleNameF = "";
            foreach (var from in rolest)
            {
                RoleNameF = from;
            }
            var role = await _roleManager.FindByNameAsync(RoleNameF);
            if (role != null)
            {
                res1.RoleID = role.Id;
            }
           
            var roles = _roleManager.Roles;
            ViewBag.RoleViewModelList = new SelectList(roles, "Id", "Name");
            var model = new EmployeeDTO();
            model.DesignationsList = await _employeetRepository.GetDesignations();
            ViewBag.DeptList = new SelectList(model.DesignationsList, "Id", "Designations");
            return View(res1);

        }

        [Authorize(Policy = "EditEmployeePolicy")]
        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeDTO oEmployeeDTO)
        {

            try
            {

                if (oEmployeeDTO != null)
                {
                    var val = new Employee();
                    val.Id = oEmployeeDTO.Id;
                    val.Name = oEmployeeDTO.Name;
                    val.LastName = oEmployeeDTO.LastName;
                    val.Email = oEmployeeDTO.Email;
                    val.Age = oEmployeeDTO.Age;
                    val.Doj = oEmployeeDTO.Doj;
                    val.Gender = oEmployeeDTO.Gender;
                    val.IsMarried = oEmployeeDTO.IsMarried;
                    val.IsActive = oEmployeeDTO.IsActive;
                    val.DesignationID = oEmployeeDTO.DesignationID;
                    await _employeetRepository.UpdateEmployees(val);

                    string roleId = oEmployeeDTO.RoleID;
                    var role = await _roleManager.FindByIdAsync(roleId);
                    var IdValueUser = _employeetRepository.GetUserIDValue(oEmployeeDTO.Email);
                    string UserName = oEmployeeDTO.Email;

                    List<UserRole> authors = new List<UserRole>
                    {
                        new UserRole { UserId = IdValueUser, UserName =UserName }

                    };
                    for (int i = 0; i < authors.Count; i++)
                    {
                        var user = await _userManager.FindByIdAsync(authors[i].UserId);
                        IdentityResult result = null;
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    }

                }

            }
            catch (Exception ex)
            {
                if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                {
                    return NotFound();
                }

                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            var posts = await _employeetRepository.GetEmployeesByID(id);
            if (posts == null)
            {
                return NotFound();
            }
            var res1 = new EmployeeDTO();
            res1.Id = posts.Id;
            res1.Name = posts.Name;
            res1.LastName = posts.LastName;
            res1.Email = posts.Email;
            res1.Gender = posts.Gender;
            res1.IsActive = posts.IsActive;
            res1.Age = posts.Age;
            res1.IsMarried = posts.IsMarried;
            res1.Doj = posts.Doj;
            res1.UserID = posts.UserID;
            res1.UserNameOrEmail = posts.UserNameOrEmail;
            var user = await _userManager.FindByIdAsync(res1.UserID);
            var rolest = await _userManager.GetRolesAsync(user);
            res1.DesignationID = posts.DesignationID;

            string RoleNameF = "";
            foreach (var from in rolest)
            {
                RoleNameF = from;
            }
            var role = await _roleManager.FindByNameAsync(RoleNameF);
            res1.RoleID = role.Id;
            var roles = _roleManager.Roles;
            ViewBag.RoleViewModelList = new SelectList(roles, "Id", "Name");
            var model = new EmployeeDTO();
            model.DesignationsList = await _employeetRepository.GetDesignations();
            ViewBag.DeptList = new SelectList(model.DesignationsList, "Id", "Designations");
            return View(res1);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeDTO oEmployeeDTO)
        {

            try
            {

                if (oEmployeeDTO != null)
                {
                    var val = new Employee();
                    val.Id = oEmployeeDTO.Id;
                    val.Name = oEmployeeDTO.Name;
                    val.LastName = oEmployeeDTO.LastName;
                    val.Email = oEmployeeDTO.Email;
                    val.Age = oEmployeeDTO.Age;
                    val.Doj = oEmployeeDTO.Doj;
                    val.Gender = oEmployeeDTO.Gender;
                    val.IsMarried = oEmployeeDTO.IsMarried;
                    val.IsActive = oEmployeeDTO.IsActive;
                    val.DesignationID = oEmployeeDTO.DesignationID;
                    string roleId = oEmployeeDTO.RoleID;
                    var role = await _roleManager.FindByIdAsync(roleId);
                    var IdValueUser = _employeetRepository.GetUserIDValue(oEmployeeDTO.Email);
                    string UserName = oEmployeeDTO.Email;

                    List<UserRole> authors = new List<UserRole>
                    {
                        new UserRole { UserId = IdValueUser, UserName =UserName }

                    };
                    for (int i = 0; i < authors.Count; i++)
                    {
                        var user = await _userManager.FindByIdAsync(authors[i].UserId);
                        IdentityResult result = null;
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    await _employeetRepository.Delete(val);
                }

            }
            catch (Exception ex)
            {
                if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                {
                    return NotFound();
                }

                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
   
       
    
    
    }
}











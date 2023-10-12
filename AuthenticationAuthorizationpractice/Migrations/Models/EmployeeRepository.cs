using AuthenticationAuthorizationpractice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AuthenticationAuthorizationpractice.Models
{
    public class EmployeeRepository: IEmployeetRepository
    {

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeRepository(AppDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager; 
        }

        public int GetDuplicateEmployees(EmployeeDTO oEmployeeDTO)
        {
            int i = 0;
           var va = _context.TblEmployee.Where(x => x.Email == oEmployeeDTO.Email).ToList();
            if (va.Count > 0)
            {
                i++;
            }
            return i;
        }


        public async Task<List<EmployeeDTO>> GetEmployees()
        {
            if (_context != null)
            {

                List<EmployeeDTO> obj = new List<EmployeeDTO>();
                var orderlist = (from a in _context.TblEmployee
                                 join b in _context.TblDesignation on a.DesignationID equals b.Id
                                 join c in _userManager.Users on a.Email equals c.Email
                                 select new
                                 {
                                     Id = (int)a.Id,
                                     Name = a.Name,
                                     LastName = a.LastName,
                                     Email = a.Email,
                                     Age = a.Age,
                                     Doj = a.Doj,
                                     Gender = a.Gender,
                                     IsMarried = a.IsMarried,
                                     IsActive = a.IsActive,
                                     DesignationID = a.DesignationID,
                                     Designations = b.Designations,
                                     UserNameOrEmail = c.UserName,
                                     UserID = c.Id
                                 });
                                foreach (var item in orderlist)
                                {

                                    EmployeeDTO clr = new EmployeeDTO();
                                    clr.Id =(int) item.Id;
                                    clr.Name = item.Name;
                                    clr.LastName = item.LastName;
                                    clr.Email = item.Email;
                                    clr.UserNameOrEmail = item.UserNameOrEmail;
                                    clr.Age = item.Age;
                                    clr.Doj = item.Doj;
                                    clr.Gender = item.Gender;
                                    clr.IsActive = item.IsActive;
                                    clr.IsMarried = item.IsMarried;
                                    clr.DesignationID = item.DesignationID;
                                    clr.Designations = item.Designations;
                                    clr.UserID = item.UserID;
                                    var user = await _userManager.FindByIdAsync(clr.UserID);
                                    var rolest = await _userManager.GetRolesAsync(user);
                                    string RoleNameF = "";
                                    foreach (var from in rolest)
                                    {
                                        RoleNameF = from;
                                    }
                                    clr.RoleName= RoleNameF;
                                    var role = await _roleManager.FindByNameAsync(RoleNameF);
                                    clr.RoleID = role.Id;
                                    obj.Add(clr);
                                }
                      return  obj;
               
            }

            return null;
        }

        public async Task<EmployeeDTO> GetEmployeesByID(int? empId)
        {
                       
            if (_context != null)
            {


                return await (from a in _context.TblEmployee
                              join b in _context.TblDesignation on a.DesignationID equals b.Id
                              join c in _userManager.Users on a.Email equals c.Email
                              where a.Id == empId
                              select new EmployeeDTO
                              {
                                  Id = (int)a.Id,
                                  Name = a.Name,
                                  LastName = a.LastName,
                                  Email = a.Email,
                                  Age = a.Age,
                                  Doj = a.Doj,
                                  Gender = a.Gender,
                                  IsMarried = a.IsMarried,
                                  IsActive = a.IsActive,
                                  DesignationID = a.DesignationID,
                                  Designations = b.Designations,
                                  UserNameOrEmail = c.UserName,
                                  UserID=c.Id
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

      
        public async Task<Employee> InsertEmployees(Employee oEmployee)
        {
            if (_context != null)
            {
                //Delete that post
                _context.TblEmployee.Update(oEmployee);
                await _context.SaveChangesAsync();
            }


            return null;
        }

        public async Task UpdateEmployees(Employee oEmployee)
        {

            if (_context != null)
            {
                //Delete that post
                _context.TblEmployee.Update(oEmployee);
                await _context.SaveChangesAsync();
            }

        }

        public async Task Delete(Employee oEmployee)
        {
            if(oEmployee != null)
            {
                // Employee existing = _context.TblEmployee.Find(id);
                _context.TblEmployee.Remove(oEmployee);
                _context.SaveChanges();
            }
          
        }

        public async Task<List<Designation>> GetDesignations()
        {
            if (_context != null)
            {
                return await _context.TblDesignation.ToListAsync();
            }



            return null;
        }

        public async Task<List<AspNetUser>> GetUserID(string userid)
        {
            if (userid != null)
            {

                List<AspNetUser> objuserid = new List<AspNetUser>();
                var orderlists = (from c in _userManager.Users
                                  where c.UserName.StartsWith(userid)
                                  select new { c.UserName, c.Id });
                foreach (var items in orderlists)
                {

                    AspNetUser clrs = new AspNetUser();
                    clrs.Id = items.Id;
                    clrs.UserName = items.UserName;
                    objuserid.Add(clrs);
                }

                return objuserid;
            }
            return null;
        }


        public string GetUserIDValue(string userid)
        {
            string resultvalue = "";
            if (userid != null)
            {
                var orderlists = (from c in _userManager.Users
                                  where c.UserName.StartsWith(userid)
                                  select new { c.UserName, c.Id });
                foreach (var items in orderlists)
                {  
                    resultvalue= items.Id;
                }

                return resultvalue;
            }
            return null;
        }



        public string GetUserIdForRole(string useremail)
        {
            string returnresult = "";
            if (useremail != null)
            {

                var orderlists = (from c in _userManager.Users
                                  where c.UserName.StartsWith(useremail)
                                  select new { c.UserName, c.Id });
                foreach (var items in orderlists)
                {

                    returnresult = items.Id;
                   
                }

                return returnresult;
            }
            return null;
        }



        public void InsertUserRole(string userId, string emil, string RoleID, string roleName)
        {



        }
    }
}

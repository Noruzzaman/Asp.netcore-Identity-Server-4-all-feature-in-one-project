using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationAuthorizationpractice.Models
{
    public interface IEmployeetRepository
    {
        Task<List<EmployeeDTO>> GetEmployees();       

        Task<EmployeeDTO> GetEmployeesByID(int? Id);

        Task<Employee> InsertEmployees(Employee oEmployee);

        Task Delete(Employee oEmployee);


        Task UpdateEmployees(Employee oEmployee);
       
        Task<List<Designation>> GetDesignations();

        int GetDuplicateEmployees(EmployeeDTO oEmployeeDTO);


        Task<List<AspNetUser>> GetUserID(string userid);

        string GetUserIdForRole(string oEmployeeDTO);

        string GetUserIDValue(string Email);

        void InsertUserRole(string st1,string st2,string st3,string st4);
    }
}

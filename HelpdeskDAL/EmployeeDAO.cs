using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public class EmployeeDAO
    {
        public async Task<Employee> GetByEmail(String email)
        {
            Employee selectedEmployee = null;
            try
            {
                HelpdeskContext _db = new HelpdeskContext();
                selectedEmployee = await _db.Employees.FirstOrDefaultAsync(emp => emp.Email == email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return selectedEmployee;
        }


        public async Task<Employee> GetById(int id)
        {
            Employee selectedEmployee = null;

            try
            {
                HelpdeskContext _db = new HelpdeskContext();
                selectedEmployee = await _db.Employees.FirstOrDefaultAsync(emp => emp.Id == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return selectedEmployee;
        }

        public async Task<List<Employee>> GetAll()
        {
            List<Employee> allEmployees = new List<Employee>();
            try
            {
                HelpdeskContext _db = new HelpdeskContext();
                allEmployees = await _db.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return allEmployees;
        }

        public async Task<int> Add(Employee newEmployee)
        {
            try
            {
                HelpdeskContext _db = new HelpdeskContext();
                await _db.Employees.AddAsync(newEmployee);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return newEmployee.Id;
        }

        public async Task<int> Update(Employee updatedEmployee)
        {
            int EmployeesUpdated = -1;
            try
            {
                HelpdeskContext _db = new HelpdeskContext();
                Employee currentEmployee = await _db.Employees.FirstOrDefaultAsync(emp => emp.Id == updatedEmployee.Id);
                _db.Entry(currentEmployee).CurrentValues.SetValues(updatedEmployee);
                EmployeesUpdated = await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return EmployeesUpdated;
        }

        public async Task<int> Delete(int id)
        {
            int EmployeesDeleted = -1;
            try
            {
                HelpdeskContext _db = new HelpdeskContext();
                Employee selectedEmployee = await _db.Employees.FirstOrDefaultAsync(emp => emp.Id == id);
                _db.Employees.Remove(selectedEmployee);
                EmployeesDeleted = await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return EmployeesDeleted;
        }
    }
}

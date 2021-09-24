using System;
using Xunit;
using HelpdeskDAL;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseStudyTests
{
    namespace CaseStudyTests
    {
        public class DAOTests
        {
            [Fact]
            public async Task Employee_GetByEmailTest()
            {
                EmployeeDAO dao = new EmployeeDAO();
                Employee selectedEmployee = await dao.GetByEmail("bs@abc.com");
                Assert.NotNull(selectedEmployee);
            }

            [Fact]
            public async Task Employee_GetByIdTest()
            {
                EmployeeDAO dao = new EmployeeDAO();
                Employee selectedEmployee = await dao.GetById(1);
                Assert.NotNull(selectedEmployee);
            }

            [Fact]
            public async Task Employee_GetAllTest()
            {
                EmployeeDAO dao = new EmployeeDAO();
                List<Employee> selectedEmployees = await dao.GetAll();
                Assert.NotEmpty(selectedEmployees);
            }

            [Fact]
            public async Task Employee_AddTest()
            {
                EmployeeDAO dao = new EmployeeDAO();
                int EmployeeId = await dao.Add(new Employee
                {
                    FirstName = "Zacharias",
                    LastName = "Wilsons",
                    PhoneNo = "(519)636-9226",
                    Title = "Mrs.",
                    DepartmentId = 100,
                    Email = "zack@coldstream.org"
                });
                Assert.True(EmployeeId > 0);
            }

            [Fact]
            public async Task Employee_UpdateTest()
            {
                EmployeeDAO dao = new EmployeeDAO();
                int EmployeesUpdated = await dao.Update(new Employee
                {
                    Id = 1003,
                    FirstName = "Zacharias",
                    LastName = "Wilsons",
                    PhoneNo = "(519)636-9226",
                    Title = "Mr.",
                    DepartmentId = 100,
                    Email = "zack@coldstream.org"
                });
                Assert.True(EmployeesUpdated > 0);
            }

            [Fact]
            public async Task Employee_DeleteTest()
            {
                EmployeeDAO dao = new EmployeeDAO();
                int selectedEmployee = await dao.Delete(1003);
                Assert.True(selectedEmployee > 0);
            }
        }
    }
}
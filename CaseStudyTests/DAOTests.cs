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
                    PhoneNo = "(519)636-9225",
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
                Employee EmployeeForUpdate = await dao.GetByEmail("zack@coldstream.org");

                if(EmployeeForUpdate != null)
                {
                    string oldPhoneNo = EmployeeForUpdate.PhoneNo;
                    string newPhoneNo = oldPhoneNo == "(519)636-9225" ? "555-555-5555" : "(519)636-9225";
                    EmployeeForUpdate.PhoneNo = newPhoneNo;
                }
                Assert.True(await dao.Update(EmployeeForUpdate) == UpdateStatus.Ok);
            }

            [Fact]
            public async Task Employee_DeleteTest()
            {
                EmployeeDAO dao = new EmployeeDAO();
                int selectedEmployee = await dao.Delete(1003);
                Assert.True(selectedEmployee > 0);
            }


            [Fact]
            public async Task Employee_ConcurrencyTest()
            {
                EmployeeDAO dao1 = new EmployeeDAO();
                EmployeeDAO dao2 = new EmployeeDAO();
                Employee EmployeeForUpdate1 = await dao1.GetByEmail("zack@coldstream.org");
                Employee EmployeeForUpdate2 = await dao2.GetByEmail("zack@coldstream.org");

                if (EmployeeForUpdate1 != null)
                {
                    string oldPhoneNo = EmployeeForUpdate1.PhoneNo;
                    string newPhoneNo = oldPhoneNo == "(519)636-9225" ? "555-555-5555" : "(519)636-9225";
                    EmployeeForUpdate1.PhoneNo = newPhoneNo;
                    if (await dao1.Update(EmployeeForUpdate1) == UpdateStatus.Ok)
                    {
                        //need to change the phone # to something else
                        EmployeeForUpdate2.PhoneNo = "666-666-6666";
                        Assert.True(await dao2.Update(EmployeeForUpdate2) == UpdateStatus.Stale);
                    }
                    else
                        Assert.True(false);
                }
            }
        }
    }
}
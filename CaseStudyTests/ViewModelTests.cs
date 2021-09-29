using Xunit;
using HelpDeskViewModels;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CaseStudyTests
{
    public class ViewModelTests
    {
        [Fact]
        public async Task Employee_GetByEmailTest()
        {
            EmployeeViewModel vm = null;
            try
            {
                vm = new EmployeeViewModel { Email = "bs@abc.com" };
                await vm.GetByEmail();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Employee_GetByIdTest()
        {
            EmployeeViewModel vm = null;
            try
            {
                vm = new EmployeeViewModel { Id = 1 };
                await vm.GetById();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Employee_GetAllTest()
        {
            List<EmployeeViewModel> vms = new List<EmployeeViewModel>();
            try
            {
                EmployeeViewModel vm = new EmployeeViewModel();
                vms = await vm.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
            Assert.NotEmpty(vms);
        }

        [Fact]
        public async Task Employee_AddTest()
        {
            EmployeeViewModel vm = null;
            try
            {
                vm = new EmployeeViewModel
                {

                    Title = "Mr.",
                    Firstname = "Zachari",
                    Lastname = "Wilson",
                    Phoneno = "519-636-9225",
                    Email = "zachariwils@gmail.com",
                    DepartmentId = 100
                };
                await vm.Add();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
            Assert.True(vm.Id > 0);
        }

        [Fact]
        public async Task Employee_UpdateTest()
        {
            int EmployeesUpdated = -1;
            try
            {
                EmployeeViewModel vm = new EmployeeViewModel { Email = "zachariwils@gmail.com" };
                await vm.GetByEmail();
                vm.Phoneno = vm.Phoneno == "519-636-9225" ? "519-636-9226" : "519-636-9225";
                EmployeesUpdated = await vm.Update();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
            Assert.True(EmployeesUpdated > 0);
        }

        [Fact]
        public async Task Employee_DeleteTest()
        {
            int EmployeesDeleted = -1;
            try
            {
                EmployeeViewModel vm = new EmployeeViewModel { Email = "zachariwils@gmail.com" };
                await vm.GetByEmail();
                EmployeesDeleted = await vm.Delete();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
            Assert.True(EmployeesDeleted > 0);
        }


    }
}

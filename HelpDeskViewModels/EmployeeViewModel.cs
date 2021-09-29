using HelpdeskDAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace HelpDeskViewModels
{
    public class EmployeeViewModel
    {
        private readonly EmployeeDAO _dao;

        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public string Timer { get; set; }
        public int DepartmentId { get; set; }
        public int Id { get; set; }
        public bool isTech { get; set; }
        public string StaffPicture64 { get; set; }

        //constructor
        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }


        //find Employee using Lastname property
        public async Task GetByEmail()
        {
            try
            {
                Employee emp = await _dao.GetByEmail(Email);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);

                }
                Timer = Convert.ToBase64String(emp.Timer);

            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
        }

        //find Employee using Id property
        public async Task GetById()
        {
            try
            {
                Employee emp = await _dao.GetById(Id);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);

                }
                Timer = Convert.ToBase64String(emp.Timer);

            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
        }

        //Retrive all the Employees
        public async Task<List<EmployeeViewModel>> GetAll()
        {
            List<EmployeeViewModel> allVms = new List<EmployeeViewModel>();
            try
            {
                List<Employee> allEmployees = await _dao.GetAll();
                foreach (Employee emp in allEmployees)
                {
                    EmployeeViewModel empVm = new EmployeeViewModel
                    {

                        Title = emp.Title,
                        Firstname = emp.FirstName,
                        Lastname = emp.LastName,
                        Phoneno = emp.PhoneNo,
                        Email = emp.Email,
                        Id = emp.Id,
                        DepartmentId = emp.DepartmentId,
                        Timer = Convert.ToBase64String(emp.Timer),
                    };
                    allVms.Add(empVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return allVms;
        }

        public async Task Add()
        {
            Id = -1;
            try
            {
                Employee emp = new Employee
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    DepartmentId = DepartmentId
                };
                Id = await _dao.Add(emp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<int> Update()
        {
            int EmployeeUpdated = -1;
            try
            {
                Employee emp = new Employee
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    Id = Id,
                    DepartmentId = DepartmentId
                };
                if (StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }

                emp.Timer = Convert.FromBase64String(Timer);
                EmployeeUpdated = await _dao.Update(emp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return EmployeeUpdated;
        }

        public async Task<int> Delete()
        {
            int EmployeeDeleted = -1;

            try
            {
                EmployeeDeleted = await _dao.Delete(Id);
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw;
            }
            return EmployeeDeleted;
        }
    }
}

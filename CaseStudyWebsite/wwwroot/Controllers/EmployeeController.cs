using HelpDeskViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace CaseStudyWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpGet("{email}")]

        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                EmployeeViewModel viewmodel = new EmployeeViewModel
                {
                    Email = email
                };
                await viewmodel.GetByEmail();
                return Ok(viewmodel);
            } 
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); //something went wrong
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(EmployeeViewModel viewmodel)
        {
            try
            {
                int retVal = await viewmodel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Employee " + viewmodel.Lastname + " updated!" }),
                    -1 => Ok(new { msg = "Employee " + viewmodel.Lastname + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for  " + viewmodel.Lastname + ", Employee not updated!" }),
                    _ => Ok(new { msg = "Employee " + viewmodel.Lastname + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); //something went wrong
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                EmployeeViewModel viewModel = new EmployeeViewModel();
                List<EmployeeViewModel> allEmployees = await viewModel.GetAll();
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); //something went wrong
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(EmployeeViewModel viewmodel)
        {
            try
            {
                await viewmodel.Add();
                return viewmodel.Id > 1
                    ? Ok(new { msg = "Employee " + viewmodel.Lastname + " added!" })
                    : Ok(new { msg = "Employee " + viewmodel.Lastname + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                EmployeeViewModel viewmodel = new EmployeeViewModel { Id = id };
                return await viewmodel.Delete() == 1
                    ? Ok(new { msg = "Employee " + id + " deleted!" })
                    : Ok(new { msg = "Employee " + id + " not deleted!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

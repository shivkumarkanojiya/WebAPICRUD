using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebAPICRUD.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebAPICRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration ,IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        public JsonResult Get()
        {

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBCS"));
            SqlCommand cmd = new SqlCommand("SpSelEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            con.Close();
            return new JsonResult(dt);

        }
        [HttpPost]
        public JsonResult Post(Employee emp)
        {

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBCS"));
            SqlCommand cmd = new SqlCommand("SpInsEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
            cmd.Parameters.AddWithValue("@Department", emp.Department);
            cmd.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
            cmd.Parameters.AddWithValue("@PhotoFilrName", emp.PhotoFilrName);
            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();
            return new JsonResult("Data Added successfully");

        }
        [HttpPut]
        public JsonResult Put(Employee emp)
        {

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBCS"));
            SqlCommand cmd = new SqlCommand("SpUpdateEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
            cmd.Parameters.AddWithValue("@EmployeeName",emp.EmployeeName);
            cmd.Parameters.AddWithValue("@Department", emp.Department);
            cmd.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
            cmd.Parameters.AddWithValue("@PhotoFilrName", emp.PhotoFilrName);
            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();
            return new JsonResult("Data Updated successfully");

        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBCS"));
            SqlCommand cmd = new SqlCommand("SpDeleteEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", id);
            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();
            return new JsonResult("Data Deleted successfully");

        }
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalpath = _env.ContentRootPath + "/Photos/" + filename;
                using(var strem=new  FileStream(physicalpath,FileMode.Create))
                {
                    postedFile.CopyTo(strem);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }
        [Route("GetAllDepartmentName")]
        [HttpGet]
        public JsonResult GetAllDepartmentName()
        {

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBCS"));
            SqlCommand cmd = new SqlCommand("SpSelAllDepartment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            con.Close();
            return new JsonResult(dt);

        }

    }
}

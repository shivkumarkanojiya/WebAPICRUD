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

namespace WebAPICRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBCS"));
            SqlCommand cmd = new SqlCommand("SpSelDepartment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            con.Close();
            return new JsonResult(dt);

        }
        [HttpPost]
        public JsonResult Post(Department dpt)
        {

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBCS"));
            SqlCommand cmd = new SqlCommand("SpInsDepartment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DepartmentName", dpt.DepartmentName);
            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();
            return new JsonResult("Data Added successfully");

        }
        [HttpPut]
        public JsonResult Put(Department dpt)
        {

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBCS"));
            SqlCommand cmd = new SqlCommand("SpUpdateDepartment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DepartmentId", dpt.DepartmentId);
            cmd.Parameters.AddWithValue("@DepartmentName", dpt.DepartmentName);
            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();
            return new JsonResult("Data Updated successfully");

        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBCS"));
            SqlCommand cmd = new SqlCommand("SpDeleteDepartment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DepartmentId", id);
            con.Open();
            cmd.ExecuteNonQuery();

            con.Close();
            return new JsonResult("Data Deleted successfully");

        }

    }
}

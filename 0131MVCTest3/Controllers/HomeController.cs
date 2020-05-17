using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _0131MVCTest3.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

using _0131MVCTest3.DAL;
//For adding session
using Microsoft.AspNetCore.Http;

namespace _0131MVCTest3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;

        }

        public IActionResult Index()
        {
            
            
            return View();
        }

        public IActionResult Page2(City city)
        {
            //string connStr = _configuration.GetConnectionString("MyConnString");
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            DALCity dc = new DALCity(_configuration);
            city.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            int uID = dc.addCity(city);
            
            //SqlConnection conn = new SqlConnection(connStr);
            //conn.Open();

            city.UID = uID;

            //save the uid to the session
            HttpContext.Session.SetString("uID", uID.ToString()); //write to the session
            

            return View(city);
        }

        public IActionResult EditCity()
        {

            int uID = Convert.ToInt32(HttpContext.Session.GetString("uID"));
            DALCity dc = new DALCity(_configuration);
            City city = dc.getCity(uID);

            return View(city);
        }

        public IActionResult UpdateCity(City city)
        {
            int uID = Convert.ToInt32(HttpContext.Session.GetString("uID"));
            city.UID = uID;

            DALCity dc = new DALCity(_configuration);
            dc.updateCity(city);
            return View("Page2",city);

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

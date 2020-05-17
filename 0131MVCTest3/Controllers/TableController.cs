using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _0131MVCTest3.DAL;
using _0131MVCTest3.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _0131MVCTest3.Controllers
{
    public class TableController : Controller
    {
        // GET: /<controller>/
        private readonly ILogger<TableController> _logger;

        private readonly IConfiguration _configuration;

        public TableController(ILogger<TableController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;

        }

        public IActionResult Index()
        {
            DALCity dc = new DALCity(_configuration);
            List<City> lc = dc.getCityByName("1");
            return View(lc);
        }

        public string IncrementCalculation(int[] increment, int len)
        {
            int[] incre = increment;
            string str = "";
            for(int i = len-1; i>=1; i--) {
                incre[i] = increment[i] - increment[i - 1];
            }
            incre[0] = 0;
            str = String.Join(",", incre);
            return str;
        }

        public IActionResult SearchTable(City city)
        {
            DALCity dc = new DALCity(_configuration);

            List<City> lc = dc.getCityByName(city.Name);
            int len = lc.ToArray().Length;
            DateTime[] date = new DateTime[len];
            int[] confirmed = new int[len];
            int[] death = new int[len];
            int[] recovery = new int[len];

            int i = -1;
            string strDate = "";
            string strCon = "";
            string strDeath = "";
            string strRecovery = "";

            foreach (City ct in lc)
            {
                i += 1;
                date[i] = ct.Date;
                confirmed[i] = ct.ConfirmedNum;
                death[i] = ct.Death;
                recovery[i] = ct.Recovery;
            }

            strDate = String.Join(",", date);
            strCon = String.Join(",", confirmed);
            strDeath = String.Join(",", death);
            strRecovery = String.Join(",", recovery);

            ViewBag.dateInfo = strDate;
            ViewBag.confirmedInfo = strCon;
            ViewBag.deathInfo = strDeath;
            ViewBag.recoveryInfo = strRecovery;
            ViewBag.incConfirmed = this.IncrementCalculation(confirmed, len);

            ViewBag.cityName = city.Name;
            ViewBag.length = len;
            return View("Index", lc);
        }

        public IActionResult Edit(int id)
        {
            int uID = id;
            HttpContext.Session.SetString("uID", uID.ToString());

            DALCity dc = new DALCity(_configuration);
            City city = dc.getCity(uID);
            HttpContext.Session.SetString("EditDate", city.Date.ToShortDateString());
            HttpContext.Session.SetString("CityName", city.Name);

            return View(city);
        }

        public IActionResult Details(int id)
        {
            return this.Edit(id);
        }

        public IActionResult UpdateCityFromTable(City city)
        {
            int uID = Convert.ToInt32(HttpContext.Session.GetString("uID"));
            city.UID = uID;
            city.Date = Convert.ToDateTime(HttpContext.Session.GetString("EditDate"));

            DALCity dc = new DALCity(_configuration);
            dc.updateCity(city);
            return this.SearchTable(city);

        }

        public IActionResult Delete(int id)
        {
            return this.Edit(id);
        }

        public IActionResult DeleteCityFromTable()
        {
            int uID = Convert.ToInt32(HttpContext.Session.GetString("uID"));
            City temp = new City();
            temp.Name = HttpContext.Session.GetString("CityName");


            DALCity dc = new DALCity(_configuration);
            dc.deleteCity(uID);
            return this.SearchTable(temp);

        }

    }
}

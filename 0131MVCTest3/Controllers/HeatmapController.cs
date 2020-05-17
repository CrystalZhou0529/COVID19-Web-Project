using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0131MVCTest3.DAL;
using _0131MVCTest3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace _0131MVCTest3.Controllers
{
    public class HeatmapController : Controller
    {
        private readonly ILogger<TableController> _logger;

        private readonly IConfiguration _configuration;



        public HeatmapController(ILogger<TableController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;

        }

        public IActionResult Index()
        {
            return View();
        }

        public string IncrementCalculation(int[] increment, int len)
        {
            int[] incre = increment;
            string str = "";
            for (int i = len - 1; i >= 1; i--)
            {
                incre[i] = increment[i] - increment[i - 1];
            }
            incre[0] = 0;
            str = String.Join(",", incre);
            return str;
        }

        

        public IActionResult SearchTable(City city)
        {
            DALCity dc = new DALCity(_configuration);
            try
            {


                List<City> lc = dc.getCityByDate(city.Date);
                int len = lc.ToArray().Length;
                DateTime[] date = new DateTime[len];
                int[] confirmed = new int[len];
                int[] death = new int[len];
                int[] recovery = new int[len];
                int[] remained = new int[len];
                string[] name = new string[len];

                int i = -1;
                string strDate = "";
                string strCon = "";
                string strDeath = "";
                string strRecovery = "";
                string strRemained = "";
                string strName = "";

                foreach (City ct in lc)
                {
                    i += 1;
                    date[i] = ct.Date;
                    confirmed[i] = ct.ConfirmedNum;
                    death[i] = ct.Death;
                    recovery[i] = ct.Recovery;
                    name[i] = ct.Name;
                    remained[i] = confirmed[i] - death[i] - recovery[i];
                }

                /*strDate = String.Join(",", date);
                strCon = String.Join(",", confirmed);
                strDeath = String.Join(",", death);
                strRecovery = String.Join(",", recovery);*/
                strRemained = String.Join(",", remained);
                strName = String.Join(",", name);

                /*ViewBag.dateInfo = strDate;
                ViewBag.confirmedInfo = strCon;
                ViewBag.deathInfo = strDeath;
                ViewBag.recoveryInfo = strRecovery;
                ViewBag.incConfirmed = this.IncrementCalculation(confirmed, len);*/

                ViewBag.remainedInfo = strRemained;

                ViewBag.cityName = strName;
                ViewBag.length = len;

                return View("Index", lc);
            }
            catch
            {
                return View();
            }

        }


    }
}
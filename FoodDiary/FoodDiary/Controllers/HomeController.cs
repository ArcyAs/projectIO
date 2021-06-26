﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodDiary.Models;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using FoodDiary.Data;
using FoodDiary.Repositories.Entities;
using System.Net;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FoodDiary.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        UserManager<AppUser> _userManager;
        private DbSet<UserDetailsEntity> UserDetailsEntities { get; set; }


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            UserDetailsEntities = _context.UserDetailsEntities;

        }
        public IActionResult IndexAsync()
        {
            var currentUser = _userManager.Users.FirstOrDefault(p => p.Email == User.FindFirstValue(ClaimTypes.Email));
            if (currentUser != null)
            {
                var userDetailsEntity = _context.UserDetailsEntities.FirstOrDefault(p => p.UserId == Guid.Parse(currentUser.Id));
                var data = _context.DiaryEntities.Where(x => x.DiaryId == userDetailsEntity.DiaryId && x.AddDate.Date == DateTime.Today && x.IdProduct != Guid.Empty).ToList();
                var totalKcal = data.AsEnumerable().Sum(row => row.Kcal);
                var userBmr = userDetailsEntity.Bmr - totalKcal;
                var viewModel = new HomeViewModel()
                {
                    sumKcal = totalKcal,
                    userbmr = Convert.ToInt32(userBmr)
                };
                return View(viewModel);
            }
            else
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        
    }

    public class HomeViewModel
    {
        public int userbmr { get; set; }
        public int sumKcal { get; set; }
    }
}
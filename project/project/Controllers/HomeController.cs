using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace project.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index(int? id, string? email, int page = 1, SortState sortorder =SortState.IdAsc)
        {
           
            IQueryable<User> users = db.Users;
            // Фильтрация или список
           
            if (id != null && id > 0)
            {
                users = users.Where(p=>p.Id==id);
            }
            if (!String.IsNullOrEmpty(email))
            {
                users = users.Where(p => p.Email.Contains(email));
            }
            //сортировка
            switch (sortorder){
                case SortState.IdAsc:
                    {
                        users = users.OrderBy(p => p.Id);
                        break;
                    }
                case SortState.IdDesc:
                    {
                        users = users.OrderByDescending(p => p.Id);
                        break; }
                case SortState.EmailAsc:
                    {
                        users = users.OrderBy(p => p.Email);
                        break;
                    }
                case SortState.EmailDesc:
                    {
                        users = users.OrderByDescending(p => p.Email);
                        break;
                    }
            };
            // Пигинация
            int pagesize = 3;
            var count = await users.CountAsync();
            var item = await users.Skip((page-1)*pagesize).Take(pagesize).ToArrayAsync();
            
            IndexViewModel viewModel = new IndexViewModel
            {
                FilterViewModel = new FilterViewModel(id, email),
                PageViewModel = new PageViewModel(count, page, pagesize),
                SortViewModel = new SortViewModel (sortorder),
                Users = item
            };
            return View(viewModel); //предположение (отображается одноименное представление)
        }
      
        [HttpPost]
        public async Task<IActionResult> Authorization(LogUser a)
        {
  
            if (a.Login != null && a.Password != null) {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Login == a.Login && predicate.Password == a.Password);
                if (user!=null )
                {
                    return RedirectToAction("Index");
                }               
            }
            return NotFound();
        }
        public IActionResult Authorization()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
            
        }

        [HttpGet]
        [ActionName("Delete")] //название действия к оторому обращаемся в представлении
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (user != null)
                {
                    return View(user); //что и как это
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");//переходим к действию Index
                }
            }
            return NotFound();
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (user != null)
                {
                    return View(user);
                }
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(predicate => predicate.Id == id);
                if (user != null)
                {
                    return View(user);
                }
            }
            return NotFound();

        }

    }
}

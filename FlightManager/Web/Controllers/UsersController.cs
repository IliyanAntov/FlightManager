using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Shared;
using Web.Models.Users;

namespace Web.Controllers
{
    public class UsersController : Controller
    {

        private const int PageSize = 10;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Users
        public ActionResult List(UsersListViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<UsersViewModel> items = new List<UsersViewModel>();
            foreach (var item in _context.Users.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).ToList())
            {
                var viewModel = new UsersViewModel()
                {
                    Id = item.Id,
                    Username = item.UserName,
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    UCN = item.UCN,
                    Address = item.Address,
                    PhoneNumber = item.PhoneNumber
                };

                var role = _userManager.GetRolesAsync(item).Result.FirstOrDefault();
                viewModel.Role = role;


                items.Add(viewModel);
            }

            items = items.OrderBy(x => x.Username).ToList();


            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(_context.Users.Count() / (double)PageSize);

            return View(model);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            UsersCreateViewModel model = new UsersCreateViewModel();
            return View(model);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsersCreateViewModel createModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = createModel.Username,
                    Email = createModel.Email,
                    FirstName = createModel.FirstName,
                    LastName = createModel.LastName,
                    UCN = createModel.UCN,
                    Address = createModel.Address,
                    PhoneNumber = createModel.PhoneNumber

                };
                var createUser = _userManager.CreateAsync(user, createModel.Password).Result;
                if (createUser.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Employee").Wait();
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(List));
        
            }
            return View(createModel);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            ApplicationUser user = _context.Users.Find(id);
            if(user == null)
            {
                return NotFound();
            }

            UsersEditViewModel model = new UsersEditViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UCN = user.UCN,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsersEditViewModel editModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _userManager.FindByIdAsync(editModel.Id).Result;
                user.UserName = editModel.Username;
                user.Email = editModel.Email;
                user.FirstName = editModel.FirstName;
                user.LastName = editModel.LastName;
                user.UCN = editModel.UCN;
                user.Address = editModel.Address;
                user.PhoneNumber = editModel.PhoneNumber;

                try
                {
                    _userManager.UpdateAsync(user).Wait();
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }

                return RedirectToAction(nameof(List));
            }

            return View(editModel);

        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            ApplicationUser user = _userManager.FindByIdAsync(id).Result;
            _userManager.DeleteAsync(user).Wait();
            return RedirectToAction(nameof(List));
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
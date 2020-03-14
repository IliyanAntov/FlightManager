using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Entity;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles="Admin")]
        public ActionResult List(UsersListViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;
            model.Pager.PageSize = model.Pager.PageSize <= 0 ? 10 : model.Pager.PageSize;
            List<ApplicationUser> users = new List<ApplicationUser>();

            if(model.FilterCriteria != null && model.Filter != null)
            {
                switch (model.FilterCriteria)
                {
                    case "email":
                        users = _context.Users.Where(x => x.Email.Contains(model.Filter)).ToList();
                        break;
                    case "username":
                        users = _context.Users.Where(x => x.UserName.Contains(model.Filter)).ToList();
                        break;
                    case "firstName":
                        users = _context.Users.Where(x => x.FirstName.Contains(model.Filter)).ToList();
                        break;
                    case "lastName":
                        users = _context.Users.Where(x => x.LastName.Contains(model.Filter)).ToList();
                        break;
                }
            }
            else
            {
                users = _context.Users.ToList();
            }

            List<UsersViewModel> items = new List<UsersViewModel>();
            foreach (var item in users.Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize).ToList())
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
            model.Pager.PagesCount = (int)Math.Ceiling(users.Count() / (double)model.Pager.PageSize);

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            UsersCreateViewModel model = new UsersCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
                if(_userManager.FindByNameAsync(createModel.Username).Result != null)
                {
                    return RedirectToAction("Error", "Home", new { errorMessage = "Username already exists" });
                }
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

        [Authorize(Roles = "Admin")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(UsersEditViewModel editModel)
        {
            if (ModelState.IsValid)
            {
                var result = _userManager.FindByNameAsync(editModel.Username).Result;
                if (result != null && result.Id != editModel.Id)
                {
                    return RedirectToAction("Error", "Home", new { errorMessage = "Username already exists" });
                }

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

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            ApplicationUser user = _userManager.FindByIdAsync(id).Result;
            _userManager.DeleteAsync(user).Wait();
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
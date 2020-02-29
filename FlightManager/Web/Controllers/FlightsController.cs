using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models.Flights;
using Web.Models.Shared;
using Web.Models.Users;

namespace Web.Controllers
{
    public class FlightsController : Controller
    {

        private const int PageSize = 10;
        private readonly ApplicationDbContext _context;


        public FlightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminList(FlightsAdminListViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<FlightsAdminViewModel> items = new List<FlightsAdminViewModel>();
            foreach (var item in _context.Flights.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).ToList())
            {
                var viewModel = new FlightsAdminViewModel()
                {
                    Id = item.Id,
                    LocationFrom = item.LocationFrom,
                    LocationTo = item.LocationTo,
                    DepartureTime = item.DepartureTime,
                    LandingTime = item.LandingTime,
                    PlaneType = item.PlaneType,
                    PlaneNumber = item.PlaneNumber,
                    PilotName = item.PilotName,
                    RegularSeats = item.RegularSeats,
                    BusinessSeats = item.BusinessSeats
                };
                items.Add(viewModel);
            }


            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(_context.Users.Count() / (double)PageSize);

            return View(model);
        }


        public ActionResult UserList(FlightsUserListViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<FlightsUserViewModel> items = new List<FlightsUserViewModel>();
            foreach (var item in _context.Flights.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).ToList())
            {
                var viewModel = new FlightsUserViewModel()
                {
                    Id = item.Id,
                    LocationFrom = item.LocationFrom,
                    LocationTo = item.LocationTo,
                    DepartureTime = item.DepartureTime,
                    Duration = item.LandingTime - item.DepartureTime,
                    PlaneType = item.PlaneType,
                    RegularSeats = item.RegularSeats,
                    BusinessSeats = item.BusinessSeats
                };
                items.Add(viewModel);
            }


            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(_context.Users.Count() / (double)PageSize);

            return View(model);
        }


        // GET: Flights/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: Flights/Create
        public ActionResult Create()
        {
            FlightsCreateViewModel model = new FlightsCreateViewModel();
            return View(model);
        }

        // POST: Flights/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FlightsCreateViewModel createModel)
        {
            if (ModelState.IsValid)
            {
                Flight flight = new Flight
                {
                    LocationFrom = createModel.LocationFrom,
                    LocationTo = createModel.LocationTo,
                    DepartureTime = createModel.DepartureTime,
                    LandingTime = createModel.LandingTime,
                    PlaneType = createModel.PlaneType,
                    PlaneNumber = createModel.PlaneNumber,
                    PilotName = createModel.PilotName,
                    RegularSeats = createModel.RegularSeats,
                    BusinessSeats = createModel.BusinessSeats

                };
                _context.Flights.Add(flight);
                _context.SaveChanges();
                return RedirectToAction(nameof(AdminList));

            }
            return View(createModel);
        }



        // GET: Flights/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Flights/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Flights/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Flights/Delete/5
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
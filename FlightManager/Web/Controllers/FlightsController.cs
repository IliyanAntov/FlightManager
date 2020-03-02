using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entity;
using Data.Enumeration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public ActionResult AdminList(AdminListViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<FlightAdminListViewModel> items = new List<FlightAdminListViewModel>();
            foreach (var item in _context.Flights.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).ToList())
            {
                int[] availableSeats = GetAvailableTickets(item.Id);
                var viewModel = new FlightAdminListViewModel()
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
            model.Pager.PagesCount = (int)Math.Ceiling(_context.Flights.Count() / (double)PageSize);

            return View(model);
        }

        private int[] GetAvailableTickets(int id)
        {
            new List<Passanger>() { new Passanger() };
            var flight = _context.Flights.Find(id);
            int availableRegularSeats = flight.RegularSeats;
            int availableBusinessSeats = flight.BusinessSeats;
            var reservations = _context.Reservations
                .Where(x => x.FlightId == id)
                .Include(x => x.Passangers);

            foreach(var reservation in reservations)
            {
                foreach (var passanger in reservation.Passangers)
                {
                    if (passanger.TicketType == TicketTypeEnum.Regular)
                    {
                        availableRegularSeats--;
                    }
                    else
                    {
                        availableBusinessSeats--;
                    }
                }
                
            }

            return new int[2] { availableRegularSeats, availableBusinessSeats };
        }

        public ActionResult UserList(UserListViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<FlightUserListViewModel> items = new List<FlightUserListViewModel>();
            foreach (var item in _context.Flights.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).ToList())
            {
                int[] availableSeats = GetAvailableTickets(item.Id);
                var viewModel = new FlightUserListViewModel()
                {
                    Id = item.Id,
                    LocationFrom = item.LocationFrom,
                    LocationTo = item.LocationTo,
                    DepartureTime = item.DepartureTime,
                    Duration = item.LandingTime - item.DepartureTime,
                    PlaneType = item.PlaneType,
                    RegularSeats = availableSeats[0],
                    BusinessSeats = availableSeats[1]
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
            var flight = _context.Flights.Find(id);
            int[] availableSeats = GetAvailableTickets(id);
            FlightDetailsViewModel model = new FlightDetailsViewModel()
            {
                FlightId = flight.Id,
                LocationFrom = flight.LocationFrom,
                LocationTo = flight.LocationTo,
                DepartureTime = flight.DepartureTime,
                LandingTime = flight.LandingTime,
                PlaneType = flight.PlaneType,
                PlaneNumber = flight.PlaneNumber,
                PilotName = flight.PilotName,
                RegularSeats = availableSeats[0],
                BusinessSeats = availableSeats[1]
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(FlightDetailsViewModel createModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createModel);
            }
            else
            {
                ReserveViewModel model = new ReserveViewModel
                {
                    FlightId = createModel.FlightId,
                    TicketNum = createModel.TicketNum
                };
                return RedirectToAction("Reserve", "Reservations", model);
            }
        }


        // GET: Flights/Create
        public ActionResult Create()
        {
            FlightCreateViewModel model = new FlightCreateViewModel();
            return View(model);
        }

        // POST: Flights/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FlightCreateViewModel createModel)
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
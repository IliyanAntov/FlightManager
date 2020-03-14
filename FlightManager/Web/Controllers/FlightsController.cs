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
using Web.Models.Reservations;
using Web.Models.Shared;
using Web.Models.Users;

namespace Web.Controllers
{
    public class FlightsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private int PageSize { get; set; }

        public FlightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminList(AdminListViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;
            model.Pager.PageSize = model.Pager.PageSize <= 0 ? 10 : model.Pager.PageSize;

            List<FlightAdminListViewModel> items = new List<FlightAdminListViewModel>();
            foreach (var item in _context.Flights.Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize).ToList())
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
            model.Pager.PagesCount = (int)Math.Ceiling(_context.Flights.Count() / (double)model.Pager.PageSize);

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
            model.Pager.PageSize = model.Pager.PageSize <= 0 ? 10 : model.Pager.PageSize;

            List<FlightUserListViewModel> items = new List<FlightUserListViewModel>();
            foreach (var item in _context.Flights.Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize).ToList())
            {
                int[] availableSeats = GetAvailableTickets(item.Id);
                var duration = (item.LandingTime - item.DepartureTime).TotalMinutes;
                string durationString = String.Format("{0}:{1:00}", (int)(duration / 60), (duration % 60));
                var viewModel = new FlightUserListViewModel()
                {
                    Id = item.Id,
                    LocationFrom = item.LocationFrom,
                    LocationTo = item.LocationTo,
                    DepartureTime = item.DepartureTime,
                    Duration = durationString,
                    PlaneType = item.PlaneType,
                    RegularSeats = availableSeats[0],
                    BusinessSeats = availableSeats[1]
                };
                items.Add(viewModel);
            }


            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(_context.Flights.Count() / (double)model.Pager.PageSize);

            return View(model);
        }


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
                return RedirectToAction("Reserve", "Reservations", new { flightid = createModel.FlightId, ticketNum = createModel.TicketNum, availableRegularSeats = createModel.RegularSeats, availableBusinessSeats = createModel.BusinessSeats });
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            FlightCreateViewModel model = new FlightCreateViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Flight flight = _context.Flights.Find(id);
            if (flight == null)
            {
                return NotFound();
            }

            FlightEditViewModel model = new FlightEditViewModel
            {
                Id = flight.Id,
                LocationFrom = flight.LocationFrom,
                LocationTo = flight.LocationTo,
                DepartureTime = flight.DepartureTime,
                LandingTime = flight.LandingTime,
                PlaneType = flight.PlaneType,
                PlaneNumber = flight.PlaneNumber,
                PilotName = flight.PilotName,
                RegularSeats = flight.RegularSeats,
                BusinessSeats = flight.BusinessSeats
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(FlightEditViewModel editModel)
        {
            if (ModelState.IsValid)
            {
                Flight flight = new Flight()
                {
                    Id = editModel.Id,
                    LocationFrom = editModel.LocationFrom,
                    LocationTo = editModel.LocationTo,
                    DepartureTime = editModel.DepartureTime,
                    LandingTime = editModel.LandingTime,
                    PlaneType = editModel.PlaneType,
                    PlaneNumber = editModel.PlaneNumber,
                    PilotName = editModel.PilotName,
                    RegularSeats = editModel.RegularSeats,
                    BusinessSeats = editModel.BusinessSeats
                };
                try
                {
                    _context.Flights.Update(flight);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(AdminList));
            };
            return View(editModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Flight flight = _context.Flights.Find(id);
            _context.Flights.Remove(flight);
            _context.SaveChanges();
            return RedirectToAction(nameof(AdminList));
        }

    }
}
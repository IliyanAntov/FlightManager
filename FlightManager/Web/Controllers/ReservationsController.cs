using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models.Flights;

namespace Web.Controllers
{
    public class ReservationsController : Controller
    {
        private const int PageSize = 10;
        private readonly ApplicationDbContext _context;


        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Reservations
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reserve(ReserveViewModel model)
        {
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ReserveViewModel createModel)
        {
            if (ModelState.IsValid)
            {

                Reservation reservation = new Reservation()
                {
                    FlightId = createModel.FlightId,
                    Email = createModel.Email,
                    Passangers = new List<Passanger>()
                };
                foreach (var passangerViewModel in createModel.Passangers)
                {
                    Passanger passanger = new Passanger
                    {
                        FirstName = passangerViewModel.FirstName,
                        MiddleName = passangerViewModel.MiddleName,
                        LastName = passangerViewModel.LastName,
                        UCN = passangerViewModel.UCN,
                        PhoneNumber = passangerViewModel.PhoneNumber,
                        Nationality = passangerViewModel.Nationality,
                        TicketType = passangerViewModel.TicketType
                    };
                    reservation.Passangers.Add(passanger);
                }
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
            }
            return RedirectToAction("UserList", "Flights");
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reservations/Edit/5
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

        // GET: Reservations/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reservations/Delete/5
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Data.Entity;
using Data.Enumeration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Flights;
using Web.Models.Reservations;
using Web.Models.Shared;
using Web.Models.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using RazorLight;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;

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
        public ActionResult List(ReservationListViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<ReservationFlightDataViewModel> items = new List<ReservationFlightDataViewModel>();
            foreach (var flight in _context.Flights.Skip((model.Pager.CurrentPage - 1) * PageSize).Take(PageSize).ToList())
            {
                var viewModel = new ReservationFlightDataViewModel()
                {
                    Id = flight.Id,
                    DepartureTime = flight.DepartureTime,
                    FlightSource = flight.LocationFrom,
                    FlightDestination = flight.LocationTo,
                    PlaneNum = flight.PlaneNumber,
                    Reservations = new List<ReservationDataViewModel>()


                };
                foreach (var reservation in _context.Reservations.Where(x => x.FlightId == flight.Id).Include(x => x.Passangers).ToList())
                {
                    var reservationModel = new ReservationDataViewModel()
                    {
                        Id = reservation.Id,
                        Email = reservation.Email,
                        NumberOfTickets = reservation.Passangers.Count()
                    };
                    viewModel.Reservations.Add(reservationModel);
                }
                items.Add(viewModel);
            }


            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(_context.Flights.Count() / (double)PageSize);

            return View(model);
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
                var regularTicketNum = 0;
                var businessTicketNum = 0;
                foreach (var passanger in createModel.Passangers)
                {
                    if (passanger.TicketType == TicketTypeEnum.Regular)
                    {
                        regularTicketNum++;
                    }
                    else
                    {
                        businessTicketNum++;
                    }
                }

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
                return RedirectToAction("Confirmation", "Reservations", new { reservationId = reservation.Id, flightId = createModel.FlightId, email = createModel.Email });
            }
            return RedirectToAction("UserList", "Flights");
        }

        public ActionResult Confirmation(int reservationId, int flightId, string email)
        {
            var flight = _context.Flights.FirstOrDefault(x => x.Id == flightId);
            var reservation = _context.Reservations.Include(y => y.Passangers).FirstOrDefault(x => x.Id == reservationId);

            var confirmationModel = new ConfirmationViewModel()
            {
                Email = email,
                DepartureTime = flight.DepartureTime,
                FlightSource = flight.LocationFrom,
                FlightDestination = flight.LocationTo,
                Passangers = new List<PassangerDataViewModel>()
            };
            foreach (var passanger in reservation.Passangers)
            {
                PassangerDataViewModel passangerModel = new PassangerDataViewModel()
                {
                    FirstName = passanger.FirstName,
                    MiddleName = passanger.MiddleName,
                    LastName = passanger.LastName,
                    UCN = passanger.UCN,
                    PhoneNumber = passanger.PhoneNumber,
                    Nationality = passanger.Nationality,
                    TicketType = passanger.TicketType
                };
                confirmationModel.Passangers.Add(passangerModel);
            }
            SendMailAsync(confirmationModel);
            return View(confirmationModel);
        }

        private async void SendMailAsync(ConfirmationViewModel model)
        {
            string path = "Views/Reservations/Confirmation.cshtml";
            string template = "";
            using (StreamReader sr = new StreamReader(path))
            {
                template = sr.ReadToEnd();
            }

            var engine = new RazorLightEngineBuilder()
              .UseFileSystemProject("C:/Users/SoMe0nE/Desktop/FlightManager/FlightManager/Web/Views")
              .UseMemoryCachingProvider()
              .Build();

            string result = await engine.CompileRenderAsync("Reservations/Email.cshtml", model);
            var email = new MailMessage();
            email.To.Add(new MailAddress(model.Email));
            email.From = new MailAddress("flightsmanager.iantov@gmail.com");
            email.Subject = "Reservation details";
            email.Body = result;
            email.IsBodyHtml = true;
            var smtp = new SmtpClient();
            var credential = new NetworkCredential
            {
                UserName = "flightsmanager.iantov@gmail.com",  // replace with valid value
                Password = "flightsmanager"  // replace with valid value
            };
            smtp.Credentials = credential;
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(email);
        }



        // GET: Reservations/Details/5
        public ActionResult Details(int reservationId)
        {
            var model = new ReservationDetailsListViewModel();
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            var reservation = _context.Reservations.Include(x => x.Passangers).FirstOrDefault(y => y.Id == reservationId);

            List<PassangerDataViewModel> items = new List<PassangerDataViewModel>();

            foreach (var passanger in reservation.Passangers)
            {
                var viewModel = new PassangerDataViewModel()
                {
                    FirstName = passanger.FirstName,
                    MiddleName = passanger.MiddleName,
                    LastName = passanger.LastName,
                    UCN = passanger.UCN,
                    Nationality = passanger.Nationality,
                    PhoneNumber = passanger.PhoneNumber,
                    TicketType = passanger.TicketType
                };

                items.Add(viewModel);
            }

            model.PlaneNum = _context.Flights.FirstOrDefault(x => x.Id == reservation.FlightId).PlaneNumber;
            model.Items = items;
            model.Pager.PagesCount = (int)Math.Ceiling(model.Items.Count() / (double)PageSize);

            return View(model);
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
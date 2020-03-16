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
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    public class ReservationsController : Controller
    {

        private readonly ApplicationDbContext _context;


        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Employee")]
        public ActionResult List(ReservationListViewModel model)
        {
            model.Pager ??= new PagerViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;
            model.Pager.PageSize = model.Pager.PageSize <= 0 ? 10 : model.Pager.PageSize;
            List<Flight> flights = new List<Flight>();
            if (model.FilterCriteria != null && model.Filter != null)
            {
                flights = _context.Flights.ToList();
            }
            else
            {
                flights = _context.Flights.Skip((model.Pager.CurrentPage - 1) * model.Pager.PageSize).Take(model.Pager.PageSize).ToList();
            }

            List<ReservationFlightDataViewModel> items = new List<ReservationFlightDataViewModel>();
            foreach (var flight in flights)
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
                List<Reservation> reservations = new List<Reservation>();
                if (model.FilterCriteria != null && model.Filter != null)
                {
                    switch (model.FilterCriteria)
                    {
                        case "email":
                            reservations = _context.Reservations.Include(x => x.Passangers).Where(x => x.Email.Contains(model.Filter)).ToList();
                            break;
                    }
                }
                else
                {
                    reservations = _context.Reservations.Include(x => x.Passangers).ToList();
                }

                foreach (var reservation in reservations.Where(x => x.FlightId == flight.Id).ToList())
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
            model.Pager.PagesCount = (int)Math.Ceiling(_context.Flights.Count() / (double)model.Pager.PageSize);

            return View(model);
        }

        public ActionResult Reserve(int flightId, int ticketNum, int availableRegularSeats, int availableBusinessSeats)
        {
            var model = new ReserveViewModel()
            {
                FlightId = flightId,
                TicketNum = ticketNum,
                AvailableRegularSeats = availableRegularSeats,
                AvailableBusinessSeats = availableBusinessSeats
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reserve(ReserveViewModel createModel)
        {
            if (ModelState.IsValid)
            {
                var requestedRegularTickets = 0;
                var requestedBusinessTickets = 0;
                foreach (var passanger in createModel.Passangers)
                {
                    if (passanger.TicketType == TicketTypeEnum.Regular)
                    {
                        requestedRegularTickets++;
                    }
                    else
                    {
                        requestedBusinessTickets++;
                    }
                }

                var flight = _context.Flights.First(x => x.Id == createModel.FlightId);
                var availableRegularTickets = flight.RegularSeats;
                var availableBusinessTickets = flight.BusinessSeats;
                var reservations = _context.Reservations.Include(x => x.Passangers).Where(x => x.FlightId == createModel.FlightId);
                foreach (var flightReservation in reservations)
                {
                    foreach(var flightPassanger in flightReservation.Passangers)
                    {
                        if(flightPassanger.TicketType == TicketTypeEnum.Regular)
                        {
                            availableRegularTickets--;
                        }
                        else{
                            availableBusinessTickets--;
                        }
                    }
                }

                if((availableRegularTickets - requestedRegularTickets < 0 )||(availableBusinessTickets - requestedBusinessTickets < 0))
                {
                    return RedirectToAction("Error", "Home", new { errorMessage = "Not enough seats available" });
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
            else
            {
                foreach (var passanger in createModel.Passangers)
                {
                    if(passanger.TicketType == TicketTypeEnum.Regular)
                    {
                        createModel.AvailableRegularSeats--;
                    }
                    else
                    {
                        createModel.AvailableBusinessSeats--;
                    }
                }
                return View(createModel);
            }
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
                UserName = "flightsmanager.iantov@gmail.com",
                Password = "flightsmanager"
            };
            smtp.Credentials = credential;
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(email);
        }

        [Authorize(Roles = "Admin,Employee")]
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
            model.Pager.PagesCount = (int)Math.Ceiling(model.Items.Count() / (double)model.Pager.PageSize);

            return View(model);
        }


    }
}
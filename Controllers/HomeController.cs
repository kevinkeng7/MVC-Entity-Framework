using CarInsurance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsurance.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QuoteForm(string firstName, string lastName, string emailAddress, 
            DateTime dateofBirth, int carYear, string carMake, 
            string carModel, int ticketNumber, bool dui = false, bool coverage = false)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress)
                || string.IsNullOrEmpty(dateofBirth.ToString()) || string.IsNullOrEmpty(carYear.ToString())
                || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel) || string.IsNullOrEmpty(ticketNumber.ToString())
                || string.IsNullOrEmpty(dui.ToString()) || string.IsNullOrEmpty(coverage.ToString()))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using (CarInsuranceEntities db = new CarInsuranceEntities())
                {
                    var quote = new QuoteForm();
                    quote.FirstName = firstName;
                    quote.LastName = lastName;
                    quote.EmailAddress = emailAddress;
                    quote.DateOfBirth = dateofBirth;
                    quote.CarYear = carYear;
                    quote.CarMake = carMake;
                    quote.CarModel = carModel;
                    quote.TicketNumber = ticketNumber;
                    quote.DUI = dui;
                    quote.FullCoverage = coverage;

                    var today = DateTime.Today;
                    var age = today.Year - dateofBirth.Year;

                    var finalQuote = 50m;

                    if (age <= 18) //If user is 18 and under, add $100 to the monthly total.
                    {
                        finalQuote += 100;
                    }
                    else if (age >= 19 && age <= 25) //If user between 19 and 25, add $50 to the monthly total.
                    {
                        finalQuote += 50;
                    }
                    else if (age > 25) //If user over 25, add $25 to the monthly total.
                    {
                        finalQuote += 25; 
                    }

                    if (carYear < 2000) //If car's year is before 2000, add $25 to the monthly total.
                    {
                        finalQuote += 25;
                    }
                    else if (carYear > 2015) //If car's year is after 2015, add $25 to the monthly total.
                    {
                        finalQuote += 25;
                    }

                    if (carMake == "Porsche") //If car's Make is a Porsche, add $25 to the price.
                    {
                        finalQuote += 25;
                    }
                    else if (carMake == "Porsche" && carModel == "911 Carrera") //If car's Make is a Porsche and model is a 911 Carrera, add an additional $25 to the price.
                    {
                        finalQuote += 25;
                    }

                    if (ticketNumber >= 1) //Add $10 to the monthly total for every speeding ticket the user has.
                    {
                        finalQuote = finalQuote + (finalQuote * 10);
                    }
                    else
                    {
                        finalQuote += 0;
                    }

                    if (dui == true) //If the user has ever had a DUI, add 25% to the total.
                    {
                        finalQuote = finalQuote + (finalQuote * 0.25m);
                    }
                    else
                    {
                        finalQuote += 0;
                    }

                    if (coverage == true) //If it's full coverage, add 50% to the total.
                    {
                        finalQuote = finalQuote + (finalQuote * 0.50m);
                    }
                    else
                    {
                        finalQuote += 0;
                    }

                    quote.FinalQuote = finalQuote;

                    db.QuoteForms.Add(quote);
                    db.SaveChanges();
                }
                return View("Success");
            }
        }
    }
}
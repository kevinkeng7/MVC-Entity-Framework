using CarInsurance.Models;
using CarInsurance.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsurance.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (CarInsuranceEntities db = new CarInsuranceEntities())
            {
                var QuoteForms = db.QuoteForms;
                var quoteFormVms = new List<QuoteFormVm>();
                foreach (var quote in QuoteForms)
                {
                    var quoteVm = new QuoteFormVm();
                    quoteVm.Id = quote.Id;
                    quoteVm.FirstName = quote.FirstName;
                    quoteVm.LastName = quote.LastName;
                    quoteVm.EmailAddress = quote.EmailAddress;
                    quoteVm.FinalQuote = Convert.ToDecimal(quote.FinalQuote);
                    quoteFormVms.Add(quoteVm);
                }

                return View(quoteFormVms);
            }        
        }
    }
}
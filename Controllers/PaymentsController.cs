using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Data;
using Portfolio.Models;
using Stripe;

namespace Portfolio.Controllers
{
    public class PaymentsController : Controller
    {
        UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbcontext;

        public PaymentsController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbcontext)
        {
            this.userManager = userManager;
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Charge()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult SalesHistory(string email)
        {
            //var id = userManager.GetUserId(currentuser);

            //var customer = dbcontext.Roles.Where(c => c.Id == "Customer");
            //var purchased = dbcontext.PurchasedTickets.Where(p => p.EventId ==)


            return View();
        }

        public IActionResult Payment(string stripeEmail, string stripeToken, int amount, decimal amountDollars, int eventId)
        {
            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            try
            {
                var charge = charges.Create(new StripeChargeCreateOptions
                {
                    Amount = amount,
                    Description = "Sample Charge",
                    Currency = "usd",
                    CustomerId = customer.Id
                });
            } catch(Stripe.StripeException e)
            {
                System.Diagnostics.Debug.WriteLine("Error was: " + e.Message);
                return View("Failed", e.Message);
            }
                

            // get current user id
            System.Security.Claims.ClaimsPrincipal currentuser = this.User;
            var id = userManager.GetUserId(currentuser);

            // go to purchasedticket table and set purchasedID
            var paid = dbcontext.PurchasedTickets.Where(p => p.UserId == id).FirstOrDefault();

            PurchasedTicket pt = new PurchasedTicket
            {
                UserId = paid.UserId,
                PurchaseAmount = paid.PurchaseAmount,
                EventId = paid.EventId

            };
            dbcontext.SaveChanges();




            return View("Success");
        }
    }
}
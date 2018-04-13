using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;

namespace Portfolio.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        //public DbSet<IPN> IPNs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }

    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }
        [Display(Name = "Max Seats")]
        public int MaxSeats { get; set; }
        public decimal Price { get; set; }
        [Display(Name = "Image URL")]
        public string ImageURL { get; set; }

        public virtual List<Event> Events { get; set; }
    }

    public class PurchasedTicket
    {
        [Key]
        public int PurchasedId { get; set; }

        public int EventId { get; set; }

        //[Display(Name = "Transaction ID")]
        //public string TransactionID { get; set; }

        //[Display(Name = "Session ID")]
        //public string SessionId { get; set; }

        [Display(Name = "Purchase Amount")]
        public decimal PurchaseAmount { get; set; }

        [Display(Name = "User ID")]
        public string UserId { get; set; }

        //Refer to the parent tables
        public virtual ApplicationUser User { get; set; }
        public virtual Event Event { get; set; }
        //public virtual IPN IPNs { get; set; }


    }
    //public class IPN
    //{

    //    [Key]
    //    [Display(Name = "Transaction ID")]
    //    public string TransactionID { get; set; }

    //    [Display(Name = "Purchase Time")]
    //    public string TxTime { get; set; }

    //    [Display(Name = "First Name")]
    //    public string PayerFirstName { get; set; }

    //    [Display(Name = "Last Name")]
    //    public string PayerLastName { get; set; }

    //    [Display(Name = "Session ID")]
    //    public string SessionId { get; set; }

    //    [Display(Name = "Total Tickets")]
    //    public string Quantity;

    //    public string Status;

    //    [Display(Name = "Transaction Amount")]
    //    public string Amount { get; set; }

    //}

}

using FinalProject.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Contexts
{
    public class FinalProjectDatbase:IdentityDbContext<AppUser>
    {
        public FinalProjectDatbase(DbContextOptions<FinalProjectDatbase> options):base(options)
        {

        }
        public DbSet<IntrestMessage> IntrestMessages { get; set; }
        public DbSet<AboutPage> aboutPages { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<AgentedHouse> AgentedHouses { get; set; }
        public DbSet<AgentedHousePhoto> agentedHousePhotos { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Typee> Typees { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Distinct> Distincts { get; set; }
        public DbSet<Favorite> Favorities { get; set; }
        public DbSet<HouseImage> HouseImages { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Metro> Motros { get; set; }
        public DbSet<ReplyComment> replyComments { get;set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<PageAbout> PageAbouts { get; set; }
        public DbSet<Advertise> Advertises { get; set; }
        public DbSet<SalePrice> SalePrices { get; set; }
        public DbSet<RoomCount> RoomCounts { get;set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Muraciet> Muraciets { get; set; }
    }
}

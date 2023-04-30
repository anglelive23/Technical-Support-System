namespace Gateway.API.Entities.Data
{
    public class TicketingContext : IdentityDbContext<ApplicationUser>
    {
        #region Constructors
        public TicketingContext(DbContextOptions<TicketingContext> options) : base(options) { }
        #endregion

        #region DbSets
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                .ToTable("Roles", "security");

            builder.Entity<ApplicationUser>()
                .ToTable("Users", "security");

            builder.Entity<IdentityRoleClaim<string>>()
                .ToTable("RoleClaims", "security");

            builder.Entity<IdentityUserClaim<string>>()
                .ToTable("UserClaims", "security");

            builder.Entity<IdentityUserLogin<string>>()
                .ToTable("UserLogins", "security");

            builder.Entity<IdentityUserRole<string>>()
                .ToTable("UserRoles", "security");

            builder.Entity<IdentityUserToken<string>>()
                .ToTable("UserTokens", "security");

            builder.Entity<RefreshToken>()
                .ToTable("RefreshTokens", "security");


            builder.Entity<Ticket>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
        }
        #endregion
    }
}

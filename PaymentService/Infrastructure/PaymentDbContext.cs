using Microsoft.EntityFrameworkCore;

namespace PaymentService.Infrastructure
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options) { }

        public DbSet<Domain.Payment> Payments => Set<Domain.Payment>();
    }
}

using Microsoft.EntityFrameworkCore;

namespace ClinicaOdontologica
{
    public class ClinicaDbContext : DbContext
    {
        public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Dentista> Dentistas { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Clinica> Clinicas { get; set; }
    }
}

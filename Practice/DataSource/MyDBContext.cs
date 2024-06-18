using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Practice.Model;

namespace Practice.DataSource
{
    class MyDBContext : DbContext
    {
        // Change your Correct DB Info
        private ServerVersion ServerVersion = new MariaDbServerVersion(new Version(11, 3, 2));

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString, ServerVersion);
        }

        public DbSet<EmployeeModel> tb_employee{ get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bumbo.Data.Context;

public partial class BumboDbContext
{
    private readonly IConfiguration _configuration;
    
    public BumboDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public BumboDbContext(DbContextOptions<BumboDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Bumbo"));
            optionsBuilder.UseSqlServer("Server = .; Database = Bumbo; Integrated Security = True; TrustServerCertificate = True;");
        }
    }
}
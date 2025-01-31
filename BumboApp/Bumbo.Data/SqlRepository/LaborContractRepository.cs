using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.Data.SqlRepository;

public class LaborContractRepository(BumboDbContext context) : ILaborContractRepository
{
    private readonly BumboDbContext _context = context;

    public IQueryable<LaborContract> GetAllLaborContracts()
    {
        return _context.LaborContracts;
    }
}
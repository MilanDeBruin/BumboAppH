using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.Data.SqlRepository;

public class BranchRepository(BumboDbContext context) : IBranchRepository
{
    private readonly BumboDbContext _context = context;

    public IQueryable<Branch> GetAllBranches()
    {
        return _context.Branches;
    }
}
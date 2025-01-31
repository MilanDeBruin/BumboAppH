using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface IBranchRepository
{
    public IQueryable<Branch> GetAllBranches();
}
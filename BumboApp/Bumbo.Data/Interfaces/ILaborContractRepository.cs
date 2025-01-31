using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface ILaborContractRepository
{
    public IQueryable<LaborContract> GetAllLaborContracts();
}
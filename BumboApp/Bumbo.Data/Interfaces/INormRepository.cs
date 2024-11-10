using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces
{
    public interface INormRepository
    {
        List<Norm> GetAll();
        public List<Norm> GetAllFromBranch(int i);
        public List<int> GetAllUniqueBranchIds();
        Norm GetOne(int value);
        Norm Update(Norm norm);
    }
}

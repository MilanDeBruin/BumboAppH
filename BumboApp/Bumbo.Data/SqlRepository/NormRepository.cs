using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bumbo.Data.SqlRepository
{
    public class NormRepository : INormRepository
    {
        readonly BumboDbContext ctx;

        public NormRepository(BumboDbContext ctx)
        {
            this.ctx = ctx;
        }

        

        public List<Norm> GetAll() => ctx.Norms.ToList();

        public List<Norm> GetAllFromBranch(int i) => ctx.Norms.Where(n => n.BranchId == i).ToList();

        public List<int> GetAllUniqueBranchIds() => ctx.Norms.Select(n => n.BranchId).Distinct().ToList();


        public Norm GetOne(int id)
        {
            return ctx.Norms
                      .Include(n => n.Branch) // Include the related Branch entity
                      .FirstOrDefault(n => n.BranchId == id); // Find the Norm by BranchId
        }

        public Norm Update(Norm norm) 
        {
            ctx.Norms.Update(norm);
            ctx.SaveChanges();
            return norm;
        }
        public async Task UpdateAsync(Norm norm)
        {
            ctx.Norms.Update(norm); // Mark the entity as modified
            await ctx.SaveChangesAsync(); // Save changes to the database
        }

    }
}

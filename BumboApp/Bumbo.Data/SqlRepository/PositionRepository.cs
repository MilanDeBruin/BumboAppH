using Bumbo.Data.Context;
using Bumbo.Data.Interfaces;
using Bumbo.Data.Models;

namespace Bumbo.Data.SqlRepository;

public class PositionRepository(BumboDbContext context) : IPositionRepository
{
    private readonly BumboDbContext _context = context;

    public IQueryable<Position> GetAllPositions()
    {
        return _context.Positions;
    }
}
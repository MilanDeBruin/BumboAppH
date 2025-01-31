using Bumbo.Data.Models;

namespace Bumbo.Data.Interfaces;

public interface IPositionRepository
{
    public IQueryable<Position> GetAllPositions();
}
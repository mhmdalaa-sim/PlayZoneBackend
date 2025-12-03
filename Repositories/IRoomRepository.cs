using PlayZone.Models;

namespace PlayZone.Repositories;

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(int id);
    Task<Room> UpdateAsync(Room room);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync();
}

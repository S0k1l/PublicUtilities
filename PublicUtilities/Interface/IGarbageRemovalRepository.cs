using PublicUtilities.Models;

namespace PublicUtilities.Interface
{
    public interface IGarbageRemovalRepository
    {
        Task<GarbageRemoval> GetGarbageRemovalAsync();
        bool Update(GarbageRemoval garbageRemoval);
        bool Save();
    }
}

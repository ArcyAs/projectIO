using System.Threading.Tasks;
using FoodDiary.Repositories.Entities;

namespace Repositories.Abstract
{
    public interface IUserRepository
    {
        Task AddUserDetails(UserDetailsEntity userDetailsEntity);
    }
}
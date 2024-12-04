using BanhXeoProject.Entities;

namespace BanhXeoProject.Service
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }
}

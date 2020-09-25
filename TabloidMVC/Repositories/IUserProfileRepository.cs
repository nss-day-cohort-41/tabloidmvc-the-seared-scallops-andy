using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        List<UserProfile> GetAllUsers();
        UserProfile GetById(int id);
        List<UserType> GetAllTypes();
        void UpdateUserProfile(UserProfile profile);
        List<UserProfile> CheckForAdmins();
    }
}
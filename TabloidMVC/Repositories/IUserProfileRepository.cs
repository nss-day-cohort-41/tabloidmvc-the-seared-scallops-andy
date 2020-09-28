using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        List<UserProfile> GetAllUsers(int IsActive);
        UserProfile GetById(int id);
        List<UserType> GetAllTypes();
        void UpdateUserProfile(UserProfile profile);
        List<UserProfile> CheckForAdmins();
        List<UserProfile> CheckForActiveAdmins();
        bool VerifyAdminStatus(int id);
        void AddNew(UserProfile profile);
    }
}
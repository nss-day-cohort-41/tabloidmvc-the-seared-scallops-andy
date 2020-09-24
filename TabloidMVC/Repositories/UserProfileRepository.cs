using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public List<UserProfile> GetAllUsers()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT  up.id, 
                                                DisplayName, 
                                                FirstName, 
                                                LastName, 
                                                Email, 
                                                CreateDateTime, 
                                                ImageLocation, 
                                                UserTypeId, 
                                                ut.[Name] AS Role
                                            FROM UserProfile up
                                            LEFT JOIN UserType ut ON ut.Id = up.UserTypeId
                                            ORDER BY DisplayName DESC
                                            ";
                    SqlDataReader reader = cmd.ExecuteReader();
                    var users = new List<UserProfile>();

                    while (reader.Read())
                    {
                        //Uses BuildUserProfile Method to assemble the UserProfile instance
                        var user = BuildUserProfile(reader);
                        users.Add(user);                        
                    }
                    reader.Close();
                    return users;
                }
            }
        }

        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT  up.id, 
                                                DisplayName, 
                                                FirstName, 
                                                LastName, 
                                                Email, 
                                                CreateDateTime, 
                                                ImageLocation, 
                                                UserTypeId, 
                                                ut.[Name] AS Role
                                            FROM UserProfile up
                                            LEFT JOIN UserType ut ON ut.Id = up.UserTypeId
                                        WHERE up.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = BuildUserProfile(reader);
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }

        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }

        public void UpdateUserProfile(UserProfile profile)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        UPDATE UserProfile
		                                        SET UserTypeId = @userType
		                                    WHERE Id = @id
";
                    cmd.Parameters.AddWithValue("@userType", profile.UserTypeId);
                    cmd.Parameters.AddWithValue("@id", profile.Id);

                    cmd.ExecuteNonQuery();

                }
            }

        }

        //Get the list of all available user types
        public List<UserType> GetAllTypes()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT  Id, 
                                                Name 
                                        FROM UserType    ";
                    SqlDataReader reader = cmd.ExecuteReader();
                    var types = new List<UserType>();
                    while (reader.Read())
                    {
                        var type = new UserType()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        types.Add(type);
                    }
                    return types;
                }
            }
        }
        //Method to buid a user profile instance and return it to the caller

        private UserProfile BuildUserProfile(SqlDataReader reader)
        {
            var user = new UserProfile()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                UserType = new UserType()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                        Name = reader.GetString(reader.GetOrdinal("Role"))
                    },
            };
            return user;
        }
    }
}

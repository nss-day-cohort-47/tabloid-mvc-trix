using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public List<UserProfile> GetAllUserProfiles()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT u.id, u.DisplayName, u.FirstName, u.LastName, u.Email,
                                 u.ImageLocation, u.UserTypeId, ut.[Name] 
                            FROM UserProfile u
                            LEFT JOIN UserType ut on ut.Id = u.UserTypeId
                            ORDER BY u.DisplayName
                            ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<UserProfile> userprofiles = new List<UserProfile>();
                    while (reader.Read())
                    {
                        UserProfile userprofile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            ImageLocation = reader.IsDBNull(reader.GetOrdinal("ImageLocation")) ? "Image was not found" : reader.GetString(reader.GetOrdinal("ImageLocation")),
                            UserType = new UserType
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };

                        userprofiles.Add(userprofile);
                    }
                    reader.Close();

                    return userprofiles;
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

        public UserProfile Add(UserProfile user)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserProfile (DisplayName, FirstName, LastName, Email, CreateDateTime, ImageLocation UserTypeId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@displayName, @firstName, @lastName, @email, @createDateTime, @imageLocation, @userTypeId);";


                    cmd.Parameters.AddWithValue("@displayName", user.DisplayName);
                    cmd.Parameters.AddWithValue("@firstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", user.LastName);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@createDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@userTypeId", 2);

                    if (user.ImageLocation == null)
                    {
                        cmd.Parameters.AddWithValue("@imageLocation", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@imageLocation", user.ImageLocation);
                    }

                    int newlyCreatedUserId = (int)cmd.ExecuteScalar();

                    user.Id = newlyCreatedUserId;
                }
            }
        }
    }
}

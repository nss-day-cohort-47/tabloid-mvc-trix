using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }

        public List<Comment> GetCommentsById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT c.Id, 
                               c.PostId, 
                               c.UserProfileId, 
                               u.Id,
                               c.Subject, 
                               c.Content, 
                               c.CreateDateTime
                        FROM Comment c
                             LEFT JOIN Post p ON p.Id = c.PostId  
                             LEFT JOIN UserProfile u on u.Id = c.UserProfileId
                        WHERE p.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();
                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        };

                        comments.Add(comment);
                    }

                    reader.Close();

                    return comments;
                }
            }
        }

        public void AddComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (PostId, UserProfileId, Subject, Content, CreateDateTime)
                        OUTPUT INSERTED.ID
                        VALUES (@postid, @userprofileid, @subject, @content, @createdatetime)
                        ";


                    cmd.Parameters.AddWithValue("@postid", comment.PostId);
                    cmd.Parameters.AddWithValue("@userprofileid", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@createdatetime", comment.CreateDateTime);

                    int id = (int)cmd.ExecuteScalar();

                    comment.Id = id;

                }
            }
        }

    }
}

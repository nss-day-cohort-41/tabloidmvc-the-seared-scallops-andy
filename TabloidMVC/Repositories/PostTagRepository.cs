using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class PostTagRepository : BaseRepository, IPostTagRepository
    {
        public PostTagRepository(IConfiguration config) : base(config) { }

        public List<Tags> GetPostTags(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT pt.Id, pt.PostId, pt.TagId, t.Name
                            FROM PostTag pt
                            JOIN Tag t on t.id = pt.TagId
                            WHERE pt.PostId = @id
                                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();
                    var tags = new List<Tags>();
                    while(reader.Read());
                    {
                        Tags tag = new Tags
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        tags.Add(tag);
                    }

                    reader.Close();
                    return tags;
                }
            }
        }

    public List<Tags> GetTagsRemainderByPost(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT t.Id, t.Name
                            FROM Tag t
                            Left OUTER JOIN PostTag pt
                            ON (t.id = pt.TagId AND pt.PostId = @id)
                            where pt.TagId IS NULL
                                       ";

                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();
                    var Tags = new List<Tags>();
                    while(reader.Read())
                    {
                        Tags tag = new Tags
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        Tags.Add(tag);
                    }
                    reader.Close();
                    return Tags;
                }
            }
        }


        public void AddTagToPost(int postId, int tagId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            INSERT INTO PostTag (PostId, TagId)
                            OUTPUT INSERTED.Id
                            VALUES(@postId, @tagId)
                                       ";
                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);

                    cmd.ExecuteNonQuery();
                }
            }


    }
    }

    
}

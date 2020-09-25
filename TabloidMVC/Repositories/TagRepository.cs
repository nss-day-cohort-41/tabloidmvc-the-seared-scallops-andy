using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration config) : base(config) {  }

        public List<Tags> GetAllTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT
                                         Id,
                                         Name
                                    FROM Tag
                                    WHERE Flag_Delete = 0
                                    ORDER BY Name
                                        ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Tags> tags = new List<Tags>();

                    while (reader.Read())
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

        public Tags GetTagById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT Id,
                                   Name
                            FROM Tag
                            WHERE Id = @id 
                            AND Flag_Delete = 0
                                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    if(reader.Read())
                    {
                        Tags tag = new Tags()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };

                        reader.Close();
                        return tag;
                    }
                    reader.Close();
                    return null;
                }
            }
        }

        public void AddTag(Tags tag)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            INSERT INTO Tag (Name, Flag_Delete)
                            OUTPUT INSERTED.Id
                            VALUES (@name, 0)
                                        ";

                    cmd.Parameters.AddWithValue("@Name", tag.Name);

                    int id = (int)cmd.ExecuteScalar();

                    tag.Id = id;

                }
            }
        }

        public void UpdateTag(Tags tag)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Tag
                        SET Name = @name
                        WHERE Id = @id
                                        ";
                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTag(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                UPDATE Tag
                                SET Flag_Delete = 1
                                WHERE Id = @id
                                        ";

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}

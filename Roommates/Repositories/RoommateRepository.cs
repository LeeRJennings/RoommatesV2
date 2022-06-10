using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionstring) : base(connectionstring) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT rm.FirstName, r.Name, rm.RentPortion FROM Roommate rm LEFT JOIN Room r on r.Id = rm.RoomId WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                Room = new Room
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                }
                            };
                        }
                        return roommate;
                    }
                }
            }
        }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Roommate";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommateList = new List<Roommate>();
                        while (reader.Read())
                        {
                            int idValue = reader.GetInt32(reader.GetOrdinal("Id"));
                            string firstNameValue = reader.GetString(reader.GetOrdinal("FirstName"));
                            string lastNameValue = reader.GetString(reader.GetOrdinal("LastName"));

                            Roommate rm = new Roommate
                            {
                                Id = idValue,
                                FirstName = firstNameValue,
                                LastName = lastNameValue
                            };
                            roommateList.Add(rm);
                        }
                        return roommateList;
                    }
                }
            }
        }
    }
}

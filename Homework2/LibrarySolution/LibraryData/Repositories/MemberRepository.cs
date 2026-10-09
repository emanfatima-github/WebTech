using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using LibraryData.Models;

namespace LibraryData.Repositories
{
    public class MemberRepository
    {
        public bool AddMember(Member member)
        {
            using SqlConnection connection =
                new SqlConnection(DatabaseHelper.ConnectionString);

            string query =
                @"INSERT INTO Members (MemberId, Name)
                  VALUES (@MemberId, @Name)";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@MemberId", member.MemberId);
            command.Parameters.AddWithValue("@Name", member.Name);

            connection.Open();

            return command.ExecuteNonQuery() > 0;
        }

        public List<Member> GetAllMembers()
        {
            List<Member> members = new List<Member>();

            using SqlConnection connection =
                new SqlConnection(DatabaseHelper.ConnectionString);

            string query =
                "SELECT MemberId, Name FROM Members";

            using SqlCommand command =
                new SqlCommand(query, connection);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Member member = new Member();

                member.MemberId =
                    Convert.ToInt32(reader["MemberId"]);

                member.Name =
                    reader["Name"].ToString();

                members.Add(member);
            }

            return members;
        }
    }
}

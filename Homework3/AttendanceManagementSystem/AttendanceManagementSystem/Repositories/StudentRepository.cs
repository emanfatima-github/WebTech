
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using AttendanceManagementSystem.Models;

namespace AttendanceManagementSystem.Repositories
{
    public class StudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT * FROM Students ORDER BY StudentId", connection);

            DataTable table = new DataTable();

            await Task.Run(() => adapter.Fill(table));

            List<Student> students = new List<Student>();

            foreach (DataRow row in table.Rows)
            {
                students.Add(new Student
                {
                    StudentId = Convert.ToInt32(row["StudentId"]),
                    StudentName = row["StudentName"].ToString()!,
                    RegistrationNumber = row["RegistrationNumber"].ToString()!,
                    Department = row["Department"].ToString()!,
                    AttendancePercentage =
                        Convert.ToDecimal(row["AttendancePercentage"])
                });
            }

            return students;
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT * FROM Students WHERE StudentId = @Id", connection);

            adapter.SelectCommand!.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            DataTable table = new DataTable();

            await Task.Run(() => adapter.Fill(table));

            if (table.Rows.Count == 0)
                return null;

            DataRow row = table.Rows[0];

            return new Student
            {
                StudentId = Convert.ToInt32(row["StudentId"]),
                StudentName = row["StudentName"].ToString()!,
                RegistrationNumber = row["RegistrationNumber"].ToString()!,
                Department = row["Department"].ToString()!,
                AttendancePercentage =
                    Convert.ToDecimal(row["AttendancePercentage"])
            };
        }

        public async Task AddAsync(Student student)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand command = new SqlCommand(
                @"INSERT INTO Students
                (StudentName, RegistrationNumber, Department, AttendancePercentage)
                VALUES (@Name, @Registration, @Department, @Attendance)",
                connection);

            AddParameters(command, student);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand command = new SqlCommand(
                @"UPDATE Students
                SET StudentName = @Name,
                    RegistrationNumber = @Registration,
                    Department = @Department,
                    AttendancePercentage = @Attendance
                WHERE StudentId = @Id",
                connection);

            AddParameters(command, student);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = student.StudentId;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand command = new SqlCommand(
                "DELETE FROM Students WHERE StudentId = @Id", connection);

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        private static void AddParameters(SqlCommand command, Student student)
        {
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 100)
                .Value = student.StudentName;

            command.Parameters.Add("@Registration", SqlDbType.NVarChar, 30)
                .Value = student.RegistrationNumber;

            command.Parameters.Add("@Department", SqlDbType.NVarChar, 50)
                .Value = student.Department;

            command.Parameters.Add("@Attendance", SqlDbType.Decimal).Value =
                student.AttendancePercentage;

            command.Parameters["@Attendance"].Precision = 5;
            command.Parameters["@Attendance"].Scale = 2;
        }
    }
}

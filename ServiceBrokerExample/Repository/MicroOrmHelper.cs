using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ServiceBrokerExample.Model;

namespace ServiceBrokerExample
{
    public class MicroOrmHelper<T>
    {
        private static string cnnStr;
        public MicroOrmHelper()
        {
            cnnStr = "Server=localhost; Database=ServiceBrokerExample; User ID=sa; Password=Password1234;";
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(cnnStr);
            }
        }
        public int Add(T t)
        {
            int rowAffected = 0;

            using (IDbConnection con = Connection)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var query = @"INSERT INTO Customer VALUES (@Name , @Surname);";

                rowAffected = con.Execute(query, t);
            }

            return rowAffected;
        }

        public T GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Customer"
                               + " WHERE Id = @Id";
                dbConnection.Open();
                return dbConnection.Query<T>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "DELETE FROM Customer"
                             + " WHERE Id = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }

        public void Update(T t)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "UPDATE Customer SET Name = @Name,"
                               + " Surname = @Surname";
                dbConnection.Open();
                dbConnection.Query(sQuery, t);
            }
        }
    }
}
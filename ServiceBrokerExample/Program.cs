using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ServiceBrokerExample.Model;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;

namespace ServiceBrokerExample
{
    public class Program
    {
        public static string _conn = "Server=localhost; Database=ServiceBrokerExample; User ID=sa; Password=Password1234;";
        public static Tester tester = new Tester();
        public static void Main(string[] args)
        {
            Task.Run(() => tester.TesterMethod(true));
            // The mapper object is used to map model properties that do not have a corresponding table column name.
            // In case all properties of your model have same name of table columns, you can avoid to use the mapper.
            var mapper = new ModelToTableMapper<Customer>();
            mapper.AddMapping(c => c.Surname, "Surname");
            mapper.AddMapping(c => c.Name, "Name");

            // Here - as second parameter - we pass table name: this is necessary only if the model name is 
            // different from table name (in our case we have Customer vs Customers). 
            // If needed, you can also specifiy schema name.
            using (var dep = new SqlTableDependency<Customer>(_conn, tableName: "Customer", mapper: mapper))
            {
                dep.OnChanged += Changed;
                dep.Start();

                Console.WriteLine("Press a key to exit");
                Console.Read();

                dep.Stop();
            }

        }
        public static void Changed(object sender, RecordChangedEventArgs<Customer> e)
        {
            var changedEntity = e.Entity;

            Console.WriteLine("DML operation: " + e.ChangeType);
            Console.WriteLine("ID: " + changedEntity.Id);
            Console.WriteLine("Name: " + changedEntity.Name);
            Console.WriteLine("Surname: " + changedEntity.Surname);
        }

        

    }
}

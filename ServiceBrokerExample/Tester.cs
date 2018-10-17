using System.Threading;
using ServiceBrokerExample.Model;

namespace ServiceBrokerExample
{
    public class Tester
    {
        private readonly MicroOrmHelper<Customer> _dbService;
        public Tester()
        {
            _dbService = new MicroOrmHelper<Customer>();
        }

        
        public void TesterMethod (bool val) {

            if (!val) return;

            while (true)
            {
                Thread.Sleep(1000);
                _dbService.Add(new Customer() { Name = "Bulk User Name", Surname= "Bulk User Surname" });
            }

        }

    }
}
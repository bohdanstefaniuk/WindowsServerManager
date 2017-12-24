using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepareTestData
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException();
            }

            var createParameterContains = args.Any(x => x.Contains("-c"));
            var deleteParameterContains = args.Any(y => y.Contains("-d"));

            if (createParameterContains && !deleteParameterContains)
            {
                var applicationPool = new ApplicationPool();

                Console.WriteLine("Create application pool");
                applicationPool.CreateApplicationPool();
                Console.WriteLine("Create application pool - DONE");

                Console.WriteLine("Create web site");
                applicationPool.CreateSite();
                Console.WriteLine("Create web - DONE");

                Console.WriteLine("Create application");
                applicationPool.CreateApplication();
                Console.WriteLine("Create application - DONE");


            }
            else if (!createParameterContains && deleteParameterContains)
            {
                var applicationPool = new ApplicationPool();
                applicationPool.DeleteAllTestData();
                Console.WriteLine("Deleting all test IIS Data - DONE");
            }
            else
            {
                throw new ArgumentException();
            }

            Console.ReadKey();
        }
    }
}

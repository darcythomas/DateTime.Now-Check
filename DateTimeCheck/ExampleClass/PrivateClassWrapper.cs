using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleClass
{
    public class PrivateClassWrapper
    {
        private class PrivateClassWithDateTime
        {
            private void PrivateMethod()
            {
                Console.WriteLine(DateTime.Now);
            }
        }
    }
}

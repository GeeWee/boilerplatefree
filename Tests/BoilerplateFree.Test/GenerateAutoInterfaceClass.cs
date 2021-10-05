using System;

namespace BoilerplateFree.Test
{
        [AutoGenerateInterface]
        public class GenerateAutoInterfaceClass
        {
            public void Foo()
            {
                Console.WriteLine("Foo");
            }

            public int Bar(int param1) => 1 + param1;
            
            public int SuperSecret(int param1) => 1 + param1;

            public int MyInt => 3;
            public int MyNextInt { get; set; }
        }
}
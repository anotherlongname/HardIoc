using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Handlers
{
    public delegate string DoThing();

    public class DoThingHandler
    {

        public string Handle()
            => "Hello World!";
    }
}

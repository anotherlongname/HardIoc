namespace ExampleAspNetCoreWebApplication.Handlers
{
    public delegate string DoThing();

    public class DoThingHandler
    {

        public string Handle()
            => "Hello World!";
    }
}

namespace RedRoverChallenge
{
    internal class Program
    {
        /*
         * 
         * Example: "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)"
         * 
         */
        static int Main(string[] args)
        {
            var sorted = args.Contains("--sorted");
            var input = string.Join(' ', args.Where(arg => arg != "--sorted"));

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.Error.WriteLine("No input provided.");
                return 1;
            }

            var output = Parser.Parse(input, sorted);
            Console.WriteLine(output);

            return 0;
        }
    }
}

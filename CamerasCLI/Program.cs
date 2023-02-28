namespace InfiNickyCodes
{
    using CommandLine;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the Command Line Options a user can use.
    /// Usage: --option <arg>
    /// </summary>
    public class Options
    {
        [Option("num")]
        public int ID { get; set; }

        [Option("name")]
        public string Name { get; set; }
    }

    public class Program
    {
        public static CSVDataHandler dataHandler;

        static void Main(string[] args)
        {
            string csv_file_path = "Data/cameras-defb.csv";
            dataHandler = new CSVDataHandler(csv_file_path);

            Parser.Default.ParseArguments<Options>(args).WithParsed(option =>
            {
                if (option.Name != null)
                {
                    string name = option.Name;
                    dataHandler.Search(name);
                }
                else if (option.ID != 0)
                {
                    int id = (int)option.ID;
                    dataHandler.Search(id);
                }
                else
                {
                    Console.WriteLine("Usage: dotnet run --name <string>\n");
                    Console.WriteLine("   or: dotnet run --num <int>\n");
                }
            });
        }
    }

}
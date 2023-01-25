using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Globalization;
using System.Text.Json;
using WorkerServiceDimo.Models;

namespace WorkerServiceDimo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly FileSystemWatcher _fileSystemWatcher;
        private readonly string _Path;

        public Worker(ILogger<Worker> logger, IOptions<ReportsPath> options )
        {
            _logger = logger;
            _fileSystemWatcher = new FileSystemWatcher(options.Value.Path);
            _Path = options.Value.Path;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _fileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
            _fileSystemWatcher.Created += OnFileCreated;
            _fileSystemWatcher.EnableRaisingEvents = true;

            Console.WriteLine("Press 'q' to quit.");
            while (Console.Read() != 'q') { }
        }

         async void OnFileCreated(object sender, FileSystemEventArgs e)
        {
           await SleepFor(10000);
            if (Path.GetExtension(e.FullPath) == ".csv" || Path.GetExtension(e.FullPath) == ".xlsx")
            {
                Console.WriteLine("File created: " + e.Name);
                //  var records = ReadFromCsv<SmartPayReport>(e.FullPath);
                var records = ReadFromExcel<SmartPayReport>(e.FullPath);
                CreateNewTextFile(_Path+"\\" +DateTime.Now.ToFileTime()+".txt", JsonSerializer.Serialize(records.FirstOrDefault()));
                Console.WriteLine();

            }
        }

        static List<T> ReadFromExcel<T>(string filePath) where T : class, new()
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rows = worksheet.Cells.Rows;
                var properties = typeof(T).GetProperties();

                var records = new List<T>();
                for (int i = 2; i <= rows; i++)
                {
                    var record = new T();
                    for (int j = 1; j <= properties.Length; j++)
                    {
                        var value = worksheet.Cells[i, j].Value;
                        properties[j - 1].SetValue(record, value);
                    }
                    records.Add(record);
                }
                return records;
            }
        }

        static void CreateNewTextFile(string filePath, string data)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine(data);
            }
        }
        static async Task SleepFor(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }


        //static List<T> ReadFromCsv<T>(string filePath) where T : class, new()
        //{
        //    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        //    {
        //       // HasHeaderRecord= true,
        //        BadDataFound = null,
        //       // CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("enUS")
        //    };

        //    using (var reader = new StreamReader(filePath))
        //    using (var csv = new CsvReader(reader, config))
        //    {
        //        var records = csv.GetRecords<T>().ToList();
        //        return records;
        //    }
        //}
    }
}
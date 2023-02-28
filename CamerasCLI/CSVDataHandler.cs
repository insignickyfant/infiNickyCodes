using System.Data;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;
using System.Globalization;
using static System.Reflection.Metadata.BlobBuilder;
using System.Formats.Asn1;
using System.Reflection.PortableExecutable;
using CsvHelper;

namespace InfiNickyCodes
{
    public class CSVDataHandler
    {
        string csv_file_path;
        int cameraID;

        /// <summary>
        /// List of all Cameras found in database
        /// </summary>
        public static List<Camera> Cameras { get; private set; }

        public CSVDataHandler(string PATH)
        {
            csv_file_path = PATH;
            Cameras = new List<Camera>();
            //CreateCamerasFromCSV();
            //Cameras = CreateCamerasFromCSV();
        }

        /// <summary>
        /// Tries to fetch camera data from CSV file and creates camera objects with parsed properties.
        /// </summary>
        public List<Camera> CreateCamerasFromCSV() // string csv_file_path
        {
            try
            {
                using (TextFieldParser fieldParser = new(csv_file_path))
                {
                    fieldParser.SetDelimiters(new string[] { ";" });
                    // Comment out the Errors in CSV file
                    fieldParser.CommentTokens = new string[] { "ERROR" };
                
                    // first line contains the column names (Camera, Latitude, Longitude)
                    string[] columnFields = fieldParser.ReadFields();

                    while (!fieldParser.EndOfData)
                    {
                        string[] fieldData = fieldParser.ReadFields();
                        
                        if (fieldData != null)
                        {
                            // set empty values to null
                            for (int i = 0; i < fieldData.Length; i++)
                            {
                                if (fieldData[i] == "")
                                {
                                    fieldData[i] = null;
                                }
                            }

                            string cameraName = fieldData[0];
                            string latitude = fieldData[1];
                            string longitude = fieldData[2];

                            // match a pattern of 3 digits to get the camera number from cameraID
                            string pattern = @"\d{3}";
                            Regex rgx = new Regex(pattern);
                            Match match = rgx.Match(cameraName);
                            if (match.Success)
                            {
                                cameraID = Int32.Parse(match.Value);
                            }
                            Camera camera = new Camera(cameraID, cameraName, latitude, longitude);
                            Cameras.Add(camera);
                        }
                    }
                    return Cameras;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Searches Camera names for containing input string, while ignoring case.
        /// </summary>
        /// <param name="s">string to search for in Camera names</param>
        public void Search(string s)
        {
            List<Camera> result = Cameras.FindAll(
                delegate (Camera cam)
                {
                    return cam.Name.Contains(s, System.StringComparison.CurrentCultureIgnoreCase);
                }
            );
            if (result.Count != 0)
            {
                foreach (var camera in result)
                {
                    Console.WriteLine(camera.Number + " | " + camera.Name + " | " + 
                                      camera.Longitude + " | " + camera.Latitude);
                }
            }
            else
            {
                Console.WriteLine("Not found: {0}", s);
            }
        }

        public void Search(int n)
        {
            Camera result = Cameras.Find(
                delegate (Camera cam)
                {
                    return cam.Number == n;
                }
            );
            if (result != null)
            {
                Console.WriteLine(result.Number + " | " + result.Name + " | " +
                                  result.Longitude + " | " + result.Latitude);
            }
            else
            {
                Console.WriteLine("Not found: {0}", n);
            }
        }
    }
}

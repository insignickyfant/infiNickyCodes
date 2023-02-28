using System.Globalization;

namespace InfiNickyCodes
{
    public class Camera
    {
        public int ID { get; set; }
        
        public int Number { get; set; }

        public string Name { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        /// <summary>
        /// Constructs camera object and sets its properties.
        /// </summary>
        /// <param name="data">row of fields from csv file</param>
        public Camera(int n, string name, string latitude, string longitude) 
        {
            Number = n;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;

            // optional: parse coordinate strings into doubles
            // ParseCoordinates(latitude, longitude);
        }

        /// <summary>
        /// Setup to create doubles from strings of coordinates, if needed.
        /// </summary>
        /// <param name="latitude">string</param>
        /// <param name="longitude">string</param>
        private void ParseCoordinates(string latitude, string longitude)
        {
            double coordinate;
            if (Double.TryParse(latitude, NumberStyles.Number, CultureInfo.InvariantCulture, out coordinate))
            {
                double parsed_latitude = coordinate;
            }
            else
            {
                Console.WriteLine("Unable to parse '{0}'.", latitude);
            }
            if (Double.TryParse(longitude, NumberStyles.Number, CultureInfo.InvariantCulture, out coordinate))
            {
                double parsed_longitude = coordinate;
            }
            else
            {
                Console.WriteLine("Unable to parse '{0}'.", longitude);
            }
        }
    }
}

namespace CamerasWebApp.Models
{
    public class Camera
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string? Name { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

        public Camera()
        {

        }
    }
}

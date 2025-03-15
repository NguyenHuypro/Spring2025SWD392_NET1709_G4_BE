namespace CarRescueSystem.Common.DTO.Vehicle
{
    public class CreateVehicleDTO
    {
        public Guid CustomerId { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public string brand { get; set; }
        public int numberOfSeats { get; set; }
        public string licensePlate { get; set; }
    }
} 
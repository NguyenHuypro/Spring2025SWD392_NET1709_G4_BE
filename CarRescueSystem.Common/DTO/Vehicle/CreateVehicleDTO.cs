namespace CarRescueSystem.Common.DTO.Vehicle
{
    public class CreateVehicleDTO
    {
        public Guid CustomerId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleBrand { get; set; }
        public int NumberOfSeats { get; set; }
    }
} 
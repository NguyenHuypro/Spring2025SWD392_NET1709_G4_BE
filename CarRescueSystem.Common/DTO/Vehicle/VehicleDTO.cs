namespace CarRescueSystem.Common.DTO.Vehicle
{
    public class VehicleDTO
    {
        public Guid VehicleId { get; set; }
        public Guid CustomerId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleBrand { get; set; }
        public int NumberOfSeats { get; set; }
        public string CustomerName { get; set; }
    }
} 
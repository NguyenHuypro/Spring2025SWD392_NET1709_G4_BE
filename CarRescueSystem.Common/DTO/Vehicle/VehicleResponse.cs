namespace CarRescueSystem.Common.DTO.Vehicle
{
    public class VehicleResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public VehicleDTO Data { get; set; }
    }
} 
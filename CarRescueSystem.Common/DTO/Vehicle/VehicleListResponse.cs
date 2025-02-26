namespace CarRescueSystem.Common.DTO.Vehicle
{
    public class VehicleListResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<VehicleDTO> Data { get; set; }
    }
} 
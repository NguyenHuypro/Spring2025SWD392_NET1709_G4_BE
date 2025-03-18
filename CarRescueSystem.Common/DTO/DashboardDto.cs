using System;

namespace CarRescueSystem.Common.DTO
{
    public class DashboardDto
    {
        public int CustomerCount { get; set; }
        public int StaffCount { get; set; }
        public int ReceptionistCount { get; set; }
        public int PackageCount { get; set; }
        public int ServiceCount { get; set; }
        public int RegisteredCarCount { get; set; }
        public int BookingCount { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public List<MonthlyRevenueDto> MonthlyRevenues { get; set; }
    }

    public class MonthlyRevenueDto
        {
        public int Month { get; set; }
        public decimal Revenue { get; set; }
        }
} 
public class DetailBookingDTO
{
    public Guid id { get; set; }
    public string name { get; set; } = "Không xác định";
    public string phone { get; set; } = "Không xác định";
    public string description { get; set; }
    public string licensePlate { get; set; } = "Không xác định";
    public string location { get; set; } = "Không xác định";
    public string evidence { get; set; } = "Không xác định";
    public string status { get; set; } = "Không xác định";
    public decimal totalPrice { get; set; } = 0;

    public DateTime arrivalDate { get; set; }
    public DateTime completedDate { get; set; }

    public List<ServiceDetailInBookingDTO> services { get; set; } = new List<ServiceDetailInBookingDTO>();

    // 👇 Gộp danh sách nhân viên vào đây
    public StaffDTO staff1 { get; set; }

    public StaffDTO staff2 { get; set; }
}

public class StaffDTO
{
    public Guid? id { get; set; }
    public string fullName { get; set; } = "Không xác định";
    public string phone { get; set; } = "Không xác định";
    public bool? confirmStaff { get; set; }
}
public class ServiceDetailInBookingDTO
{
    public Guid id { get; set; }
    public string name { get; set; }
    public decimal? price { get; set; }
}

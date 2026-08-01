namespace Domain.Enums;

public enum PaymentStatus
{
    Pending = 1,     // Chờ thanh toán
    Paid = 2,        // Đã thanh toán
    Failed = 3,      // Thanh toán thất bại
    Cancelled = 4,   // Đã hủy
    Refunded = 5     // Đã hoàn tiền
}
namespace FoodOrderApi.Core.Enums
{
    public enum OrderStatus
    {
        Pending = 1,    // Sipariş Alındı
        Preparing = 2,  // Hazırlanıyor
        OnTheWay = 3,   // Yolda
        Delivered = 4,  // Teslim Edildi
        Cancelled = 5   // İptal Edildi
    }
}

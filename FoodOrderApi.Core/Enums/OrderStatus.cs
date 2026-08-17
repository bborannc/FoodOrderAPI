namespace FoodOrderApi.Core.Enums
{
    public enum OrderStatus
    {
        Pending = 1,     // 1: Sipariş Alındı / Onay Bekliyor
        Preparing = 2,   // 2: Hazırlanıyor
        InTransit = 3,   // 3: Kuryede / Yolda
        Delivered = 4,   // 4: Teslim Edildi
        Cancelled = 5    // 5: İptal Edildi
    }
}

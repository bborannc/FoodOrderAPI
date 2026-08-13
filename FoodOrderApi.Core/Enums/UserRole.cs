namespace FoodOrderApi.Core.Enums
{
    public enum UserRole
    {
        Admin = 1,           // Sistem Yöneticisi (Tüm yetkilere sahip)
        RestaurantOwner = 2, // Restoran Sahibi (Kendi menüsünü ve gelen siparişleri yönetir)
        Customer = 3         // Müşteri (Sipariş verir, menüleri inceler)
    }
}


namespace FoodOrderApi.Core.Constants
{
    public static class Permissions
    {
        public static class Orders
        {
            public const string View = "Permissions.Orders.View";
            public const string Create = "Permissions.Orders.Create";
            public const string UpdateStatus = "Permissions.Orders.UpdateStatus";
            public const string Cancel = "Permissions.Orders.Cancel";
        }

        public static class MenuItems
        {
            public const string View = "Permissions.MenuItems.View";
            public const string Create = "Permissions.MenuItems.Create";
            public const string Edit = "Permissions.MenuItems.Edit";
            public const string Delete = "Permissions.MenuItems.Delete";
        }

        public static class Restaurants
        {
            public const string Create = "Permissions.Restaurants.Create";
            public const string Edit = "Permissions.Restaurants.Edit";
            public const string Delete = "Permissions.Restaurants.Delete";
        }
    }
}

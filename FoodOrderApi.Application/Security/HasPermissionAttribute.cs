namespace FoodOrderApi.Application.Security
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class HasPermissionAttribute : Attribute
    {
        public string Permission { get; }

        public HasPermissionAttribute(string permission)
        {
            Permission = permission;
        }
    }
}

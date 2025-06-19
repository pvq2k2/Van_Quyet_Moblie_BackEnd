using System.Reflection;

namespace Domain.Permissions
{
    public static class AppPermissions
    {
        public static class User
        {
            public const string Get = "User.Get";
            public const string Create = "User.Create";
            public const string Update = "User.Update";
            public const string Delete = "User.Delete";
        }

        public static IEnumerable<string> All =>
            typeof(AppPermissions)
            .GetNestedTypes()
            .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Select(f => f.GetValue(null)?.ToString())
            .Where(p => !string.IsNullOrWhiteSpace(p))!;
    }

}

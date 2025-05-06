using Library_API_2._0.Domain.Policies.UpdateRolePolicy;

namespace Library_API_2._0.Domain.Factories
{
    public class RoleUpdatePolicyFactory
    {
        public static IRoleUpdatePolicy CreatePolicy(string currentRole)
        {
            return currentRole switch
            {
                "Admin" => new AdminRoleUpdatePolicy(),
                "User" => new UserRoleUpdatePolicy(),
                _ => throw new ArgumentException("Unsupported role for update policy")
            };
        }
    }
}
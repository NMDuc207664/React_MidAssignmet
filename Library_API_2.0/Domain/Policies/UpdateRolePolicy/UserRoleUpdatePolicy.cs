namespace Library_API_2._0.Domain.Policies.UpdateRolePolicy
{
    public class UserRoleUpdatePolicy : IRoleUpdatePolicy
    {
        public bool CanReadOtherUsers(string targetRole)
        {
            return false;
        }

        public bool CanUpdateToRole(string targetRole)
        {
            return false;
        }
    }
}
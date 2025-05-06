namespace Library_API_2._0.Domain.Policies.UpdateRolePolicy
{
    public class AdminRoleUpdatePolicy : IRoleUpdatePolicy
    {
        public bool CanReadOtherUsers(string targetRole)
        {
            return true;
        }

        public bool CanUpdateToRole(string targetRole)
        {
            return true; // Admin có thể cập nhật lên bất kỳ role nào
        }
    }
}
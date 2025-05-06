namespace Library_API_2._0.Domain.Policies.UpdateRolePolicy
{
    public interface IRoleUpdatePolicy
    {
        bool CanUpdateToRole(string targetRole);
        bool CanReadOtherUsers(string targetRole);
    }
}
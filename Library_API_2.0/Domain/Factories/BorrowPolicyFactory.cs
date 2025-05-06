using Library_API_2._0.Domain.Policies.BorrowRequestPolicy;

namespace Library_API_2._0.Domain.Factories
{
    public class BorrowPolicyFactory
    {
        public static IBorrowPolicy CreatePolicy(string role)
        {
            return role switch
            {
                "User" => new UserBorrowPolicy(),
                "Admin" => new AdminBorrowPolicy(),
                _ => throw new ArgumentException("Invalid role")
            };
        }
    }
}
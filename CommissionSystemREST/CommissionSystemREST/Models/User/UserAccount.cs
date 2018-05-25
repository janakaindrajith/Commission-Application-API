namespace MRPSystem.Models.User
{
    public class UserAccount
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string UserDisplayName { get; set; }
        public string Company { get; set; }

        public string BranchCode { get; set; }

        public string Epf { get; set; }


        public int UserRoleCode { get; set; }

    }
}
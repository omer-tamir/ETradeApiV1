using System;
using System.Collections.Generic;
using System.Text;

namespace ETradeApiV1.Client.Dtos
{

    public class AccountsListDto
    {
        public AccountListResponse AccountListResponse { get; set; }
    }


    public class Account
    {
        public string AccountId { get; set; }
        public string AccountIdKey { get; set; }
        public string AccountMode { get; set; }
        public string AccountDesc { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string InstitutionType { get; set; }
        public string AccountStatus { get; set; }
        public int ClosedDate { get; set; }
    }

    public class Accounts
    {
        public List<Account> Account{ get; set; }
    }

    public class AccountListResponse
    {
        public Accounts Accounts { get; set; }
    }


}

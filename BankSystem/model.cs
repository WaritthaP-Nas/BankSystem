using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.IBAN.Model;

namespace BankSystem
{   

    public class Account_IBAN
    {

        public string Code { get; set; }
        public int account_id { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public string ssn { get; set; }
        public decimal amount { get; set; }
        public string Username { get; set; }
        public string PIN { get; set; }
    }

    public class ListAccount
    {

        public string Code { get; set; }
    }

}

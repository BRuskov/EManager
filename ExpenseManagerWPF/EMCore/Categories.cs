using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManagerWPF.EMCore
{
    public enum ExpenseCategory : byte
    {
        Loan = 0,
        HealthCare,
        Food,
        Entertainment,
        Education,
        Automobile,
        Personal,
        Marketing,
        Bank,
        Utilities,
        Family,
        HomeOffice,
        Other
    }

    public enum IncomeCategory : byte
    {
        Salary = 0,
        Wage,
        Commision,
        Bonus,
        Stocks,
        Shares,
        Royalties,
        Bussiness,
        Relatives,
        CapitalGains,
        Rental,
        MultiLevelMarketing,
        Other
    }
}

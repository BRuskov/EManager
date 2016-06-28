using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManagerWPF.EMCore
{
    public interface IReturnable
    {
        DateTime GetDate();
        double GetAmount();
        string GetDescription();
        int GetCategory();
        string GetCategoryName();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExpenseManagerWPF.EMCore
{
    public class MoneyFlow : IReturnable
    {
        public double Amount { set; get; }

        public MoneyFlow() : this(0) { }
        public MoneyFlow(double Amount)
        {
            this.Amount = Amount;
        }

        public virtual DateTime GetDate() { return new DateTime(1, 1, 1); }

        public virtual string GetDescription() { return null; }

        public virtual int GetCategory() { return 0; }

        public virtual double GetAmount() { return this.Amount; }

        public virtual string GetCategoryName() { return "BaseClas:None";}
    }
}

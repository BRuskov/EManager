﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExpenseManagerWPF.EMCore
{
    public class Expense : MoneyFlow, IReturnable
    {
        public string Description { set; get; }
        public EMCore.ExpenseCategory MFCategory { set; get; }
        public DateTime CustomDate { set; get; }

        public Expense() : this(0, "", 0, new DateTime(1111, 1, 1)) { }
        public Expense(double Amount, string Description, ExpenseCategory MFCategory, DateTime CustomDate) : base(Amount)
        {
            this.Description = Description;
            this.MFCategory = MFCategory;
            this.CustomDate = CustomDate;
        }

        public override DateTime GetDate()
        {
            return this.CustomDate;
        }
        public override string GetDescription()
        {
            return this.Description;
        }
        public override int GetCategory()
        {
            return (int)MFCategory;
        }
        public override double GetAmount()
        {
            return this.Amount;
        }
        public override string GetCategoryName()
        {
            return Enum.GetName(typeof(ExpenseCategory), (int)this.MFCategory);
        }


    }
}

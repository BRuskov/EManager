using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExpenseManagerWPF.EMCore
{
    public class PersonalFinance
    {
        public string PersonalName { set; get; }
        public ObservableCollection<Income> PIncome { set; get; }
        public ObservableCollection<Expense> PExpense { set; get; }
        public ObservableCollection<MoneyFlow> PFlow { set; get; }

        public PersonalFinance()
        {
            this.PersonalName = "DefNm";
            this.PExpense = new ObservableCollection<Expense>();
            this.PIncome = new ObservableCollection<Income>();
            this.PFlow = new ObservableCollection<MoneyFlow>();
        }

        public PersonalFinance(string PersonalName) : this()
        {
            this.PersonalName = PersonalName;
        }


        public void AddTransaction(MoneyFlow transaction)
        {
            PFlow.Add(transaction);
            if (transaction.GetType() == new Expense().GetType())
            {
                PExpense.Add((Expense)transaction);
            }

            else
            {
                PIncome.Add((Income)transaction);
            }
        }

        public void SaveXml(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(PersonalFinance));
                XML.Serialize(stream, this);
            }
        }

        public static PersonalFinance LoadXml(String fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var XML = new XmlSerializer(typeof(PersonalFinance));
                return (PersonalFinance)XML.Deserialize(stream);
            }
        }
    }
}

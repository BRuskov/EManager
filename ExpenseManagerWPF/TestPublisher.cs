using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManagerWPF
{
    class TestPublisher
    {
        public String UserName { set; get; }
        public double UserAmount { set; get; }
        public UserCategory UC { set; get; }

        public TestPublisher() {
            this.UserAmount = 433.32f;
            this.UserName = "Jacob";
            this.UC = UserCategory.Expert;
        }
        public TestPublisher(string UserName, double UserAmount) {
            this.UserName = UserName;
            this.UserAmount = UserAmount;
            this.UC = (UserCategory) 3;
        }

    }

    public enum UserCategory : byte
    {
        Novice,
        Apprentice,
        Intermediate,
        Master,
        Expert
    }
}

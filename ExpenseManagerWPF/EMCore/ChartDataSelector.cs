using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseManagerWPF.EMCore
{
    public static class ChartDataSelector
    {
        public static KeyValuePair<string, double>[] TranscationToCategoryFormat<T>(ObservableCollection<T> transCol)
        {
            KeyValuePair<string, double>[] chartDataContext =  new KeyValuePair<string, double>[12];
            foreach (var transaction in transCol)
            {
                var trans = transaction as MoneyFlow;
                var key = trans.GetCategoryName();
                var i = isInKVP(chartDataContext, key);
                if (i >= 0)
                    chartDataContext[i] = new KeyValuePair<string, double>(trans.GetCategoryName(), (double)(chartDataContext[i].Value + trans.Amount));
            }
            return chartDataContext;

        }
        private static int isInKVP(KeyValuePair<string, double>[] chartDataContext, string key)
        {
            for (int i = 0; i < chartDataContext.Length; i++)
                if (chartDataContext[i].Key == key)
                    return i;
            return -1;
        }

    }
}

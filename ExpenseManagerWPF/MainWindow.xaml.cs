using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExpenseManagerWPF.EMCore;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Windows.Controls;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.DataVisualization.Charting;

namespace ExpenseManagerWPF
{
    public partial class MainWindow : Window
    {
        public PersonalFinance pFinance;
        private double _totIncome, _totExpense;

        public MainWindow()
        {
            InitializeComponent();
            // this.Window_Loaded();
        }

        public MainWindow(string path, bool newFile)
        {
            InitializeComponent();

            ////Log MB
            //path = System.IO.Path.GetFileName(path);
            //MessageBox.Show(path);

            if (newFile)
            {
                this.pFinance = new PersonalFinance(path);
                this.pFinance.SaveXml(path + ".xml");

                ////Log MB
                //MessageBox.Show("FileCreated and object initialized.");
            }

            else
            {
                this.pFinance = new PersonalFinance();
                this.pFinance = PersonalFinance.LoadXml(System.IO.Path.GetFileName(path));

                ////Log MB
                //MessageBox.Show("FileLoaded and object initialized.");
            }

            this.Window_Loaded();
            this.LoadProfileData();

            ////Log MB
            //MessageBox.Show(pFinance.PersonalName);
        }

        private void Window_Loaded()
        {
            cbExpenseC.ItemsSource = Enum.GetValues(typeof(ExpenseCategory));
            cbExpenseC.SelectedIndex = 0;
            cbIncomeC.ItemsSource = Enum.GetValues(typeof(IncomeCategory));
            cbIncomeC.SelectedIndex = 0;

            //DataGrid Fill
            userExpensePublisher.ItemsSource = pFinance.PExpense;
            userIncomePublisher.ItemsSource = pFinance.PIncome;
            tbWelcome.Text = "Welcome, " + this.pFinance.PersonalName;

            //Fields reset
            this.tbDescription.Text = string.Empty;
            this.tbTransactionAmount.Text = string.Empty;
            this.rbExpense.IsChecked = true;
            this.rbCurrent.IsChecked = true ;
            //To ?
        }

        private void LoadProfileData()
        {
            //User mFlow totals initialize
            this._totIncome = TotalFlowAmount(pFinance.PIncome);
            this._totExpense = TotalFlowAmount(pFinance.PExpense);
            UpdateTotals(true);
            UpdateTotals(false);

            LoadColumnChartData();
        }

        //SaveButton Click_Event
        public void SaveButtonClick_KeyPress(object sender, RoutedEventArgs e)
        {

            try
            {
                var amount = Double.Parse(tbTransactionAmount.Text, CultureInfo.InvariantCulture);

                var description = tbDescription.Text;
                var transactionType = (rbExpense.IsChecked == true) ? true : false;

                DateTime defDate;
                if (rbCurrent.IsChecked == true)
                {
                    defDate = DateTime.Now;
                }

                else {
                    defDate = new DateTime(datePicker.SelectedDate.Value.Year, datePicker.SelectedDate.Value.Month, datePicker.SelectedDate.Value.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }

                if (transactionType)
                {
                    var category = cbExpenseC.SelectedIndex;
                    pFinance.AddTransaction(new Expense(amount, description, (ExpenseCategory)category, defDate));
                    UpdateTotals(true);
                }
                else
                {
                    var category = cbIncomeC.SelectedIndex;
                    pFinance.AddTransaction(new Income(Convert.ToDouble(amount), description, (IncomeCategory)category, defDate));
                    UpdateTotals(false);
                }
            }
            catch (FormatException ex)
            {

                MessageBox.Show("Wrong Amount input! " + ex.Message);
            }


            pFinance.SaveXml(pFinance.PersonalName + ".xml");
            LoadColumnChartData();

            ////Log MB
            // MessageBox.Show("Transaction added and saved!");
        }

        //ResetButton Click_Event
        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {
            this.tbDescription.Text = string.Empty;
            this.tbTransactionAmount.Text = string.Empty;
            this.rbExpense.IsChecked = true;
            this.rbCurrent.IsChecked = true;
        }

        private void LoadColumnChartData()
        {
            string[] expenseCat = new string[] {
                "Loan", "HealthCare", "Food", "Entertainemt", "Education", "Automobile", "Personal", "Marketing",
                "Bank", "Utilities", "Family", "HomeOffice", "Other"
            };

            string[] incomeCat = new string[] {
                "Salary", "Wage", "Comission", "Bonus", "Stocks", "Shares", "Royalties", "Bussiness",
                "Relatives", "CapitalGains", "Rental", "MultiLevelMarketing", "Other"
            };

            KeyValuePair<string, int>[] myKVPI = new KeyValuePair<string, int>[13];
            KeyValuePair<string, int>[] myKVPE = new KeyValuePair<string, int>[13];

            for (int j = 0; j < expenseCat.Length; j++)
            {
                myKVPE[j] = new KeyValuePair<string, int>(expenseCat[j], CheckCategory(expenseCat[j], pFinance.PExpense));
                myKVPI[j] = new KeyValuePair<string, int>(incomeCat[j], CheckCategory(incomeCat[j], pFinance.PIncome));
            }
            ((ColumnSeries)ExpenseListChart.Series[0]).ItemsSource = myKVPE;
            ((ColumnSeries)IncomeListChart.Series[0]).ItemsSource = myKVPI;


        }

        private int CheckCategory<T>(string category, ObservableCollection<T> transactions)
        {
            Type myType = transactions.GetType().GetGenericArguments().Single();
            IEnumerable<T> searchQuery;
            if (myType == typeof(Expense))
            {
                searchQuery = from transaction in transactions
                                  where Enum.GetName(typeof(ExpenseCategory), (transaction as Expense).MFCategory).Equals(category)
                                  select transaction;
            }
            else {
                 searchQuery = from transaction in transactions
                                  where Enum.GetName(typeof(IncomeCategory), (transaction as Income).MFCategory).Equals(category)
                                  select transaction;
            }

            double i = 0;
            foreach (var trans in searchQuery)
            {
                i += (trans as MoneyFlow).Amount;
            }
            return (int)i;
        }

        private void UpdateTotals(bool isExpense)
        {
            //IncomeTotalUpdate
            if (isExpense)
            {
                this._totExpense = TotalFlowAmount(pFinance.PExpense);
                tbTotalExpense.Text = "Total: " + this._totExpense.ToString() + "$";
            }

            //ExpenseTotalUpdate
            else
            {
                this._totIncome = TotalFlowAmount(pFinance.PIncome);
                tbTotalIncome.Text = "Total: " + this._totIncome.ToString() + "$";
            }

            //BalanceUpdate, positive
            if (this._totIncome > this._totExpense)
            {
                tbTotalBalance.Text = "Total Balance: " + (this._totIncome - this._totExpense).ToString() + "$";
                tbTotalBalance.Foreground = Brushes.Green;
            }

            //BalanceUpdate, neutral
            else if (this._totExpense == this._totIncome)
            {
                tbTotalBalance.Text = "Total Balance: 0 $";
                tbTotalBalance.Foreground = Brushes.Black;
            }

            //BalanceUpdate, negative
            else
            {
                tbTotalBalance.Text = "Total Balance: " + (this._totExpense - this._totIncome).ToString() + "$";
                tbTotalBalance.Foreground = Brushes.Red;
            }
        }

        //AmountField paste regulation
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                    e.CancelCommand();
            }
            else {
                e.CancelCommand();
            }
        }

        //AmountFieldRules
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.]+");
            return !regex.IsMatch(text);
        }

        //AmountFieldRules
        private void tbTransactionAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        //TotalAmountCalculation
        public double TotalFlowAmount<T>(ObservableCollection<T> transCol)
        {
            var totalFlow = 0d;
            foreach (var transaction in transCol)
            {
                MoneyFlow mf = transaction as MoneyFlow;
                totalFlow += mf.Amount;
            }
            return totalFlow;
        }
    }
}


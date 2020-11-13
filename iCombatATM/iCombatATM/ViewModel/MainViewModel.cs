using Acr.UserDialogs;
using iCombatATM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using iCombatATM.Services;

namespace iCombatATM.ViewModel
{
    public class MainViewModel:BaseViewModel
    {

        #region Properties
        private ObservableCollection<Bills> bills;
        private int amountWitdraw;
        public int AmountWitdraw
        {
            get => amountWitdraw;
            set => SetProperty(ref amountWitdraw, value);
        }

        public ObservableCollection<Bills> Bills
        {
                get => bills;
                set => SetProperty(ref bills, value);
        }

        private int totalAmount;
        public int TotalAmount
        {
            get => totalAmount;
            set => SetProperty(ref totalAmount, value);
        }


    #endregion

        #region Command definitions
        public Command OpenBrowser { get; }
        public Command Restock { get; }
        public Command Withdraw { get; }


    #endregion

        #region Class Constructor
        public MainViewModel()
        {
            OpenBrowser = new Command(Navigating);
            Withdraw = new Command(OnWithdraw);
            Restock = new Command(RestockBills);
           
                InitialStockBills();

            
           
        }


        #endregion

        #region Methods Implementation

        public  void InitialStockBills()
        {
           
            this.bills = new ObservableCollection<Bills>();
            Bills bill = new Bills();
            bill.BillValue = 100;
            bill.BillAmounts = 10;
            this.bills.Add(bill);

            bill = new Bills();
            bill.BillValue = 50;
            bill.BillAmounts = 10;
            this.bills.Add(bill);

            bill = new Bills();
            bill.BillValue = 20;
            bill.BillAmounts = 10;
            this.bills.Add(bill);

            bill = new Bills();
            bill.BillValue = 10;
            bill.BillAmounts = 10;
            bills.Add(bill);

            bill = new Bills();
            bill.BillValue = 5;
            bill.BillAmounts = 10;
            bills.Add(bill);

            bill = new Bills();
            bill.BillValue = 1;
            bill.BillAmounts = 10;
            bills.Add(bill);
            Bills = bills;
            totalAmount = bills.Sum(x => x.BillAmounts * x.BillValue);
        }


        public void RestockBills()
        {

            this.Bills = new ObservableCollection<Bills>();
            Bills bill = new Bills();
            bill.BillValue = 100;
            bill.BillAmounts = 10;
            this.Bills.Add(bill);

            bill = new Bills();
            bill.BillValue = 50;
            bill.BillAmounts = 10;
            this.Bills.Add(bill);

            bill = new Bills();
            bill.BillValue = 20;
            bill.BillAmounts = 10;
            this.Bills.Add(bill);

            bill = new Bills();
            bill.BillValue = 10;
            bill.BillAmounts = 10;
            Bills.Add(bill);

            bill = new Bills();
            bill.BillValue = 5;
            bill.BillAmounts = 10;
            Bills.Add(bill);

            bill = new Bills();
            bill.BillValue = 1;
            bill.BillAmounts = 10;
            Bills.Add(bill);
           
            TotalAmount = Bills.Sum(x => x.BillAmounts * x.BillValue);
        }


        public  void OnWithdraw()
        {

            try
            {
                int value1 = Convert.ToInt32(amountWitdraw);//trigger an error if the user type a letter or special character.

                if (amountWitdraw > bills.Sum(x => x.BillAmounts * x.BillValue))
                {
                    UserDialogs.Instance.Alert("The amount requested is greather than the total of money we have available.", "Failure: insufficient funds", "OK");
                }
                else
                {
                   if(amountWitdraw>=1)
                   {
                        FactoryMethods Calculation = new FactoryMethods(ref bills, amountWitdraw);
                        if (Calculation.Success)
                        {
                            UserDialogs.Instance.Alert("The amount requested has been dispensed:$" + amountWitdraw.ToString(), "Success", "OK");
                            var results = bills;
                            Bills = new ObservableCollection<Bills>();
                            Bills = results;
                            TotalAmount = results.Sum(x => x.BillAmounts * x.BillValue);
                            AmountWitdraw = 0;
                        }
                        else
                        {
                            UserDialogs.Instance.Alert("The amount requested is Lower than than our lowest bill.", "Failure", "OK");
                        }
                    }
                   else
                    {
                        UserDialogs.Instance.Alert("Please type an Integer value  greather than 0, not cents or float values.", "Error", "OK");
                    }
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Please type an Integer value greather than 0, not cents or float values.", "Error", "OK");
            }


        }
        public async void Navigating(object obj)
        {
            try
            {
                await Browser.OpenAsync("https://www.icombat.com/");
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }
        #endregion

    }
}

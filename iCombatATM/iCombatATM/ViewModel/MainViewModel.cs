using Acr.UserDialogs;
using iCombatATM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace iCombatATM.ViewModel
{
    public class MainViewModel:BaseViewModel
    {

    #region Properties
        private ObservableCollection<Bill> _bills;
        public ObservableCollection<Bill> Bills
        {
            get => _bills;
            set => SetProperty(ref _bills, value);
           
        }

        private Bill _localBill;
        public Bill LocalBill
        {
            get => _localBill;
            set
            {
                SetProperty(ref _localBill, value);
                _bills.Add(value);

            }
        }

        private int? _amountWitdraw;
        public int? AmountWitdraw
        {
            get => _amountWitdraw;
            set  
            {
                if (value >= 1 || value is null)
                {
                    SetProperty(ref _amountWitdraw, value);
                }
                else
                {
                    UserDialogs.Instance.Alert("Please type an Integer value  greather than 0, not cents or float values.", "Error", "OK");
                }
            }
        }

        private int _totalAmount;
        public int TotalAmount
        {
            get => _totalAmount;
            set => SetProperty(ref _totalAmount, value);
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
            Restock = new Command(InitialStockBills);
            Bills = new ObservableCollection<Bill>();
            InitialStockBills();
        }
    #endregion

    #region Methods Implementation

        private  void InitialStockBills()
        {

            Bills.Clear();
            int[] BillsValues = { 100, 50, 20, 10, 5, 1 };
            CreateBillStock(BillsValues);
            TotalAmount = Bills.Sum(x => x.BillAmounts * x.BillValue);
            AmountWitdraw = null;
        }


        private void CreateBillStock(int[] Values)
        {
            foreach(int value in Values)
            {
                LocalBill = new Bill();
                LocalBill.BillValue = value;
                LocalBill.BillAmounts = 10;
            }
        }

        public  void OnWithdraw()
        {
            try
            {
                int value1 = Convert.ToInt32(_amountWitdraw);//trigger an error if the user type a letter or special character.
                if (_amountWitdraw > _bills.Sum(x => x.BillAmounts * x.BillValue))
                {
                    UserDialogs.Instance.Alert("The amount requested is greather than the total of money we have available.", "Failure: insufficient funds", "OK");
                }
                else
                {
                    if (WithdrawCalc(_amountWitdraw))
                    {
                        UserDialogs.Instance.Alert("The amount requested has been dispensed:$" + _amountWitdraw.ToString(), "Success", "OK");
                        TotalAmount = Bills.Sum(x => x.BillAmounts * x.BillValue);
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("The amount requested is Lower than than our lowest bill.", "Failure", "OK");
                    }
                 }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Please type an Integer value greather than 0, not cents or float values.", "Error", "OK");
            }
            finally
            {
                AmountWitdraw = null;
            }
        }

        public bool WithdrawCalc( int? total)
        {
            var acum = 0;
            List<Bill> CalcValues = new List<Bill>();
            foreach(var element in _bills)
                {
                    Bill NewValue = new Bill();
                    NewValue.BillAmounts = element.BillAmounts;
                    NewValue.BillValue = element.BillValue;
                    CalcValues.Add(NewValue);
                }

            bool Success = false;
            var diff =(int) total;
            var elements = 0;
            for (int i = 0; i < _bills.Count; i++)
            {
                if (acum == total)
                {
                   Success = true;
                    break;
                }

                if (CalcValues[i].BillAmounts > 0)
                {
                    if ((diff >= CalcValues[i].BillAmounts * CalcValues[i].BillValue))
                    {
                        acum = acum + (CalcValues[i].BillAmounts * CalcValues[i].BillValue);
                        CalcValues[i].BillAmounts = 0;
                        diff = total.Value - acum;
                    }
                    else
                    {
                        elements = diff / CalcValues[i].BillValue;
                        CalcValues[i].BillAmounts -= elements;
                        acum = acum + (elements * CalcValues[i].BillValue);
                        diff = total.Value - acum;
                    }
                }
            }
            if (acum == total)
            {
               Success = true;
               Bills.Clear();
               foreach(Bill element in CalcValues)
               {
                    Bills.Add(element);
               }
            }
           

            return Success;

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

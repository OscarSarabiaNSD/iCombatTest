using iCombatATM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace iCombatATM.Services
{
    public class FactoryMethods
    {
        public string Result { get; set; }
        public bool Success { get; set; }

        public FactoryMethods(ref ObservableCollection<Bills> bills, int total)
        {
            var acum = 0;
            Result = "";
            this.Success = false;
            var diff = total;
            var elements = 0;
            for(int i=0; i<bills.Count; i++)
            {
                if (acum == total)
                {
                  
                    this.Success = true;
                    break;

                }
                    
                if ( bills[i].BillAmounts > 0)
                {
                    if ((diff >= bills[i].BillAmounts * bills[i].BillValue))
                    {
                        acum = acum + (bills[i].BillAmounts * bills[i].BillValue);
                        bills[i].BillAmounts = 0;
                        diff = total-acum;
                    }
                    else
                    {
                        elements = diff / bills[i].BillValue;
                        bills[i].BillAmounts -= elements;
                        acum = acum + (elements * bills[i].BillValue);
                        diff = total - acum;
                    }
                }
            }
            if (acum == total)
            {
                this.Success = true;
            }



            }
        
    }
}

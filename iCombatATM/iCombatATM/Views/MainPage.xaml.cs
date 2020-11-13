using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iCombatATM
{
    public partial class MainPage : ContentPage
    {
        public iCombatATM.ViewModel.MainViewModel vm;
        public MainPage()
        {
            InitializeComponent();
            vm = new ViewModel.MainViewModel();
            BindingContext = vm;
        }

       
    }
}

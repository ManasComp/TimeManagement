using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsAndActivityMasterView :  MasterDetailPage
    {
        public SettingsAndActivityMasterView()
        {
            InitializeComponent();
        }
    }
}
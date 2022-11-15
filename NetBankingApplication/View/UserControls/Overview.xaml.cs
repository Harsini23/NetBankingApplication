using Library.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

//overview - bank account details, personal details, account summary
namespace NetBankingApplication.View.UserControls
{
    public sealed partial class Overview : UserControl
    {
        private User user;
        public User CurrentUser
        {
            get { return user; }
            set { this.user = value;
            }
        }
        public Overview()
        {
            this.InitializeComponent();
        }
    }
}

using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
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

namespace NetBankingApplication.View.UserControls
{
    public sealed partial class ViewAndEditPayee : UserControl
    {
        private GetAllPayeeBaseViewModel GetAllPayeeViewModel;

        PresenterService GetAllPayeeVMserviceProviderInstance;


       
        //List<Payee> allRecipients = new List<Payee>();

        private string currentUserId;
        public ViewAndEditPayee(string userId)
        {
            this.InitializeComponent();
            currentUserId = userId;
            GetAllPayeeVMserviceProviderInstance = PresenterService.GetInstance();
            GetAllPayeeViewModel = GetAllPayeeVMserviceProviderInstance.Services.GetService<GetAllPayeeBaseViewModel>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetAllPayeeViewModel.GetAllPayee(currentUserId);
            //allRecipients.Clear();
            //allRecipients = GetAllPayeeViewModel.AllPayee;
        }
    }
}

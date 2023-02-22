using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
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
    public sealed partial class Overview : UserControl,INotifyPropertyChanged
    {

      
        private readonly User user;
        // private readonly string _userId;

        private string _userId;
        public string UserId
        {
            get { return this._userId; }
            set
            {
                _userId = value;
                OnPropertyChangedAsync(nameof(UserId));
                //SetProperty(ref _response, value);
            }
        }

        private OverviewBaseViewModel _overviewViewModel;
        private GetAllAccountsBaseViewModel _getAllAccountsViewModel;
    
        public Overview(User currentUser)
        {
         
            user = currentUser;
            UserId = currentUser.UserId;
            this.InitializeComponent();
            _overviewViewModel = PresenterService.GetInstance().Services.GetService<OverviewBaseViewModel>();
            _getAllAccountsViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();
            _overviewViewModel.setUser(user.UserId);
            _overviewViewModel.getData(currentUser.UserId);
            _getAllAccountsViewModel.GetAllAccounts(user.UserId);
        }
        protected void OnPropertyChangedAsync(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RPBTest_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
          
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
          var  windowHeight = e.NewSize.Height;
           var windowWidth = e.NewSize.Width;

            if (windowHeight > 640)
                VisualStateManager.GoToState(this, "WideLayout", false);
            else
                VisualStateManager.GoToState(this, "NarrowLayout", false);
        }
    }
}

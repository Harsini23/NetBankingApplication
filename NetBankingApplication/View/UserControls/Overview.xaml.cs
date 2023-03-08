using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            _overviewViewModel.setUser(currentUser);
            _overviewViewModel.getData(currentUser.UserId);
            _getAllAccountsViewModel.GetAllAccounts(user.UserId);
            //UserUpdate.OverViewViewModelInstance = _overviewViewModel;
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
            Debug.WriteLine("Width & height : "+windowWidth + " " + windowHeight);

           
             if(windowWidth<=1000 && windowWidth >= 900)
            {
                VisualStateManager.GoToState(this, "Intermediate", false);
            }
           else if (windowWidth < 900 || windowHeight <= 440)
            {
                if (windowWidth < 550)
                {
                    VisualStateManager.GoToState(this, "Intermediate", false);

                }
                else
                {
                    VisualStateManager.GoToState(this, "NarrowLayout", false);

                }
            }
           
            else
            {
                VisualStateManager.GoToState(this, "WideLayout", false);
            }
        }
    }
}

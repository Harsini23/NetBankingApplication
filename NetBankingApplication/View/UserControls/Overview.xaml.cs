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
    public sealed partial class Overview : UserControl, INotifyPropertyChanged
    {
        private OverviewBaseViewModel _overviewViewModel;
        private GetAllAccountsBaseViewModel _getAllAccountsViewModel;

        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(Overview), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        public Overview()
        {
            this.InitializeComponent();
            _overviewViewModel = PresenterService.GetInstance().Services.GetService<OverviewBaseViewModel>();
            _getAllAccountsViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();
        }
        
        protected void OnPropertyChangedAsync(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var windowHeight = e.NewSize.Height;
            var windowWidth = e.NewSize.Width;

            if (windowWidth <= 1000 && windowWidth >= 900)
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _overviewViewModel.getData(User.UserId);
            _getAllAccountsViewModel.GetAllAccounts(User.UserId);
        }
    }
}

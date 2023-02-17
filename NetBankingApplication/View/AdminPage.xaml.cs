using Library.Model;
using Library.Model.Enum;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.View.UserControls;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NetBankingApplication.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminPage : Page, INotifyPropertyChanged
    {
       
        private LoginBaseViewModel LoginViewModel;

        public AdminPage()
        {
            this.InitializeComponent();
            LoginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
         
        }

        private void AdminPageNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem == AddUser)
            {
                AddUserView addUserView = new AddUserView();
                CurrentSelectedModule = addUserView;
                HeaderTitle = "Add user";
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        
            AddUserView addUserView = new AddUserView();
            CurrentSelectedModule = addUserView;
            HeaderTitle = "Add user";
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private UserControl _currentSelectedModule;
        public UserControl CurrentSelectedModule
        {
            get { return _currentSelectedModule; }
            set
            {
                _currentSelectedModule = value;
                NotifyPropertyChanged();
            }
        }
        private String _headerTitle = "Add user";
        public String HeaderTitle
        {
            get { return _headerTitle; }
            set
            {
                _headerTitle = value;
                NotifyPropertyChanged();
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //logout logic
            LoginViewModel.Logout();
        }

    }
}

using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace NetBankingApplication.View.UserControls
{
   
    public sealed partial class DetailedAccountOverview : Page, ISwitchUserView
    {

        private AccountTransactionBObj _currentSelectedTransaction;
        private AccountTransactionBObj _selectedItem;
        private AccountTransactionsBaseViewModel _accountTransactionsViewModel;
        private GetAllAccountsBaseViewModel _getAllAccountsViewModel;
        private static bool _itemSelected;
        private bool _narrowLayout;
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(DetailedAccountOverview), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }


        public DetailedAccountOverview()
        {
            this.InitializeComponent();
            _accountTransactionsViewModel = PresenterService.GetInstance().Services.GetService<AccountTransactionsBaseViewModel>();
            _getAllAccountsViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();
            _getAllAccountsViewModel.TransferAmountView = this;

        }

        private void DefaultDisplayWideLayout()
        {
          
        }

        MenuFlyout selectAccountList;
        private void MenuFlyout_Opening(object sender, object e)
        {

            selectAccountList = sender as MenuFlyout;
            selectAccountList.Items.Clear();
            foreach (var i in _getAllAccountsViewModel.AllAccountNumbers)
            {
                if (i != _getAllAccountsViewModel.CurrentAccountSelection)
                {
                    var item = new MenuFlyoutItem();
                    item.Text = i;
                    item.Click += Account_Selection; ;
                    item.MinWidth = 150;
                    selectAccountList.Items.Add(item);
                }
            }
        }

        private void Account_Selection(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            _getAllAccountsViewModel.CurrentAccountSelection = selectedItem.Text;
            SelectAccountDropdown.Content = selectedItem.Text;
            GetAllAccountsBaseViewModel.PreviousSelection=selectedItem.Text;
            _accountTransactionsViewModel.GetAllTransactions(_getAllAccountsViewModel.CurrentAccountSelection, User.UserId);
        }

        public void SwitchBasedOnUserAccount()
        {

            if (GetAllAccountsBaseViewModel.PreviousSelection != null)
            {
                _getAllAccountsViewModel.CurrentAccountSelection = GetAllAccountsBaseViewModel.PreviousSelection;
                SelectAccountDropdown.Content = _getAllAccountsViewModel.CurrentAccountSelection;
            }
            else if (_getAllAccountsViewModel.AllAccountNumbers.Count() > 0)
            {
                _getAllAccountsViewModel.CurrentAccountSelection = _getAllAccountsViewModel.AllAccountNumbers[0];
                SelectAccountDropdown.Content = _getAllAccountsViewModel.AllAccountNumbers[0];
            }

            _accountTransactionsViewModel.GetAllTransactions(_getAllAccountsViewModel.CurrentAccountSelection, User.UserId);


            if (_getAllAccountsViewModel.AllAccountNumbers.Count == 1)
            {
                SingleAccountnumberTextblock.Visibility = Visibility.Visible;
            }
            else if(_getAllAccountsViewModel.AllAccountNumbers.Count > 1)
            {
                SelectAccountDropdown.Visibility = Visibility.Visible;
            }
            Bindings.Update();

        }


        private void BackToList_Click(object sender, RoutedEventArgs e)
        {
            AllTransactionsOnAccountListView.Visibility = Visibility.Visible;
            BackToList.Visibility = Visibility.Collapsed;
            _itemSelected = false;
            Grid.SetColumn(TransactionListings,0);
            Grid.SetColumnSpan(TransactionListings,3);
            TransactionListings.Visibility = Visibility.Visible;
            TransactionGridSplitter.Visibility = Visibility.Collapsed;
            TransactionDetailGrid.Visibility = Visibility.Collapsed;
        }


        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double windowHeight = e.NewSize.Height;
            double windowWidth = e.NewSize.Width;

            if (windowHeight < 300 || windowWidth < 850)
            {
                _narrowLayout = true;
                CloseButton.Visibility = Visibility.Collapsed;

                if (_itemSelected)
                {

                    TransactionGridSplitter.Visibility = Visibility.Collapsed;
                    TransactionListings.Visibility = Visibility.Collapsed;
                    TransactionDetailGrid.Visibility = Visibility.Visible;
                    TransactionDetails.Visibility = Visibility.Visible;
                    Grid.SetColumn(TransactionListings, 2);
                    Grid.SetColumnSpan(TransactionListings, 1);

                    Grid.SetColumn(TransactionDetailGrid, 0);
                    Grid.SetColumnSpan(TransactionDetailGrid, 3);
                    BackToList.Visibility = Visibility.Visible;
                }
            }
            else
            {
                _narrowLayout = false;
                CloseButton.Visibility = Visibility.Visible ;

                if (_itemSelected)
                {
                    Grid.SetColumn(TransactionListings, 0);
                    Grid.SetColumn(TransactionGridSplitter, 1);
                    Grid.SetColumn(TransactionDetailGrid, 2);
                    Grid.SetColumnSpan(TransactionListings, 1);
                    Grid.SetColumnSpan(TransactionGridSplitter, 1);
                    Grid.SetColumnSpan(TransactionDetailGrid, 1);
                    TransactionListings.Visibility = Visibility.Visible;
                    TransactionGridSplitter.Visibility = Visibility.Visible;
                    TransactionDetailGrid.Visibility = Visibility.Visible;
                    BackToList.Visibility = Visibility.Collapsed;
                }

            }

        }


        private void AllTransactionsOnAccountListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _itemSelected = true;
            if (_narrowLayout)
            {
                _narrowLayout = true;
                TransactionListings.Visibility = Visibility.Collapsed;
                TransactionGridSplitter.Visibility = Visibility.Collapsed;
                TransactionDetailGrid.Visibility = Visibility.Visible;
                Grid.SetColumn(TransactionDetailGrid, 0);
                Grid.SetColumnSpan(TransactionDetailGrid, 3);
                BackToList.Visibility = Visibility.Visible;
            }
            else
            {
                _narrowLayout = false;
                Grid.SetColumn(TransactionListings, 0);
                Grid.SetColumn(TransactionGridSplitter, 1);
                Grid.SetColumn(TransactionDetailGrid, 2);
                Grid.SetColumnSpan(TransactionListings, 1);
                Grid.SetColumnSpan(TransactionGridSplitter, 1);
                Grid.SetColumnSpan(TransactionDetailGrid, 1);
                TransactionListings.Visibility = Visibility.Visible;
                TransactionGridSplitter.Visibility = Visibility.Visible;
                TransactionDetailGrid.Visibility = Visibility.Visible;
            }
                _currentSelectedTransaction = e.ClickedItem as AccountTransactionBObj;
            TransactionDetails.DataContext = _currentSelectedTransaction;
         
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            TransactionListings.Visibility = Visibility.Visible;
            TransactionGridSplitter.Visibility = Visibility.Collapsed;
            TransactionDetailGrid.Visibility = Visibility.Collapsed;
            Grid.SetColumn(TransactionListings, 0);
            Grid.SetColumnSpan(TransactionListings, 3);
            _itemSelected = false;
            //if (NarrowLayout)
            //{
            //    AllTransactionsOnAccountListView.Visibility = Visibility.Visible;
            //    BackToList.Visibility = Visibility.Collapsed;

            //    Grid.SetColumn(TransactionListings, 0);
            //    Grid.SetColumnSpan(TransactionListings, 3);
            //    TransactionListings.Visibility = Visibility.Visible;
            //    TransactionGridSplitter.Visibility = Visibility.Collapsed;
            //    TransactionDetailGrid.Visibility = Visibility.Collapsed;
            //}
            //else
            //{

            //}
        }

        private void DetailView_Loaded(object sender, RoutedEventArgs e)
        {

            _itemSelected = false;

            if (User.UserId != null)
            {
                _getAllAccountsViewModel.GetAllAccounts(User.UserId);
            }
            Bindings.Update();

        }
    }
}

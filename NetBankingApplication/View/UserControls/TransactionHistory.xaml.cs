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
using Windows.UI;
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
    public sealed partial class TransactionHistory : UserControl
    { 
        private string currentUserId;
        public static bool IsNarrowLayout;
        private TransactionHistoryBaseViewModel TransactionViewModel;
        public double windowHeight;
        public double windowWidth;

        PresenterService TransactionVMserviceProviderInstance;

        public TransactionHistory(string userId)
        {
            TransactionVMserviceProviderInstance = PresenterService.GetInstance();
            TransactionViewModel = TransactionVMserviceProviderInstance.Services.GetService<TransactionHistoryBaseViewModel>();
            this.InitializeComponent();
            currentUserId = userId;
            IsNarrowLayout = false;
        }
        public TransactionHistory()
        {
            TransactionVMserviceProviderInstance = PresenterService.GetInstance();
            TransactionViewModel = TransactionVMserviceProviderInstance.Services.GetService<TransactionHistoryBaseViewModel>();
            currentUserId = UserId;
            this.InitializeComponent();
            IsNarrowLayout = false;
        }

         public string UserId
        {
            get { return (string)GetValue(UserIdProperty); }
            set { SetValue(UserIdProperty, value);
                currentUserId = UserId;
            }
        }

        public static readonly DependencyProperty UserIdProperty =
            DependencyProperty.Register("UserId", typeof(string), typeof(TransactionHistory), new PropertyMetadata(null));
    

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TransactionViewModel.GetTransactionData(currentUserId);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
             windowHeight = e.NewSize.Height;
             windowWidth = e.NewSize.Width;

            if (windowHeight < 300 || windowWidth < 800)
            {
                //VisualStateManager.GoToState(this, "Large", true);
                IsNarrowLayout = true;

            }
            else
            {
                //VisualStateManager.GoToState(this, "Small", true);
                IsNarrowLayout=false;
            }
        }

      
    }

    public class AlternatingColorTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EvenTemplate { get; set; }
        public DataTemplate OddTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {

            //TransactionBObj t = item as TransactionBObj;
            //var listViewItem = (ListViewItem)container;
            //var listView2 = ItemsControl.ItemsControlFromItemContainer(listViewItem);
            //if (t.Index % 2 == 0)
            //{
            //    return EvenTemplate;
            //}
            //else
            //{
            //    return OddTemplate;
            //}
            var element = container as FrameworkElement;
            if (TransactionHistory.IsNarrowLayout)
            {
                return OddTemplate;
            }
            else
            {
                return EvenTemplate;
            }
          //  return EvenTemplate;

        }
    }

}

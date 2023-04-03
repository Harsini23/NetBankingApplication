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
        public static bool IsNarrowLayout;
        private TransactionHistoryBaseViewModel TransactionViewModel;
        public double windowHeight;
        public double windowWidth;
        public bool ShowOnlyRecentTransactions { get;set; }
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(Overview), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public TransactionHistory()
        {
            TransactionViewModel = PresenterService.GetInstance().Services.GetService<TransactionHistoryBaseViewModel>();
            this.InitializeComponent();
            IsNarrowLayout = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
                TransactionViewModel.GetTransactionData(User.UserId, ShowOnlyRecentTransactions);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
             windowHeight = e.NewSize.Height;
             windowWidth = e.NewSize.Width;

            if (windowHeight < 300 || windowWidth < 700)
            {
                AllTransactionListView.ItemTemplate = (DataTemplate)Resources["NarrowTemplate"];
                AllTransactionListView.HeaderTemplate = (DataTemplate)Resources["NarrowHeader"];
                IsNarrowLayout = true;

            }
            else
            {
                AllTransactionListView.ItemTemplate = (DataTemplate)Resources["WideTemplate"];
                AllTransactionListView.HeaderTemplate = (DataTemplate)Resources["WideHeader"];
                IsNarrowLayout = false;
            }
        }

      
    }

    //public class AlternatingColorTemplateSelector : DataTemplateSelector
    //{
    //    public DataTemplate EvenTemplate { get; set; }
    //    public DataTemplate OddTemplate { get; set; }

    //    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    //    {

    //        //TransactionBObj t = item as TransactionBObj;
    //        //var listViewItem = (ListViewItem)container;
    //        //var listView2 = ItemsControl.ItemsControlFromItemContainer(listViewItem);
    //        //if (t.Index % 2 == 0)
    //        //{
    //        //    return EvenTemplate;
    //        //}
    //        //else
    //        //{
    //        //    return OddTemplate;
    //        //}
    //        var element = container as FrameworkElement;
    //        if (TransactionHistory.IsNarrowLayout)
    //        {
    //            return OddTemplate;
    //        }
    //        else
    //        {
    //            return EvenTemplate;
    //        }
    //      //  return EvenTemplate;

    //    }
    //}

}

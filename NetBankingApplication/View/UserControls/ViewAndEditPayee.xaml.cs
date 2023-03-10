﻿using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public sealed partial class ViewAndEditPayee : UserControl, IDeleteNotificationAlert,IEditNotificationAlert //, IViewAndEditPayeeVM
    {
        private GetAllPayeeBaseViewModel GetAllPayeeViewModel;
        private DeletePayeeBaseViewModel DeletePayeeViewModel;
        private EditPayeeBaseViewModel EditPayeeViewModel;
        private double windowWidth;
        private double windowHeight;
        private Payee _currentPayee;

        public ObservableCollection<Payee> PayeeCollection = new ObservableCollection<Payee>();
        public ObservableCollection<Payee> SelctionPayeeCollection = new ObservableCollection<Payee>();

        //List<Payee> allRecipients = new List<Payee>();
        public event Action<string> RaiseNotification;

        private string currentUserId;
        public ViewAndEditPayee(string userId)
        {
            this.InitializeComponent();
            currentUserId = userId;
            GetAllPayeeViewModel = PresenterService.GetInstance().Services.GetService<GetAllPayeeBaseViewModel>();
            DeletePayeeViewModel = PresenterService.GetInstance().Services.GetService<DeletePayeeBaseViewModel>();
            EditPayeeViewModel = PresenterService.GetInstance().Services.GetService<EditPayeeBaseViewModel>();
            DeletePayeeViewModel.AddEditPayeeView = this;
            EditPayeeViewModel.AddEditPayeeView = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           // GetAllPayeeBaseViewModel.ChangeVisibility = this;
            PayeeCollection.Clear();
            GetAllPayeeViewModel.GetAllPayee(currentUserId);
            PayeeCollection = GetAllPayeeViewModel.AllPayeeCollection;
           
            //allRecipients.Clear();
            //allRecipients = GetAllPayeeViewModel.AllPayee;
        }

        private async void DeletePayee_Click(object sender, RoutedEventArgs e)
        {
            //Button button = (Button)sender;
            //var listViewItem = button.Parent;
            //Debug.WriteLine(listViewItem,"  ----------- ");
            var button = (Button)sender;
            var recipient = (Payee)button.DataContext;

            DeletePayeeViewModel.DeletePayee(recipient);
            //SelctionPayeeCollection.Clear();
            //GetAllPayeeViewModel.GetAllPayee(currentUserId);
            //PayeeCollection = GetAllPayeeViewModel.AllPayeeCollection;
            //AllTransactionListView.ItemsSource = PayeeCollection;
            //SuggestboxPayeeChange.Text = String.Empty;
            //EmptyList.Visibility = Visibility.Collapsed;
          

            //DeletePayeeAcknowledgementDialogue.ShowAsync();
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += (s, args) =>
            //{
            //    DeletePayeeAcknowledgementDialogue.Hide();
            //    timer.Stop();
            //};
            //timer.Start();

        }

      
        private void SuggestboxPayeeChange_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //var suitableItems = new List<Payee>();
                var splitText = sender.Text.ToLower().Split(" ");
                if(splitText.Length > 0)
                {
                    AllTransactionListView.ItemsSource = SelctionPayeeCollection;
                    SelctionPayeeCollection.Clear();
                    foreach (var i in GetAllPayeeViewModel.AllPayeeCollection)
                    {
                        var found = splitText.All((key) =>
                        {
                            return i.PayeeName.ToLower().Contains(key);
                        });
                        if (found)
                        {
                            EmptyList.Visibility = Visibility.Collapsed;
                            SelctionPayeeCollection.Add(i);
                        }
                    }
                    if (SelctionPayeeCollection.Count == 0)
                    {
                        EmptyList.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    SelctionPayeeCollection = PayeeCollection;
                    AllTransactionListView.ItemsSource = PayeeCollection;
                }
              
               // sender.ItemsSource = suitableItems;
            }

        }

        public void ChangeVisibility(bool visible)
        {
            if (visible)
            {
                EmptyList.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyList.Visibility = Visibility.Collapsed;
            }
            Bindings.Update();
        }

        private void EditPayee_Click(object sender, RoutedEventArgs e)
        {
            //populate data
            var button = sender as Button;
            Payee dataItem = (Payee)button.DataContext;
            populateEdittingDetails(dataItem);
            EditPayeePopup.IsOpen = true;
            double horizontalOffset = Window.Current.Bounds.Width / 2 - EditPayeePopup.ActualWidth / 2 +60;
            double verticalOffset = Window.Current.Bounds.Height / 2 - EditPayeePopup.ActualHeight / 2-110;
            EditPayeePopup.HorizontalOffset = horizontalOffset;
            EditPayeePopup.VerticalOffset = verticalOffset;
            ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
             windowHeight = e.NewSize.Height;
             windowWidth = e.NewSize.Width;
        }
        private void populateEdittingDetails(Payee dataItem)
        {
            _currentPayee=dataItem;
            PayeeNameTextBox.Text = dataItem.PayeeName;
            AccountHolderTextBox.Text = dataItem.AccountHolderName;
            AccountNumberTextBox.Text=dataItem.AccountNumber;
            BankNameTextBox.Text = dataItem.BankName;
            IFSCCodeTextBox.Text = dataItem.IfscCode;
        }

        private void EditPayeeDetails_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PayeeNameTextBox.Text) || string.IsNullOrWhiteSpace(PayeeNameTextBox.Text))
            {
                ErrorMessage.Text = "Name cannot be empty";
                ErrorMessage.Visibility = Visibility.Visible;
            }
           else if (string.IsNullOrEmpty(AccountHolderTextBox.Text) || string.IsNullOrWhiteSpace(AccountHolderTextBox.Text))
            {
                ErrorMessage.Text = "Account Holder name cannot be empty";
                ErrorMessage.Visibility = Visibility.Visible;
            }
           else if (string.IsNullOrEmpty(IFSCCodeTextBox.Text) || string.IsNullOrWhiteSpace(IFSCCodeTextBox.Text))
            {
                ErrorMessage.Text = "IFSC code cannot be empty";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else if (string.IsNullOrEmpty(BankNameTextBox.Text) || string.IsNullOrWhiteSpace(BankNameTextBox.Text))
            {
                ErrorMessage.Text = "Bank name cannot be empty";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else if (PayeeNameTextBox.Text == _currentPayee.PayeeName && AccountHolderTextBox.Text==_currentPayee.AccountHolderName && IFSCCodeTextBox.Text==_currentPayee.IfscCode && BankNameTextBox.Text==_currentPayee.BankName)
            {
                //no changes
                RaiseNotification?.Invoke("No changes to be edited ");
                EditPayeePopup.IsOpen = false;
            }
            else
            {
                Payee editedPayee = new Payee
                {
                    UserID = currentUserId,
                    PayeeName = PayeeNameTextBox.Text,
                    AccountHolderName = AccountHolderTextBox.Text,
                    AccountNumber = AccountNumberTextBox.Text,
                    IfscCode = IFSCCodeTextBox.Text,
                    BankName = BankNameTextBox.Text,
                };
                EditPayeeViewModel.EditPayee(editedPayee);


                EditPayeePopup.IsOpen = false;

                //EditPayeeAcknowledgementDialogue.ShowAsync();
                //DispatcherTimer timer = new DispatcherTimer();
                //timer.Interval = TimeSpan.FromSeconds(1);
                //timer.Tick += (s, args) =>
                //{ 
                //    EditPayeeAcknowledgementDialogue.Hide();
                //    timer.Stop();
                //};
                //timer.Start();
            }
        }

       
        public void CallEditNotificationNotification()
        {
            RaiseNotification?.Invoke(EditPayeeViewModel.ResponseValue);
        }

        public void CallDeleteNotificationNotification()
        {
            RaiseNotification?.Invoke(DeletePayeeViewModel.ResponseValue);
        }
    }
}

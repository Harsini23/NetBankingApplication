using Library.Model;
using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NetBankingApplication.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Overview : Page
    {
        public Overview()
        {
            this.InitializeComponent();
        }

        public static User Currentuser;
        String userId, userName, emailId;
        //
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Currentuser = (User)e.Parameter;
            //setting temporarily

            if (Currentuser != null)
            {
                userId = Currentuser.UserId;
                userName = Currentuser.UserName;
                emailId = Currentuser.EmailId;
            }
            Debug.WriteLine(userId + " " + userName);
            //UserId.Text = userId;
            //UserName.Text= userName;
            //EmailId.Text= emailId;
        }
    }
}

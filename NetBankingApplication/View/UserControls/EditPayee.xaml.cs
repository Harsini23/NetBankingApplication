using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
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
    public sealed partial class EditPayee : UserControl
    {
        public EditPayee()
        {
            this.InitializeComponent();
        }
    }



    //internal static class ThemeHelperService
    //{
    //    private static Dictionary<UIContext, FrameworkElement> XamlRootCollections { get; } = new Dictionary<UIContext, FrameworkElement>();

    //    private static ElementTheme? CurrentTheme { get; set; } = null;


    //    private static void CheckForAppSetting()//Runs only once when app Registers its root Frame
    //    {
    //        if (CurrentTheme is not null)
    //        {
    //            return;
    //        }
    //        try
    //        {
    //            var appTheme = (int)ApplicationData.Current.LocalSettings.Values["themeIndex"];
    //            CurrentTheme = (ElementTheme)appTheme;
    //        }
    //        catch (KeyNotFoundException)
    //        {

    //            ApplicationData.Current.LocalSettings.Values["themeIndex"] = GetCurrentTheme();

    //        }

    //    }

    //    public static ElementTheme GetPreferenceThemeIfSet()
    //    {
    //        try
    //        {
    //            var preferedThemeIndex = (int)ApplicationData.Current.LocalSettings.Values["themeIndex"];
    //            return (ElementTheme)preferedThemeIndex;
    //        }
    //        catch (KeyNotFoundException)
    //        {
    //            return ElementTheme.Default;
    //        }
    //    }
    //    private static ElementTheme GetCurrentTheme()
    //    {
    //        if (Window.Current.Content is Frame rootFrame)
    //        {
    //            XamlRootCollections.Add(rootFrame.UIContext, rootFrame);

    //            return rootFrame.RequestedTheme;
    //        }
    //        return ElementTheme.Default;
    //    }

    //    public static bool RegisterElement(FrameworkElement rootElement)
    //    {
    //        CheckForAppSetting();
    //        try
    //        {
    //            XamlRootCollections.Add(rootElement.UIContext, rootElement);
    //            return true;
    //        }
    //        catch (ArgumentException)//if Duplicate xaml root is not added, returned false
    //        {
    //            return false;
    //        }
    //    }
    //    public static bool UnRegisterElement(FrameworkElement rootElement)
    //    {
    //        try
    //        {
    //            XamlRootCollections.Remove(rootElement.UIContext);
    //            return true;
    //        }
    //        catch (ArgumentException)
    //        {
    //            return false;
    //        }
    //    }

    //    public async static Task<bool> ChangeTheme(ElementTheme theme)
    //    {

    //        if (CurrentTheme == theme)
    //        {
    //            return false;
    //        }

    //        ApplicationData.Current.LocalSettings.Values["themeIndex"] = (int)theme;
    //        CurrentTheme = theme;

    //        foreach (var rootCollection in XamlRootCollections)
    //        {
    //            await rootCollection.Value.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (() =>
    //            {
    //                rootCollection.Value.RequestedTheme = CurrentTheme.GetValueOrDefault();

    //            }));
    //        }
    //        return true;
    //    }

    //}
}

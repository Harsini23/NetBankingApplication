﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NetBankingApplication.View
{
    internal static class ThemeSwitch
    {
        private static Dictionary<UIContext, FrameworkElement> XamlRootCollections { get; } = new Dictionary<UIContext, FrameworkElement>();
        public static ElementTheme? CurrentTheme { get; set; }

        public static bool RegisterElement(FrameworkElement rootElement)
        {
            ThemeSetting();
            try
            {
                XamlRootCollections.Add(rootElement.UIContext, rootElement);
                ChangeTheme((ElementTheme)CurrentTheme);
                return true;
            }
            catch (ArgumentException)//if Duplicate xaml root is not added, returned false
            {
                return false;
            }
        }
        public static bool UnRegisterElement(FrameworkElement rootElement)
        {
            try
            {
                XamlRootCollections.Remove(rootElement.UIContext);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }


       
        public static void ThemeSetting()
        {
            try
            {
                var check = ApplicationData.Current.LocalSettings.Values["Theme"];
                if(check == null)
                {
                    ApplicationData.Current.LocalSettings.Values["Theme"] = GetCurrentTheme();
                }
                CurrentTheme = (ElementTheme)((int)check);
                
            }
            catch (KeyNotFoundException)
            {
               // ApplicationData.Current.LocalSettings.Values["Theme"] = (int)ElementTheme.Default;
            }
        }

        //set default theme in local settings for first time
        private static ElementTheme GetCurrentTheme()
        {
            if (Window.Current.Content is Frame rootFrame)
            {
                XamlRootCollections.Add(rootFrame.UIContext, rootFrame);

                return rootFrame.RequestedTheme;
            }
            return ElementTheme.Default;
        }

      
        public async static Task<bool> ChangeTheme(ElementTheme theme)
        {

            //if (CurrentTheme == theme)
            //{
            //    return false;
            //}

            ApplicationData.Current.LocalSettings.Values["Theme"] = (int)theme;
            CurrentTheme = theme;

            foreach (var rootCollection in XamlRootCollections)
            {
                await rootCollection.Value.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (() =>
                {
                    rootCollection.Value.RequestedTheme = CurrentTheme.GetValueOrDefault();

                }));
            }
            return true;
        }



    }




}

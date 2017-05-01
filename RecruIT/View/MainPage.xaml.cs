using System;
using System.Collections.Generic;
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
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml.Navigation;
using RecruIT.ViewModel;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace RecruIT.View
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page, IView
    {
        public MainPage()
        {
            MainViewModel = ViewModelLocator.Main;
            MainViewModel.View = this as IView;
            this.InitializeComponent();
        }

        public MainViewModel MainViewModel;

        public ContentDialog addPostContentDialog => AddPostContentDialog;
        public ContentDialog addDepartmentDialog => AddDepartmentContentDialog;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (home.IsSelected)
            {
              //  myFrame.Navigate(typeof(home));
                TitleTextBlock.Text = "Главная";
            }
            else if (share.IsSelected)
            {
               // myFrame.Navigate(typeof(share));
                TitleTextBlock.Text = "Поделиться";
            }
            else if (settings.IsSelected)
            {
               // myFrame.Navigate(typeof(settings));
                TitleTextBlock.Text = "Настройки";
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {            
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }


        public async void ShowAddPostsDialogWindow()
        {
           await AddPostContentDialog.ShowAsync();
        }

        public async void ShowAddDepartmentDialogWindow()
        {
            await AddDepartmentContentDialog.ShowAsync();
        }


    }
    public interface IView
    {
        void ShowAddPostsDialogWindow();

        void ShowAddDepartmentDialogWindow();
    }
}

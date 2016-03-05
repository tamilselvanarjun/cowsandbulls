using Cowbull;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BullsAndCows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        

        public MainPage()
        {
            this.InitializeComponent();
        }

        async private void startGameButton_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            c.u = c.t = c.h = 0;
            c.lives = 5;
            while (c.u == c.t || c.t == c.h || c.h == c.u)
            {
                c.u = rnd.Next(0, 10);
                c.t = rnd.Next(0, 10);
                c.h = rnd.Next(1, 10);
            }

            StorageFile sampleFile = await c.storage.GetFileAsync("dataFile.txt");
            int score = Int32.Parse(await FileIO.ReadTextAsync(sampleFile));
            c.cur_streak = score % 1000;
            c.max_streak = score / 1000;

            //Frame.Navigate(typeof(BlankPage2));
            Frame.Navigate(typeof(GamePage));
        }

        private void instructionsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Instructions));
        }

        private async void statsButton_Click(object sender, RoutedEventArgs e)
        {
            if (await c.storage.TryGetItemAsync("dataFile.txt") == null)
            {
                c.cur_streak = 0;
                c.max_streak = 0;
                int score1 = c.max_streak * 1000 + c.cur_streak;
                StorageFile sampleFile1 = await c.storage.CreateFileAsync("dataFile.txt",
                   CreationCollisionOption.FailIfExists);
                await FileIO.WriteTextAsync(sampleFile1, score1.ToString());

            }

            StorageFile sampleFile = await c.storage.GetFileAsync("dataFile.txt");
            int score = Int32.Parse(await FileIO.ReadTextAsync(sampleFile));
            c.cur_streak = score % 1000;
            c.max_streak = score / 1000;
            // Frame.Navigate(typeof(BlankPage4));
            Frame.Navigate(typeof(Stats));
        }

        private async void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog m3 = new MessageDialog("Do you Really want to Exit?");
            m3.Title = "Quit";
            m3.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(this.CommandInvokedHandler)));
            m3.Commands.Add(new UICommand("No", new UICommandInvokedHandler(this.CommandInvokedHandler)));
            m3.DefaultCommandIndex = 0;
            await m3.ShowAsync();
        }

        private async void CommandInvokedHandler(IUICommand com)
        {
            if (com.Label == "Yes")
            {
                int score = c.max_streak * 1000 + c.cur_streak;
                StorageFile sampleFile = await c.storage.CreateFileAsync("dataFile.txt",
                CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, score.ToString());
                Application.Current.Exit();
            }
            else
                Frame.Navigate(typeof(MainPage));
        }


    }
}

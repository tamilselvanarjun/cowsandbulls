using Cowbull;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BullsAndCows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        public GamePage()
        {
            this.InitializeComponent();
            guessesLeft.Text = "Guesses left: " + c.lives.ToString();


            buttonArray = new int[10];
            for (int i = 0; i < 10; i++)
                buttonArray[i] = 0;

     //       textBlock.Text = c.h.ToString() + c.t.ToString() + c.u.ToString();
     //       textBlock1.Text = "Lives: " + c.lives.ToString();

        }

        private int[] buttonArray;
       // private string green = "#FF51EA1D";
       // private string red = "#FFFF392E";

        private void Number_clicked(object sender, RoutedEventArgs e)
        {

            Button mybutton = sender as Button;

            if (buttonArray[Convert.ToInt32(mybutton.Content)] == 0)
            {
                mybutton.Background = new SolidColorBrush(Colors.Red);
                buttonArray[Convert.ToInt32(mybutton.Content)] = 1;
            }
            else if(buttonArray[Convert.ToInt32(mybutton.Content)] == 1)
            {
                mybutton.Background = new SolidColorBrush(Colors.Green);
                buttonArray[Convert.ToInt32(mybutton.Content)] = 0;
            }
               
           
        }

        private async void submit_Click(object sender, RoutedEventArgs e)
        {
            int num = 0;
            int flag = 1;
            int cows = 0, bulls = 0;
            try {
                if (guessNumber.Text == "" || Int32.Parse(guessNumber.Text) >= 1000 || Int32.Parse(guessNumber.Text) < 100)
                {
                    MessageDialog m = new MessageDialog("Please enter a three digit nunmber.");
                    await m.ShowAsync();
                    flag = 0;
                }
                else
                {
                    
                    cows = 0;
                    bulls = 0;
                    
                    num = Int32.Parse(guessNumber.Text);
                    int u = num % 10, t = (num / 10) % 10, h = num / 100;
                    if (c.u == h || c.t == h) cows++;
                    if (c.h == t || c.u == t) cows++;
                    if (c.t == u || c.h == u) cows++;
                    if (c.u == u) bulls++;
                    if (c.t == t) bulls++;
                    if (c.h == h) bulls++;



                    resultBlock.Text = cows.ToString() + " cows, " + bulls.ToString() + " bulls";
                    if (bulls == 3)
                    {
                        c.cur_streak++;
                        if (c.cur_streak > c.max_streak)
                            c.max_streak = c.cur_streak;
                        c.lives = 5;

                        int score = c.max_streak * 1000 + c.cur_streak;
                        StorageFile sampleFile = await c.storage.CreateFileAsync("dataFile.txt",
                        CreationCollisionOption.ReplaceExisting);
                        await FileIO.WriteTextAsync(sampleFile, score.ToString());

                        MessageDialog m2 = new MessageDialog("Well Played", "You Win");
                        m2.Content = "New Game";
                        m2.Commands.Add(new UICommand("Main Menu", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                        m2.DefaultCommandIndex = 0;
                        await m2.ShowAsync();

                    }
                    c.lives--;
                    guessesLeft.Text = "Guesses left: " + c.lives.ToString();

                }

            }
            catch
            {
                MessageDialog m = new MessageDialog("Please enter a 3-digit number.", "Error");
                await m.ShowAsync();
                flag = 0;
            }

            if (c.lives == 4 && flag==1)
            {
                move1Guess.Text = num.ToString();

                if (bulls == 3)
                    move1_1.Source = move1_2.Source = move1_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));
                else if(bulls==2)
                    move1_1.Source = move1_2.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));
                else if(bulls==1)
                    move1_1.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));

                if (cows == 3)
                    move1_1.Source = move1_2.Source = move1_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
                else if (cows == 2)
                    move1_3.Source = move1_2.Source =  new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
                else if (cows == 1)
                    move1_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));

            }

            if (c.lives == 3 && flag == 1)
            {

                move2Guess.Text = num.ToString();
   
                if (bulls == 3)
                    move2_1.Source = move2_2.Source = move2_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));
                else if (bulls == 2)
                    move2_1.Source = move2_2.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));
                else if (bulls == 1)
                    move2_1.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));

                if (cows == 3)
                    move2_1.Source = move2_2.Source = move2_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
                else if (cows == 2)
                    move2_3.Source = move2_2.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
                else if (cows == 1)
                    move2_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
            }
                
            if (c.lives == 2 && flag == 1)
            {

                if (bulls == 3)
                    move3_1.Source = move3_2.Source = move3_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));
                else if (bulls == 2)
                    move3_1.Source = move3_2.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));
                else if (bulls == 1)
                    move3_1.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));

                if (cows == 3)
                    move3_1.Source = move3_2.Source = move3_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
                else if (cows == 2)
                    move3_3.Source = move3_2.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
                else if (cows == 1)
                    move3_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));

                move3Guess.Text = num.ToString();
            }
                
            if (c.lives == 1 && flag == 1)
            {

                if (bulls == 3)
                    move4_1.Source = move4_2.Source = move4_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));
                else if (bulls == 2)
                    move4_1.Source = move4_2.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));
                else if (bulls == 1)
                    move4_1.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));

                if (cows == 3)
                    move4_1.Source = move4_2.Source = move4_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
                else if (cows == 2)
                    move4_3.Source = move4_2.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
                else if (cows == 1)
                    move4_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));

                move4Guess.Text = num.ToString();
            }
                
            if (c.lives == 0 && flag == 1)
            {

                if (bulls == 3)
                    move5_1.Source = move5_2.Source = move5_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));
                else if (bulls == 2)
                    move5_1.Source = move5_2.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));
                else if (bulls == 1)
                    move5_1.Source = new BitmapImage(new Uri("ms-appx:///Assets/bull.jpg"));

                if (cows == 3)
                    move5_1.Source = move5_2.Source = move5_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
                else if (cows == 2)
                    move5_3.Source = move5_2.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));
                else if (cows == 1)
                    move5_3.Source = new BitmapImage(new Uri("ms-appx:///Assets/cow.jpg"));

                move5Guess.Text = num.ToString();
            }
                

            if (c.lives == 0 && flag == 1)
            {
                c.cur_streak = 0;
                int score = c.max_streak * 1000 + c.cur_streak;
                StorageFile sampleFile = await c.storage.CreateFileAsync("dataFile.txt",
                CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, score.ToString());

                int actualNum = c.u + 10 * c.t + 100 * c.h;
                MessageDialog m1 = new MessageDialog("Better Luck Next Time ! \n The number is - " + Convert.ToString(actualNum), "You Lose !");
                m1.Commands.Add(new UICommand("Main Menu", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                m1.DefaultCommandIndex = 0;
                await m1.ShowAsync();
            }


        }

        private void CommandInvokedHandler(IUICommand com)
        {
            Frame.Navigate(typeof(MainPage));
        }

        

    }

    


}

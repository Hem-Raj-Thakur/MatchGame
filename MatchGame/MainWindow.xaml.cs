using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Author: Hem Raj
    /// </summary>
    public partial class MainWindow : Window
    {
        // Create the timer and keep track of time passed while playing with count of matches found.
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        // variables for TextBlock_MouseDown() method
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        int hpCounts; // to check that all the animals are matched and are set to hidden.
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += dispatcherTimer_Tick;
            SetUpGame();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timerTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timerTextBlock.Text += " - Play Again?";
            }
        }

        protected void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "😹","😹",
                "🙊","🙊",
                "🦁","🦁",
                "🐗","🐗",
                "🐨","🐨",
                "🐘","🐘",
                "🐇","🐇",
                "🐪","🐪"
            };

            Random random = new Random();
            int usableTxtBlocksCnt = 0;
            foreach(TextBlock txtBlock in animalGrid.Children.OfType<TextBlock>())
            {
                usableTxtBlocksCnt += 1;
                //if (txtBlock.Name != "timerTextBlock") // Working
                if (usableTxtBlocksCnt <= animalGrid.Children.OfType<TextBlock>().Count() - 1)
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    txtBlock.Text = nextEmoji;
                    txtBlock.Visibility = Visibility.Visible;
                    txtBlock.Foreground = Brushes.Navy;
                    animalEmoji.RemoveAt(index);
                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            hpCounts = 0;
            TextBlock textBlock = sender as TextBlock;
            if(!findingMatch)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if(textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }

            foreach (TextBlock txtBlock in animalGrid.Children.OfType<TextBlock>())
            {
                if (txtBlock.Visibility == Visibility.Hidden)
                {
                    hpCounts++;
                }
            }

            if (hpCounts == animalGrid.Children.OfType<TextBlock>().Count()-1)
            {
                MessageBox.Show("Congrats! You have matched all the animal pairs.");
            }
        }

        private void timerTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}

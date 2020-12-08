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
        int totalCountDownTime = 120;

        // variables for TextBlock_MouseDown() method
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        int hpCounts; // to check that all the animals are matched and are set to hidden.

        // variables for SetupGame()
        Random hpRandom = new Random();
        double prevBestTime;
        double currElapsedTime;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += timer_Tick;
            SetUpGame();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //tenthsOfSecondsElapsed++;
            tenthsOfSecondsElapsed--;
            //timerTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timerTextBlock.Text += " - Play Again?";
                return;
            }
            
            currElapsedTime = Math.Round((tenthsOfSecondsElapsed / 10F), 1);
            timerTextBlock.Text = currElapsedTime + "s";
            // added for count down in place of up (++)
            currElapsedTime = totalCountDownTime - currElapsedTime;
        }

        protected void SetUpGame()
        {
            Brush[] brushes = new Brush[] { Brushes.DarkRed, Brushes.Navy, Brushes.ForestGreen, Brushes.DarkSlateBlue };
            int[] animalGroupsStartNum = new int[] { 0, 16, 32, 48 };
            //string[] animalGroups = new string[] { "0-48", "16-32", "32-16", "48-0" };
            //int[] animalGrpsLastNum = new int[] { 48, 32, 16, 0 };
            int[] animalGrpsLastNum = new int[animalGroupsStartNum.Length];
            Array.Copy(animalGroupsStartNum, animalGrpsLastNum, animalGroupsStartNum.Length);
            Array.Reverse(animalGrpsLastNum);
            List<string> animalEmoji = new List<string>()
            {
                "😹","😹",
                "🙊","🙊",
                "🦁","🦁",
                "🐗","🐗",
                "🐨","🐨",
                "🐘","🐘",
                "🐇","🐇",
                "🐪","🐪",

                "🐱‍🏍","🐱‍🏍",
                "🐵","🐵",
                "🐶","🐶",
                "🦒","🦒",
                "🦝","🦝",
                "🐰","🐰",
                "🐻","🐻",
                "🐸","🐸",

                "🦓","🦓",
                "🐴","🐴",
                "🐔","🐔",
                "🐲","🐲",
                "🐒","🐒",
                "🦍","🦍",
                "🐕‍🦺","🐕‍🦺",
                "🦌","🦌",
                
                "🐎","🐎",
                "🐬","🐬",
                "🐢","🐢",
                "🐋","🐋",
                "🦔","🦔",
                "🐿","🐿",
                "🦖","🦖",
                "🦏","🦏"
            };

            Random random = new Random();
            int usableTxtBlocksCnt = 0;
            int emojiGroupIndex = hpRandom.Next(animalGroupsStartNum.Count());
            int randomMinVal = animalGroupsStartNum[emojiGroupIndex];
            int randomMaxVal;
            foreach (TextBlock txtBlock in animalGrid.Children.OfType<TextBlock>())
            {
                usableTxtBlocksCnt += 1;
                //if (txtBlock.Name != "timerTextBlock") // Working
                if (usableTxtBlocksCnt <= animalGrid.Children.OfType<TextBlock>().Count() - 2)
                {
                    //int index = random.Next(animalEmoji.Count);
                    //int index = random.Next(0, animalEmoji.Count - 15);
                    randomMaxVal = (emojiGroupIndex == animalGroupsStartNum.Count() - 1) ? animalEmoji.Count : animalEmoji.Count - (animalGrpsLastNum[emojiGroupIndex]);
                    int index = random.Next(randomMinVal, randomMaxVal);
                    string nextEmoji = animalEmoji[index];
                    txtBlock.Text = nextEmoji;
                    txtBlock.Visibility = Visibility.Visible;
                    txtBlock.Foreground = brushes[emojiGroupIndex]; //Brushes.Navy;
                    animalEmoji.RemoveAt(index);
                }
            }
            timer.Start();
            //tenthsOfSecondsElapsed = 0;
            tenthsOfSecondsElapsed = totalCountDownTime * 10; // * 10 because it will devide it by 10
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

            if (hpCounts == animalGrid.Children.OfType<TextBlock>().Count()-2)
            {
                string hpMessage = "Congrats! You have matched all the animal pairs.";
                //
                if (prevBestTime == 0)
                {
                    prevBestTime = currElapsedTime;
                }
                else if (prevBestTime > currElapsedTime)
                {
                    hpMessage = "Congrats! You have matched all the animal pairs in less than previous time.";
                    prevBestTime = currElapsedTime;
                }
                else if (prevBestTime == currElapsedTime)
                {
                    hpMessage = "Congrats! You have matched all the animal pairs in equal of previous time.";
                }
                bestTimeTextBlock.Text = $"Best Time: {prevBestTime.ToString("0.0s")}";
                MessageBox.Show(hpMessage,"Info!");
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

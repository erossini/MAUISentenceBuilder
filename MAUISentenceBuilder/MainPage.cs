using Microsoft.Maui.Controls;

namespace MAUISentenceBuilder
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            var sentenceBuilder = new SentenceBuilder
            {
                AvailableWords = new List<string> { "Hello", "world", "this", "is", "MAUI" },
                ButtonColor = Colors.Green,
                PlaceholderColor = Colors.LightGray,
                FontFamily = "Helvetica",
                TextSize = 20
            };
            sentenceBuilder.SentenceValidated += OnSentenceValidated;

            Content = new StackLayout
            {
                Children = { sentenceBuilder }
            };
        }

        private void OnSentenceValidated(object sender, bool isCorrect)
        {
            if (isCorrect)
            {
                DisplayAlert("Correct!", "You have formed the correct sentence.", "OK");
            }
            else
            {
                DisplayAlert("Incorrect", "The sentence is not correct. Try again.", "OK");
            }
        }
    }
}
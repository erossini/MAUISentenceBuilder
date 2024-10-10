using Microsoft.Maui.Controls;

namespace MAUISentenceBuilder
{
    public class MainPage : ContentPage
    {
        private SentenceBuilder sentenceBuilder;

        public MainPage()
        {
            sentenceBuilder = new SentenceBuilder
            {
                AvailableWords = new List<string> { "Hello", "world", "this", "is", "MAUI" },
                ButtonColor = Colors.Green,
                PlaceholderColor = Colors.LightGray,
                FontFamily = "Helvetica",
                TextSize = 20
            };
            sentenceBuilder.SentenceValidated += OnSentenceValidated;
            sentenceBuilder.CanValidateSentenceChanged += OnCanValidateSentenceChanged;

            var validateButton = new Button
            {
                Text = "Validate",
                FontSize = 18,
                IsVisible = false
            };
            validateButton.Clicked += (sender, e) => sentenceBuilder.OnValidateButtonClicked(sender, e);

            Content = new StackLayout
            {
                Children = { sentenceBuilder, validateButton }
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

        private void OnCanValidateSentenceChanged(object sender, EventArgs e)
        {
            var validateButton = (Button)((StackLayout)Content).Children.Last();
            validateButton.IsVisible = sentenceBuilder.SelectedWords.Any();
        }
    }
}
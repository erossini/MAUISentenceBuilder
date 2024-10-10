using Microsoft.Maui.Controls;

namespace MAUISentenceBuilder
{
    public class MainPage : ContentPage
    {
        private Label sentenceLabel;
        private string formedSentence = "";

        public MainPage()
        {
            sentenceLabel = new Label
            {
                Text = "Formed Sentence: ",
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };

            var words = new[] { "Hello", "world", "this", "is", "MAUI" };
            var wordButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Spacing = 10
            };

            foreach (var word in words)
            {
                var button = new Button
                {
                    Text = word,
                    FontSize = 18
                };
                button.Clicked += OnWordButtonClicked;
                wordButtons.Children.Add(button);
            }

            Content = new StackLayout
            {
                Children = { wordButtons, sentenceLabel }
            };
        }

        private void OnWordButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                formedSentence += button.Text + " ";
                sentenceLabel.Text = "Formed Sentence: " + formedSentence;
            }
        }
    }
}
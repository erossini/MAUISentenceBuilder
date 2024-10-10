using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAUISentenceBuilder
{
    public class MainPage : ContentPage
    {
        private StackLayout availableWordsLayout;
        private StackLayout selectedWordsLayout;
        private List<string> availableWords;
        private List<string> selectedWords;

        public MainPage()
        {
            availableWords = new List<string> { "Hello", "world", "this", "is", "MAUI" };
            selectedWords = new List<string>();

            availableWordsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Spacing = 10
            };

            selectedWordsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Spacing = 10
            };

            UpdateWordButtons();

            Content = new StackLayout
            {
                Children = { availableWordsLayout, selectedWordsLayout }
            };
        }

        private void UpdateWordButtons()
        {
            availableWordsLayout.Children.Clear();
            selectedWordsLayout.Children.Clear();

            foreach (var word in availableWords)
            {
                var button = new Button
                {
                    Text = word,
                    FontSize = 18
                };
                button.Clicked += OnAvailableWordClicked;
                availableWordsLayout.Children.Add(button);
            }

            foreach (var word in selectedWords)
            {
                var button = new Button
                {
                    Text = word,
                    FontSize = 18
                };
                button.Clicked += OnSelectedWordClicked;
                selectedWordsLayout.Children.Add(button);
            }
        }

        private async void OnAvailableWordClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                await AnimateButton(button, availableWordsLayout, selectedWordsLayout);
                availableWords.Remove(button.Text);
                selectedWords.Add(button.Text);
                UpdateWordButtons();
            }
        }

        private async void OnSelectedWordClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                await AnimateButton(button, selectedWordsLayout, availableWordsLayout);
                selectedWords.Remove(button.Text);
                availableWords.Add(button.Text);
                UpdateWordButtons();
            }
        }

        private async Task AnimateButton(Button button, StackLayout fromLayout, StackLayout toLayout)
        {
            var initialPosition = button.Bounds;
            fromLayout.Children.Remove(button);
            toLayout.Children.Add(button);

            var finalPosition = button.Bounds;
            button.TranslationX = initialPosition.X - finalPosition.X;
            button.TranslationY = initialPosition.Y - finalPosition.Y;

            await button.TranslateTo(0, 0, 500, Easing.CubicInOut);
        }
    }
}
using Microsoft.Maui.Controls;
using System.Collections.Generic;

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

        private void OnAvailableWordClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                availableWords.Remove(button.Text);
                selectedWords.Add(button.Text);
                UpdateWordButtons();
            }
        }

        private void OnSelectedWordClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                selectedWords.Remove(button.Text);
                availableWords.Add(button.Text);
                UpdateWordButtons();
            }
        }
    }
}
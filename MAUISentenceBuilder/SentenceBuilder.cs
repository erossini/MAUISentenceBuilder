using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAUISentenceBuilder
{
    public class SentenceBuilder : ContentView
    {
        public static readonly BindableProperty AvailableWordsProperty =
            BindableProperty.Create(nameof(AvailableWords), typeof(List<string>), typeof(SentenceBuilder), new List<string>(), propertyChanged: OnAvailableWordsChanged);

        public List<string> AvailableWords
        {
            get => (List<string>)GetValue(AvailableWordsProperty);
            set => SetValue(AvailableWordsProperty, value);
        }

        public event EventHandler<bool> SentenceValidated;

        private StackLayout availableWordsLayout;
        private StackLayout selectedWordsLayout;
        private List<string> selectedWords;
        private Dictionary<string, BoxView> placeholders;
        private Button validateButton;

        public SentenceBuilder()
        {
            selectedWords = new List<string>();
            placeholders = new Dictionary<string, BoxView>();

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

            validateButton = new Button
            {
                Text = "Validate Sentence",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                IsVisible = false
            };
            validateButton.Clicked += OnValidateButtonClicked;

            Content = new StackLayout
            {
                Children = { availableWordsLayout, selectedWordsLayout, validateButton }
            };

            UpdateWordButtons();
        }

        private static void OnAvailableWordsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SentenceBuilder)bindable;
            control.UpdateWordButtons();
        }

        private void UpdateWordButtons()
        {
            availableWordsLayout.Children.Clear();
            selectedWordsLayout.Children.Clear();

            foreach (var word in AvailableWords)
            {
                var button = new Button
                {
                    Text = word,
                    FontSize = 18
                };
                button.Clicked += OnAvailableWordClicked;
                availableWordsLayout.Children.Add(button);

                if (!placeholders.ContainsKey(word))
                {
                    var placeholder = new BoxView
                    {
                        Color = Colors.Gray,
                        WidthRequest = 80,
                        HeightRequest = 40,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    };
                    placeholders[word] = placeholder;
                }
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

            foreach (var word in placeholders.Keys)
            {
                if (!AvailableWords.Contains(word))
                {
                    availableWordsLayout.Children.Add(placeholders[word]);
                }
            }

            validateButton.IsVisible = selectedWords.Any();
        }

        private async void OnAvailableWordClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                AvailableWords.Remove(button.Text);
                selectedWords.Add(button.Text);
                await AnimateButton(button, availableWordsLayout, selectedWordsLayout);
                UpdateWordButtons();
            }
        }

        private async void OnSelectedWordClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                selectedWords.Remove(button.Text);
                AvailableWords.Add(button.Text);
                await AnimateButton(button, selectedWordsLayout, availableWordsLayout);
                UpdateWordButtons();
            }
        }

        private async Task AnimateButton(Button button, Layout fromLayout, Layout toLayout)
        {
            var initialPosition = button.Bounds;
            fromLayout.Children.Remove(button);
            toLayout.Children.Add(button);

            var finalPosition = button.Bounds;
            button.TranslationX = initialPosition.X - finalPosition.X;
            button.TranslationY = initialPosition.Y - finalPosition.Y;

            await button.TranslateTo(0, 0, 500, Easing.CubicInOut);
        }

        private void OnValidateButtonClicked(object sender, EventArgs e)
        {
            bool isCorrect = selectedWords.SequenceEqual(AvailableWords);
            SentenceValidated?.Invoke(this, isCorrect);
        }
    }
}
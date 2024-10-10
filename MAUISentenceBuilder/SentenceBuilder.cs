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

        public static readonly BindableProperty ButtonColorProperty =
            BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(SentenceBuilder), Colors.Blue);

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(SentenceBuilder), Colors.Gray);

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(SentenceBuilder), "Arial");

        public static readonly BindableProperty TextSizeProperty =
            BindableProperty.Create(nameof(TextSize), typeof(double), typeof(SentenceBuilder), 18.0);

        public static readonly BindableProperty AnimationDurationProperty =
            BindableProperty.Create(nameof(AnimationDuration), typeof(uint), typeof(SentenceBuilder), 500u);

        public List<string> AvailableWords
        {
            get => (List<string>)GetValue(AvailableWordsProperty);
            set => SetValue(AvailableWordsProperty, value);
        }

        public Color ButtonColor
        {
            get => (Color)GetValue(ButtonColorProperty);
            set => SetValue(ButtonColorProperty, value);
        }

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public double TextSize
        {
            get => (double)GetValue(TextSizeProperty);
            set => SetValue(TextSizeProperty, value);
        }

        public uint AnimationDuration
        {
            get => (uint)GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }

        public List<string> SelectedWords => selectedWords;

        public event EventHandler<bool> SentenceValidated;
        public event EventHandler CanValidateSentenceChanged;

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
                FontSize = TextSize,
                FontFamily = FontFamily,
                BackgroundColor = ButtonColor,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                IsVisible = false
            };
            validateButton.Clicked += OnValidateButtonClicked;

            Content = new StackLayout
            {
                Children = { availableWordsLayout, selectedWordsLayout, validateButton }
            };

            AddGestureRecognizers();
            UpdateWordButtons();
        }

        #region Gestures

        private void AddGestureRecognizers()
        {
            var swipeLeftGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            swipeLeftGesture.Swiped += OnSwiped;

            var swipeRightGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
            swipeRightGesture.Swiped += OnSwiped;

            var swipeUpGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Up };
            swipeUpGesture.Swiped += OnSwiped;

            var swipeDownGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Down };
            swipeDownGesture.Swiped += OnSwiped;

            this.GestureRecognizers.Add(swipeLeftGesture);
            this.GestureRecognizers.Add(swipeRightGesture);
            this.GestureRecognizers.Add(swipeUpGesture);
            this.GestureRecognizers.Add(swipeDownGesture);
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    // Handle left swipe
                    break;
                case SwipeDirection.Right:
                    // Handle right swipe
                    break;
                case SwipeDirection.Up:
                    // Handle up swipe
                    break;
                case SwipeDirection.Down:
                    // Handle down swipe
                    break;
            }
        }

        #endregion

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
                var button = CreateDraggableButton(word);
                availableWordsLayout.Children.Add(button);

                if (!placeholders.ContainsKey(word))
                {
                    var placeholder = new BoxView
                    {
                        Color = PlaceholderColor,
                        WidthRequest = 80,
                        HeightRequest = 40,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        IsVisible = false
                    };
                    placeholders[word] = placeholder;
                }

                availableWordsLayout.Children.Add(placeholders[word]);
            }

            foreach (var word in selectedWords)
            {
                var button = CreateDraggableButton(word);
                selectedWordsLayout.Children.Add(button);
            }

            validateButton.IsVisible = selectedWords.Any();
            CanValidateSentenceChanged?.Invoke(this, EventArgs.Empty);
        }

        private Button CreateDraggableButton(string word, bool isSelected = false)
        {
            var button = new Button
            {
                Text = word,
                FontSize = TextSize,
                FontFamily = FontFamily,
                BackgroundColor = ButtonColor
            };

            var dragGesture = new DragGestureRecognizer();
            dragGesture.DragStarting += (s, e) => OnDragStarting(s, e, word);
            button.GestureRecognizers.Add(dragGesture);

            var dropGesture = new DropGestureRecognizer();
            dropGesture.Drop += (s, e) => OnDrop(s, e, word);
            button.GestureRecognizers.Add(dropGesture);

            var tapGesture = new TapGestureRecognizer();
            if (isSelected)
                tapGesture.Tapped += (s, e) => OnSelectedWordClicked(s, e);
            else
                tapGesture.Tapped += (s, e) => OnAvailableWordClicked(s, e);
            button.GestureRecognizers.Add(tapGesture);

            return button;
        }

        private void OnDragStarting(object sender, DragStartingEventArgs e, string word)
        {
            e.Data.Properties.Add("Word", word);
            e.Data.Properties.Add("Source", sender);
        }

        private void OnDrop(object sender, DropEventArgs e, string word)
        {
            if (e.Data.Properties.ContainsKey("Word"))
            {
                var draggedWord = e.Data.Properties["Word"].ToString();
                var sourceButton = e.Data.Properties["Source"] as Button;

                if (selectedWords.Contains(draggedWord))
                {
                    selectedWords.Remove(draggedWord);
                    var dropIndex = selectedWords.IndexOf(word);
                    selectedWords.Insert(dropIndex, draggedWord);
                }
                else
                {
                    AvailableWords.Remove(draggedWord);
                    selectedWords.Add(draggedWord);
                }

                UpdateWordButtons();
            }
        }

        private async void OnAvailableWordClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                placeholders[button.Text].IsVisible = true;
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
                placeholders[button.Text].IsVisible = false;
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

            await button.TranslateTo(0, 0, AnimationDuration, Easing.CubicInOut);
        }

        public void OnValidateButtonClicked(object sender, EventArgs e)
        {
            bool isCorrect = selectedWords.SequenceEqual(AvailableWords);
            SentenceValidated?.Invoke(this, isCorrect);
        }
    }
}
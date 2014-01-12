using System.Windows;
using System.Windows.Input;

namespace TaskMaster
{
    /// <summary>
    /// Description for PlayedTimesSummaryView.
    /// </summary>
    public partial class PlayedTimesSummaryView : Window
    {
        /// <summary>
        /// Initializes a new instance of the PlayedTimesSummaryView class.
        /// </summary>
        public PlayedTimesSummaryView()
        {
            InitializeComponent();
        }

        private void Exit_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;

            this.DragMove();
        }
    }
}
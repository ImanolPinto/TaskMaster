using System.Windows;
using System.Windows.Input;

namespace TaskMaster
{
    /// <summary>
    /// Description for ArchiveView.
    /// </summary>
    public partial class ArchiveView : Window
    {
        /// <summary>
        /// Initializes a new instance of the ArchiveView class.
        /// </summary>
        public ArchiveView()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;

            this.DragMove();
        }

        private void Exit_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Maximize_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Maximized;
        }

        private void Restore_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Normal;
        }

        private void Minimize_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
    }
}
using System.Windows;
using System.Windows.Input;
using TaskMaster.Model;
using TaskMaster.ViewModel;
using WPF.JoshSmith.ServiceProviders.UI;

namespace TaskMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ListViewDragDropManager<TaskItem> dragMgr;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (TaskListView != null)
                this.dragMgr = new ListViewDragDropManager<TaskItem>(this.TaskListView);
            SetTagChooserVisibility(Visibility.Collapsed);
        }

        private void ChooseATag_Click(object sender, RoutedEventArgs e)
        {
            if (GetTagChooserVisibility() == Visibility.Collapsed)
            {
                SetTagChooserVisibility(Visibility.Visible);
            }
            else
            {
                SetTagChooserVisibility(Visibility.Collapsed);
            }
        }

        private void TaskListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SetTagChooserVisibility(Visibility.Collapsed);
        }

        private Visibility GetTagChooserVisibility()
        {
            return this.ChooseATagListView.Visibility;
        }

        private void SetTagChooserVisibility(Visibility visibility)
        {
            this.ChooseATagListView.Visibility = visibility;
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

        private void TextBlock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AboutView about = new AboutView();
            about.ShowDialog();
        }
    }
}
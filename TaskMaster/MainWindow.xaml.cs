using System.Windows;
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
            return this.ChooseATagLabel.Visibility;
        }

        private void SetTagChooserVisibility(Visibility visibility)
        {
            this.ChooseATagLabel.Visibility = visibility;
            this.ChooseATagListView.Visibility = visibility;
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
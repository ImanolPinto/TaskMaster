using System.Reflection;
using System.Windows;

namespace TaskMaster
{
    /// <summary>
    /// Description for MvvmView1.
    /// </summary>
    public partial class AboutView : Window
    {
        /// <summary>
        /// Initializes a new instance of the MvvmView1 class.
        /// </summary>
        public AboutView()
        {
            InitializeComponent();

            this.VersionInfo.Content = "v" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void Exit_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
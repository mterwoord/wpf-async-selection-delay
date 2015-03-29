using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAsyncSelectionDelay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty InputProperty =
            DependencyProperty.Register("Input", typeof(string), typeof(MainWindow),
                new FrameworkPropertyMetadata(null, OnInputChanged));

        public string Input
        {
            get { return (string)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        private static void OnInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var xTarget = (MainWindow)d;
            var xOldInput = (string)e.OldValue;
            var xNewInput = xTarget.Input;
            xTarget.OnInputChanged(xOldInput, xNewInput);
        }

        protected virtual async void OnInputChanged(string oldInput, string newInput)
        {
            if (mCancelToken != null)
            {
                mCancelToken.Cancel();
                // we need to yield here. Only then the TaskCanceledException occurs
                await Task.Yield();
            }
            try
            {
                using (mCancelToken = new CancellationTokenSource())
                {
                    try
                    {
                        await Task.Delay(1000, mCancelToken.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        return;
                    }
                    HandleValueChanged();
                }
            }
            finally
            {
                mCancelToken = null;
            }
        }

        private CancellationTokenSource mCancelToken;

        private void HandleValueChanged()
        {
            AppendLog(string.Format("New text: '{0}'", Input));
        }

        private void AppendLog(string line)
        {
            var xLog = blockLog.Text;
            xLog = xLog + "\r\n" + line;
            blockLog.Text = xLog.Trim();
        }
    }
}

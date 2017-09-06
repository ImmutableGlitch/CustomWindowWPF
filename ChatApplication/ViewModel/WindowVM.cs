using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatApplication
{
    public class WindowVM : BaseVM
    {
        #region Private Members
        
        /// <summary>
        /// The window the view model controls
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The invisible margin around the window to allow drop shadow
        /// </summary>
        private int mOuterMarginSize = 10;

        /// <summary>
        /// Radius of the window corners
        /// </summary>
        private int mWindowRadius = 10;

        #endregion

        #region Public Properties

        /// <summary>
        /// The smallest width the window can be sized
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;

        /// <summary>
        /// The smallest height the window can be sized
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        /// <summary>
        /// The thickness of the resize border around the window
        /// </summary>
        public int ResizeBorder { get; set; } = 6;

        /// <summary>
        /// The thickness of the resize border around the window, accounting for the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

        /// <summary>
        /// The padding of the windows inner content
        /// </summary>
        public Thickness InnerContentPadding { get { return new Thickness(ResizeBorder); } }

        /// <summary>
        /// The invisible margin around the window to allow drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                // If fullscreen then remove outer margin drop shadow
                return mWindow.WindowState == WindowState.Maximized ? 0 : mOuterMarginSize;
            }
            set
            {
                mOuterMarginSize = value;
            }
        }

        /// <summary>
        /// The invisible margin around the window to allow drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }

        /// <summary>
        /// Radius of the window edges
        /// </summary>
        public int WindowRadius
        {
            get
            {
                // If fullscreen then remove outer margin drop shadow
                return mWindow.WindowState == WindowState.Maximized ? 0 : mWindowRadius;
            }
            set
            {
                mWindowRadius = value;
            }
        }

        /// <summary>
        /// Radius of the window edges
        /// </summary>
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The height of the title bar
        /// </summary>
        public int TitleHeight { get; set; } = 42;

        /// <summary>
        /// The height of the title bar
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }

        #endregion

        #region Commands

        /// <summary>
        /// The command to minimise a window
        /// </summary>
        public ICommand MinimiseCommand { get; set; }

        /// <summary>
        /// The command to maximise a window
        /// </summary>
        public ICommand MaximiseCommand { get; set; }

        /// <summary>
        /// The command to close a window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to show the system menu of a window
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window"></param>
        public WindowVM(Window window)
        {
            mWindow = window;

            // Listen for the window resizing
            mWindow.StateChanged += (sender , e) =>
            {
                // Fire events for all properties affected by a window resize
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            // Create commands
            MinimiseCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximiseCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            // Fix window fullscreen issue
            var resizer = new WindowResizer(mWindow);
        }

        #region Helper methods
        /// <summary>
        /// Get the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(mWindow);
            // Add the window position to get actual on-screen coordinates
            return new Point(position.X + mWindow.Left , position.Y + mWindow.Top);
        }
        #endregion
    }
}

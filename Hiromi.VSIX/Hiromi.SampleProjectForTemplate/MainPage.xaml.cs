using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;
using Hiromi;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Hiromi.SampleProjectForTemplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : SwapChainBackgroundPanel
    {
        readonly MainGame _game;

        public MainPage(string launchArguments)
        {
            
            this.InitializeComponent();

            // Create the game.
            _game = XamlGame<MainGame>.Create(launchArguments, Window.Current.CoreWindow, this);
        }

        private void SwapChainBackgroundPanel_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}

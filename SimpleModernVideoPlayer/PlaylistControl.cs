using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SimpleModernVideoPlayer
{
    public sealed class PlaylistControl:MediaTransportControls
    {
        public event EventHandler<EventArgs> Switch;

        public PlaylistControl()
        {
            this.DefaultStyleKey = typeof(PlaylistControl);
        }

        protected override void OnApplyTemplate()
        {
            // This is where you would get your custom button and create an event handler for its click method.
            Button listButton = GetTemplateChild("PlaylistButton") as Button;
            listButton.Click += ListButton_Click;
            base.OnApplyTemplate();
        }

        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise an event on the custom control when 'like' is clicked
            Switch?.Invoke(this, EventArgs.Empty);
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace LuisterBieb
{
	public partial class PlayPauseControl : UserControl
	{
		public PlayState playState;
		
		public PlayPauseControl()
		{
			InitializeComponent();
		}

		private void UserControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			// ToDo: Fire an event when the state has changed
			if (playState == PlayState.Play) {
				Play.Visibility = Visibility.Collapsed;
				Pause.Visibility = Visibility.Visible;
				playState = PlayState.Pause;
			}
			if (playState == PlayState.Pause) {
				Play.Visibility = Visibility.Visible;
				Pause.Visibility = Visibility.Collapsed;
				playState = PlayState.Play;
			}
		}
	}
	
	public enum PlayState {
		Play,
		Pause
	}
}

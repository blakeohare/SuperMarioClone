using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SuperMarioBros
{
	public class JukeBox
	{
		private static int alternator = 0;
		private static MediaElement[] sfx_players;

		public static void Init(params MediaElement[] sfx_players)
		{
			JukeBox.sfx_players = sfx_players;
		}

		public static void PlaySound(string file)
		{
			MediaElement player = sfx_players[alternator];
			player.Stop();
			player.Source = new Uri("Sounds/" + file + ".mp3", UriKind.Relative);
			player.Volume = 1;
			player.Position = TimeSpan.FromMilliseconds(200);
			player.Play();

			alternator = (alternator + 1) % sfx_players.Length;
		}
	}
}

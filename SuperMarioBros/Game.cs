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
	public class Game
	{
		private static Game instance = new Game();
		public static Game Instance { get { return Game.instance; } }

		public int LifeCount { get; set; }
		public int MajorLevel { get; set; }
		public int MinorLevel { get; set; }
		public int Score { get; set; }
		public int CoinCount { get; private set; }

		public bool IsBig { get; set; }
		public bool IsFire { get; set; }

		public Game()
		{
			this.Reset();
		}

		public void Reset()
		{
			this.LifeCount = 3;
			this.MajorLevel = 1;
			this.MinorLevel = 1;
			this.Score = 0;
			this.IsBig = false;
			this.IsFire = false;
		}

		public void AddCoins(int count)
		{
			this.CoinCount += count;
			JukeBox.PlaySound("Coin");
			while (this.CoinCount >= 100)
			{
				this.CoinCount -= 100;
				this.LifeCount++;
			}
		}
	}
}

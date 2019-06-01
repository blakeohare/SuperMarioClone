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
	public class Lives : IState
	{
		private int counter = 0;
		private Grid splash;
		private IState next = null;
		private int lives = Game.Instance.LifeCount;
		public Lives()
		{
			this.next = this;
			this.splash = new Grid();

			if (this.lives > 0)
			{
				this.splash.Children.Add(Util.CreateTextAt(" x " + this.lives.ToString(), 300, 200));
				Util.BlitBrushOntoGrid(this.splash, "Mario_Right_Stand", 240, 200);
			}
			else
			{
				this.splash.Children.Add(Util.CreateTextAt("GAME OVER", 304, 291));
			}
		}

		public void Update()
		{
			this.counter++;

			foreach (InputEvent evt in InputManager.Instance.GetEvents())
			{
			}

			if (this.lives > 0 && this.counter >= 60)
			{
				GamePlay gameplay = new GamePlay();
				gameplay.InitializeTo("1-1");
				this.next = gameplay;
			}
			else if (this.lives == 0)
			{
				if (this.counter == 1)
				{
					JukeBox.PlaySound("GameOver");
				}
				else if (this.counter == 200)
				{
					this.next = new Title();
					Game.Instance.Reset();
				}
			}
		}

		public IState NextState
		{
			get
			{
				return this.next;
			}
		}

		public FrameworkElement RenderField
		{
			get
			{
				return this.splash;
			}
		}
	}
}

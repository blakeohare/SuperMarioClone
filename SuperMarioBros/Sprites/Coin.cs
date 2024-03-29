﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SuperMarioBros.Sprites
{
	public class Coin : Sprite
	{
		public Coin(int x, int y) :
			base(x, y)
		{
			this.Radius = 10;
		}

		public override Brush GetImage(int counter)
		{
			return ImageLibrary.Get("Sprite_Coin_" + (1 + ((counter / 10) % 1)).ToString());
		}

		public override void Update(GamePlay level, Mario mario)
		{
			this.lifetime++;

			if (this.IsCollision(mario.X, mario.Y))
			{
				this.RemoveMe = true;
				Game.Instance.AddCoins(1);
			}
		}
	}
}

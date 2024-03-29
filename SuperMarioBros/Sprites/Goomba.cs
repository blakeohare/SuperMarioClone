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
	public class Goomba : Sprite
	{
		private bool goingLeft = true;

		public Goomba(int x, int y) :
			base(x, y)
		{
			this.Radius = 24;
		}

		public override Brush GetImage(int counter)
		{
			return ImageLibrary.Get("Sprite_Goomba_" + (1 + (counter / 10) % 2).ToString());
		}

		public override void Update(GamePlay level, Mario mario)
		{
			this.lifetime++;

			if (this.IsCollision(mario.X, mario.Y))
			{
				if (!mario.OnGround && mario.VY > 0)
				{
					this.RemoveMe = true;
					JukeBox.PlaySound("Clobber");
				}
				else if (!mario.IsDead)
				{
					mario.IsDead = true;
				}
			}

			if (!mario.IsDead)
			{
				int proposedX = this.X + (this.goingLeft ? -2 : 2);
				Block b = level.GetBlockForPixel(proposedX, this.Y);
				if (b == null || b.IsPassable)
				{
					this.X = proposedX;
				}
				else
				{
					this.goingLeft = !this.goingLeft;
				}
			}

		}
	}
}

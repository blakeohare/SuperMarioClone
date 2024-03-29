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
	public class Mushroom : Sprite
	{
		private bool headingRight;

		public Mushroom(int x, int y, bool headingRight) :
			base(x, y)
		{
			this.Radius = 24;
			this.headingRight = headingRight;
		}

		public override Brush GetImage(int counter)
		{
			return ImageLibrary.Get("Sprite_Mushroom");
		}

		public override void Update(GamePlay level, Mario mario)
		{
			this.lifetime++;

			if (this.lifetime == 1)
			{
				JukeBox.PlaySound("Coin");//TODO: find a mushroom noise
			}

			if (this.IsCollision(mario.X, mario.Y))
			{
				this.RemoveMe = true;
				Game.Instance.IsBig = true;
				JukeBox.PlaySound("Powerup");
			}
			else if (this.lifetime > 16)
			{
				int proposedX = this.X + (this.headingRight ? 2 : -2);
				Block b = level.GetBlockForPixel(proposedX, this.Y);
				if (b == null || b.IsPassable)
				{
					this.X = proposedX;
				}
				else
				{
					this.headingRight = !this.headingRight;
				}
			}
		}

		public override void DrawMeOn(Grid spriteLayer, int counter, int renderXOffset)
		{
			base.DrawMeOn(spriteLayer, counter, renderXOffset);
			if (this.lifetime < 16)
			{
				Canvas mushroom = spriteLayer.Children[spriteLayer.Children.Count - 1] as Canvas;
				if (mushroom != null)
				{
					mushroom.Height = this.lifetime * 3; // TODO: add vertical offset
					mushroom.Margin = new Thickness(mushroom.Margin.Left, this.Y + 24 - this.lifetime * 3, 0, 0);
				}
			}
			
		}
	}
}

﻿using System;
using System.Collections.Generic;
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
	public enum SpriteType
	{
		Coin,
		Mushroom,
		Goomba
		
	}

	public class Sprite
	{
		public int X { get; protected set; }
		public int Y { get; protected set; }
		public int Radius { get; protected set; }
		public bool RemoveMe { get; protected set; }
		public SpriteType Type { get; protected set; }

		protected int lifetime = 0;
		protected List<Brush> animation = new List<Brush>();

		public Sprite(int x, int y) {
				this.X = x;
			this.Y = y;
		}

		public bool IsCollision(int x, int y)
		{
			int dx = this.X - x;
			int dy = this.Y - y;
			int r = this.Radius + 24;

			return (dx * dx + dy * dy < r * r);
		}

		public virtual void Update(GamePlay level, Mario mario)
		{
		}

		public virtual Brush GetImage(int counter)
		{
			return null;
		}

		public virtual void DrawMeOn(Grid spriteLayer, int counter, int renderXOffset)
		{
			Util.BlitBrushOntoGrid(
				spriteLayer,
				this.GetImage(counter),
				this.X - renderXOffset - this.Radius,
				this.Y - this.Radius,
				48,
				48);
		}
	}
}
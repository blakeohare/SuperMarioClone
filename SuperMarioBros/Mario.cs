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

namespace SuperMarioBros
{
	public class Mario
	{
		public int X { get; set; }
		public int Y { get; set; }
		public double VX { get; set; }
		public double VY { get; set; }
		public bool OnGround { get; set; }
		public int DeadCounter { get; set; }
		public bool IsDead
		{
			get { return this.DeadCounter > 0; }
			set { this.DeadCounter = value ? 1 : 0; }
		}
		public bool FacingRight { get; set; }

		public Mario()
		{
			this.DeadCounter = 0;
			this.FacingRight = true;
		}

		public Brush Image(int counter)
		{
			string direction = (this.FacingRight ? "Right" : "Left");
			string file = this.OnGround ?
					(this.VX != 0 ?
						"Mario_" + direction + "_Walk_" + ((counter / 2 % 3) + 1).ToString() :
						"Mario_" + direction + "_Stand") :
					"Mario_" + direction + "_Jump";
			if (this.IsDead)
			{
				file = "Mario_Dead";
			}

			return ImageLibrary.Get(file);
		}

		public Mario Clone
		{
			get
			{
				return new Mario()
				{
					X = this.X,
					Y = this.Y,
					VX = this.VX,
					VY = this.VY,
					FacingRight = this.FacingRight,
					OnGround = this.OnGround
				};
			}
		}
	}
}

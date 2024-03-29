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
	public class Pause : IState
	{
		private int counter = 0;
		private Grid render;
		private GamePlay previous;
		private IState next;

		public Pause(GamePlay previousState)
		{
			this.previous = previousState;
			
			this.render = new Grid();
			TextBlock tb = new TextBlock();
			tb.HorizontalAlignment = HorizontalAlignment.Center;
			tb.VerticalAlignment = VerticalAlignment.Center;
			tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
			this.render.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
			tb.Text = "PAUSE";
			this.render.Children.Add(tb);

			this.next = this;
		}

		public void Update()
		{
			if (this.counter == 0)
			{
				JukeBox.PlaySound("Pause");
			}

			foreach (InputEvent evt in InputManager.Instance.GetEvents())
			{
				if (evt.Down && evt.Key == Key.Start)
				{
					this.next = this.previous;
					this.previous.Unpause();
				}
			}
			this.counter++;
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
				return this.render;
			}
		}
	}
}

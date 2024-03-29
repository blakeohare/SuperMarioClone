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
	public class Title : IState
	{
		private int counter = 0;
		private Grid splash;
		private IState next = null;

		public Title()
		{
			this.next = this;
			this.splash = new Grid();
		}

		public void Update()
		{
			this.counter++;

			foreach (InputEvent evt in InputManager.Instance.GetEvents())
			{
				if (!evt.Down && evt.Key == Key.Start)
				{
					this.next = new Lives();
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

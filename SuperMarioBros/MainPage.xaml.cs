﻿using System;
using System.Collections.Generic;
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
	
	
	public partial class MainPage : UserControl
	{
		private static FontFamily arcadeFont;
		public static FontFamily ArcadeFont
		{
			get
			{
				return arcadeFont;
			}
		}

		private System.Windows.Threading.DispatcherTimer timer;
		private IState state;
		private static TextBlock debugScreen;

		public MainPage()
		{
			InitializeComponent();

			this.Loaded += new RoutedEventHandler(MainPage_Loaded);
			
			this.KeyDown += new KeyEventHandler(PressKey);
			this.KeyUp += new KeyEventHandler(LiftKey);

			ImageLibrary.Instance.InitializeResources(this.Resources);
			JukeBox.Init(this.SFX_1, this.SFX_2);

			MainPage.arcadeFont = this.TextTemplate.FontFamily;

			this.state = new Title();
			debugScreen = this.DebugText;
		}

		public static void Debug(string text)
		{
			debugScreen.Text = text;
		}

		void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			this.timer = new System.Windows.Threading.DispatcherTimer();
			this.timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
			this.timer.Tick += new EventHandler(timer_Tick);

			this.timer.Start();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			this.state.Update();
			IState nextState = this.state.NextState;
			if (nextState != this.state)
			{
				this.state = nextState;
				this.LayoutRoot.Child = nextState.RenderField;
			}
		}

		private void PressKey(object sender, System.Windows.Input.KeyEventArgs e)
		{
			InputManager.Instance.HandleKeyEvent(e, true);
		}

		private void LiftKey(object sender, System.Windows.Input.KeyEventArgs e)
		{
			InputManager.Instance.HandleKeyEvent(e, false);
		}
	}
}
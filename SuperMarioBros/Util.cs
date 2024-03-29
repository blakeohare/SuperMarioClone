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
	public static class Util
	{
		public static TextBlock CreateTextAt(string text, int x, int y)
		{
			TextBlock tb = new TextBlock();
			tb.FontFamily = MainPage.ArcadeFont;
			tb.HorizontalAlignment = HorizontalAlignment.Left;
			tb.VerticalAlignment = VerticalAlignment.Top;
			tb.FontSize = 32;
			tb.Margin = new Thickness(x, y, 0, 0);
			tb.Width = 300;
			tb.Height = 32;
			tb.Text = text;
			tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

			return tb;
		}

		public static void BlitBrushOntoGrid(Grid target, string brush, int x, int y)
		{
			BlitBrushOntoGrid(target, ImageLibrary.Get(brush), x, y, 48, 48);
		}
		public static void BlitBrushOntoGrid(Grid target, Brush brush, int x, int y, int width, int height)
		{
			Canvas canvas = new Canvas();
			canvas.HorizontalAlignment = HorizontalAlignment.Left;
			canvas.VerticalAlignment = VerticalAlignment.Top;
			canvas.Width = width;
			canvas.Height = height;
			canvas.Margin = new Thickness(x, y, 0, 0);
			canvas.Background = brush;
			target.Children.Add(canvas);
		}

	}
}

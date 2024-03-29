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
	public class ImageLibrary
	{
		private static ImageLibrary instance = null;
		private ResourceDictionary resources;
		private Dictionary<string, ImageBrush> imageCache = new Dictionary<string, ImageBrush>();

		public void InitializeResources(ResourceDictionary resources)
		{
			this.resources = resources;
		}

		public static ImageBrush Get(string key)
		{
			if (!instance.imageCache.ContainsKey(key))
			{
				ImageBrush brush = new ImageBrush();
				brush.ImageSource = ((Image)instance.resources[key]).Source;
				instance.imageCache.Add(key, brush);
			}
			return instance.imageCache[key];
		}

		public static ImageLibrary Instance
		{
			get
			{
				if (ImageLibrary.instance == null)
				{
					ImageLibrary.instance = new ImageLibrary();
				}
				return ImageLibrary.instance;
			}
		}


	}
}

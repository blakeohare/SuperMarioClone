using System;
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
	public enum Key
	{
		Unknown,
		Left,
		Right,
		Down,
		Up,
		A,
		B,
		Start
	}
	
	public class InputManager
	{
		private static InputManager instance = new InputManager();
		public static InputManager Instance { get { return instance; } }

		private Dictionary<Key, bool> isPressed = new Dictionary<Key, bool>()
		{
			{ Key.A, false },
			{ Key.B, false },
			{ Key.Down, false },
			{ Key.Left, false },
			{ Key.Right, false },
			{ Key.Up, false },
			{ Key.Start, false },
			{ Key.Unknown, false }

		};

		private List<InputEvent> events = new List<InputEvent>();
		
		public InputEvent[] GetEvents()
		{
			InputEvent[] events = this.events.ToArray();
			this.events = new List<InputEvent>();
			return events;
		}

		public void HandleKeyEvent(System.Windows.Input.KeyEventArgs args, bool keyDown)
		{
			Key key = this.GetKey(args);
			if (key != Key.Unknown && keyDown != this.isPressed[key])
			{
				this.events.Add(new InputEvent() { Down = keyDown, Key = key });
				this.isPressed[key] = keyDown;
			}
		}

		public bool IsPressed(Key key)
		{
			return this.isPressed[key];
		}

		private Key GetKey(System.Windows.Input.KeyEventArgs args)
		{
			switch (args.Key)
			{
				case System.Windows.Input.Key.Left:
					return Key.Left;

				case System.Windows.Input.Key.Right:
					return Key.Right;

				case System.Windows.Input.Key.Down:
					return Key.Down;

				case System.Windows.Input.Key.A:
					return Key.A;

				case System.Windows.Input.Key.B:
					return Key.B;

				case System.Windows.Input.Key.Enter:
					return Key.Start;

				case System.Windows.Input.Key.Up:
					return Key.Up;

				default:
					return Key.Unknown;
			}
			
		}
	}

	public class InputEvent
	{
		public Key Key { get; set; }
		public bool Down { get; set; }
		public bool Up { get { return !this.Down; } }
	}
}

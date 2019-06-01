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
	public class GamePlay : IState
	{
		private int counter = 0;

		private IState next;

		private ScrollViewer cropper = new ScrollViewer();
		private Grid gameField = new Grid();
		private Grid blockGrid;
		private Grid spriteGrid;
		private Canvas marioCanvas;

		private Mario mario;
		
		private double g = 6;

		private bool timeWarningPlayed = false;
		private int timeLeft = 9029;

		private int tileWidth;
		private int tileHeight;

		private Block[,] blocks;
		private Canvas[,] canvasGrid = new Canvas[17, 15];

		private TextBlock timeText;
		private TextBlock coinsText;
		private TextBlock worldText;
		private TextBlock pointsText;

		private List<Sprite> spriteList = new List<Sprite>();

		public GamePlay()
		{
			this.blockGrid = new Grid();

			this.cropper.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
			this.cropper.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
			this.cropper.Content = this.gameField;
			this.cropper.BorderBrush = null;
			this.cropper.BorderThickness = new Thickness(0);
			
			this.spriteGrid = new Grid();
			
			this.marioCanvas = new Canvas();
			this.marioCanvas.HorizontalAlignment = HorizontalAlignment.Left;
			this.marioCanvas.VerticalAlignment = VerticalAlignment.Top;
			this.marioCanvas.Width = 48;
			this.marioCanvas.Height = Game.Instance.IsBig ? 96 : 48;

			this.gameField.Children.Add(this.blockGrid);
			this.gameField.Children.Add(this.spriteGrid);
			this.gameField.Children.Add(marioCanvas);

			Grid status = new Grid();

			status.Children.Add(Util.CreateTextAt("MARIO", 72, 24));
			status.Children.Add(Util.CreateTextAt("WORLD", 432, 24));
			status.Children.Add(Util.CreateTextAt("TIME", 600, 24));

			this.pointsText = Util.CreateTextAt("000000", 72, 48);
			this.worldText = Util.CreateTextAt(" 1-1", 432, 48);
			this.coinsText = Util.CreateTextAt("00", 312, 48);
			this.timeText = Util.CreateTextAt("300", 600, 48);

			status.Children.Add(this.pointsText);
			status.Children.Add(this.coinsText);
			status.Children.Add(this.worldText);
			status.Children.Add(this.timeText);

			this.gameField.Children.Add(status);
			this.next = this;

			for (int y = 0; y < 15; ++y)
			{
				for (int x = 0; x < 17; ++x)
				{
					Canvas c = new Canvas();
					c.Width = 48;
					c.Height = 48;
					c.HorizontalAlignment = HorizontalAlignment.Left;
					c.VerticalAlignment = VerticalAlignment.Top;
					c.Margin = new Thickness(x * 48, y * 48, 0, 0);

					this.canvasGrid[x, y] = c;
					this.blockGrid.Children.Add(c);
				}
			}
		}

		public void UpdateSprites()
		{
			List<Sprite> newSprites = new List<Sprite>();

			//TODO: sprite batching
			foreach (Sprite sprite in this.spriteList)
			{
				sprite.Update(this, this.mario);
				if (!sprite.RemoveMe)
				{
					newSprites.Add(sprite);
				}
			}

			this.spriteList = newSprites;
		}

		public int RenderMarioX
		{
			get { return this.mario.X - this.RenderBeginX; }
		}
		public int RenderMarioY
		{
			get { return this.mario.Y; }
		}

		public int RenderBeginX
		{
			get
			{
				if (this.mario.X < 384)
				{
					return 0;
				}
				return this.mario.X - 384;
			}
		}

		public void InitializeTo(string level)
		{
			LevelParser parser = new LevelParser("1-1");

			this.blocks = parser.BlockGrid;
			this.tileHeight = 15;
			this.tileWidth = parser.Width;
			this.gameField.Background = parser.Background;
			this.mario = parser.DefaultMario;

			//TODO: put this in level paresr
			this.spriteList.Add(new Sprites.Goomba(500, 576));
		}

		public void Update()
		{
			this.counter++;
			if (!this.mario.IsDead)
			{
				this.timeLeft--;
			}

			this.cropper.ScrollToVerticalOffset(0);
			this.cropper.ScrollToHorizontalOffset(0);

			foreach (InputEvent evt in InputManager.Instance.GetEvents())
			{
				if (!this.mario.IsDead)
				{
					if (evt.Key == Key.A)
					{
						if (evt.Down && this.mario.OnGround)
						{
							this.mario.VY = -60;
							this.mario.OnGround = false;
							JukeBox.PlaySound("Jump");
						}
						else if (evt.Up && !this.mario.OnGround && this.mario.VY < 0)
						{
							this.mario.VY = 0;
						}
					}
					else if (evt.Key == Key.Start)
					{
						if (evt.Down)
						{
							this.next = new Pause(this);
						}
					}
				}
				
			}

			int multiplier = InputManager.Instance.IsPressed(Key.B) ? 2 : 1;

			if (!this.mario.IsDead && InputManager.Instance.IsPressed(Key.Left))
			{
				this.mario.FacingRight = false;
				this.mario.VX = -8 * multiplier;
			}
			else if (!this.mario.IsDead && InputManager.Instance.IsPressed(Key.Right))
			{
				this.mario.FacingRight = true;
				this.mario.VX = 8 * multiplier;
			}
			else
			{
				this.mario.VX = 0;
			}

			this.mario.VY = System.Math.Min(47, this.mario.VY + this.g);
			int proposedX = this.mario.X + Convert.ToInt32(this.mario.VX);
			int proposedY = this.mario.OnGround ? this.mario.Y : Convert.ToInt32(this.mario.Y + this.mario.VY);

			CollisionDescriptor collision = this.FindCollision(proposedX, proposedY, this.mario.X, this.mario.Y);
			if (collision.IsCollision)
			{
				proposedX = collision.X;
				proposedY = collision.Y;
				int dir = collision.OClock;

				if (dir == 12 || dir == 1 || dir == 11)
				{
					proposedY = (proposedY / 48) * 48 - 24;
					this.mario.OnGround = true;
					this.mario.VY = 0;
				}
				else if (dir >= 5 && dir <= 7)
				{
					proposedY = (proposedY / 48 + 1) * 48 + 2;
					this.mario.VY = 0;
					//play bump noise depending on collision type
					// possibly break or create things
					Block b = this.GetBlockForPixel(collision.X, collision.Y);
					if (b != null && b.Type == BlockType.Question)
					{
						this.DoStuffWhenBlockHit(collision.X, collision.Y);
					}
				}
				else if (dir > 6)
				{
					proposedX = proposedX / 48 * 48 - 1;
				}
				else
				{
					proposedX = proposedX / 48 * 48 + 48 + 1;
				}
				
			}

			this.mario.X = proposedX;
			this.mario.Y = proposedY;
			if (this.mario.OnGround)
			{
				Block below = this.GetBlockForPixel(this.mario.X, this.mario.Y + 48);
				if (below == null || below.IsPassable)
				{
					this.mario.OnGround = false;
				}
			}

			if (!this.mario.IsDead && this.mario.Y > 700)
			{
				this.mario.DeadCounter = 1;
			}

			this.UpdateSprites();

			this.DrawMap();
			this.DrawMario();
			this.DrawSprites();

			this.UpdateStatus();

			if (this.mario.IsDead)
			{
				if (this.mario.DeadCounter == 1)
				{
					JukeBox.PlaySound("Death");
				}
				this.mario.DeadCounter++;

				if (this.mario.DeadCounter > 120)
				{
					Game.Instance.LifeCount--;

					this.next = new Lives();
				}
			}

		}
		
		private void UpdateStatus()
		{
			int time = Convert.ToInt32(this.timeLeft / 30);
			if (!timeWarningPlayed && time == 100)
			{
				timeWarningPlayed = true;
				JukeBox.PlaySound("TimeWarning");
			}
			this.timeText.Text = time.ToString();
		}

		private Point firstTileCollision(int proposedX, int proposedY, int fromX, int fromY)
		{
			
			//horizontal case
			if (fromY == proposedY)
			{
				int startTile = fromX / 48;
				int endTile = proposedX / 48;
				int y = fromY / 48;
				if (startTile == endTile)
				{
					return new Point(-1, -1);
				}
				int direction = startTile < endTile ? 1 : -1;

				for (int x = fromX; x != proposedX; x += direction)
				{
					Block b = this.GetBlockForPixel(x, fromY);
					if (b != null && !b.IsPassable)
					{
						return new Point(x, fromY);
					}
				}
			}
			//vertical case
			else if (fromX == proposedX)
			{
				int startTile = fromY / 48;
				int endTile = proposedY / 48;
				int x = fromX / 48;
				if (startTile == endTile)
				{
					return new Point(-1, -1);
				}
				int direction = startTile < endTile ? 1 : -1;

				for (int y = fromY; y != proposedY; y += direction)
				{
					Block b = this.GetBlockForPixel(fromX, y);
					if (b != null && !b.IsPassable)
					{
						return new Point(fromX, y);
					}
				}
			}
			//sloped case
			else
			{
				int dx = proposedX - fromX;
				int dy = proposedY - fromY;
				double slope = (0.0 + dy) / dx;
				int step = 1;
				double stepX = fromX;
				double stepY = fromY;

				//shallow - walk x
				if (Math.Abs(slope) < .5)
				{
					step = proposedX > fromX ? 1 : -1;

					while (stepX * step < proposedX * step)
					{
						int x = Convert.ToInt32(stepX);
						int y = Convert.ToInt32(stepY);
						Block b = this.GetBlockForPixel(x, y);
						if (b != null && !b.IsPassable)
						{
							return new Point(x, y);
						}

						stepY += slope * step;
						stepX += step;
					}
				}
				//steep - walk y
				else
				{
					step = proposedY > fromY ? 1 : -1;

					while (stepY * step < proposedY * step)
					{
						int x = Convert.ToInt32(stepX);
						int y = Convert.ToInt32(stepY);
						Block b = this.GetBlockForPixel(x, y);
						if (b != null && !b.IsPassable)
						{
							return new Point(x, y);
						}

						stepX += step / slope;
						stepY += step;
					}
				}
			}

			return new Point(-1, -1);
		}
		private class CollisionDescriptor
		{
			public int OClock { get; set; }
			public int X { get; set; }
			public int Y { get; set; }
			public bool IsCollision { get; set; }
		}

		private CollisionDescriptor FindCollision(int proposedX, int proposedY, int fromX, int fromY)
		{
			int newTileX = Convert.ToInt32(proposedX / 48);
			int newTileY = Convert.ToInt32(proposedY / 48);
			int oldTileX = Convert.ToInt32(fromX / 48);
			int oldTileY = Convert.ToInt32(fromY / 48);

			if (newTileX == oldTileX && newTileY == oldTileY)
			{
				return new CollisionDescriptor() { IsCollision = false };
			}

			Point firstCollision = this.firstTileCollision(proposedX, proposedY, fromX, fromY);
			if (firstCollision.X == -1 || firstCollision.Y == -1)
			{
				return new CollisionDescriptor() { IsCollision = false };
			}

			int toX = Convert.ToInt32(firstCollision.X);
			int toY = Convert.ToInt32(firstCollision.Y);

			int oclock = 12;

			if (toX == fromX)
			{
				oclock = toY > fromY ? 12 : 6;
			}
			else if (toY == fromY)
			{
				oclock = toX > fromX ? 9 : 3;
			}
			else
			{
				double slope = Math.Abs((0.0 + toY - fromY) / (toX - fromX));

				if (toX > fromX)
				{
					if (toY > fromY)
					{
						oclock = slope > .5 ? 11 : 10;
					}
					else
					{
						oclock = slope > .5 ? 7 : 8;
					}
				}
				else
				{
					if (toY > fromY)
					{
						oclock = slope > .5 ? 1 : 2;
					}
					else
					{
						oclock = slope > .5 ? 5 : 4;
					}
				}
			}

			return new CollisionDescriptor() { IsCollision = true, X = toX, Y = toY, OClock = oclock };
		}

		private void DrawMap()
		{
			int offset = this.RenderBeginX / 48;
			for (int x = 0; x < 17; ++x)
			{
				for (int y = 0; y < this.tileHeight; ++y)
				{
					this.canvasGrid[x, y].Background = this.GetTileForCoordinate(x + offset, y);
					
				}
			}
			
			this.blockGrid.Margin = new Thickness(-1 * (this.RenderBeginX % 48), -24, 0, 0);
		}

		private Brush GetTileForCoordinate(int x, int y)
		{
			if (x < 0 || y < 0 || x >= this.tileWidth || y >= this.tileHeight)
			{
				return null;
			}

			Block b = this.blocks[x, y];
			if (b != null)
			{
				return b.Image;
			}
			return null;
		}

		public Block GetBlockForPixel(int x, int y)
		{
			x = x / 48;
			y = y / 48;
			if (x < 0 || y < 0 || x >= this.tileWidth || y >= this.tileHeight)
			{
				return null;
			}
			return this.blocks[x, y];
		}

		private void DrawSprites()
		{
			this.spriteGrid.Children.Clear();
			foreach (Sprite sprite in this.spriteList)
			{
				sprite.DrawMeOn(this.spriteGrid, this.counter, this.RenderBeginX);
			}
		}

		private void DrawMario()
		{
			this.marioCanvas.Background = this.mario.Image(this.counter);
			
			this.marioCanvas.Margin = new Thickness(
				this.RenderMarioX - 24,
				this.RenderMarioY - (Game.Instance.IsBig ? 48 : 24) - 24,
				0,
				0);
		}

		public IState NextState
		{
			get
			{
				return this.next;
			}
		}

		public void Unpause()
		{
			this.next = this;
		}

		public FrameworkElement RenderField
		{
			get
			{
				return this.cropper;
			}
		}

		private void DoStuffWhenBlockHit(int x, int y)
		{
			int tileX = x / 48;
			int tileY = y / 48;

			Sprites.Mushroom mushroom = new SuperMarioBros.Sprites.Mushroom(tileX * 48 + 24, (tileY - 1) * 48, true);
			this.spriteList.Add(mushroom);

			this.blocks[tileX, tileY] = Block.Create(BlockType.Block);
		}
	}
}

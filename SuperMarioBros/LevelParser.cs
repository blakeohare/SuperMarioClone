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
	public class LevelParser
	{
		private string[] rows;
		private string level;

		public LevelParser(string level)
		{
			this.level = level;
			this.rows = LevelParser.levels[level].Split('\n');
		}

		private Block GetBlock(int x, int y)
		{
			if (rows.Length > y)
			{
				string row = rows[y];
				if (row.Length > x)
				{
					switch (row[x])
					{
						case ' ':
							return null;
						case '1':
							return Block.Create(BlockType.Ground);
						case '2':
							return Block.Create(BlockType.Brick);
						case '3':
							return Block.Create(BlockType.Question);
						case '4':
							return Block.Create(BlockType.Block);
						case '5':
							return Block.Create(BlockType.PipeRimLeft);
						case '6':
							return Block.Create(BlockType.PipeRimRight);
						case '7':
							return Block.Create(BlockType.PipeShaftLeft);
						case '8':
							return Block.Create(BlockType.PipeShaftRight);
						case 'a':
							return Block.Create(BlockType.HillLeft);
						case 'b':
							return Block.Create(BlockType.HillRight);
						case 'c':
							return Block.Create(BlockType.HillSolid);
						case 'd':
							return Block.Create(BlockType.HillSpots);
						case 'e':
							return Block.Create(BlockType.HillTop);
						case 'f':
							return Block.Create(BlockType.ShrubberyLeft);
						case 'g':
							return Block.Create(BlockType.ShrubberyCenter);
						case 'h':
							return Block.Create(BlockType.ShrubberyRight);
						case 'i':
							return Block.Create(BlockType.CloudTopLeft);
						case 'j':
							return Block.Create(BlockType.CloudTopCenter);
						case 'k':
							return Block.Create(BlockType.CloudTopRight);
						case 'l':
							return Block.Create(BlockType.CloudBottomLeft);
						case 'm':
							return Block.Create(BlockType.CloudBottomCenter);
						case 'n':
							return Block.Create(BlockType.CloudBottomRight);
						default:
							return null;
					}
				}
			}
			return null;
		}
		private int width = -1;

		public int Width
		{
			get
			{
				if (this.width < 0)
				{
					foreach (string row in this.rows)
					{
						this.width = Math.Max(width, row.Length);
					}
				}
				return this.width;
			}
		}

		public Brush Background
		{
			get
			{
				return background[this.level];
			}
		}

		public Mario DefaultMario
		{
			get
			{
				return defaultMario[this.level].Clone;
			}
		}

		private static Dictionary<string, Mario> defaultMario = new Dictionary<string, Mario>()
		{
			{ "1-1", new Mario() { OnGround=true, X=120, Y=600 } }
		};

		public Block[,] BlockGrid
		{
			get
			{
				Block[,] blocks = new Block[this.Width, 15];

				for (int x = 0; x < this.Width; ++x)
				{
					for (int y = 0; y < 15; ++y)
					{
						blocks[x, y] = this.GetBlock(x, y);
					}
				}

				return blocks;
			}
		}

		private static string createLevel(params string[] rows) {
			string output = "";
			for (int i = 0; i < rows.Length; ++i)
			{
				if (i > 0) output += "\n";
				output += rows[i];
			}
			return output;
		}

		private static readonly Dictionary<string, string> levels = new Dictionary<string, string>() {
			{"1-1", createLevel(
				"                                                                                                                                                                                                                                  ",
				"                                                                                                                                                                                                                                  ",
				"                   ijk              ijjk                           ijk              ijjk                           ijk              ijjk                           ijk              ijjk                                          ",
				"        ijk        lmn     ijjjk    lmmn                ijk        lmn     ijjjk    lmmn               ijk         lmn     ijjjk    lmmn                ijk        lmn     ijjjk    lmmn                ijk                       ",
				"        lmn                lmmmn                        lmn                lmmmn                       lmn                 lmmmn                        lmn                lmmmn                        lmn                       ",
				"                      2                                                         22222222   2223              3           222    2332                                                        44                                    ",
				"                                                                                                                                                                                           444                                    ",
				"                                                                                                                                                                                          4444                                    ",
				"                                                                4                                                                                                                        44444             222                    ",
				"                3   23232                     56         56                  232              2     22    3  3  3     2          22      4  4          44  4            2232            444444             222                    ",
				"  e                                   56      78  e      78                                       e                                     44  44    e   444  44                          4444444    e       22222                   ",
				" adb             e          56        78      78 adb     78      e                               adb             e                     444  444  adb 4444  444   e 56              56 44444444   adb      22 22  e                ",
				"adcdb      fggghadb    fgh  78        78 fggh 78adcdb    78fggghadb    fgh               fggh   adcdb      fggghadb    fgh            4444gg4444adcd44444  4444hadb78  fgh         78444444444  adcdb 4   22 22hadb               ",
				"111111111111111111111111111111111111111111111111111111111111111111111  111111111111111   1111111111111111111111111111111111111111111111111111111111111111  11111111111111111111111111111111111111111111111111111111111111111111111",
				"111111111111111111111111111111111111111111111111111111111111111111111  111111111111111   1111111111111111111111111111111111111111111111111111111111111111  11111111111111111111111111111111111111111111111111111111111111111111111") }
		};

		/*
			1	ground
			2	brick
			3	[?]
			4	block
			5	pipe rim left
			6	pipe rim right
			7	pipe shaft left
			8	pipe shaft right
			a	Hill left
			b	Hill right
			c	Hill solid
			d	Hill specs
			e	Hill top
			f	Shrubbery left
			g	Shrubbery center
			h	Shrubbery right
			i	Cloud top left
			j	Cloud top center
			k	Cloud top right
			l	Cloud bottom left
			m	Cloud bottom center
			n	Cloud bottom right
		 */

		private static readonly Brush SkyColor = new SolidColorBrush(Color.FromArgb(255, 108, 144, 255));
		private static readonly Brush Black = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));


		private static readonly Dictionary<string, Brush> background = new Dictionary<string, Brush>() {
			{"1-1", SkyColor },
			{"1-2", Black },
			{"1-3", SkyColor },
			{"1-4", Black }
		};
	}
}

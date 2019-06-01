using System;
using System.Net;
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
	public enum BlockType {
		Ground,
		Brick,
		Block,
		Question,
		PipeRimLeft,
		PipeRimRight,
		PipeShaftLeft,
		PipeShaftRight,
		ShrubberyLeft,
		ShrubberyCenter,
		ShrubberyRight,
		HillLeft,
		HillSolid,
		HillRight,
		HillSpots,
		HillTop,
		CloudTopLeft,
		CloudTopCenter,
		CloudTopRight,
		CloudBottomLeft,
		CloudBottomCenter,
		CloudBottomRight
	}

	public class Block
	{
		private static readonly Dictionary<BlockType, Block> Prototypes = new Dictionary<BlockType, Block>()
		{
			{ BlockType.Block, new Block(BlockType.Block, false, ImageLibrary.Get("Tile_Block")) },
			{ BlockType.Brick, new Block(BlockType.Brick, false, ImageLibrary.Get("Tile_Brick")) },
			{ BlockType.Ground, new Block(BlockType.Ground, false, ImageLibrary.Get("Tile_Ground")) },
			{ BlockType.Question, new Block(BlockType.Question, false, ImageLibrary.Get("Tile_Question")) },
			{ BlockType.PipeRimLeft, new Block(BlockType.PipeRimLeft, false, ImageLibrary.Get("Tile_Pipe_Rim_Left")) },
			{ BlockType.PipeRimRight, new Block(BlockType.PipeRimRight, false, ImageLibrary.Get("Tile_Pipe_Rim_Right")) },
			{ BlockType.PipeShaftLeft, new Block(BlockType.PipeShaftLeft, false, ImageLibrary.Get("Tile_Pipe_Shaft_Left")) },
			{ BlockType.PipeShaftRight, new Block(BlockType.PipeShaftRight, false, ImageLibrary.Get("Tile_Pipe_Shaft_Right")) },
			{ BlockType.ShrubberyLeft, new Block(BlockType.ShrubberyLeft, true, ImageLibrary.Get("Shrubbery_Left")) },
			{ BlockType.ShrubberyCenter, new Block(BlockType.ShrubberyCenter, true, ImageLibrary.Get("Shrubbery_Center")) },
			{ BlockType.ShrubberyRight, new Block(BlockType.ShrubberyRight, true, ImageLibrary.Get("Shrubbery_Right")) },
			{ BlockType.HillLeft, new Block(BlockType.HillLeft, true, ImageLibrary.Get("Hill_Left")) },
			{ BlockType.HillSolid, new Block(BlockType.HillSolid, true, ImageLibrary.Get("Hill_Solid")) },
			{ BlockType.HillRight, new Block(BlockType.HillRight, true, ImageLibrary.Get("Hill_Right")) },
			{ BlockType.HillSpots, new Block(BlockType.HillSpots, true, ImageLibrary.Get("Hill_Spots")) },
			{ BlockType.HillTop, new Block(BlockType.HillTop, true, ImageLibrary.Get("Hill_Top")) },
			{ BlockType.CloudTopLeft, new Block(BlockType.CloudTopLeft, true, ImageLibrary.Get("Cloud_Top_Left")) },
			{ BlockType.CloudTopCenter, new Block(BlockType.CloudTopCenter, true, ImageLibrary.Get("Cloud_Top_Center")) },
			{ BlockType.CloudTopRight, new Block(BlockType.CloudTopRight, true, ImageLibrary.Get("Cloud_Top_Right")) },
			{ BlockType.CloudBottomLeft, new Block(BlockType.CloudBottomLeft, true, ImageLibrary.Get("Cloud_Bottom_Left")) },
			{ BlockType.CloudBottomCenter, new Block(BlockType.CloudBottomCenter, true, ImageLibrary.Get("Cloud_Bottom_Center")) },
			{ BlockType.CloudBottomRight, new Block(BlockType.CloudBottomRight, true, ImageLibrary.Get("Cloud_Bottom_Right")) }
		};

		public static Block Create(BlockType type)
		{
			return Block.Prototypes[type];
		}

		private bool isPassable;
		private Brush image;
		private BlockType type;

		private Block(BlockType type, bool isPassable, Brush image)
		{
			this.isPassable = isPassable;
			this.image = image;
			this.type = type;
		}

		public bool IsPassable { get { return this.isPassable; } }
		public Brush Image { get { return this.image; } }
		public BlockType Type { get { return this.type; } }
	}
}

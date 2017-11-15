using System;


public class Tile
{
	private int x;
	private int y;
	private int isBlock;

	public Tile (int size, int height, int isFreeTile)
	{
		x = size;
		y = height;
		isBlock = isFreeTile;
	}

	public int GetSize(){
		return x;
	}

	public int GetHeight(){
		return y;
	}
	public int IsWalkable(){
		return isBlock;
	}
}


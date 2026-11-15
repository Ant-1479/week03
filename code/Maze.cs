/// <summary>
/// Defines a maze using a dictionary. The dictionary is provided by the
/// user when the Maze object is created. The dictionary will contain the
/// following mapping:
///
/// (x,y) : [left, right, up, down]
///
/// 'x' and 'y' are integers and represent locations in the maze.
/// 'left', 'right', 'up', and 'down' are boolean and represent valid directions.
/// y=1 is the top row, so moving up decreases y.
/// </summary>
public class Maze
{
    private readonly Dictionary<ValueTuple<int, int>, bool[]> _mazeMap;
    private int _currX = 1;
    private int _currY = 1;

    public Maze(Dictionary<ValueTuple<int, int>, bool[]> mazeMap)
    {
        _mazeMap = mazeMap;
    }

    /// <summary>Check to see if you can move left. If you can, update location; otherwise throw.</summary>
    public void MoveLeft()
    {
        if (!_mazeMap[(_currX, _currY)][0])
            throw new InvalidOperationException("Can't go left!");
        _currX--;
    }

    /// <summary>Check to see if you can move right. If you can, update location; otherwise throw.</summary>
    public void MoveRight()
    {
        if (!_mazeMap[(_currX, _currY)][1])
            throw new InvalidOperationException("Can't go right!");
        _currX++;
    }

    /// <summary>Check to see if you can move up. If you can, update location; otherwise throw.</summary>
    public void MoveUp()
    {
        if (!_mazeMap[(_currX, _currY)][2])
            throw new InvalidOperationException("Can't go up!");
        _currY--;
    }

    /// <summary>Check to see if you can move down. If you can, update location; otherwise throw.</summary>
    public void MoveDown()
    {
        if (!_mazeMap[(_currX, _currY)][3])
            throw new InvalidOperationException("Can't go down!");
        _currY++;
    }

    public void ShowStatus()
    {
        Console.WriteLine($"Current location (x={_currX}, y={_currY})");
    }
}
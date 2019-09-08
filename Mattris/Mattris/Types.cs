
namespace Mattris
{
    public enum BlockTypes
    {
        Empty, 
        Static,
        Moving,
        Pivot
    }

    public enum BlockColors
    {
        Red = 0, Blue = 1, Green = 2, Orange = 3, Purple = 4
    }

    public enum GameState
    {
        FirstLoaded,
        Paused,
        InProgress,
        GameOver
    }

    public enum PieceShape
    {
        I = 0, Square = 1, L = 2, J = 3, Z = 4, S = 5, T = 6
    }

    public enum Direction
    {
        Down, Left, Right
    }
}

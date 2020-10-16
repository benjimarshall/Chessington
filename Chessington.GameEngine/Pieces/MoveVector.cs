using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessington.GameEngine.Pieces
{
    class MoveVector
    {
        public readonly int RowChange;
        public readonly int ColChange;

        public MoveVector(int rowChange, int colChange)
        {
            RowChange = rowChange;
            ColChange = colChange;
        }

        public static IEnumerable<Square> FindAvailableMovesFromVectors(IEnumerable<MoveVector> moves, Board board, Square location)
        {
            var possibleMoves = moves.Select(vector =>
                new Square(
                    location.Row + vector.RowChange,
                    location.Col + vector.ColChange
                )
            );

            var possibleMovesInBoard = RemoveInvalidSquares(possibleMoves);

            // Remove current location
            var availableLocations = possibleMovesInBoard.Where(square => square != location);

            return availableLocations;
        }

        public IEnumerable<Square> GetMovesInDirection(Board board, Square start)
        {
            var availableMoves = new List<Square>();
            var player = board.CurrentPlayer;

            for (var i = 1; i < 8; i++)
            {
                var square = new Square(start.Row + RowChange * i, start.Col + ColChange * i);

                // If square doesn't exist, stop moving that way
                if (!Board.SquareIsOnBoard(square))
                {
                    break;
                }
                var occupier = board.GetPiece(square);

                // If we can move onto that square, add it to list
                if (occupier == null || player != occupier.Player)
                {
                    availableMoves.Add(square);
                }

                // If that square was occupied, then we won't be able to move past it
                if (occupier != null)
                {
                    break;
                }
            }

            return availableMoves;
        }

        private static IEnumerable<Square> RemoveInvalidSquares(IEnumerable<Square> squares)
        {
            return squares.Where(square =>
                0 <= square.Col && square.Col <= 7 && 0 <= square.Row && square.Row <= 7
            );
        }
    }
}

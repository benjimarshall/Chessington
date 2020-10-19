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

        private static IEnumerable<Square> RemoveInvalidSquares(IEnumerable<Square> squares)
        {
            return squares.Where(square =>
                0 <= square.Col && square.Col <= 7 && 0 <= square.Row && square.Row <= 7
            );
        }
    }
}

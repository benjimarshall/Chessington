using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Knight : Piece
    {
        public Knight(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);

            var oneMinusOne = new List<int>(new[] { 1, -1 });
            var twoMinusTwo = new List<int>(new[] { 2, -2 });

            // Make set of possible jumps: (+-2, +- 1) u (+-1, +-2)
            var colChangeJumps = oneMinusOne.SelectMany(
                x => twoMinusTwo,
                (rowChange, colChange) => new MoveVector(rowChange, colChange)
            );
            var rowChangeJumps = twoMinusTwo.SelectMany(
                x => oneMinusOne,
                (rowChange, colChange) => new MoveVector(rowChange, colChange)
            );
            var jumps = rowChangeJumps.Union(colChangeJumps);

            return MoveVector.FindAvailableMovesFromVectors(jumps, board, currentSquare);
        }
    }
}
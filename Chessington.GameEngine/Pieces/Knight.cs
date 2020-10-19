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
            var jumps = new List<MoveVector>(new []
            {
                new MoveVector(-2, -1),
                new MoveVector(-2, 1),
                new MoveVector(2, -1),
                new MoveVector(2, 1),
                new MoveVector(-1, -2),
                new MoveVector(-1, 2),
                new MoveVector(1, -2),
                new MoveVector(1, 2),
            });

            return MoveVector.FindAvailableMovesFromVectors(jumps, board, currentSquare);
        }
    }
}
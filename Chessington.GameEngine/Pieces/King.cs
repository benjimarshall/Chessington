using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class King : Piece
    {
        public King(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);

            var minusOneToOne = new List<int>(new[] { -1, 0, 1 });

            var moves = new List<MoveVector>(new[]
            {
                new MoveVector(-1, -1),
                new MoveVector(-1, 0),
                new MoveVector(-1, 1),
                new MoveVector(0, -1),
                new MoveVector(0, 1),
                new MoveVector(1, -1),
                new MoveVector(1, 0),
                new MoveVector(1, 1),
            });

            return MoveVector.FindAvailableMovesFromVectors(moves, board, currentSquare);
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public class Pawn : Piece
    {
        private bool hasMoved = false;

        public Pawn(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            var location = board.FindPiece(this);

            var availableLocations = new List<Square>();

            var direction = Player == Player.Black ? 1 : -1;
            var oneSquareAhead = new Square(location.Row + direction * 1, location.Col);
            var twoSquaresAhead = new Square(location.Row + direction * 2, location.Col);

            // Pawn can't move at all if a piece is directly in front
            if (board.SquareIsOccupied(oneSquareAhead))
            {
                return availableLocations;
            }

            // Normal advancing
            availableLocations.Add(oneSquareAhead);

            // Two places on first move, if not obstructed
            if (!hasMoved  && !board.SquareIsOccupied(twoSquaresAhead))
            {
                availableLocations.Add(twoSquaresAhead);
            }

            return availableLocations;
        }

        public new void MoveTo(Board board, Square newSquare)
        {
            base.MoveTo(board, newSquare);
            hasMoved = true;
        }
    }
}
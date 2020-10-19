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

            // Pawn can't move at all if at end of the board, or if a piece is directly in front
            if (location.Row == 0 && Player == Player.White
                || location.Row == 7 && Player == Player.Black
                || board.SquareIsOccupied(oneSquareAhead))
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

            // Pawns can take pieces diagonally
            var diagonalLeft = new Square(location.Row + direction*1, location.Col - 1);
            if (diagonalLeft.Col >= 0 && board.SquareIsOccupied(diagonalLeft) &&
                board.GetPiece(diagonalLeft).Player != Player)
            {
                availableLocations.Add(diagonalLeft);
            }

            var diagonalRight = new Square(location.Row + direction * 1, location.Col + 1);
            if (diagonalRight.Col <= 7 && board.SquareIsOccupied(diagonalRight) &&
                board.GetPiece(diagonalRight).Player != Player)
            {
                availableLocations.Add(diagonalRight);
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
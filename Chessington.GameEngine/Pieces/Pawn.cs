using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    enum HoriztonalDirection : int
    {
        Left = -1,
        Right = 1
    }

    public class Pawn : Piece
    {
        private bool hasMoved = false;
        private int verticalDirection => Player == Player. Black ? 1 : -1;

        public Pawn(Player player)
            : base(player) { }

        public override IEnumerable<Square> GetAvailableMoves(Board board)
        {
            var location = board.FindPiece(this);

            var availableLocations = new List<Square>();

            var oneSquareAhead = new Square(location.Row + verticalDirection * 1, location.Col);
            var twoSquaresAhead = new Square(location.Row + verticalDirection * 2, location.Col);

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
            var diagonalLeft = GetDiagonalMove(board, location, HoriztonalDirection.Left);
            if (diagonalLeft != null) availableLocations.Add(diagonalLeft.Value);


            var diagonalRight = GetDiagonalMove(board, location, HoriztonalDirection.Right);
            if (diagonalRight != null) availableLocations.Add(diagonalRight.Value);

            return availableLocations;
        }

        private Square? GetDiagonalMove(Board board, Square location, HoriztonalDirection horizontalDirection)
        {
            var diagonal = new Square(
                location.Row + verticalDirection,
                location.Col + (int)horizontalDirection
            );

            if (Board.SquareIsOnBoard(diagonal)
                && board.SquareIsOccupied(diagonal)
                && board.GetPiece(diagonal).Player != Player)
            {
                return diagonal;
            }
            else
            {
                return null;
            }
        }

        public new void MoveTo(Board board, Square newSquare)
        {
            base.MoveTo(board, newSquare);
            hasMoved = true;
        }
    }
}
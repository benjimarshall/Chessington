using System;
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
        private bool hasJustDoubleMoved = false;

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

            // Pawns can take pieces diagonally, and en passant
            return availableLocations
                .Concat(GetDiagonalMoves(board, location, HoriztonalDirection.Left))
                .Concat(GetDiagonalMoves(board, location, HoriztonalDirection.Right));
        }

        private IEnumerable<Square> GetDiagonalMoves(Board board, Square location, HoriztonalDirection horizontalDirection)
        {
            var availableLocations = new List<Square>();

            var diagonal = new Square(
                location.Row + verticalDirection,
                location.Col + (int)horizontalDirection
            );

            if (Board.SquareIsOnBoard(diagonal)
                   && board.SquareIsOccupied(diagonal)
                   && board.GetPiece(diagonal).Player != Player)
            {
                availableLocations.Add(diagonal);
            }

            // En passant
            var side = Square.At(location.Row, location.Col + (int)horizontalDirection);
            if (Board.SquareIsOnBoard(side)
                && !board.SquareIsOccupied(diagonal)
                && board.SquareIsOccupied(side)
                && board.GetPiece(side).CanBeTakenEnPassant(Player))
            {
                availableLocations.Add(diagonal);
            }

            return availableLocations;
        }

        public override void MoveTo(Board board, Square newSquare)
        {
            var currentSquare = board.FindPiece(this);
            hasJustDoubleMoved = Math.Abs(currentSquare.Row - newSquare.Row) == 2;

            // En passant
            var squareBehind = new Square(newSquare.Row - verticalDirection, newSquare.Col);
            if (currentSquare.Col != newSquare.Col
                && board.GetPiece(newSquare) == null
                && board.SquareIsOccupied(squareBehind))
            {
                board.CapturePieceOnSquare(squareBehind);
            }

            base.MoveTo(board, newSquare);
            hasMoved = true;
        }

        public override bool CanBeTakenEnPassant(Player player)
        {
            return hasJustDoubleMoved && player != Player;
        }
    }
}
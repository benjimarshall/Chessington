using System;
using System.Collections.Generic;
using System.Linq;

namespace Chessington.GameEngine.Pieces
{
    public abstract class Piece
    {
        protected Piece(Player player)
        {
            Player = player;
        }

        public Player Player { get; private set; }

        public abstract IEnumerable<Square> GetAvailableMoves(Board board);

        public virtual void MoveTo(Board board, Square newSquare)
        {
            var currentSquare = board.FindPiece(this);
            board.MovePiece(currentSquare, newSquare);
        }

        protected IEnumerable<Square> GetAvailableLateralMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);

            var availableMoves = new List<Square>();

            var moveVectors = new[]
            {
                new MoveVector(0, 1),
                new MoveVector(0, -1),
                new MoveVector(1, 0),
                new MoveVector(-1, 0)
            };

            foreach (var moveVector in moveVectors)
            {
                availableMoves.AddRange(moveVector.GetMovesInDirection(board, currentSquare));
            }

            return availableMoves;
        }

        protected IEnumerable<Square> GetAvailableDiagonalMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);

            var availableMoves = new List<Square>();

            var moveVectors = new[]
            {
                new MoveVector(1, 1),
                new MoveVector(-1, -1),
                new MoveVector(1, -1),
                new MoveVector(-1, 1)
            };

            foreach (var moveVector in moveVectors)
            {
                availableMoves.AddRange(moveVector.GetMovesInDirection(board, currentSquare));
            }

            return availableMoves;
        }
    }
}

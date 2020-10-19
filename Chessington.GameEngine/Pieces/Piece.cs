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

        public void MoveTo(Board board, Square newSquare)
        {
            var currentSquare = board.FindPiece(this);
            board.MovePiece(currentSquare, newSquare);
        }

        protected IEnumerable<Square> GetAvailableLateralMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);

            var zeroes = Enumerable.Repeat(0, 15);
            var minusSevenToSeven = Enumerable.Range(-7, 15);

            var xChangedMoveVectors = minusSevenToSeven.Select(x => new MoveVector(x, 0));
            var yChangedMoveVectors = minusSevenToSeven.Select(y => new MoveVector(0, y));
            var lateralMoveVectors = xChangedMoveVectors.Concat(yChangedMoveVectors);

            return MoveVector.FindAvailableMovesFromVectors(lateralMoveVectors, board, currentSquare);
        }

        protected IEnumerable<Square> GetAvailableDiagonalMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);

            // Produce a set of diagonal move vectors, from 1 to 7 places diagonally in each direction
            var oneToSeven = Enumerable.Range(1, 7);
            var bothPositiveDiagonal = oneToSeven.Select(x => new MoveVector(x, x));
            var xNegativeDiagonal = oneToSeven.Select(x => new MoveVector(-x, x));
            var yNegativeDiagonal = oneToSeven.Select(x => new MoveVector(x, -x));
            var bothNegativeDiagonal = oneToSeven.Select(x => new MoveVector(-x, -x));
            var allDiagonalMoves = bothPositiveDiagonal
                .Concat(xNegativeDiagonal)
                .Concat(yNegativeDiagonal)
                .Concat(bothNegativeDiagonal);

            return MoveVector.FindAvailableMovesFromVectors(allDiagonalMoves, board, currentSquare);
        }
    }
}

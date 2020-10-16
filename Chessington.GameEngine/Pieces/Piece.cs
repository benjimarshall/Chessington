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

            var moveVectors = zeroes.Zip(minusSevenToSeven, (x, y) => new MoveVector(x,y));
            moveVectors = moveVectors.Union(minusSevenToSeven.Zip(zeroes, (x, y) => new MoveVector(x, y)));

            return MoveVector.FindAvailableMovesFromVectors(moveVectors, board, currentSquare);
        }

        protected IEnumerable<Square> GetAvailableDiagonalMoves(Board board)
        {
            var currentSquare = board.FindPiece(this);

            // Produce a zipped set of diagonal directions (South West, South East, North West, North East)
            var diagonalDirections = new List<MoveVector>(new[]
            {
                new MoveVector(-1, -1),
                new MoveVector(-1, 1),
                new MoveVector(1, -1),
                new MoveVector(1, 1)
            });

            // Send diagonal lines out 8 squares in each diagonal direction
            var moveVectors = diagonalDirections.Select(vector =>
            {
                return Enumerable.Range(1, 7).Select(i => new MoveVector(
                    vector.RowChange * i,
                    vector.ColChange * i)
                );
            });

            // Flatten from four lists of lists of squares to one list
            var moveVectorsFlattened = moveVectors.SelectMany(x => x).ToList();

            return MoveVector.FindAvailableMovesFromVectors(moveVectorsFlattened, board, currentSquare);
        }
    }
}

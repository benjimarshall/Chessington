using System.Linq;
using Chessington.GameEngine.Pieces;
using FluentAssertions;
using NUnit.Framework;

namespace Chessington.GameEngine.Tests.Pieces
{
    [TestFixture]
    public class PawnTests
    {
        [Test]
        public void WhitePawns_CanMoveOneSquareUp()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(7, 0), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().Contain(Square.At(6, 0));
        }

        [Test]
        public void BlackPawns_CanMoveOneSquareDown()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(1, 0), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().Contain(Square.At(2, 0));
        }

        [Test]
        public void WhitePawns_WhichHaveNeverMoved_CanMoveTwoSquareUp()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(7, 5), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().Contain(Square.At(5, 5));
        }

        [Test]
        public void BlackPawns_WhichHaveNeverMoved_CanMoveTwoSquareUp()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(1, 3), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().Contain(Square.At(3, 3));
        }

        [Test]
        public void WhitePawns_WhichHaveAlreadyMoved_CanOnlyMoveOneSquare()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(7, 2), pawn);

            pawn.MoveTo(board, Square.At(6, 2));
            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().HaveCount(1);
            moves.Should().Contain(square => square.Equals(Square.At(5, 2)));
        }

        [Test]
        public void BlackPawns_WhichHaveAlreadyMoved_CanOnlyMoveOneSquare()
        {
            var board = new Board(Player.Black);
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(5, 2), pawn);

            pawn.MoveTo(board, Square.At(6, 2));
            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().HaveCount(1);
            moves.Should().Contain(square => square.Equals(Square.At(7, 2)));
        }

        [Test]
        public void Pawns_CannotMove_IfThereIsAPieceInFront()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            var blockingPiece = new Rook(Player.White);
            board.AddPiece(Square.At(1, 3), pawn);
            board.AddPiece(Square.At(2, 3), blockingPiece);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().BeEmpty();
        }

        [Test]
        public void Pawns_CannotMoveTwoSquares_IfThereIsAPieceTwoSquaresInFront()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            var blockingPiece = new Rook(Player.White);
            board.AddPiece(Square.At(1, 3), pawn);
            board.AddPiece(Square.At(3, 3), blockingPiece);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().NotContain(Square.At(3, 3));
        }

        [Test]
        public void WhitePawns_CannotMove_AtTheTopOfTheBoard()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(0, 3), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().BeEmpty();
        }

        [Test]
        public void BlackPawns_CannotMove_AtTheBottomOfTheBoard()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(7, 3), pawn);

            var moves = pawn.GetAvailableMoves(board);

            moves.Should().BeEmpty();
        }

        [Test]
        public void BlackPawns_CanMoveDiagonally_IfThereIsAPieceToTake()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            var firstTarget = new Pawn(Player.White);
            var secondTarget = new Pawn(Player.White);
            board.AddPiece(Square.At(5, 3), pawn);
            board.AddPiece(Square.At(6, 4), firstTarget);
            board.AddPiece(Square.At(6, 2), secondTarget);

            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().Contain(Square.At(6, 2));
            moves.Should().Contain(Square.At(6, 4));
        }

        [Test]
        public void WhitePawns_CanMoveDiagonally_IfThereIsAPieceToTake()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            var firstTarget = new Pawn(Player.Black);
            var secondTarget = new Pawn(Player.Black);
            board.AddPiece(Square.At(7, 3), pawn);
            board.AddPiece(Square.At(6, 4), firstTarget);
            board.AddPiece(Square.At(6, 2), secondTarget);

            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().Contain(Square.At(6, 2));
            moves.Should().Contain(Square.At(6, 4));
        }

        [Test]
        public void BlackPawns_CannotMoveDiagonally_IfThereIsNoPieceToTake()
        {
            var board = new Board();
            var pawn = new Pawn(Player.Black);
            board.AddPiece(Square.At(5, 3), pawn);

            var friendlyPiece = new Pawn(Player.Black);
            board.AddPiece(Square.At(6, 2), friendlyPiece);

            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().NotContain(Square.At(6, 2));
            moves.Should().NotContain(Square.At(6, 4));
        }

        [Test]
        public void WhitePawns_CannotMoveDiagonally_IfThereIsNoPieceToTake()
        {
            var board = new Board();
            var pawn = new Pawn(Player.White);
            board.AddPiece(Square.At(7, 3), pawn);

            var friendlyPiece = new Pawn(Player.White);
            board.AddPiece(Square.At(6, 2), friendlyPiece);

            var moves = pawn.GetAvailableMoves(board).ToList();

            moves.Should().NotContain(Square.At(6, 2));
            moves.Should().NotContain(Square.At(6, 4));
        }

        [Test]
        public void WhitePawns_CanEnPassant()
        {
            var board = new Board(Player.Black);
            var whitePawn = new Pawn(Player.White);
            board.AddPiece(new Square(3, 4), whitePawn);

            var blackPawn = new Pawn(Player.Black);
            board.AddPiece(new Square(1, 5), blackPawn);

            blackPawn.MoveTo(board, new Square(3, 5));

            whitePawn.GetAvailableMoves(board).Should().Contain(
                new Square(2, 5),
                "White pawn should be able to en passant"
            );

            whitePawn.MoveTo(board, new Square(2, 5));

            board.GetPiece(new Square(3, 5)).Should().BeNull("Black pawn should be taken during en passant");
        }

        [Test]
        public void BlackPawns_CanEnPassant()
        {
            var board = new Board(Player.White);
            var blackPawn = new Pawn(Player.Black);
            board.AddPiece(new Square(4, 6), blackPawn);

            var whitePawn = new Pawn(Player.White);
            board.AddPiece(new Square(6, 7), whitePawn);

            whitePawn.MoveTo(board, new Square(4, 7));

            blackPawn.GetAvailableMoves(board).Should().Contain(
                new Square(5, 7),
                "Black pawn should be able to en passant"
            );

            blackPawn.MoveTo(board, new Square(5, 7));

            board.GetPiece(new Square(4, 7)).Should().BeNull("White pawn should be taken during en passant");
        }

        [Test]
        public void WhitePawns_CannotEnPassant_IfNotAfterDoubleMove()
        {
            var board = new Board(Player.Black);
            var whitePawn = new Pawn(Player.White);
            board.AddPiece(new Square(4, 4), whitePawn);

            var blackPawn = new Pawn(Player.Black);
            board.AddPiece(new Square(2, 5), blackPawn);

            blackPawn.MoveTo(board, new Square(3, 5));

            whitePawn.GetAvailableMoves(board).Should().NotContain(
                new Square(2, 5),
                "White pawn should not be able to en passant if black pawn hasn't double moved"
            );
        }

        [Test]
        public void BlackPawns_CannotEnPassant_IfNotAfterDoubleMove()
        {
            var board = new Board(Player.White);
            var blackPawn = new Pawn(Player.Black);
            board.AddPiece(new Square(4, 6), blackPawn);

            var whitePawn = new Pawn(Player.White);
            board.AddPiece(new Square(5, 7), whitePawn);

            whitePawn.MoveTo(board, new Square(4, 7));

            blackPawn.GetAvailableMoves(board).Should().NotContain(
                new Square(5, 7),
                "Black pawn should not be able to en passant if white pawn hasn't double moved"
            );
        }
    }
}
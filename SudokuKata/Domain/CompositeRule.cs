namespace SudokuKata
{
    class CompositeRule : IBoardChange
    {
        public CompositeRule(IBoardChange first, IBoardChange then)
        {
            First = first;
            Then = then;
        }

        private IBoardChange First { get; }
        private IBoardChange Then { get; }

        public (bool changeMade, bool stepChangeMade, int[] nextBoard) Apply(int[] candidateMasks, bool changeMade, bool stepChangeMade, int[] board)
        {
            (changeMade, stepChangeMade, board) = First.Apply(candidateMasks, changeMade, stepChangeMade, board);
            return Then.Apply(candidateMasks, changeMade, stepChangeMade, board);
        }
    }
}
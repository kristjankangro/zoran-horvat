namespace SudokuKata
{
    static class RuleExtensions
    {
        public static IBoardChange Then(this IBoardChange first, IBoardChange then)
            => new CompositeRule(first, then);
    }
}
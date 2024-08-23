using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuKata
{
    public class NNumbersInNCellsRule : IBoardChange
    {
        private Dictionary<int, int> _maskToOnesCount { get; }
        private List<IGrouping<int, CellGroup>> _cellGroups;

        public NNumbersInNCellsRule(Dictionary<int, int> maskToOnesCount, List<IGrouping<int, CellGroup>> cellGroups)
        {
            _maskToOnesCount = maskToOnesCount;
            _cellGroups = cellGroups;
        }

        
        public (bool changeMade, bool stepChangeMade, int[] nextBoard) Apply(int[] candidateMasks, bool changeMade, bool stepChangeMade, int[] board)
        {
                 #region Try to find groups of digits of size N which only appear in N cells within row/column/block
                    // When a set of N digits only appears in N cells within row/column/block, then no other digit can appear in the same set of cells
                    // All other candidates can then be removed from those cells

                    if (!changeMade && !stepChangeMade)
                    {
                        IEnumerable<int> masks =
                            _maskToOnesCount
                                .Where(tuple => tuple.Value > 1)
                                .Select(tuple => tuple.Key).ToList();

                        var groupsWithNMasks =
                            masks
                                .SelectMany(mask =>
                                    _cellGroups
                                        .Where(group => group.All(cell => board[cell.Index] == 0 || (mask & (1 << (board[cell.Index] - 1))) == 0))
                                        .Select(group => new
                                        {
                                            Mask = mask,
                                            Description = group.First().Description,
                                            Cells = group,
                                            CellsWithMask =
                                                group.Where(cell => board[cell.Index] == 0 && (candidateMasks[cell.Index] & mask) != 0).ToList(),
                                            CleanableCellsCount =
                                                group.Count(
                                                    cell => board[cell.Index] == 0 && 
                                                            (candidateMasks[cell.Index] & mask) != 0 &&
                                                            (candidateMasks[cell.Index] & ~mask) != 0)
                                        }))
                                .Where(group => group.CellsWithMask.Count() == _maskToOnesCount[group.Mask])
                                .ToList();

                        foreach (var groupWithNMasks in groupsWithNMasks)
                        {
                            int mask = groupWithNMasks.Mask;

                            if (groupWithNMasks.Cells
                                .Any(cell =>
                                    (candidateMasks[cell.Index] & mask) != 0 &&
                                    (candidateMasks[cell.Index] & ~mask) != 0))
                            {
                                StringBuilder message = new StringBuilder();
                                message.Append($"In {groupWithNMasks.Description} values ");

                                string separator = string.Empty;
                                int temp = mask;
                                int curValue = 1;
                                while (temp > 0)
                                {
                                    if ((temp & 1) > 0)
                                    {
                                        message.Append($"{separator}{curValue}");
                                        separator = ", ";
                                    }
                                    temp = temp >> 1;
                                    curValue += 1;
                                }

                                message.Append(" appear only in cells");
                                foreach (var cell in groupWithNMasks.CellsWithMask)
                                {
                                    message.Append($" ({cell.Row + 1}, {cell.Column + 1})");
                                }

                                message.Append(" and other values cannot appear in those cells.");

                                Console.WriteLine(message.ToString());
                            }

                            foreach (var cell in groupWithNMasks.CellsWithMask)
                            {
                                int maskToClear = candidateMasks[cell.Index] & ~groupWithNMasks.Mask;
                                if (maskToClear == 0)
                                    continue;

                                candidateMasks[cell.Index] &= groupWithNMasks.Mask;
                                stepChangeMade = true;

                                int valueToClear = 1;

                                string separator = string.Empty;
                                StringBuilder message = new StringBuilder();

                                while (maskToClear > 0)
                                {
                                    if ((maskToClear & 1) > 0)
                                    {
                                        message.Append($"{separator}{valueToClear}");
                                        separator = ", ";
                                    }
                                    maskToClear = maskToClear >> 1;
                                    valueToClear += 1;
                                }

                                message.Append($" cannot appear in cell ({cell.Row + 1}, {cell.Column + 1}).");
                                Console.WriteLine(message.ToString());

                            }
                        }
                    }

                    #endregion
            return (changeMade, stepChangeMade, board);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuKata
{
    public class TwoNumbersInTwoCellsRule : IBoardChange
    {
        private Dictionary<int, int> _maskToOnesCount { get; }
        private List<IGrouping<int, CellGroup>> _cellGroups;
        public TwoNumbersInTwoCellsRule(Dictionary<int, int> maskToOnesCount, List<IGrouping<int, CellGroup>> cellGroups)
        {
            _maskToOnesCount = maskToOnesCount;
            _cellGroups = cellGroups;
        }

        
        
        public (bool changeMade, bool stepChangeMade, int[] nextBoard) Apply(int[] candidateMasks, bool changeMade, bool stepChangeMade, int[] board)
        {
               #region Try to find pairs of digits in the same row/column/block and remove them from other colliding cells
                    if (!changeMade)
                    {
                        IEnumerable<int> twoDigitMasks =
                            candidateMasks.Where(mask => _maskToOnesCount[mask] == 2).Distinct().ToList();

                        var groups =
                            twoDigitMasks
                                .SelectMany(mask =>
                                    _cellGroups
                                        .Where(group => group.Count(tuple => candidateMasks[tuple.Index] == mask) == 2)
                                        .Where(group => group.Any(tuple => candidateMasks[tuple.Index] != mask && (candidateMasks[tuple.Index] & mask) > 0))
                                        .Select(group => new
                                        {
                                            Mask = mask,
                                            Discriminator = group.Key,
                                            Description = group.First().Description,
                                            Cells = group
                                        }))
                                .ToList();

                        if (groups.Any())
                        {
                            foreach (var group in groups)
                            {
                                var cells =
                                    group.Cells
                                        .Where(
                                            cell =>
                                                candidateMasks[cell.Index] != group.Mask &&
                                                (candidateMasks[cell.Index] & group.Mask) > 0)
                                        .ToList();

                                var maskCells =
                                    group.Cells
                                        .Where(cell => candidateMasks[cell.Index] == group.Mask)
                                        .ToArray();


                                if (cells.Any())
                                {
                                    int upper = 0;
                                    int lower = 0;
                                    int temp = group.Mask;

                                    int value = 1;
                                    while (temp > 0)
                                    {
                                        if ((temp & 1) > 0)
                                        {
                                            lower = upper;
                                            upper = value;
                                        }
                                        temp = temp >> 1;
                                        value += 1;
                                    }

                                    Console.WriteLine(
                                        $"Values {lower} and {upper} in {group.Description} are in cells ({maskCells[0].Row + 1}, {maskCells[0].Column + 1}) and ({maskCells[1].Row + 1}, {maskCells[1].Column + 1}).");

                                    foreach (var cell in cells)
                                    {
                                        int maskToRemove = candidateMasks[cell.Index] & group.Mask;
                                        List<int> valuesToRemove = new List<int>();
                                        int curValue = 1;
                                        while (maskToRemove > 0)
                                        {
                                            if ((maskToRemove & 1) > 0)
                                            {
                                                valuesToRemove.Add(curValue);
                                            }
                                            maskToRemove = maskToRemove >> 1;
                                            curValue += 1;
                                        }

                                        string valuesReport = string.Join(", ", valuesToRemove.ToArray());
                                        Console.WriteLine($"{valuesReport} cannot appear in ({cell.Row + 1}, {cell.Column + 1}).");

                                        candidateMasks[cell.Index] &= ~group.Mask;
                                        stepChangeMade = true;
                                    }
                                }
                            }
                        }

                    }
                    #endregion

                    return (changeMade, stepChangeMade, board);
        }

        
        
    }
}
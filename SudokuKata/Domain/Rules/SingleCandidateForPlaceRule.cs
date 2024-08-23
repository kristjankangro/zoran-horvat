using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuKata
{
    public class SingleCandidateForPlaceRule : IBoardChange
    {
        public SingleCandidateForPlaceRule(Random rng)
        {
            _rng = rng;
        }

        private Random _rng { get; }

        public (bool changeMade, bool stepChangeMade, int[] nextBoard) Apply(int[] candidateMasks, bool changeMade, bool stepChangeMade, int[] board)
        {
            if (!changeMade)
            {
                List<string> groupDescriptions = new List<string>();
                List<int> candidateRowIndices = new List<int>();
                List<int> candidateColIndices = new List<int>();
                List<int> candidates = new List<int>();

                for (int digit = 1; digit <= 9; digit++)
                {
                    int mask = 1 << (digit - 1);
                    for (int cellGroup = 0; cellGroup < 9; cellGroup++)
                    {
                        int rowNumberCount = 0;
                        int indexInRow = 0;

                        int colNumberCount = 0;
                        int indexInCol = 0;

                        int blockNumberCount = 0;
                        int indexInBlock = 0;

                        for (int indexInGroup = 0; indexInGroup < 9; indexInGroup++)
                        {
                            int rowStateIndex = 9 * cellGroup + indexInGroup;
                            int colStateIndex = 9 * indexInGroup + cellGroup;
                            int blockRowIndex = (cellGroup / 3) * 3 + indexInGroup / 3;
                            int blockColIndex = (cellGroup % 3) * 3 + indexInGroup % 3;
                            int blockStateIndex = blockRowIndex * 9 + blockColIndex;

                            if ((candidateMasks[rowStateIndex] & mask) != 0)
                            {
                                rowNumberCount += 1;
                                indexInRow = indexInGroup;
                            }

                            if ((candidateMasks[colStateIndex] & mask) != 0)
                            {
                                colNumberCount += 1;
                                indexInCol = indexInGroup;
                            }

                            if ((candidateMasks[blockStateIndex] & mask) != 0)
                            {
                                blockNumberCount += 1;
                                indexInBlock = indexInGroup;
                            }
                        }

                        if (rowNumberCount == 1)
                        {
                            groupDescriptions.Add($"Row #{cellGroup + 1}");
                            candidateRowIndices.Add(cellGroup);
                            candidateColIndices.Add(indexInRow);
                            candidates.Add(digit);
                        }

                        if (colNumberCount == 1)
                        {
                            groupDescriptions.Add($"Column #{cellGroup + 1}");
                            candidateRowIndices.Add(indexInCol);
                            candidateColIndices.Add(cellGroup);
                            candidates.Add(digit);
                        }

                        if (blockNumberCount == 1)
                        {
                            int blockRow = cellGroup / 3;
                            int blockCol = cellGroup % 3;

                            groupDescriptions.Add($"Block ({blockRow + 1}, {blockCol + 1})");
                            candidateRowIndices.Add(blockRow * 3 + indexInBlock / 3);
                            candidateColIndices.Add(blockCol * 3 + indexInBlock % 3);
                            candidates.Add(digit);
                        }
                    } // for (cellGroup = 0..8)
                } // for (digit = 1..9)

                if (candidates.Count > 0)
                {
                    int index = _rng.Next(candidates.Count);
                    string description = groupDescriptions.ElementAt(index);
                    int row = candidateRowIndices.ElementAt(index);
                    int col = candidateColIndices.ElementAt(index);
                    int digit = candidates.ElementAt(index);

                    string message = $"{description} can contain {digit} only at ({row + 1}, {col + 1}).";

                    int stateIndex = 9 * row + col;
                    board[stateIndex] = digit;
                    candidateMasks[stateIndex] = 0;

                    changeMade = true;

                    Console.WriteLine(message);
                }
            }


            return (changeMade, stepChangeMade, board);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuKata
{
    public class SingleCandidateChangeRule : IBoardChange
    {
        public SingleCandidateChangeRule(Random rng, Dictionary<int, int> maskToOnesCount, Dictionary<int, int> singleBitToIndex)
        {
            _rng = rng;
            _maskToOnesCount = maskToOnesCount;
            _singleBitToIndex = singleBitToIndex;
        }

        private Dictionary<int, int> _maskToOnesCount { get; }
        private Random _rng { get; }
        private Dictionary<int, int> _singleBitToIndex { get;  }

        public (bool changeMade, bool stepChangeMade, int[] nextBoard) Apply(int[] candidateMasks, bool changeMade, bool stepChangeMade, int[] board)
        {
            int[] singleCandidateIndices =
                candidateMasks
                    .Select((mask, index) => new
                    {
                        CandidatesCount = _maskToOnesCount[mask],
                        Index = index
                    })
                    .Where(tuple => tuple.CandidatesCount == 1)
                    .Select(tuple => tuple.Index)
                    .ToArray();

            if (singleCandidateIndices.Length > 0)
            {
                int pickSingleCandidateIndex = _rng.Next(singleCandidateIndices.Length);
                int singleCandidateIndex = singleCandidateIndices[pickSingleCandidateIndex];
                int candidateMask = candidateMasks[singleCandidateIndex];
                int candidate = _singleBitToIndex[candidateMask];

                int row = singleCandidateIndex / 9;
                int col = singleCandidateIndex % 9;


                board[singleCandidateIndex] = candidate + 1;
                candidateMasks[singleCandidateIndex] = 0;
                changeMade = true;

                Console.WriteLine("({0}, {1}) can only contain {2}.", row + 1, col + 1, candidate + 1);
            }

            throw new System.NotImplementedException();
        }
    }
}
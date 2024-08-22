using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuKata
{
    class Program
    {
        static void Play()
        {
            int[] solvedState = CreatePopulatedBoard();
            Presentation.PrintToConsole(solvedState, "\nFinal look of the solved board:");
            
            var initialState = CreateInitialBoard(solvedState);
            Presentation.PrintToConsole(initialState, "\nInitial look of the board to solve:");

            var state = initialState.ToArray();
            var rng = new Random();
            #region Prepare lookup structures that will be used in further execution
            Console.WriteLine();
            Console.WriteLine(new string('=', 80));
            Console.WriteLine();

            Dictionary<int, int> maskToOnesCount = new Dictionary<int, int>();
            maskToOnesCount[0] = 0;
            for (int i = 1; i < (1 << 9); i++)
            {
                int smaller = i >> 1;
                int increment = i & 1;
                maskToOnesCount[i] = maskToOnesCount[smaller] + increment;
            }

            Dictionary<int, int> singleBitToIndex = new Dictionary<int, int>();
            for (int i = 0; i < 9; i++)
                singleBitToIndex[1 << i] = i;

            int allOnes = (1 << 9) - 1;
            #endregion

            bool changeMade = true;
            while (changeMade)
            {
                changeMade = false;

                #region Calculate candidates for current state of the board
                int[] candidateMasks = new int[state.Length];

                for (int i = 0; i < state.Length; i++)
                    if (state[i] == 0)
                    {

                        int row = i / 9;
                        int col = i % 9;
                        int blockRow = row / 3;
                        int blockCol = col / 3;

                        int colidingNumbers = 0;
                        for (int j = 0; j < 9; j++)
                        {
                            int rowSiblingIndex = 9 * row + j;
                            int colSiblingIndex = 9 * j + col;
                            int blockSiblingIndex = 9 * (blockRow * 3 + j / 3) + blockCol * 3 + j % 3;

                            int rowSiblingMask = 1 << (state[rowSiblingIndex] - 1);
                            int colSiblingMask = 1 << (state[colSiblingIndex] - 1);
                            int blockSiblingMask = 1 << (state[blockSiblingIndex] - 1);

                            colidingNumbers = colidingNumbers | rowSiblingMask | colSiblingMask | blockSiblingMask;
                        }

                        candidateMasks[i] = allOnes & ~colidingNumbers;
                    }
                #endregion

                var cellGroups = BuildCellGroups(state);
                var stepChangeMade = true;
                while (stepChangeMade)
                {
                    (changeMade, stepChangeMade, state) = 
                        new SingleCandidateRule(rng, maskToOnesCount, singleBitToIndex)
                            .Then(new SingleCandidateForPlaceRule(rng))
                            .Then(new TwoNumbersInTwoCellsRule(maskToOnesCount, cellGroups))
                            .Then(new NNumbersInNCellsRule(maskToOnesCount, cellGroups))
                            .Apply(candidateMasks, changeMade, false, state);
                }

                #region Final attempt - look if the board has multiple solutions
                if (!changeMade)
                {
                    // This is the last chance to do something in this iteration:
                    // If this attempt fails, board will not be entirely solved.

                    // Try to see if there are pairs of values that can be exchanged arbitrarily
                    // This happens when board has more than one valid solution

                    Queue<int> candidateIndex1 = new Queue<int>();
                    Queue<int> candidateIndex2 = new Queue<int>();
                    Queue<int> candidateDigit1 = new Queue<int>();
                    Queue<int> candidateDigit2 = new Queue<int>();

                    for (int i = 0; i < candidateMasks.Length - 1; i++)
                    {
                        if (maskToOnesCount[candidateMasks[i]] == 2)
                        {
                            int row = i / 9;
                            int col = i % 9;
                            int blockIndex = 3 * (row / 3) + col / 3;

                            int temp = candidateMasks[i];
                            int lower = 0;
                            int upper = 0;
                            for (int digit = 1; temp > 0; digit++)
                            {
                                if ((temp & 1) != 0)
                                {
                                    lower = upper;
                                    upper = digit;
                                }
                                temp = temp >> 1;
                            }

                            for (int j = i + 1; j < candidateMasks.Length; j++)
                            {
                                if (candidateMasks[j] == candidateMasks[i])
                                {
                                    int row1 = j / 9;
                                    int col1 = j % 9;
                                    int blockIndex1 = 3 * (row1 / 3) + col1 / 3;

                                    if (row == row1 || col == col1 || blockIndex == blockIndex1)
                                    {
                                        candidateIndex1.Enqueue(i);
                                        candidateIndex2.Enqueue(j);
                                        candidateDigit1.Enqueue(lower);
                                        candidateDigit2.Enqueue(upper);
                                    }
                                }
                            }
                        }
                    }

                    // At this point we have the lists with pairs of cells that might pick one of two digits each
                    // Now we have to check whether that is really true - does the board have two solutions?

                    List<int> stateIndex1 = new List<int>();
                    List<int> stateIndex2 = new List<int>();
                    List<int> value1 = new List<int>();
                    List<int> value2 = new List<int>();

                    while (candidateIndex1.Any())
                    {
                        int index1 = candidateIndex1.Dequeue();
                        int index2 = candidateIndex2.Dequeue();
                        int digit1 = candidateDigit1.Dequeue();
                        int digit2 = candidateDigit2.Dequeue();

                        int[] alternateState = new int[solvedState.Length];
                        Array.Copy(state, alternateState, alternateState.Length);

                        if (solvedState[index1] == digit1)
                        {
                            alternateState[index1] = digit2;
                            alternateState[index2] = digit1;
                        }
                        else
                        {
                            alternateState[index1] = digit1;
                            alternateState[index2] = digit2;
                        }

                        // What follows below is a complete copy-paste of the solver which appears at the beginning of this method
                        // However, the algorithm couldn't be applied directly and it had to be modified.
                        // Implementation below assumes that the board might not have a solution.
                        var stateStack = new Stack<int[]>();
                        var rowIndexStack = new Stack<int>();
                        var colIndexStack = new Stack<int>();
                        var usedDigitsStack = new Stack<bool[]>();
                        var lastDigitStack = new Stack<int>();

                        var command = "expand";
                        while (command != "complete" && command != "fail")
                        {
                            if (command == "expand")
                            {
                                int[] currentState = new int[9 * 9];

                                if (stateStack.Any())
                                {
                                    Array.Copy(stateStack.Peek(), currentState, currentState.Length);
                                }
                                else
                                {
                                    Array.Copy(alternateState, currentState, currentState.Length);
                                }

                                int bestRow = -1;
                                int bestCol = -1;
                                bool[] bestUsedDigits = null;
                                int bestCandidatesCount = -1;
                                int bestRandomValue = -1;
                                bool containsUnsolvableCells = false;

                                for (int index = 0; index < currentState.Length; index++)
                                    if (currentState[index] == 0)
                                    {

                                        int row = index / 9;
                                        int col = index % 9;
                                        int blockRow = row / 3;
                                        int blockCol = col / 3;

                                        bool[] isDigitUsed = new bool[9];

                                        for (int i = 0; i < 9; i++)
                                        {
                                            int rowDigit = currentState[9 * i + col];
                                            if (rowDigit > 0)
                                                isDigitUsed[rowDigit - 1] = true;

                                            int colDigit = currentState[9 * row + i];
                                            if (colDigit > 0)
                                                isDigitUsed[colDigit - 1] = true;

                                            int blockDigit = currentState[(blockRow * 3 + i / 3) * 9 + (blockCol * 3 + i % 3)];
                                            if (blockDigit > 0)
                                                isDigitUsed[blockDigit - 1] = true;
                                        } // for (i = 0..8)

                                        int candidatesCount = isDigitUsed.Where(used => !used).Count();

                                        if (candidatesCount == 0)
                                        {
                                            containsUnsolvableCells = true;
                                            break;
                                        }

                                        int randomValue = rng.Next();

                                        if (bestCandidatesCount < 0 ||
                                            candidatesCount < bestCandidatesCount ||
                                            (candidatesCount == bestCandidatesCount && randomValue < bestRandomValue))
                                        {
                                            bestRow = row;
                                            bestCol = col;
                                            bestUsedDigits = isDigitUsed;
                                            bestCandidatesCount = candidatesCount;
                                            bestRandomValue = randomValue;
                                        }

                                    } // for (index = 0..81)

                                if (!containsUnsolvableCells)
                                {
                                    stateStack.Push(currentState);
                                    rowIndexStack.Push(bestRow);
                                    colIndexStack.Push(bestCol);
                                    usedDigitsStack.Push(bestUsedDigits);
                                    lastDigitStack.Push(0); // No digit was tried at this position
                                }

                                // Always try to move after expand
                                command = "move";

                            } // if (command == "expand")
                            else if (command == "collapse")
                            {
                                stateStack.Pop();
                                rowIndexStack.Pop();
                                colIndexStack.Pop();
                                usedDigitsStack.Pop();
                                lastDigitStack.Pop();

                                if (stateStack.Any())
                                    command = "move"; // Always try to move after collapse
                                else
                                    command = "fail";
                            }
                            else if (command == "move")
                            {

                                int rowToMove = rowIndexStack.Peek();
                                int colToMove = colIndexStack.Peek();
                                int digitToMove = lastDigitStack.Pop();

                                bool[] usedDigits = usedDigitsStack.Peek();
                                int[] currentState = stateStack.Peek();
                                int currentStateIndex = 9 * rowToMove + colToMove;

                                int movedToDigit = digitToMove + 1;
                                while (movedToDigit <= 9 && usedDigits[movedToDigit - 1])
                                    movedToDigit += 1;

                                if (digitToMove > 0)
                                {
                                    usedDigits[digitToMove - 1] = false;
                                    currentState[currentStateIndex] = 0;
                                }

                                if (movedToDigit <= 9)
                                {
                                    lastDigitStack.Push(movedToDigit);
                                    usedDigits[movedToDigit - 1] = true;
                                    currentState[currentStateIndex] = movedToDigit;

                                    if (currentState.Any(digit => digit == 0))
                                        command = "expand";
                                    else
                                        command = "complete";
                                }
                                else
                                {
                                    // No viable candidate was found at current position - pop it in the next iteration
                                    lastDigitStack.Push(0);
                                    command = "collapse";
                                }
                            } // if (command == "move")

                        } // while (command != "complete" && command != "fail")

                        if (command == "complete")
                        {   // Board was solved successfully even with two digits swapped
                            stateIndex1.Add(index1);
                            stateIndex2.Add(index2);
                            value1.Add(digit1);
                            value2.Add(digit2);
                        }
                    } // while (candidateIndex1.Any())

                    if (stateIndex1.Any())
                    {
                        int pos = rng.Next(stateIndex1.Count());
                        int index1 = stateIndex1.ElementAt(pos);
                        int index2 = stateIndex2.ElementAt(pos);
                        int digit1 = value1.ElementAt(pos);
                        int digit2 = value2.ElementAt(pos);
                        int row1 = index1 / 9;
                        int col1 = index1 % 9;
                        int row2 = index2 / 9;
                        int col2 = index2 % 9;

                        string description = string.Empty;

                        if (index1 / 9 == index2 / 9)
                        {
                            description = $"row #{index1 / 9 + 1}";
                        }
                        else if (index1 % 9 == index2 % 9)
                        {
                            description = $"column #{index1 % 9 + 1}";
                        }
                        else
                        {
                            description = $"block ({row1 / 3 + 1}, {col1 / 3 + 1})";
                        }

                        state[index1] = solvedState[index1];
                        state[index2] = solvedState[index2];
                        candidateMasks[index1] = 0;
                        candidateMasks[index2] = 0;
                        changeMade = true;

                        Console.WriteLine($"Guessing that {digit1} and {digit2} are arbitrary in {description} (multiple solutions): Pick {solvedState[index1]}->({row1 + 1}, {col1 + 1}), {solvedState[index2]}->({row2 + 1}, {col2 + 1}).");
                    }
                }
                #endregion

                if (changeMade)
                {
                    Presentation.PrintToConsole(state, string.Empty);
                }
            }
        }

        private static int[] CreateInitialBoard(int[] solvedBoard)
        {
            // Board is solved at this point.
            // Now pick subset of digits as the starting position.
            int remainingDigits = 30;
            int maxRemovedPerBlock = 6;
            int[,] removedPerBlock = new int[3, 3];
            int[] positions = Enumerable.Range(0, 9 * 9).ToArray();
            int[] state = solvedBoard.ToArray();
               

            int[] finalState = new int[state.Length];
            Array.Copy(state, finalState, finalState.Length);
            Random rng = new Random();
            int removedPos = 0;
            while (removedPos < 9 * 9 - remainingDigits)
            {
                int curRemainingDigits = positions.Length - removedPos;
                int indexToPick = removedPos + rng.Next(curRemainingDigits);

                int row = positions[indexToPick] / 9;
                int col = positions[indexToPick] % 9;

                int blockRowToRemove = row / 3;
                int blockColToRemove = col / 3;

                if (removedPerBlock[blockRowToRemove, blockColToRemove] >= maxRemovedPerBlock)
                    continue;

                removedPerBlock[blockRowToRemove, blockColToRemove] += 1;

                int temp = positions[removedPos];
                positions[removedPos] = positions[indexToPick];
                positions[indexToPick] = temp;

                int stateIndex = 9 * row + col;
                state[stateIndex] = 0;

                removedPos += 1;
            }

            return state;
        }


        private static int[] CreatePopulatedBoard()
        {
            // Construct board to be solved
            Random rng = new Random();

            // Top element is current state of the board
            Stack<int[]> stateStack = new Stack<int[]>();

            // Top elements are (row, col) of cell which has been modified compared to previous state
            Stack<int> rowIndexStack = new Stack<int>();
            Stack<int> colIndexStack = new Stack<int>();

            // Top element indicates candidate digits (those with False) for (row, col)
            Stack<bool[]> usedDigitsStack = new Stack<bool[]>();

            // Top element is the value that was set on (row, col)
            Stack<int> lastDigitStack = new Stack<int>();

            // Indicates operation to perform next
            // - expand - finds next empty cell and puts new state on stacks
            // - move - finds next candidate number at current pos and applies it to current state
            // - collapse - pops current state from stack as it did not yield a solution
            string command = "expand";
            while (stateStack.Count <= 9 * 9)
            {
                if (command == "expand")
                {
                    int[] currentState = new int[9 * 9];

                    if (stateStack.Count > 0)
                    {
                        Array.Copy(stateStack.Peek(), currentState, currentState.Length);
                    }

                    int bestRow = -1;
                    int bestCol = -1;
                    bool[] bestUsedDigits = null;
                    int bestCandidatesCount = -1;
                    int bestRandomValue = -1;
                    bool containsUnsolvableCells = false;

                    for (int index = 0; index < currentState.Length; index++)
                        if (currentState[index] == 0)
                        {

                            int row = index / 9;
                            int col = index % 9;
                            int blockRow = row / 3;
                            int blockCol = col / 3;

                            bool[] isDigitUsed = new bool[9];

                            for (int i = 0; i < 9; i++)
                            {
                                int rowDigit = currentState[9 * i + col];
                                if (rowDigit > 0)
                                    isDigitUsed[rowDigit - 1] = true;

                                int colDigit = currentState[9 * row + i];
                                if (colDigit > 0)
                                    isDigitUsed[colDigit - 1] = true;

                                int blockDigit = currentState[(blockRow * 3 + i / 3) * 9 + (blockCol * 3 + i % 3)];
                                if (blockDigit > 0)
                                    isDigitUsed[blockDigit - 1] = true;
                            } // for (i = 0..8)

                            int candidatesCount = isDigitUsed.Where(used => !used).Count();

                            if (candidatesCount == 0)
                            {
                                containsUnsolvableCells = true;
                                break;
                            }

                            int randomValue = rng.Next();

                            if (bestCandidatesCount < 0 ||
                                candidatesCount < bestCandidatesCount ||
                                (candidatesCount == bestCandidatesCount && randomValue < bestRandomValue))
                            {
                                bestRow = row;
                                bestCol = col;
                                bestUsedDigits = isDigitUsed;
                                bestCandidatesCount = candidatesCount;
                                bestRandomValue = randomValue;
                            }

                        } // for (index = 0..81)

                    if (!containsUnsolvableCells)
                    {
                        stateStack.Push(currentState);
                        rowIndexStack.Push(bestRow);
                        colIndexStack.Push(bestCol);
                        usedDigitsStack.Push(bestUsedDigits);
                        lastDigitStack.Push(0); // No digit was tried at this position
                    }

                    // Always try to move after expand
                    command = "move";

                } // if (command == "expand")
                else if (command == "collapse")
                {
                    stateStack.Pop();
                    rowIndexStack.Pop();
                    colIndexStack.Pop();
                    usedDigitsStack.Pop();
                    lastDigitStack.Pop();

                    command = "move";   // Always try to move after collapse
                }
                else if (command == "move")
                {

                    int rowToMove = rowIndexStack.Peek();
                    int colToMove = colIndexStack.Peek();
                    int digitToMove = lastDigitStack.Pop();

                    bool[] usedDigits = usedDigitsStack.Peek();
                    int[] currentState = stateStack.Peek();
                    int currentStateIndex = 9 * rowToMove + colToMove;

                    int movedToDigit = digitToMove + 1;
                    while (movedToDigit <= 9 && usedDigits[movedToDigit - 1])
                        movedToDigit += 1;

                    if (digitToMove > 0)
                    {
                        usedDigits[digitToMove - 1] = false;
                        currentState[currentStateIndex] = 0;
                    }

                    if (movedToDigit <= 9)
                    {
                        lastDigitStack.Push(movedToDigit);
                        usedDigits[movedToDigit - 1] = true;
                        currentState[currentStateIndex] = movedToDigit;

                        // Next possible digit was found at current position
                        // Next step will be to expand the state
                        command = "expand";
                    }
                    else
                    {
                        // No viable candidate was found at current position - pop it in the next iteration
                        lastDigitStack.Push(0);
                        command = "collapse";
                    }
                } // if (command == "move")

            }

            

            return stateStack.Peek();
        }

        private static List<IGrouping<int, CellGroup>> BuildCellGroups(int[] state)
        {
            var rowsIndices = state
                .Select((value, index) => new CellGroup
                {
                    Discriminator = index / 9,
                    Description = $"row #{index / 9 + 1}",
                    Index = index,
                    Row = index / 9,
                    Column = index % 9
                })
                .GroupBy(tuple => tuple.Discriminator);

            var columnIndices = state
                .Select((value, index) => new CellGroup
                {
                    Discriminator = 9 + index % 9,
                    Description = $"column #{index % 9 + 1}",
                    Index = index,
                    Row = index / 9,
                    Column = index % 9
                })
                .GroupBy(tuple => tuple.Discriminator);

            var blockIndices = state
                .Select((value, index) => new
                {
                    Row = index / 9,
                    Column = index % 9,
                    Index = index
                })
                .Select(tuple => new CellGroup
                {
                    Discriminator = 18 + 3 * (tuple.Row / 3) + tuple.Column / 3,
                    Description = $"block ({tuple.Row / 3 + 1}, {tuple.Column / 3 + 1})",
                    Index = tuple.Index,
                    Row = tuple.Row,
                    Column = tuple.Column
                })
                .GroupBy(tuple => tuple.Discriminator);

            var cellGroups = rowsIndices.Concat(columnIndices).Concat(blockIndices).ToList();
            return cellGroups;
        }
        static void Main(string[] args)
        {
            Play();

            Console.WriteLine();
            Console.Write("Press ENTER to exit... ");
            Console.ReadLine();
        }
    }
}
using System.Collections.Generic;

namespace Game.RunData
{
    public class UniqueAnswerChecker
    {
        private readonly bool[,] _line = new bool[9,9];
        private readonly bool[,] _column = new bool[9,9];
        private readonly bool[,,] _block = new bool[3,3,9];
        private bool _valid;
        private readonly List<int[]> _spaces = new();

        
        public void SolveSudoku(int[,] board) {
            for (int i = 0; i < 9; ++i) {
                for (int j = 0; j < 9; ++j) {
                    if (board[i,j] == 0) {
                        _spaces.Add(new[]{i, j});
                    } else {
                        int digit = board[i,j] - 1;
                        _line[i,digit] = _column[j,digit] = _block[i / 3, j / 3, digit] = true;
                    }
                }
            }

            Dfs(board, 0);
        }

        public void Dfs(int[,] board, int pos) {
            if (pos == _spaces.Count) {
                _valid = true;
                return;
            }

            int[] space = _spaces[pos];
            int i = space[0], j = space[1];
            for (int digit = 0; digit < 9 && !_valid; ++digit) {
                if (!_line[i,digit] && !_column[j,digit] && !_block[i / 3, j / 3, digit]) {
                    _line[i, digit] = _column[j, digit] = _block[i / 3, j / 3, digit] = true;
                    board[i, j] = digit + '0' + 1;
                    Dfs(board, pos + 1);
                    _line[i, digit] = _column[j, digit] = _block[i / 3, j / 3, digit] = false;
                }
            }
        }

    }
}
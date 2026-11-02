using System;
using System.Text;

namespace ScatterGame.Core
{
    public readonly struct GridPosition : IEquatable<GridPosition>
    {
        public readonly int Reel;
        public readonly int Row;

        public GridPosition(int reel, int row)
        {
            Reel = reel;
            Row = row;
        }

        public bool Equals(GridPosition other) => Reel == other.Reel && Row == other.Row;
        public override bool Equals(object obj) => obj is GridPosition other && Equals(other);
        public override int GetHashCode() => Reel * 31 + Row;
        public override string ToString() => "(" + Reel + "," + Row + ")";
    }

    /// <summary>5 reels x 4 rows of symbol ids. Row 0 is the TOP row. Stored reel by reel.</summary>
    public sealed class SlotGrid
    {
        private readonly int[] cells = new int[SlotConfig.Reels * SlotConfig.Rows];

        public int Get(int reel, int row)
        {
            return cells[Index(reel, row)];
        }

        public void Set(int reel, int row, int symbolId)
        {
            cells[Index(reel, row)] = symbolId;
        }

        public void Set(GridPosition position, int symbolId)
        {
            Set(position.Reel, position.Row, symbolId);
        }

        public int Get(GridPosition position)
        {
            return Get(position.Reel, position.Row);
        }

        public int CellCount => cells.Length;

        public int CountSymbol(int symbolId)
        {
            int count = 0;
            foreach (int cell in cells) if (cell == symbolId) count++;
            return count;
        }

        public SlotGrid Clone()
        {
            SlotGrid copy = new SlotGrid();
            Array.Copy(cells, copy.cells, cells.Length);
            return copy;
        }

        /// <summary>Flat copy (reel by reel, top to bottom). JsonUtility-friendly for the history save.</summary>
        public int[] ToArray()
        {
            int[] copy = new int[cells.Length];
            Array.Copy(cells, copy, cells.Length);
            return copy;
        }

        public static SlotGrid FromArray(int[] flat)
        {
            if (flat == null || flat.Length != SlotConfig.Reels * SlotConfig.Rows)
            {
                throw new ArgumentException("A grid needs exactly " + (SlotConfig.Reels * SlotConfig.Rows) + " cells.");
            }
            SlotGrid grid = new SlotGrid();
            Array.Copy(flat, grid.cells, flat.Length);
            return grid;
        }

        /// <summary>Builds a grid from 4 rows of 5 symbol ids (rows[row][reel]).</summary>
        public static SlotGrid FromRows(params int[][] rows)
        {
            if (rows == null || rows.Length != SlotConfig.Rows) throw new ArgumentException("A grid needs exactly " + SlotConfig.Rows + " rows.");

            SlotGrid grid = new SlotGrid();
            for (int row = 0; row < SlotConfig.Rows; row++)
            {
                if (rows[row] == null || rows[row].Length != SlotConfig.Reels) throw new ArgumentException("Every row needs exactly " + SlotConfig.Reels + " symbols.");
                for (int reel = 0; reel < SlotConfig.Reels; reel++) grid.Set(reel, row, rows[row][reel]);
            }
            return grid;
        }

        public bool SameAs(SlotGrid other)
        {
            if (other == null) return false;
            for (int i = 0; i < cells.Length; i++) if (cells[i] != other.cells[i]) return false;
            return true;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int row = 0; row < SlotConfig.Rows; row++)
            {
                for (int reel = 0; reel < SlotConfig.Reels; reel++)
                {
                    builder.Append(Get(reel, row)).Append(reel < SlotConfig.Reels - 1 ? " " : "");
                }
                builder.Append(row < SlotConfig.Rows - 1 ? " / " : "");
            }
            return builder.ToString();
        }

        private static int Index(int reel, int row)
        {
            if (reel < 0 || reel >= SlotConfig.Reels) throw new ArgumentOutOfRangeException(nameof(reel));
            if (row < 0 || row >= SlotConfig.Rows) throw new ArgumentOutOfRangeException(nameof(row));
            return reel * SlotConfig.Rows + row;
        }
    }
}
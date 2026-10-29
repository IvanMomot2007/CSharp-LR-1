using System;

namespace SparseMatrixApp
{
    public class SparseMatrix : ISparseMatrix
    {
        private class Node
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public double Value { get; set; }
            public Node Next { get; set; }

            public Node(int row, int col, double value, Node next = null)
            {
                Row = row;
                Col = col;
                Value = value;
                Next = next;
            }
        }

        private Node head;

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public SparseMatrix()
        {
            Rows = 1;
            Columns = 1;
            head = null;
        }

        public SparseMatrix(int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentOutOfRangeException("Розміри мають бути більше 0.");
            Rows = rows;
            Columns = cols;
            head = null;
        }

        public SparseMatrix(double[,] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            Rows = array.GetLength(0);
            Columns = array.GetLength(1);
            if (Rows <= 0 || Columns <= 0)
                throw new ArgumentOutOfRangeException("Масив не може бути порожнім.");

            head = null;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (Math.Abs(array[i, j]) > 1e-9)
                    {
                        this[i, j] = array[i, j];
                    }
                }
            }
        }

        public SparseMatrix(SparseMatrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            Rows = other.Rows;
            Columns = other.Columns;
            head = null;
            Node curr = other.head;
            while (curr != null)
            {
                this[curr.Row, curr.Col] = curr.Value;
                curr = curr.Next;
            }
        }

        public double this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= Rows || col < 0 || col >= Columns)
                    throw new IndexOutOfRangeException("Індекс поза межами.");
                Node curr = head;
                while (curr != null)
                {
                    if (curr.Row == row && curr.Col == col)
                        return curr.Value;
                    curr = curr.Next;
                }
                return 0.0;
            }
            set
            {
                if (row < 0 || row >= Rows || col < 0 || col >= Columns)
                    throw new IndexOutOfRangeException("Індекс поза межами.");
                Node prev = null;
                Node curr = head;
                while (curr != null)
                {
                    if (curr.Row == row && curr.Col == col)
                    {
                        if (Math.Abs(value) <= 1e-9)
                        {
                            if (prev == null) head = curr.Next;
                            else prev.Next = curr.Next;
                        }
                        else
                        {
                            curr.Value = value;
                        }
                        return;
                    }
                    prev = curr;
                    curr = curr.Next;
                }
                if (Math.Abs(value) > 1e-9)
                {
                    head = new Node(row, col, value, head);
                }
            }
        }

        public ISparseMatrix Add(ISparseMatrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Rows != other.Rows || Columns != other.Columns)
                throw new ArgumentException("Розмірності матриць для додавання повинні збігатися.");

            SparseMatrix otherSparse = (SparseMatrix)other;
            SparseMatrix result = new SparseMatrix(Rows, Columns);

            Node curr = head;
            while (curr != null)
            {
                result[curr.Row, curr.Col] = curr.Value;
                curr = curr.Next;
            }

            Node currB = otherSparse.head;
            while (currB != null)
            {
                result[currB.Row, currB.Col] += currB.Value;
                currB = currB.Next;
            }

            return result;
        }

        public ISparseMatrix Multiply(double scalar)
        {
            SparseMatrix result = new SparseMatrix(Rows, Columns);

            if (Math.Abs(scalar) <= 1e-9)
                return result;

            Node curr = head;
            while (curr != null)
            {
                result[curr.Row, curr.Col] = curr.Value * scalar;
                curr = curr.Next;
            }

            return result;
        }

        public ISparseMatrix Multiply(ISparseMatrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (Columns != other.Rows)
                throw new ArgumentException("Кількість стовпців A повинна дорівнювати кількості рядків B.");

            SparseMatrix otherSparse = (SparseMatrix)other;
            SparseMatrix result = new SparseMatrix(Rows, other.Columns);

            Node currA = head;
            while (currA != null)
            {
                Node currB = otherSparse.head;
                while (currB != null)
                {
                    if (currA.Col == currB.Row)
                    {
                        result[currA.Row, currB.Col] += currA.Value * currB.Value;
                    }
                    currB = currB.Next;
                }
                currA = currA.Next;
            }

            return result;
        }

        public ISparseMatrix Transpose()
        {
            SparseMatrix result = new SparseMatrix(Columns, Rows);

            Node curr = head;
            while (curr != null)
            {
                result[curr.Col, curr.Row] = curr.Value;
                curr = curr.Next;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SparseMatrix other))
                return false;
            if (Rows != other.Rows || Columns != other.Columns)
                return false;

            int countThis = 0;
            Node curr = head;
            while (curr != null)
            {
                countThis++;
                if (Math.Abs(curr.Value - other[curr.Row, curr.Col]) > 1e-6)
                    return false;
                curr = curr.Next;
            }

            int countOther = 0;
            Node currOther = other.head;
            while (currOther != null)
            {
                countOther++;
                currOther = currOther.Next;
            }

            return countThis == countOther;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Rows.GetHashCode();
                hash = hash * 23 + Columns.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sb.Append($"{this[i, j]:F2}\t");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}

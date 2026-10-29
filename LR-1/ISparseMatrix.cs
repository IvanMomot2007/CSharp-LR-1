using System;
using System.Collections.Generic;
using System.Text;

namespace SparseMatrixApp
{
    public interface ISparseMatrix
    {
        int Rows { get; }
        int Columns { get; }
        double this[int row, int col] { get; set; }
        ISparseMatrix Add(ISparseMatrix other);
        ISparseMatrix Multiply(double scalar);
        ISparseMatrix Multiply(ISparseMatrix other);
        ISparseMatrix Transpose();
    }
}

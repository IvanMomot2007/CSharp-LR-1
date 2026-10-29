using System;
using Xunit;
using SparseMatrixApp;

namespace SparseMatrixTests
{
    public class SparseMatrixUnitTests
    {
        [Fact]
        public void Add_ValidMatricesSameSize_ReturnsCorrectSum()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 1, 0 }, { 0, 2 } });
            ISparseMatrix b = new SparseMatrix(new double[,] { { 0, 3 }, { 4, 0 } });
            ISparseMatrix expected = new SparseMatrix(new double[,] { { 1, 3 }, { 4, 2 } });

            // Act
            ISparseMatrix actual = a.Add(b);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_ElementsCancelOutToZero_RemovesNodesFromStorage()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 5, 0 } });
            ISparseMatrix b = new SparseMatrix(new double[,] { { -5, 0 } });

            // Act
            ISparseMatrix actual = a.Add(b);

            // Assert
            Assert.Equal(0.0, actual[0, 0], 5);
        }

        [Fact]
        public void Add_ToZeroMatrix_ReturnsIdenticalMatrix()
        {
            // Arrange
            ISparseMatrix zero = new SparseMatrix(2, 2);
            ISparseMatrix b = new SparseMatrix(new double[,] { { 1, 2 }, { 3, 4 } });

            // Act
            ISparseMatrix actual = zero.Add(b);

            // Assert
            Assert.Equal(b, actual);
        }

        [Fact]
        public void Add_DifferentDimensions_ThrowsArgumentException()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(2, 2);
            ISparseMatrix b = new SparseMatrix(3, 2);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => a.Add(b));
        }

        [Fact]
        public void Add_NullOperand_ThrowsArgumentNullException()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(2, 2);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => a.Add(null));
        }

        [Fact]
        public void Multiply_PositiveScalar_MultipliesAllElements()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 2, 0 }, { -4, 1 } });
            ISparseMatrix expected = new SparseMatrix(new double[,] { { 5, 0 }, { -10, 2.5 } });

            // Act
            ISparseMatrix actual = a.Multiply(2.5);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiply_ZeroScalar_ClearsMatrixValues()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 3, 7 }, { 1, 4 } });

            // Act
            ISparseMatrix actual = a.Multiply(0.0);

            // Assert
            Assert.Equal(0.0, actual[0, 0], 5);
            Assert.Equal(0.0, actual[1, 1], 5);
        }

        [Fact]
        public void Multiply_NegativeScalar_NegatesValues()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 0, 5 }, { -2, 0 } });
            ISparseMatrix expected = new SparseMatrix(new double[,] { { 0, -5 }, { 2, 0 } });

            // Act
            ISparseMatrix actual = a.Multiply(-1.0);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiply_SquareCompatibleMatrices_ComputesCorrectProduct()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 1, 2 }, { 3, 4 } });
            ISparseMatrix b = new SparseMatrix(new double[,] { { 2, 0 }, { 1, 2 } });
            ISparseMatrix expected = new SparseMatrix(new double[,] { { 4, 4 }, { 10, 8 } });

            // Act
            ISparseMatrix actual = a.Multiply(b);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiply_RectangularMatrices_ReturnsProductWithCorrectDimensions()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            ISparseMatrix b = new SparseMatrix(new double[,] { { 7, 8 }, { 9, 1 }, { 2, 3 } });
            ISparseMatrix expected = new SparseMatrix(new double[,] { { 31, 19 }, { 85, 55 } });

            // Act
            ISparseMatrix actual = a.Multiply(b);

            // Assert
            Assert.Equal(2, actual.Rows);
            Assert.Equal(2, actual.Columns);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Multiply_ByZeroMatrix_ReturnsZeroMatrix()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 1, 2 }, { 3, 4 } });
            ISparseMatrix zero = new SparseMatrix(2, 2);

            // Act
            ISparseMatrix actual = a.Multiply(zero);

            // Assert
            Assert.Equal(zero, actual);
        }

        [Fact]
        public void Multiply_IncompatibleDimensions_ThrowsArgumentException()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(2, 3);
            ISparseMatrix b = new SparseMatrix(2, 3);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => a.Multiply(b));
        }

        [Fact]
        public void Multiply_NullMatrix_ThrowsArgumentNullException()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(2, 2);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => a.Multiply(null));
        }

        [Fact]
        public void Transpose_RectangularMatrix_InvertsRowsAndColumns()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
            ISparseMatrix expected = new SparseMatrix(new double[,] { { 1, 4 }, { 2, 5 }, { 3, 6 } });

            // Act
            ISparseMatrix transposed = a.Transpose();

            // Assert
            Assert.Equal(3, transposed.Rows);
            Assert.Equal(2, transposed.Columns);
            Assert.Equal(expected, transposed);
        }

        [Fact]
        public void Transpose_TwiceApplied_ReturnsOriginalMatrix()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 10, 0 }, { 0, -3 } });

            // Act
            ISparseMatrix actual = a.Transpose().Transpose();

            // Assert
            Assert.Equal(a, actual);
        }

        [Fact]
        public void Transpose_SingleElementSparseMatrix_FlipsCoordinates()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(3, 3);
            a[0, 2] = 8.0;

            // Act
            ISparseMatrix transposed = a.Transpose();

            // Assert
            Assert.Equal(8.0, transposed[2, 0], 5);
            Assert.Equal(0.0, transposed[0, 2], 5);
        }

        [Fact]
        public void Equals_IdenticalMatrices_ReturnsTrue()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(new double[,] { { 1, 2 }, { 0, 4 } });
            ISparseMatrix b = new SparseMatrix(new double[,] { { 1, 2 }, { 0, 4 } });

            // Act & Assert
            Assert.True(a.Equals(b));
        }

        [Fact]
        public void Equals_DifferentSizesOrValues_ReturnsFalse()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(2, 2);
            ISparseMatrix b = new SparseMatrix(2, 3);

            // Act & Assert
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Indexer_SetAndGetValidCoordinates_ReturnsStoredValue()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(3, 3);

            // Act
            a[1, 2] = 9.5;

            // Assert
            Assert.Equal(9.5, a[1, 2], 5);
        }

        [Fact]
        public void Indexer_NegativeIndex_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            ISparseMatrix a = new SparseMatrix(2, 2);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => { double val = a[-1, 0]; });
        }

        [Fact]
        public void Constructor_InvalidDimensions_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new SparseMatrix(0, -5));
        }
    }
}

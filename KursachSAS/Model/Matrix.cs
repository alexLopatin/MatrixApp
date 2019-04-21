using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace KursachSAS
{
    

    public class Matrix<T> : ICloneable where T : struct
    {
        public Matrix(int n, int m)
        {
            matrixTable = new T[n, m];
            rowsCount = n;
            columnsCount = m;
            lines = new List<Point>();
        }
        ~Matrix()
        {
            
        }
        public event EventHandler<EventArgs> DisposeEvent;

        public List<Point> lines;
        public  EventHandler<EventArgs> Changed;
        public void Dispose()
        {
            if (DisposeEvent != null)
                DisposeEvent(this, null);
        }
        public int rowsCount;
        public int columnsCount;
        private T[,] matrixTable;
        public T this[int i, int j]
        {
            get
            {
                return matrixTable[i, j];
            }
            set
            {
                
                matrixTable[i, j] = value;
                if (Changed != null)
                    Changed.Invoke(this, null);
            }
        }
        public event EventHandler<EventArgs> SizeChanged;
        public void Resize(int newRows, int newColumns)
        {
            
            matrixTable = new T[newRows, newColumns];
            rowsCount = newRows;
            columnsCount = newColumns;
            SizeChanged(this, null);
            if (Changed != null)
                Changed.Invoke(this, null);
        }

        public Matrix<T> GetSubMatrix(Point element)
        {
            if(lines.Count==0)
                return new Matrix<T>(rowsCount, columnsCount);

            var left = 0;
            if(lines.Count(p => p.Y == 0)!=0)
                left = lines.Where(p => p.Y == 0).Max(e =>
                {
                    if ((e.X-0.5) - element.X <= 0)
                        return e.X;
                    else
                        return 0;
                });
            var top = 0;
            if (lines.Count(p => p.Y == 1) != 0)
                top = lines.Where(p => p.Y == 1).Max(e =>
                {
                    if ((e.X - 0.5) - element.Y <= 0)
                        return e.X;
                    else
                        return 0;
                });
            var right = columnsCount;
            if (lines.Count(p => p.Y == 0) != 0)
                right = lines.Where(p => p.Y == 0).Min(e =>
                {
                    if ((e.X - 0.5) - element.X > 0)
                        return e.X;
                    else
                        return columnsCount ;
                });
            var bottom = rowsCount;
            if (lines.Count(p => p.Y == 1) != 0)
                bottom = lines.Where(p => p.Y == 1).Min(e =>
                {
                    if ((e.X - 0.5) - element.Y > 0)
                        return e.X;
                    else
                        return rowsCount ;
                });
            int rows = bottom - top;
            if (rows == 0) rows = 1;
            int columns = right - left;
            if (columns == 0) columns = 1;
            Matrix<T> res = new Matrix<T>(rows, columns);
            for (int i = top; i < bottom; i++)
                for (int j = left; j < right; j++)
                    res[i - top, j - left] = this[i, j];
            return res;
        }

        public object Clone()
        {
            Matrix<T> result = new Matrix<T>(rowsCount, columnsCount);
            for (int i = 0; i < rowsCount; i++)
                for (int j = 0; j < columnsCount; j++)
                    result.matrixTable[i, j] = matrixTable[i, j];
            result.lines = new List<Point>(lines);
            return result;
        }
        public static Matrix<T> operator +(Matrix<T> m1, Matrix<T> m2)
        {
            
            if (!(m1.rowsCount == m2.rowsCount && m1.columnsCount == m2.columnsCount))
                throw new Exception("Ошибка", null);
            if (!m1.LinesEqual(m2))
                throw new Exception("Ошибка", null);
            Matrix<T> result = new Matrix<T>(m2.rowsCount, m1.columnsCount);
            result.lines = new List<Point>(m1.lines);
            for (int i = 0; i < m1.rowsCount; i++)
                for (int j = 0; j < m1.columnsCount; j++)
                    result.matrixTable[i, j] = (dynamic)m1.matrixTable[i, j] + (dynamic)m2.matrixTable[i, j];
            return result;
        }
        public static Matrix<T> operator -(Matrix<T> m1, Matrix<T> m2)
        {
            if (!(m1.rowsCount == m2.rowsCount && m1.columnsCount == m2.columnsCount))
                throw new Exception("Ошибка", null);
            if (!m1.LinesEqual(m2))
                throw new Exception("Ошибка", null);
            Matrix<T> result = new Matrix<T>(m2.rowsCount, m1.columnsCount);
                        result.lines = new List<Point>(m1.lines);

            for (int i = 0; i < m1.rowsCount; i++)
                for (int j = 0; j < m1.columnsCount; j++)
                    result.matrixTable[i, j] = (dynamic)m1.matrixTable[i, j] - (dynamic)m2.matrixTable[i, j];
            return result;
        }
        private Point index = new Point(-1, -1);

        public void InsertSubMatrix(Matrix<T> subMatrix, Point element)
        {
            if (lines.Count == 0)
                return ;

            var left = 0;
            if (lines.Count(p => p.Y == 0) != 0)
                left = lines.Where(p => p.Y == 0).Max(e =>
                {
                    if ((e.X - 0.5) - element.X <= 0)
                        return e.X;
                    else
                        return 0;
                });
            var top = 0;
            if (lines.Count(p => p.Y == 1) != 0)
                top = lines.Where(p => p.Y == 1).Max(e =>
                {
                    if ((e.X - 0.5) - element.Y <= 0)
                        return e.X;
                    else
                        return 0;
                });
            var right = columnsCount;
            if (lines.Count(p => p.Y == 0) != 0)
                right = lines.Where(p => p.Y == 0).Min(e =>
                {
                    if ((e.X - 0.5) - element.X > 0)
                        return e.X;
                    else
                        return columnsCount;
                });
            var bottom = rowsCount;
            if (lines.Count(p => p.Y == 1) != 0)
                bottom = lines.Where(p => p.Y == 1).Min(e =>
                {
                    if ((e.X - 0.5) - element.Y > 0)
                        return e.X;
                    else
                        return rowsCount;
                });
            int rows = bottom - top;
            if (rows == 0) rows = 1;
            int columns = right - left;
            if (columns == 0) columns = 1;
            for (int i = top; i < bottom; i++)
                for (int j = left; j < right; j++)
                    this[i , j ] = subMatrix[i - top, j - left];
            
        }

        private List<Matrix<T>> BreakIntoSubMatrices()
        {
            return null;
        }
        private bool LinesEqual(Matrix<T> compare)
        {
            foreach(Point line in lines)
            {
                if (!compare.lines.Exists(p => p == line))
                    return false;
            }
            return true;
        }
        public static Matrix<T> operator *(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1.columnsCount != m2.rowsCount)
                throw new Exception("Ошибка", null);

            if(!m1.LinesEqual(m2))
                throw new Exception("Ошибка", null);
            if(m1.lines.Count>0)
            {

            }
            Matrix<T> result = new Matrix<T>(m1.rowsCount, m2.columnsCount);
            for (int i = 0; i < result.rowsCount; i++)
            {
                for (int j = 0; j < result.columnsCount; j++)
                {
                    
                    for (int k = 0; k < m1.columnsCount; k++) // OR k<b.GetLength(0)
                        result[i, j] = result[i, j] + (dynamic)m1[i, k] * m2[k, j];
                }
            }
            return result;
        }
        public Matrix<T> Transpose()
        {
            
            Matrix<T> result = new Matrix<T>(columnsCount, rowsCount);
            for (int i = 0; i < columnsCount; i++)
                for (int j = 0; j < rowsCount; j++)
                    result[i, j] = this[j, i];
            return result;
        }
        private  T Helper(int n, T[,] Mat)
        {
            T d = new T();
            int k, i, j, subi, subj;
            T[,] SUBMat = new T[n, n];
            if (n == 2)
            {
                var t = (((dynamic)Mat[0, 0] * Mat[1, 1]) - ((dynamic)Mat[1, 0] * Mat[0, 1]));
                return t;
            }
            else
            if(n==3)
            {
                var t = (dynamic)Mat[0, 0] * Mat[1, 1] * Mat[2, 2] + (dynamic)Mat[0, 1] * Mat[1, 2] * Mat[2, 0] + (dynamic)Mat[1, 0] * Mat[2, 1] * Mat[0, 2] - (dynamic)Mat[0, 2] * Mat[1, 1] * Mat[2, 0] - (dynamic)Mat[0, 0] * Mat[2, 1] * Mat[1, 2] - (dynamic)Mat[0, 1] * Mat[1, 0] * Mat[2, 2];
                return t;
            } else
            {
                return (dynamic)this[0, 0] + (dynamic)this[3, 3];
            }
            {
                for (k = 0; k < n; k++)
                {
                    subi = 0;
                    for (i = 1; i < n; i++)
                    {
                        subj = 0;
                        for (j = 0; j < n; j++)
                        {
                            if (j == k)
                            {
                                continue;
                            }
                            SUBMat[subi, subj] = Mat[i, j];
                            subj++;
                        }
                        subi++;
                    }
                    var t = (Math.Pow(-1, k) * (dynamic)Mat[0, k] * Helper(n - 1, SUBMat));
                    d = (dynamic)d +t;
                }
            }
            return d;
        }

        private T Pow(T x, int y)
        {
            if (y == 0)
                return (dynamic)x / x;

            for (int i = 0; i < y-1; i++)
                x = x *(dynamic) x;
            return x;
        }

        public static void Copy(Matrix<T> copyMatrix, Matrix<T> pasteMatrix)
        {
            pasteMatrix.Resize(copyMatrix.rowsCount, copyMatrix.columnsCount);
            for (int i = 0; i < copyMatrix.rowsCount; i++)
                for (int j = 0; j < copyMatrix.columnsCount; j++)
                    pasteMatrix[i, j] = copyMatrix[i, j];
            pasteMatrix.lines = new List<Point>(copyMatrix.lines);
            pasteMatrix.Changed = copyMatrix.Changed;
            pasteMatrix.Changed.Invoke(pasteMatrix, null);
        }

        public T Determinant()
        {
            T[,] data = new T[rowsCount, columnsCount];
            for (int i = 0; i < rowsCount; i++)
                for (int j = 0; j < columnsCount; j++)
                    data[i, j] = this[i, j];
            return Helper(rowsCount, data);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachSAS
{
    class Matrix<T>
    {
        public Matrix(int n, int m)
        {
            matrixTable = new T[n, m];
        }
        private T[,] matrixTable;
        public T this[int i, int j]
        {
            set
            {
                matrixTable[i, j] = value;
            }
            get
            {
                return matrixTable[i,j];
            }
        }

    }
}

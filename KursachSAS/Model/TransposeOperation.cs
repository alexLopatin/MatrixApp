using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachSAS.Model
{
    public class TransposeOperation<T> : IOperable<T> where T : struct
    {
        public List<IData> DataBinds
        {
            get
            {
                return null;
            }
        }

        public OperationType operationType
        {
            get
            {
                return OperationType.Unary;
            }

        }

        public string representation
        {
            get
            {
                return "(T)";
            }
        }

        public Matrix<T> Calculate(Matrix<T> matrix)
        {
            return matrix.Transpose();
        }

        public Matrix<T> Calculate(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
        {
            throw new NotImplementedException();
        }
    }

}

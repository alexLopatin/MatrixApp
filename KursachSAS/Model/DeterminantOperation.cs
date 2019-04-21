using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace KursachSAS.Model
    {
        public class DeterminantOperation<T> : IOperable<T> where T : struct
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
                    return "(Det)";
                }
            }

            public Matrix<T> Calculate(Matrix<T> matrix)
            {
                Matrix<T> result = new Matrix<T>(1, 1);
                result[0, 0] = matrix.Determinant();
                return result;
            }

            public Matrix<T> Calculate(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
            {
                throw new NotImplementedException();
            }
        }

    }


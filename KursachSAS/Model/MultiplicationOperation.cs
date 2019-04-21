using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachSAS.Model
{
    public class MultiplicationOperation<T> : IOperable<T> where T : struct
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
                return OperationType.Binary;
            }

        }

        public string representation
        {
            get
            {
                return "*";
            }
        }

        public Matrix<T> Calculate(Matrix<T> matrix)
        {
            throw new NotImplementedException();
        }

        public Matrix<T> Calculate(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
        {
            return leftMatrix * rightMatrix;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursachSAS.Model;


namespace KursachSAS.Model
{
    
    public interface IOperable<T> where T:struct
    {
        string representation { get; }
        OperationType operationType { get;  }
        Matrix<T> Calculate(Matrix<T> leftMatrix, Matrix<T> rightMatrix);
        Matrix<T> Calculate(Matrix<T> matrix);
        List<IData> DataBinds { get;  }
    }
    public enum OperationType { Unary,Binary, Multiarial }
}

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachSAS.Model
{
    

    public class Operation<T> where T:struct
    {
        public Matrix<T> leftMatrix;
        public Matrix<T> rightMatrix;
        public Matrix<T> ResultMatrix;
        public IOperable<T> operationCalculator { get; set; }
        public EventHandler<EventArgs> argumentsInvalid;
        public Operation(Matrix<T> leftMatrix, Matrix<T> rightMatrix, IOperable<T> opCalculator)
        {
            operationCalculator = opCalculator;
            this.leftMatrix = leftMatrix;
            this.rightMatrix = rightMatrix;
            
        }

        public Operation(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
        {
            operationCalculator = new SumOperation<T>();
            this.leftMatrix = leftMatrix;
            this.rightMatrix = rightMatrix;
        }

        public string GetStringRepresentation()
        {
            return operationCalculator.representation;
        }

        public Operation()
        {
            operationCalculator = new DiffOperation<T>();
        }
        public void SetOperationCalculator(IOperable<T> opCalculator)
        {
            operationCalculator = opCalculator;
        }
        public Matrix<T> Calculate()
        {
            Matrix<T> result;
            try
            {

                if (operationCalculator.operationType == OperationType.Binary)
                    result = operationCalculator.Calculate(leftMatrix, rightMatrix);
                else
                    result = operationCalculator.Calculate(leftMatrix);
                ResultMatrix = result;
                return result;
            }
            catch
            {
                argumentsInvalid.Invoke(this, null);
            }
            result = new Matrix<T>(leftMatrix.rowsCount, leftMatrix.columnsCount);
            ResultMatrix = result;
            return result;
        }
    }
}

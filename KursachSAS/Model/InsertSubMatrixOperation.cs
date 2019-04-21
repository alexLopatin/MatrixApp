using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachSAS.Model
{
    public class InsertSubMatrixOperation<T> : IOperable<T> where T : struct
    {
        private List<IData> dataBinds;
        public List<IData> DataBinds
        {
            get
            {
                return dataBinds;
            }
        }
        public OperationType operationType
        {
            get
            {
                return OperationType.Multiarial;
            }

        }
        public string representation
        {
            get
            {
                return "(InsertSubMatrix)";
            }
        }
        public Matrix<T> Calculate(Matrix<T> matrix)
        {

            var res = (Matrix<T>)matrix.Clone();
            res.InsertSubMatrix(subMatrix.data, new System.Drawing.Point(x.data, y.data));
            return res;

        }
        public InsertSubMatrixOperation()
        {
            dataBinds = new List<IData>();
            x = new DataBind<int>(0, new string[] { "X элемента, принадлежащего блоку в матрице:" });
            y = new DataBind<int>(0, new string[] { "Y элемента, принадлежащего блоку в матрице:" });
            subMatrix = new DataBind<Matrix<T>>(new Matrix<T>(1,1), new string[] { "Вставляемая подматрица:" });

            dataBinds.Add(x);
            dataBinds.Add(y);
            dataBinds.Add(subMatrix);
        }

        DataBind<int> x;
        DataBind<int> y;
        DataBind<Matrix<T>> subMatrix;
        public Matrix<T> Calculate(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
        {
            throw new NotImplementedException();
        }
    }

}

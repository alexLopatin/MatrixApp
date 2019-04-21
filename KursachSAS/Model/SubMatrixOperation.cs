using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachSAS.Model
{
    public class SubMatrixOperation<T> : IOperable<T> where T : struct
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
                return "(SubMatrix)";
            }
        }
        public Matrix<T> Calculate(Matrix<T> matrix)
        {

            var res = (Matrix<T>)matrix.Clone();

            return matrix.GetSubMatrix(new System.Drawing.Point(x.data, y.data));
            
        }
        public SubMatrixOperation()
        {
            dataBinds = new List<IData>();
            x = new DataBind<int>(0, new string[] { "X элемента, принадлежащего подматрице:" });
            y = new DataBind<int>(0, new string[] { "Y элемента, принадлежащего подматрице:" });
            dataBinds.Add(x);
            dataBinds.Add(y);
        }

        DataBind<int> x;
        DataBind<int> y;
        public Matrix<T> Calculate(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
        {
            throw new NotImplementedException();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachSAS.Model
{
    public class SplitOperation<T> : IOperable<T> where T : struct
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
                return "(Split)";
            }
        }
        public Matrix<T> Calculate(Matrix<T> matrix)
        {
            var res = (Matrix<T>)matrix.Clone();
            if (rowColNum.data == 0)
                return res;
            res.lines.Add(new System.Drawing.Point(rowColNum.data, Convert.ToInt32( isHorizontal.data)));
            return res;
            
        }

        DataBind<int> rowColNum;
        DataBind<bool> isHorizontal;
        public SplitOperation()
        {
            dataBinds = new List<IData>();
            rowColNum = new DataBind<int>(0, new string[] {"Номер строки/столбца:" });
            isHorizontal = new DataBind<bool>(false, new string[] { "Выбор ориентации:",  "Горизонтально","Вертикально" });

            dataBinds.Add(rowColNum);
            dataBinds.Add(isHorizontal);
            
        }

        public Matrix<T> Calculate(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
        {
            throw new NotImplementedException();
        }
    }

}

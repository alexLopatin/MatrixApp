using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursachSAS.Controller;
using KursachSAS.View;

namespace KursachSAS.Command
{
    class EditCommand<T>: ICommand where T:struct
    {
        private MatrixGrid grid;
        private Matrix<T> matrix;
        private T[,] oldData;
        public string name { get; set; }
        public EditCommand(MatrixGrid matrixGrid)
        {
            grid = matrixGrid;
            name = "";
        }

        public void Execute()
        {
            var data = MatrixController<T>.EditMatrix(grid);
            oldData = data.oldData;
            matrix = data.matrix;
            grid.MatrixName = name;
        }

        public void Undo()
        {
            if(oldData!=null&&matrix!=null)
            MatrixController<T>.EditMatrix(oldData, matrix);
        }
    }
}

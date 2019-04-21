using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursachSAS.Controller;
using KursachSAS.View;

namespace KursachSAS.Command
{
    class CreateCommand<T> : ICommand where T : struct
    {
        private MatrixGrid grid;
        public T[,] data { get; set; }
        private CreationType type;
        private OperationView opView;
        public CreateCommand(CreationType type)
        {
            this.type = type;
            name = "";
        }
        public string name { get; set; }

        public void Execute()
        {


            if(type == CreationType.Matrix)
            {
                if (data.GetLength(0) != 0 || data.GetLength(1) != 0)
                    grid = MatrixController<T>.CreateMatrix(data);
                grid.MatrixName = name;
            }
                
            else
            {
                opView=OperationController<T>.CreateEmptyOperation();
            }
            
        }

        public void Undo()
        {
            if (type == CreationType.Matrix&&grid!=null)
                MatrixController<T>.DeleteMatrix(grid);
            else
            {
                if(opView!=null)
                {
                    DeleteCommand<T> del = new DeleteCommand<T>(opView);
                    del.Execute();
                }
            }

        }
    }
    public enum CreationType { Matrix, Operation}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursachSAS.Controller;
using KursachSAS.View;

namespace KursachSAS.Command
{
    class DragDropCommand<T> : ICommand where T : struct
    {
        private MatrixGrid grid;
        private MatrixGrid newGrid;
        public T[,] data { get; set; }
        public DragDropCommand(MatrixGrid grid)
        {
            this.grid = grid;
            grid.dragDropExecuted += (o, e) =>
              {
                  newGrid = (MatrixGrid)o;
              };
        }

        public void Execute()
        {
            MatrixController<T>.DoDragDrop(grid);

        }

        public void Undo()
        {
            if(newGrid!=null)
            {
                var delCom = new DeleteCommand<T>(newGrid);
                delCom.Execute();
            }
        }
    }
}

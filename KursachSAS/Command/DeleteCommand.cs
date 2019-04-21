using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursachSAS.Controller;
using KursachSAS.View;
using KursachSAS.Model;

namespace KursachSAS.Command
{
    class DeleteCommand<T> : ICommand where T : struct
    {
        private MatrixGrid grid;
        private Matrix<T> matrix;
        private OperationView opView;
        private Operation<T> operation;
        private bool isMatrixController = true;
        public DeleteCommand(MatrixGrid matrixGrid)
        {
            grid = matrixGrid;
        }
        public DeleteCommand(OperationView opView)
        {
            this.opView = opView;
            isMatrixController = false;
        }
        public void Execute()
        {
            if(isMatrixController)
            {
                if (grid.side == WorkingPanelSide.List)
                {

                    var data = MatrixController<T>.DeleteMatrix(grid);
                    matrix = data.matrix;
                }
                else
                {
                    matrix = MatrixController<T>.GetMatrix(grid);
                    opView = OperationController<T>.GetOperationView(matrix);
                    MatrixController<T>.DeleteMatrix(grid);
                }
            }else
            {
                operation = OperationController<T>.GetOperation(opView);
                OperationController<T>.DeleteOperation(opView);
            }
            
            
        }

        public void Undo()
        {
            if (isMatrixController)
            {
                if (matrix != null)
                {
                    if (grid.side == WorkingPanelSide.List)
                    {
                        MatrixController<T>.CreateMatrix(matrix);
                    }
                    else
                    {
                        //костыль (:
                        //зато работает!
                        var side = grid.side;
                        grid = MatrixController<T>.CreateMatrix(matrix);
                        OperationController<T>.ExecuteDragDrop(side, grid, opView);
                        MatrixController<T>.DeleteMatrix(matrix);
                    }
                }
            }
            else
            {
                //тоже костыль :(
                opView = OperationController<T>.CreateEmptyOperation();
                var left = operation.leftMatrix;
                var right = operation.rightMatrix;
                if (left!=null)
                {
                    
                    var leftGrid = MatrixController<T>.CreateMatrix(left);
                    OperationController<T>.ExecuteDragDrop(WorkingPanelSide.Left, leftGrid, opView);
                    MatrixController<T>.DeleteMatrix(leftGrid);
                }
                if(right!=null)
                {
                    var rightGrid = MatrixController<T>.CreateMatrix(right);
                    OperationController<T>.ExecuteDragDrop(WorkingPanelSide.Right, rightGrid, opView);
                    MatrixController<T>.DeleteMatrix(rightGrid);

                }


            }
           
        }
    }
}

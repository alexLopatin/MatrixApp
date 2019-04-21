using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KursachSAS.Model;
using KursachSAS.View;
using System.Windows.Forms;

namespace KursachSAS.Controller
{
    public static class OperationController<T> where T:struct
    {
        public static Control OperationsPanel;
        private static List<Operation<T>> operations = new List<Operation<T>>();
        private static List<KeyValuePair<Operation<T>, OperationView>> operationsView = new List<KeyValuePair<Operation<T>, OperationView>>();
        public static List<IOperable<T>> operationCalculators = new List<IOperable<T>>();
        static OperationController()
        {

            operationCalculators.Add(new SumOperation<T>());
            operationCalculators.Add(new DiffOperation<T>());
            operationCalculators.Add(new TransposeOperation<T>());
            operationCalculators.Add(new MultiplicationOperation<T>());
            operationCalculators.Add(new DeterminantOperation<T>());
            operationCalculators.Add(new SplitOperation<T>());
            operationCalculators.Add(new SubMatrixOperation<T>());
            operationCalculators.Add(new InsertSubMatrixOperation<T>());
        }

        public static OperationView GetOperationView(Matrix<T> operand)
        {
            var operations = operationsView.Where(e => (
            (e.Key.leftMatrix == operand) ||
            (e.Key.rightMatrix == operand) ||
             (e.Key.ResultMatrix == operand))
             ).ToList();
            if (operations.Count > 0)
                return operations[0].Value;
            else
                return null;
        }

        public static OperationView CreateEmptyOperation()
        {
            Operation<T> op = new Operation<T>();
            operations.Add(op);
            OperationView ov = new OperationView();
            
            operationsView.Add(new KeyValuePair<Operation<T>, OperationView>(op, ov));
            ov.Parent = OperationsPanel;
            ov.Dock = DockStyle.Top;
            ov.operationLabel.Text = op.GetStringRepresentation();
            op.argumentsInvalid += (o, e) => ov.argumentsInvalid.Invoke(o, e);
            ov.изменитьАргументыToolStripMenuItem.Enabled = false;
            return ov;
        }
        public static MatrixGrid Calculate(OperationView opView)
        {
            var operationPair = operationsView.Where(e => e.Value == opView).ToList()[0];
            if (operationPair.Key.leftMatrix == null && operationPair.Key.rightMatrix == null)
            {
                var matr = new Matrix<T>(1, 1);
                var gr =  MatrixController<T>.CreateMatrix(matr);
                gr.side = WorkingPanelSide.Result;
                
                gr.Parent = opView.resultPanel;
                if (operationPair.Key.ResultMatrix != null)
                    MatrixController<T>.DeleteMatrix(operationPair.Key.ResultMatrix);
                operationPair.Key.ResultMatrix = matr;
                return gr;
            }
                


            if (operationPair.Key.ResultMatrix != null)
                MatrixController<T>.DeleteMatrix(operationPair.Key.ResultMatrix);
            var t = MatrixController<T>.matrices;
            var matrix = operationPair.Key.Calculate();
            var grid = MatrixController<T>.CreateMatrix(matrix);
            var left =MatrixController<T>.GetMatrixGrid(operationPair.Key.leftMatrix);
            grid.lines = matrix.lines;
            grid.Parent = opView.resultPanel;
            grid.side = WorkingPanelSide.Result;
            return grid;
        }
        
        public static IOperable<T> GetOperationType(OperationView opView)
        {
            var operationPair = operationsView.Where(e => e.Value == opView).ToList()[0];
            return operationPair.Key.operationCalculator;
        }

        public static void ChangeOperationType(OperationView opView, IOperable<T> opCalc)
        {
            var operationPair = operationsView.Where(e => e.Value == opView).ToList()[0];
            operationPair.Key.SetOperationCalculator(opCalc);
            opView.operationLabel.Text = operationPair.Key.GetStringRepresentation();
            
            if(opCalc.operationType==OperationType.Unary|| opCalc.operationType == OperationType.Multiarial)
            {
                opView.workingPanelRight.Enabled = false;
                opView.workingPanelRight.Visible = false;
                opView.isUnaryOrMultiarial = true;
            }
            else
            {
                opView.workingPanelRight.Enabled = true;
                opView.workingPanelRight.Visible = true;
                opView.isUnaryOrMultiarial = false;
            }
            if (operationPair.Key.operationCalculator.operationType == OperationType.Multiarial)
                opView.изменитьАргументыToolStripMenuItem.Enabled = true;
            else
                opView.изменитьАргументыToolStripMenuItem.Enabled = false;

            operationPair.Value.argumentsChanged.Invoke(opCalc, null);
        }

        public static void OpenEditForm(OperationView opView)
        {
            var operationPair = operationsView.Where(e => e.Value == opView).ToList()[0];
            var list = operationPair.Key.operationCalculator.DataBinds;
            EditOperandForm EOF = new EditOperandForm(list, opView);
            EOF.Show();
        }

        public static void ExecuteDragDrop(WorkingPanelSide side, MatrixGrid grid, OperationView opView)
        {
            
            var operationPair = operationsView.Where(e => e.Value == opView).ToList()[0];
            var oldMatrix = MatrixController<T>.matricesView.Where(e => e.Value == grid).ToList();
            var matrix = (Matrix<T>)oldMatrix[0].Key.Clone();
            var newGrid = MatrixController<T>.CreateMatrix(matrix);
            if (side == WorkingPanelSide.Left)
            {
                newGrid.Parent = operationPair.Value.workingPanelLeft;
                newGrid.side = WorkingPanelSide.Left;
                operationPair.Key.leftMatrix = matrix;
                matrix.Changed += (o, e) => operationPair.Value.argumentsChanged.Invoke(matrix, null);
                matrix.DisposeEvent += (o, e) =>
                 {

                     operationPair.Value.leftNotNull = false;
                   
                 };
            }
            else
                if (side == WorkingPanelSide.Right)
            {
                newGrid.Parent = operationPair.Value.workingPanelRight;
                newGrid.side = WorkingPanelSide.Right;
                operationPair.Key.rightMatrix = matrix;
                matrix.Changed += (o, e) => operationPair.Value.argumentsChanged.Invoke(matrix, null);
                matrix.DisposeEvent += (o, e) =>
                {
                    operationPair.Value.rightNotNull = false;

                };
            }
            newGrid.lines = matrix.lines;
            newGrid.MatrixName = grid.MatrixName;
            if (grid.dragDropExecuted != null)
                grid.dragDropExecuted.Invoke(newGrid, null);

        }
        public static void DoDragDrop(MatrixGrid grid, Matrix<T> matrix)
        {

            var operation = operationsView.Where(e =>(
            (e.Key.leftMatrix == matrix)  || 
            (e.Key.rightMatrix== matrix) ||
             (e.Key.ResultMatrix == matrix))
             ).ToList()[0].Value;

            if (grid.side == WorkingPanelSide.Left)
                operation.workingPanelLeft.AllowDrop = false;
            else if (grid.side == WorkingPanelSide.Right)
                operation.workingPanelRight.AllowDrop = false;
            else if (grid.side == WorkingPanelSide.Result)
                operation.resultPanel.AllowDrop = false;
            MatrixController<T>.panel.AllowDrop = true;
            grid.DoDragDrop(grid, DragDropEffects.Copy);
        }
        public static void DeleteArgument(OperationView opView, WorkingPanelSide side)
        {
            var operationList = operationsView.Where(e => e.Value == opView).ToList();
            //if (operationList.Count == 0)
            //    return;


            var operationPair = operationList[0];
            if(side==WorkingPanelSide.Left&& operationPair.Key.leftMatrix!=null)
                MatrixController<T>.DeleteMatrix(operationPair.Key.leftMatrix);
            else if (side == WorkingPanelSide.Right && operationPair.Key.rightMatrix != null)
                MatrixController<T>.DeleteMatrix(operationPair.Key.rightMatrix);
            else if (side == WorkingPanelSide.Result && operationPair.Key.ResultMatrix != null)
                MatrixController<T>.DeleteMatrix(operationPair.Key.ResultMatrix);

        }
        public static Operation<T> GetOperation(OperationView opView)
        {
            var operationPair = operationsView.Where(e => e.Value == opView).ToList()[0];
            return operationPair.Key;

        }

        public static void DeleteOperation(OperationView opView)
        {
            var operationPair = operationsView.Where(e => e.Value == opView).ToList()[0];
            DeleteArgument(opView, WorkingPanelSide.Left);
            DeleteArgument(opView, WorkingPanelSide.Right);
            DeleteArgument(opView, WorkingPanelSide.Result);
            operations.Remove(operationPair.Key);
            operationsView.Remove(operationPair);
            
            operationPair.Value.Dispose();
        }
    }

}

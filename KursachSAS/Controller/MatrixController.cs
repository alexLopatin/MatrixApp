using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KursachSAS.View;
using System.IO;

namespace KursachSAS.Controller
{
    static class MatrixController<T> where T:struct
    {
        public static Control panel;
        public static List<Matrix<T>> matrices = new List<Matrix<T>>();
        public static List<KeyValuePair<Matrix<T>, MatrixGrid>> matricesView = new List<KeyValuePair<Matrix<T>, MatrixGrid>>();

        public static MatrixGrid CreateMatrix(T[,] data )
        {
            
            Matrix<T> matrix = new Matrix<T>(data.GetLength(0), data.GetLength(1));
            matrices.Add(matrix);
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                    matrix[i, j] = data[i, j];
            var grid = new View.MatrixGrid() { Parent = panel, Dock=DockStyle.None };
            matrix.DisposeEvent += (o, e) => grid.Dispose();

            matrix.SizeChanged += (o, e) => grid.GridSize = new Size(matrix.columnsCount, matrix.rowsCount);
            matrix.Changed += (o, e) => grid.Invalidate();
            grid.GridSize = new Size(data.GetLength(1), data.GetLength(0));
            grid.lines =  matrix.lines;
            grid.CellNeeded += (o, e) =>
            {
                try
                {
                    e.Value = matrix[e.Cell.Y, e.Cell.X].ToString();
                }
                catch
                {
                    e.Value = "err";
                }
            };
            
            
            matricesView.Add(new KeyValuePair<Matrix<T>, MatrixGrid>(matrix, grid));
            return grid;
        }

        public static void Insert(MatrixGrid oldGrid, MatrixGrid newGrid)
        {
            var matrixPair = matricesView.Where(e => e.Value == oldGrid).ToList()[0];
            var matrix = matricesView.Where(e => e.Value == newGrid).ToList()[0].Key;
            var side = oldGrid.side;
            var parent = oldGrid.Parent;

            matricesView.Remove(matrixPair);
            matrices.Remove(matrixPair.Key);
            //DeleteMatrix(oldGrid);

            var newMatrix = (Matrix<T>)matrix.Clone();

            var grid = CreateMatrixGrid(newMatrix);
            grid.GridSizeChanged = oldGrid.GridSizeChanged;
            matrices.Add(newMatrix);
            matricesView.Add(new KeyValuePair<Matrix<T>, MatrixGrid>(newMatrix, grid));
            grid.Parent = parent;
            grid.side = side;
            grid.GridSize = grid.GridSize;
            oldGrid.Dispose();
            var oldMatrix = matrixPair.Key;
            Matrix<T>.Copy(newMatrix, oldMatrix);
        }

        public static Matrix<T>  GetMatrix(MatrixGrid grid)
        {
            var matrixPairs = matricesView.Where(e => e.Value == grid).ToList();

            if (matrixPairs.Count > 0)
                return matrixPairs[0].Key;
            else
                return null;
        }
        public static MatrixGrid GetMatrixGrid(Matrix<T> matrix)
        {
            var matrixPairs = matricesView.Where(e => e.Key == matrix).ToList();

            if (matrixPairs.Count > 0)
                return matrixPairs[0].Value;
            else
                return null;
        }

        private static MatrixGrid CreateMatrixGrid(Matrix<T> matrix)
        {
            var grid = new View.MatrixGrid() { Parent = panel, Dock = DockStyle.None };
            matrix.DisposeEvent += (o, e) => grid.Dispose();
            matrix.SizeChanged += (o, e) => grid.GridSize = new Size(matrix.columnsCount, matrix.rowsCount);
            grid.GridSize = new Size(matrix.columnsCount, matrix.rowsCount);
            matrix.Changed += (o, e) => grid.Invalidate();
            grid.lines = matrix.lines;
            grid.CellNeeded += (o, e) =>
            {
                try
                {
                    e.Value = matrix[e.Cell.Y, e.Cell.X].ToString();
                }
                catch
                {
                    e.Value = "err";
                }
            };

            return grid;
        }

        public static MatrixGrid CreateMatrix(Matrix<T> matrix)
        {

            matrices.Add(matrix);
            var grid = new View.MatrixGrid() { Parent = panel, Dock = DockStyle.None };
            matrix.DisposeEvent += (o, e) => grid.Dispose();
            matrix.SizeChanged += (o, e) => grid.GridSize = new Size(matrix.columnsCount, matrix.rowsCount);
            grid.GridSize = new Size(matrix.columnsCount, matrix.rowsCount);
            matrix.Changed += (o, e) => grid.Invalidate();
            grid.lines = matrix.lines;
            grid.CellNeeded += (o, e) =>
            {
                try
                {
                    e.Value = matrix[e.Cell.Y, e.Cell.X].ToString();
                }
                catch
                {
                    e.Value = "err";
                }
            };

            matricesView.Add(new KeyValuePair<Matrix<T>, MatrixGrid>(matrix, grid));
            return grid;
        }
        public static void PrintToFile(MatrixGrid grid)
        {
            var matrix = matricesView.Where(e => e.Value == grid).ToList()[0].Key;
            using (var SFD = new SaveFileDialog())
            {
                SFD.FileName = grid.MatrixName;
                SFD.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                DialogResult result = SFD.ShowDialog();
                if (result == DialogResult.Cancel)
                    return;
                string filename = SFD.FileName;
                string text = "";
                for(int i=0; i<matrix.rowsCount; i++)
                {
                    for (int j = 0; j < matrix.columnsCount; j++)
                        text += matrix[i, j].ToString() +((j==matrix.columnsCount-1)? "":" ");
                    text += '\n';
                }
                File.WriteAllText(filename, text);

            }
        }
        
        


        public static ControllerData<T> DeleteMatrix(Matrix<T> matrix)
        {
            var matrixPair = matricesView.Where(e => e.Key == matrix).ToList()[0];

            matricesView.Remove(matrixPair);
            matrixPair.Key.Dispose();
            matrices.Remove(matrixPair.Key);
            return new ControllerData<T>(GetData(matrixPair.Key), matrixPair.Key);
        }
        public static ControllerData<T> DeleteMatrix(MatrixGrid mg)
        {
            var matrixPair = matricesView.Where(e => e.Value == mg).ToList()[0];

            matricesView.Remove(matrixPair);
            matrixPair.Key.Dispose() ;
            matrices.Remove(matrixPair.Key);
            return new ControllerData<T>(GetData(matrixPair.Key), matrixPair.Key);
        }
        private static T[,] GetData(Matrix<T> matrix)
        {
            T[,] data = new T[matrix.rowsCount, matrix.columnsCount];
            for (int i = 0; i < matrix.rowsCount; i++)
                for (int j = 0; j < matrix.columnsCount; j++)
                    data[i, j] = matrix[i, j];

            return data;
        }

        public static ControllerData<T> EditMatrix(MatrixGrid mg)
        {
            var matrixPair = matricesView.Where(e => e.Value == mg).ToList()[0];
            //currentEditMatrix = matrixPair.Key;
            EditMatrix editMatrixForm = new EditMatrix(MatrixEditMode.Edit, matrixPair.Key.rowsCount, matrixPair.Key.columnsCount, matrixPair.Key, matrixPair.Value.MatrixName, true);
            var dgv = editMatrixForm.dataGridView1;
            dgv.RowCount = matrixPair.Key.rowsCount;
            dgv.ColumnCount = matrixPair.Key.columnsCount;
            for (int i = 0; i < matrixPair.Key.rowsCount; i++)
                for (int j = 0; j < matrixPair.Key.columnsCount; j++)
                    dgv[j, i].Value = matrixPair.Key[i,j].ToString();
            
            editMatrixForm.Show();
            return new ControllerData<T>(GetData(matrixPair.Key), matrixPair.Key);
        }
        public static void EditMatrix(T[,] data, Matrix<T> matrix, string name)
        {
            var matrixPair = matricesView.Where(e => e.Key == matrix).ToList()[0];
            matrixPair.Value.MatrixName = name;
            matrix.Resize(data.GetLength(0), data.GetLength(1));
            for (int i = 0; i < matrix.rowsCount; i++)
                for (int j = 0; j < matrix.columnsCount; j++)
                    matrix[i, j] = data[i,j];

        }
        public static void EditMatrix(T[,] data, Matrix<T> matrix)
        {
            var matrixPair = matricesView.Where(e => e.Key == matrix).ToList()[0];
            matrix.Resize(data.GetLength(0), data.GetLength(1));
            for (int i = 0; i < matrix.rowsCount; i++)
                for (int j = 0; j < matrix.columnsCount; j++)
                    matrix[i, j] = data[i, j];

        }
        public static void DoDragDrop(MatrixGrid grid)
        {
            if (grid.side != WorkingPanelSide.List)
            {
                var matrixPair = matricesView.Where(e => e.Value == grid).ToList()[0];
                OperationController<T>.DoDragDrop(grid, matrixPair.Key);
                return;
            }
                
            
            panel.AllowDrop = false;
            grid.DoDragDrop(grid, DragDropEffects.Copy);
            

        }

        public static void GetSubMatrix(MatrixGrid grid, Point cell)
        {
            var matrixPair = matricesView.Where(e => e.Value == grid).ToList()[0];
            var matrix = matrixPair.Key.GetSubMatrix(cell);
            CreateMatrix(matrix);
        }

        public static MatrixGrid ExecuteDragDrop(MatrixGrid gridDragDrop)
        {

            
                var matrixPair = matricesView.Where(e => e.Value == gridDragDrop).ToList()[0];
                var matrix = (Matrix<T>)matrixPair.Key.Clone();
                var newGrid = CreateMatrix(matrix);
            var matrixPairList = matricesView.Where(e => e.Value.side == gridDragDrop.side).ToList();
            if (gridDragDrop.dragDropExecuted != null)
                gridDragDrop.dragDropExecuted.Invoke(newGrid, null);
            newGrid.lines = matrix.lines;
            newGrid.MatrixName = gridDragDrop.MatrixName;
            return newGrid;
        }
    }
    public enum WorkingPanelSide { Left,Right, Result, List}
    public class ControllerData<T> where T:struct
    {
        public T[,] oldData { get; set; }
        public Matrix<T> matrix { get; set; }
        public ControllerData(T[,] oldData, Matrix<T> matrix)
        {
            this.oldData = oldData;
            this.matrix = matrix;
        }
    }
}

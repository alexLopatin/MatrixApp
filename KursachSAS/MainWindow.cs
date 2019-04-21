using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KursachSAS.View;
using KursachSAS.Controller;
using KursachSAS.Command;
using System.IO;

namespace KursachSAS
{
    public partial class MainWindow : Form
    {

        public CommandInvoker commandInvoker;
        private CreateCommand<int> createCommand;
        public MainWindow()
        {
            InitializeComponent();
            MatrixController<int>.panel = ListPanel;
            commandInvoker = CommandInvoker.getInstance();
            createCommand = new CreateCommand<int>(CreationType.Operation);
            OperationController<int>.OperationsPanel = panel2;
            ListPanel.Paint += Panel1_Paint;
            createCommand.Execute();
            
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            //for(int i =0; panel1.Controls.c)
            int topOffset = 0;
            int padding = 10;
            foreach(Control control in ListPanel.Controls)
            {
                control.Left = (ListPanel.ClientSize.Width - control.Width) / 2;
                control.Top = topOffset+padding- ListPanel.VerticalScroll.Value;
                topOffset += control.Height + padding;
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            EditMatrix editMatrixForm = new EditMatrix(MatrixEditMode.Create, 0,0, null, "");
            editMatrixForm.Show();
        }
        
        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            var grid = (MatrixGrid)e.Data.GetData(typeof(MatrixGrid));
            MatrixController<int>.ExecuteDragDrop(grid);
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void workingPanelLeft_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            createCommand = new CreateCommand<int>(CreationType.Operation);
            commandInvoker.SetCommand(createCommand);
            commandInvoker.Run();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z&&e.Control)
                commandInvoker.Cancel();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
             
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reference refer = new Reference();
            refer.Show();
        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                using (var SFD = new OpenFileDialog())
                {
                    SFD.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                    DialogResult result = SFD.ShowDialog();
                    if (result == DialogResult.Cancel)
                        return;
                    string filename = SFD.FileName;
                    //string text = File.ReadAllText(filename);
                    var text = File.ReadAllLines(filename);

                    // Split on `,`, convert to int32, add to array, add to outer array
                    var res = JaggedToMultidimensional(text.Select(x => (x.Split(' ').Select(Int32.Parse).ToArray())).ToArray());
                    int[,] data = new int[res.GetLength(0), res.GetLength(1)];
                    for (int i = 0; i < res.GetLength(0); i++)
                        for (int j = 0; j < res.GetLength(1); j++)
                            data[i, j] = res[i, j];
                    var grid = MatrixController<int>.CreateMatrix(data);
                    grid.MatrixName = filename.Split('\\').Last().Split('.').First();
                }

            }
            catch
            {

            }
            
        }
        public static V[,] JaggedToMultidimensional<V>(V[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray.Max(subArray => subArray.Length);
            V[,] array = new V[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                cols = jaggedArray[i].Length;
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = jaggedArray[i][j];
                }
            }
            return array;
        }
    }
    
}

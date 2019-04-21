using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KursachSAS.Controller;
using KursachSAS.View;
using KursachSAS.Command;

namespace KursachSAS
{
    public enum MatrixEditMode { Create, Edit};
    public partial class EditMatrix :Form
    {
        MatrixEditMode MEM;
        private object matrixReference;
        private CommandInvoker comInvoker;
        private CreateCommand<int> createCommand;
        public EditMatrix(MatrixEditMode mem, int rows, int columns, object matrixReference, string name, bool canBeSizeChanged=true )
        {
            comInvoker = CommandInvoker.getInstance();
            createCommand = new CreateCommand<int>(CreationType.Matrix);
            MEM = mem;
            InitializeComponent();
            if (MEM == MatrixEditMode.Create)
                button1.Text = "Создать";
            else if (MEM == MatrixEditMode.Edit)
                button1.Text = "Изменить";
            textBox1.Text = rows.ToString();
            textBox2.Text = columns.ToString();
            textBox1.TextChanged += TextBox1_TextChanged;
            textBox2.TextChanged += TextBox2_TextChanged;
            textBox3.Text = name;
            this.matrixReference = matrixReference;

            if (!canBeSizeChanged)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
            }
                
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            int columns = 0;
            if (Int32.TryParse(textBox2.Text, out columns))
                dataGridView1.ColumnCount = columns;
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                for (int j = 0; j < dataGridView1.RowCount; j++)
                    dataGridView1[i, j].Value = 0;
        }

        private void TextBox1_TextChanged(object sender, System.EventArgs e)
        {
            int rows = 0;
            if (Int32.TryParse(textBox1.Text, out rows))
                dataGridView1.RowCount = rows;
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                for (int j = 0; j < dataGridView1.RowCount; j++)
                    dataGridView1[i, j].Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[,] data = new int[dataGridView1.RowCount, dataGridView1.ColumnCount];
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                for (int j = 0; j < dataGridView1.RowCount; j++)
                    data[j, i] = Convert.ToInt32(dataGridView1[i,j].Value);
            if (MEM == MatrixEditMode.Create)
            {
                createCommand.data = data;
                createCommand.name = textBox3.Text;
                comInvoker.SetCommand(createCommand);
                comInvoker.Run();
            }
            else if (MEM == MatrixEditMode.Edit)
                MatrixController<int>.EditMatrix(data, (Matrix<int>)matrixReference, textBox3.Text);
            this.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}

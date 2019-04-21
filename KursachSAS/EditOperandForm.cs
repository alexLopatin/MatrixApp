using KursachSAS.Controller;
using KursachSAS.Model;
using KursachSAS.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursachSAS
{
    public partial class EditOperandForm : Form
    {
        private OperationView opView;
        public EditOperandForm(List<IData> data, OperationView opView)
        {
            InitializeComponent();
            this.opView = opView;
            foreach(IData info in data)
            {
                if (info.GetType() == typeof(DataBind<int>))
                {
                    var bind = (DataBind<int>)(info);
                    Panel p = new Panel();

                    TextBox tb = new TextBox();
                    tb.TextChanged += (o, e) => bind.data = Convert.ToInt32(tb.Text);
                    tb.Text = bind.data.ToString();
                    tb.Dock = DockStyle.Left;
                    tb.Width = 50;
                    p.Controls.Add(tb);
                    Label l = new Label();
                    l.Text = bind.information[0];
                    l.Dock = DockStyle.Left;
                    p.Controls.Add(l);
                    l.Width = 100;
                    p.Padding = new Padding(10);
                    panel1.Controls.Add(p);
                    p.Dock = DockStyle.Top;
                    p.BackColor = Color.White;
                }
                if (info.GetType() == typeof(DataBind<bool>))
                {
                    var bind = (DataBind<bool>)(info);
                    Panel p = new Panel();

                    RadioButton RB = new RadioButton();
                    
                    RB.CheckedChanged += (o, e) => bind.data = true;
                    RB.Text = bind.information[1];
                    RB.Dock = DockStyle.Top;
                    p.Controls.Add(RB);
                    RadioButton RB1 = new RadioButton();
                    RB1.CheckedChanged += (o, e) => bind.data = false;
                    if (bind.data)
                        RB.Checked = true;
                    else
                        RB1.Checked = true;
                    RB1.Text = bind.information[2];
                    RB1.Dock = DockStyle.Top;
                    p.Controls.Add(RB1);
                    Label l = new Label();
                    l.Text = bind.information[0];
                    l.Dock = DockStyle.Top;
                    p.Controls.Add(l);


                    p.Padding = new Padding(10);
                    p.Dock = DockStyle.Top;
                    panel1.Controls.Add(p);
                    
                    p.BackColor = Color.White;
                }
                if (info.GetType() == typeof(DataBind<Matrix<int>>))
                {

                    var bind = (DataBind<Matrix<int>>)(info);
                    var grid = MatrixController<int>.CreateMatrix(bind.data);
                    //grid.OnlyEditValues = true;
                    Panel p = new Panel();
                    grid.Parent = p;
                    grid.Dock = DockStyle.Top;
                    Label l = new Label();
                    l.Text = bind.information[0];
                    l.Dock = DockStyle.Top;
                    p.Dock = DockStyle.Top;
                    panel1.Controls.Add(p);
                    p.Controls.Add(l);
                    p.Dock = DockStyle.Top;
                    grid.GridSizeChanged+=(o,e)=> p.Height = p.Height + grid.GridSize.Height * 15;
                    p.Height = p.Height+grid.GridSize.Height*15;
                    p.BackColor = Color.White;
                }
            }
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            OperationController<int>.Calculate(opView);
            Dispose();
        }
    }
}

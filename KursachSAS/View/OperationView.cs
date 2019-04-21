using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KursachSAS.Controller;
using System.Drawing;
using KursachSAS.Model;
using KursachSAS.Command;

namespace KursachSAS.View
{
    public class OperationView : UserControl
    {
        private Label label1;
        public Panel resultPanel;
        public Panel workingPanelRight;
        public Panel workingPanelLeft;
        private Panel OperationPanel;
        private CommandInvoker comInvoker;
        public bool leftNotNull = false;
        public bool rightNotNull = false;
        private Label closeButton;
        public Label operationLabel;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        public EventHandler<EventArgs> argumentsChanged;
        public EventHandler<EventArgs> argumentsInvalid;
        public bool isUnaryOrMultiarial = false;

        private DeleteCommand<int> deleteCommand;
        public ToolStripMenuItem изменитьАргументыToolStripMenuItem;
        private ChangeOperationTypeCommand<int> changeCommand;
        public OperationView()
        {
            
            InitializeComponent();
            ClientSize = OperationPanel.Size;
            workingPanelLeft.AllowDrop = true;
            argumentsChanged += (o, e) =>
            {
                label1.Text = "=";
                label1.ForeColor = Color.Black;
                


                try
                {
                    if (leftNotNull && (rightNotNull || isUnaryOrMultiarial))
                        OperationController<int>.Calculate(this);
                }
                catch
                {
                    
                }
                

            };
            argumentsInvalid += (o, e) =>
              {
                  label1.Text = "x";
                  label1.ForeColor = Color.Red;
              };
            deleteCommand = new DeleteCommand<int>(this);
            changeCommand = new ChangeOperationTypeCommand<int>(this);
            comInvoker = CommandInvoker.getInstance();
            foreach (IOperable<int> opCalc in OperationController<int>.operationCalculators)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = opCalc.representation;
                item.Click += (o, e) =>
                {
                    changeCommand.newType = opCalc;
                    comInvoker.SetCommand(changeCommand);
                    comInvoker.Run();
                };
                contextMenuStrip1.Items.Add(item);
            }
            

        }



        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.workingPanelRight = new System.Windows.Forms.Panel();
            this.workingPanelLeft = new System.Windows.Forms.Panel();
            this.OperationPanel = new System.Windows.Forms.Panel();
            this.operationLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.изменитьАргументыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OperationPanel.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(422, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "=";
            // 
            // resultPanel
            // 
            this.resultPanel.AllowDrop = true;
            this.resultPanel.BackColor = System.Drawing.Color.White;
            this.resultPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.resultPanel.Location = new System.Drawing.Point(473, 10);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(136, 133);
            this.resultPanel.TabIndex = 4;
            this.resultPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.workingPanelLeft_Paint);
            // 
            // workingPanelRight
            // 
            this.workingPanelRight.AllowDrop = true;
            this.workingPanelRight.BackColor = System.Drawing.Color.White;
            this.workingPanelRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.workingPanelRight.Cursor = System.Windows.Forms.Cursors.Default;
            this.workingPanelRight.Location = new System.Drawing.Point(250, 10);
            this.workingPanelRight.Name = "workingPanelRight";
            this.workingPanelRight.Size = new System.Drawing.Size(136, 133);
            this.workingPanelRight.TabIndex = 4;
            this.workingPanelRight.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel2_DragEnter);
            this.workingPanelRight.Paint += new System.Windows.Forms.PaintEventHandler(this.workingPanelLeft_Paint);
            // 
            // workingPanelLeft
            // 
            this.workingPanelLeft.AllowDrop = true;
            this.workingPanelLeft.BackColor = System.Drawing.Color.White;
            this.workingPanelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.workingPanelLeft.Cursor = System.Windows.Forms.Cursors.Default;
            this.workingPanelLeft.Location = new System.Drawing.Point(3, 10);
            this.workingPanelLeft.Name = "workingPanelLeft";
            this.workingPanelLeft.Size = new System.Drawing.Size(136, 133);
            this.workingPanelLeft.TabIndex = 3;
            this.workingPanelLeft.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel2_DragEnter);
            this.workingPanelLeft.Paint += new System.Windows.Forms.PaintEventHandler(this.workingPanelLeft_Paint);
            // 
            // OperationPanel
            // 
            this.OperationPanel.AllowDrop = true;
            this.OperationPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.OperationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OperationPanel.Controls.Add(this.operationLabel);
            this.OperationPanel.Controls.Add(this.closeButton);
            this.OperationPanel.Controls.Add(this.label1);
            this.OperationPanel.Controls.Add(this.resultPanel);
            this.OperationPanel.Controls.Add(this.workingPanelRight);
            this.OperationPanel.Controls.Add(this.workingPanelLeft);
            this.OperationPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.OperationPanel.Location = new System.Drawing.Point(0, 0);
            this.OperationPanel.Name = "OperationPanel";
            this.OperationPanel.Size = new System.Drawing.Size(613, 151);
            this.OperationPanel.TabIndex = 3;
            // 
            // operationLabel
            // 
            this.operationLabel.AutoSize = true;
            this.operationLabel.Location = new System.Drawing.Point(184, 71);
            this.operationLabel.Name = "operationLabel";
            this.operationLabel.Size = new System.Drawing.Size(13, 13);
            this.operationLabel.TabIndex = 8;
            this.operationLabel.Text = "+";
            this.operationLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.operationLabel_MouseDown);
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.25F);
            this.closeButton.Location = new System.Drawing.Point(600, 0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(8, 7);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "x";
            this.closeButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.closeButton_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.изменитьАргументыToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(192, 48);
            // 
            // изменитьАргументыToolStripMenuItem
            // 
            this.изменитьАргументыToolStripMenuItem.Name = "изменитьАргументыToolStripMenuItem";
            this.изменитьАргументыToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.изменитьАргументыToolStripMenuItem.Text = "Изменить аргументы";
            this.изменитьАргументыToolStripMenuItem.Click += new System.EventHandler(this.изменитьАргументыToolStripMenuItem_Click);
            // 
            // OperationView
            // 
            this.Controls.Add(this.OperationPanel);
            this.Name = "OperationView";
            this.Size = new System.Drawing.Size(616, 151);
            this.Load += new System.EventHandler(this.OperationView_Load);
            this.OperationPanel.ResumeLayout(false);
            this.OperationPanel.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            EditMatrix editMatrixForm = new EditMatrix(MatrixEditMode.Create,0,0,null,"");
            editMatrixForm.Show();
        }

        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(MatrixGrid)))
                return;
            var grid = (MatrixGrid)e.Data.GetData(typeof(MatrixGrid));
            var t = grid.side;
            if (sender == workingPanelLeft)
            {
                if (leftNotNull)
                    OperationController<int>.DeleteArgument(this, WorkingPanelSide.Left);
                leftNotNull = true;
                
                OperationController<int>.ExecuteDragDrop(WorkingPanelSide.Left, grid, this);
                

            }
                
            else if (sender == workingPanelRight)
            {
                if (rightNotNull)
                    OperationController<int>.DeleteArgument(this, WorkingPanelSide.Right);
                rightNotNull = true;
                OperationController<int>.ExecuteDragDrop(WorkingPanelSide.Right, grid, this);
            }
                
            else if (sender == MatrixController<int>.panel)
                MatrixController<int>.ExecuteDragDrop(grid);
            MatrixController<int>.panel.AllowDrop = true;
            workingPanelLeft.AllowDrop = true;
            workingPanelRight.AllowDrop = true;
            resultPanel.AllowDrop = true;
            argumentsChanged.Invoke(this, null);
        }
        
        


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void workingPanelLeft_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void OperationView_Load(object sender, EventArgs e)
        {

        }

        private void workingPanelLeft_Paint(object sender, PaintEventArgs e)
        {
            
            Panel panel = (Panel)sender; 
            if (panel.Controls.Count == 0)
                return;
            Control control = panel.Controls[0];
            control.Left = (panel.ClientSize.Width - control.Width) / 2;
            control.Top = (panel.ClientSize.Height - control.Height) / 2 - panel.VerticalScroll.Value;
        }

        private void closeButton_MouseClick(object sender, MouseEventArgs e)
        {
            comInvoker.SetCommand(deleteCommand);
            comInvoker.Run();
        }

        private void operationLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, new Point(((Control)sender).Left, ((Control)sender).Top));
            }
        }

        private void изменитьАргументыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationController<int>.OpenEditForm(this);
        }
    }
}

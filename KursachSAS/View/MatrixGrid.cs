using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KursachSAS.Command;
using KursachSAS.Controller;

namespace KursachSAS.View
{
   
    public class MatrixGrid : UserControl, ICloneable
    {
        private CommandInvoker comInvoker;
        private Size smallSize;
        private Size gridSize;
        private bool onlyEditValues = false;
        public bool OnlyEditValues
        {
            get
            {
                return onlyEditValues;
            }
            set
            {
                onlyEditValues = value;
                printToolStripMenuItem.Enabled = !OnlyEditValues;
                разделитьToolStripMenuItem.Enabled = !OnlyEditValues;
                горизонтальноToolStripMenuItem.Enabled = !onlyEditValues;
                вертикальноToolStripMenuItem.Enabled = !onlyEditValues;
                deleteToolStripMenuItem1.Enabled = !OnlyEditValues;
                выделитьПодматрицуToolStripMenuItem.Enabled = !OnlyEditValues;
            }
        }
        public EventHandler<EventArgs> GridSizeChanged;
        public Size GridSize
        {
            get
            {
                if (gridSize.Height > 6 || gridSize.Width > 6)
                    return smallSize;
                else
                    return gridSize;
            }
            set
            {
                gridSize = value;

                if(gridSize.Height>6|| gridSize.Width>6)
                {
                    if(gridSize.Height>3)
                        smallSize.Height = 3;
                    else
                        smallSize.Height = gridSize.Height;
                    if (gridSize.Width > 3)
                        smallSize.Width = 3;
                    else
                        smallSize.Width = gridSize.Width;
                    desiredSize = new Size(40 + smallSize.Width * 20, ((smallSize.Height > 0) ? (smallSize.Height * 20 + 20) : (20 + 20)));

                }
                else
                    desiredSize = new Size(40 + gridSize.Width * 20, ((gridSize.Height > 0) ? (gridSize.Height * 20+20) : (20+20)));
                Size = desiredSize;
                DrawBrackets();
                if(GridSizeChanged!=null)
                GridSizeChanged.Invoke(this, null);
            }
        }

        public WorkingPanelSide side=WorkingPanelSide.List;
        public Point HoveredCell = new Point(-1, -1);
        private System.ComponentModel.IContainer components;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private Size desiredSize = new Size(40, 20);
        private DeleteCommand<int> deleteCommand;
        private EditCommand<int> editCommand;
        private ToolStripMenuItem printToolStripMenuItem;
        private DragDropCommand<int> dragDropCommand;
        private ToolStripMenuItem разделитьToolStripMenuItem;
        private ToolStripMenuItem вертикальноToolStripMenuItem;
        private ToolStripMenuItem горизонтальноToolStripMenuItem;
        private ToolStripMenuItem выделитьПодматрицуToolStripMenuItem;
        private ToolStripMenuItem копироватьToolStripMenuItem;
        private ToolStripMenuItem вставитьToolStripMenuItem;

        public override Size MinimumSize
        {
            get { return desiredSize; }
            set { }
        }
        public override Size MaximumSize
        {
            get { return desiredSize; }
            set { }
        }


        public event EventHandler<CellNeededEventArgs> CellNeeded;
        public event EventHandler<CellClickEventArgs> CellClick;

        public List<Point> lines;


        public MatrixGrid()
        {
            
            desiredSize = new Size(40, 40);
            DrawBrackets();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            
            InitializeComponent();
            editCommand = new EditCommand<int>(this);
            deleteCommand = new DeleteCommand<int>(this);
            dragDropCommand = new DragDropCommand<int>(this);
            comInvoker = CommandInvoker.getInstance();
            

        }
        private string matrixName = "";
        public string MatrixName

        {
            get
            {
                if (matrixName.Length > 6+GridSize.Width)
                    return matrixName.Substring(0, 6 + GridSize.Width) + "...";
                else
                    return matrixName;
            }
            set

            {
                matrixName = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            var gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.HighQuality;

            if (CellNeeded == null)
                return;
            var cw = (ClientSize.Width - 40) / GridSize.Width;
            var ch = (ClientSize.Height - 20) / GridSize.Height;
            var background = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

            gr.FillRectangle(Brushes.White, background);
            background.Inflate(-1, -1);
            int xOffset = 20;
            int yOffset = 10;
            gr.DrawImage(leftBracket, new Point(0, yOffset));
            gr.DrawImage(rightBracket, new Point(20 + 20 * GridSize.Width, yOffset));


            Rectangle rec = new Rectangle(0, 0, 40 + 20 * GridSize.Width, yOffset);
            gr.DrawString(MatrixName, Font, Brushes.Black, rec, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            rec = new Rectangle(0, desiredSize.Height - 10, 40 + 20 * GridSize.Width, yOffset);
            string size = gridSize.Height.ToString() + " x " + gridSize.Width.ToString();
            gr.DrawString(size, Font, Brushes.Black, rec, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            
            if (gridSize.Height > 6 || gridSize.Width > 6)
            {
                
                for (int j = 0; j < GridSize.Height; j++)
                    for (int i = 0; i < GridSize.Width; i++)
                    {
                        var cell = new Point(i, j);

                        //получаем значение ячейки от пользователя
                        var ea = new CellNeededEventArgs(cell);
                        CellNeeded(this, ea);

                        //рисуем ячейку
                        var rect = new Rectangle(cw * i + xOffset, ch * j + yOffset, cw, ch);
                        rect.Inflate(-1, -1);

                        //if (cell == HoveredCell)
                        //    gr.DrawRectangle(Pens.Red, rect);

                        //фон
                        if (ea.BackColor != Color.Transparent)
                            using (var brush = new SolidBrush(ea.BackColor))
                                gr.FillRectangle(brush, rect);

                        //текст
                        bool b1 = (i != 1);
                        if (GridSize.Width != 3)
                            b1 = true;
                        bool b2 = (j != 1);
                        if (GridSize.Height != 3)
                            b2 = true;
                        if (!string.IsNullOrEmpty(ea.Value) && b1 && b2)
                            gr.DrawString(ea.Value, Font, Brushes.Black, rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        else if((i!=1||j!=1)||b1||b2)
                            gr.DrawString("...", Font, Brushes.Black, rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }
                
            }
            else
            {

                foreach (Point p in lines)
                {
                    if (p.Y == 0)
                        gr.DrawLine(Pens.Black, cw * p.X + xOffset, yOffset, cw * p.X + xOffset, ch * GridSize.Height + yOffset);
                    else
                        gr.DrawLine(Pens.Black, xOffset, ch * p.X + yOffset - 1, cw * GridSize.Width + xOffset, ch * p.X + yOffset - 1);
                }
                for (int j = 0; j < GridSize.Height; j++)
                    for (int i = 0; i < GridSize.Width; i++)
                    {
                        var cell = new Point(i, j);

                        //получаем значение ячейки от пользователя
                        var ea = new CellNeededEventArgs(cell);
                        CellNeeded(this, ea);

                        //рисуем ячейку
                        var rect = new Rectangle(cw * i + xOffset, ch * j + yOffset, cw, ch);
                        rect.Inflate(-1, -1);

                        //if (cell == HoveredCell)
                        //    gr.DrawRectangle(Pens.Red, rect);

                        //фон
                        if (ea.BackColor != Color.Transparent)
                            using (var brush = new SolidBrush(ea.BackColor))
                                gr.FillRectangle(brush, rect);

                        //текст

                        string val = ea.Value;
                        if (val.Length > 2)
                            val = val[0] + "..";

                        if (!string.IsNullOrEmpty(ea.Value))
                            gr.DrawString(val, Font, Brushes.Black, rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
            }

            
            Parent.Invalidate();
            
        }

        private Image leftBracket;
        private Image rightBracket;

        private void DrawBrackets()
        {
            Bitmap leftBr = new Bitmap(20, desiredSize.Height - 20);
            Bitmap rightBr = new Bitmap(20, desiredSize.Height - 20);

            for (int i = 0; i < 20; i++)
                for (int j = 0; j < desiredSize.Height-10; j++)
                {
                    if (i > 7 && i < 9 && j > 5 && j < (desiredSize.Height - 20) - 5)
                        leftBr.SetPixel(i, j, Color.Black);
                    if (i > 7 && i < 16 && j > 5 && j < 7)
                        leftBr.SetPixel(i, j, Color.Black);
                    if (i > 7 && i < 16 && j > (desiredSize.Height - 20) - 7 && j < (desiredSize.Height - 20) - 5)
                        leftBr.SetPixel(i, j, Color.Black);
                }

            leftBracket = leftBr;
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < desiredSize.Height-20; j++)
                    rightBr.SetPixel(19 - i, j, leftBr.GetPixel(i, j));

            rightBracket = rightBr;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var loc = e.Location;
            loc.X = loc.X - 20;
            loc.Y = loc.Y - 10;

            var cell = PointToCell(loc);
            HoveredCell = cell;
            var cw = (ClientSize.Width - 40) / GridSize.Width;
            var ch = (ClientSize.Height - 20) / GridSize.Height;
            var x = (double)loc.X / cw;
            var y = (double)loc.Y / ch;
            currentLine.X = (int)Math.Round(x);
            currentLine.Y = (int)Math.Round(y);
            //return new Point(p.X / cw, p.Y / ch);
            Invalidate();

        }
        public  EventHandler<EventArgs> dragDropExecuted;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                var loc = e.Location;
                loc.X = loc.X - 20;
                loc.Y = loc.Y - 10;

                var cell = PointToCell(loc);
                OnCellClick(new CellClickEventArgs(cell, e.Location));
                HoveredCell = cell;
                comInvoker.SetCommand(dragDropCommand);
                comInvoker.Run();

            }
            else if (e.Button ==MouseButtons.Right)
            {
                var cell = PointToCell(e.Location);
                OnCellClick(new CellClickEventArgs(cell,e.Location));
                if(gridSize.Width>6|| gridSize.Height > 6)
                {
                    выделитьПодматрицуToolStripMenuItem.Enabled = false;
                    разделитьToolStripMenuItem.Enabled = false;
                }
                
                else
                {
                    выделитьПодматрицуToolStripMenuItem.Enabled = true &&!onlyEditValues;
                    разделитьToolStripMenuItem.Enabled = true;
                }
                    
                contextMenuStrip1.Show(this, new Point(e.X, e.Y));
            }
            
        }

        protected virtual void OnCellClick(CellClickEventArgs cellClickEventArgs)
        {
            if (CellClick != null)
                CellClick(this, cellClickEventArgs);
            /*
            var cw = ClientSize.Width / GridSize.Width;
            var ch = ClientSize.Height / GridSize.Height;
            double x = (float)(cellClickEventArgs.Point.X-20) / cw;
            double y = (float)(cellClickEventArgs.Point.Y-10)/ ch;
            if (y < 0)
                return;
            int px = (int)Math.Ceiling(x);
            int py = (int)Math.Ceiling(y);
            currentLine = new Point(py, px);
            */
        }
        Point currentLine = new Point(0, 0);
        Point PointToCell(Point p)
        {
            if(p.X<0||p.Y<0)
                return new Point(-1,-1);


            var cw = (ClientSize.Width-40) / GridSize.Width;
            var ch = (ClientSize.Height-20) / GridSize.Height;
            return new Point(p.X / cw, p.Y / ch);
        }

       

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.разделитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вертикальноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.горизонтальноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выделитьПодматрицуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.копироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вставитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem1,
            this.копироватьToolStripMenuItem,
            this.вставитьToolStripMenuItem,
            this.разделитьToolStripMenuItem,
            this.printToolStripMenuItem,
            this.выделитьПодматрицуToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(198, 180);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening_1);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.editToolStripMenuItem.Text = "Изменить";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            this.deleteToolStripMenuItem1.Text = "Удалить";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.DeleteToolStripMenuItem1_Click);
            // 
            // разделитьToolStripMenuItem
            // 
            this.разделитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вертикальноToolStripMenuItem,
            this.горизонтальноToolStripMenuItem});
            this.разделитьToolStripMenuItem.Name = "разделитьToolStripMenuItem";
            this.разделитьToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.разделитьToolStripMenuItem.Text = "Разделить";
            // 
            // вертикальноToolStripMenuItem
            // 
            this.вертикальноToolStripMenuItem.Name = "вертикальноToolStripMenuItem";
            this.вертикальноToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.вертикальноToolStripMenuItem.Text = "Вертикально";
            this.вертикальноToolStripMenuItem.Click += new System.EventHandler(this.разделитьToolStripMenuItem_Click);
            // 
            // горизонтальноToolStripMenuItem
            // 
            this.горизонтальноToolStripMenuItem.Name = "горизонтальноToolStripMenuItem";
            this.горизонтальноToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.горизонтальноToolStripMenuItem.Text = "Горизонтально";
            this.горизонтальноToolStripMenuItem.Click += new System.EventHandler(this.горизонтальноToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.printToolStripMenuItem.Text = "Напечатать";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // выделитьПодматрицуToolStripMenuItem
            // 
            this.выделитьПодматрицуToolStripMenuItem.Name = "выделитьПодматрицуToolStripMenuItem";
            this.выделитьПодматрицуToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.выделитьПодматрицуToolStripMenuItem.Text = "Выделить подматрицу";
            this.выделитьПодматрицуToolStripMenuItem.Click += new System.EventHandler(this.выделитьПодматрицуToolStripMenuItem_Click);
            // 
            // копироватьToolStripMenuItem
            // 
            this.копироватьToolStripMenuItem.Name = "копироватьToolStripMenuItem";
            this.копироватьToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.копироватьToolStripMenuItem.Text = "Копировать";
            this.копироватьToolStripMenuItem.Click += new System.EventHandler(this.копироватьToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // вставитьToolStripMenuItem
            // 
            this.вставитьToolStripMenuItem.Name = "вставитьToolStripMenuItem";
            this.вставитьToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.вставитьToolStripMenuItem.Text = "Вставить";
            this.вставитьToolStripMenuItem.Click += new System.EventHandler(this.вставитьToolStripMenuItem_Click);
            // 
            // MatrixGrid
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MatrixGrid";
            this.Load += new System.EventHandler(this.MatrixGrid_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void DeleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            comInvoker.SetCommand(deleteCommand);
            comInvoker.Run();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comInvoker.SetCommand(editCommand);
            comInvoker.Run();

        }

        public object Clone()
        {
            return null;
            //throw new NotImplementedException();
        }

        private void MatrixGrid_Load(object sender, EventArgs e)
        {

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MatrixController<int>.PrintToFile(this);
        }

        private void разделитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(currentLine.X!=0&&currentLine.X!=gridSize.Width)
            lines.Add(new Point(currentLine.X, 0));
            Invalidate();
        }

        private void горизонтальноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentLine.Y != 0 && currentLine.Y != gridSize.Height)
                lines.Add(new Point(currentLine.Y, 1));
            Invalidate();
        }

        private void contextMenuStrip1_Opening_1(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void выделитьПодматрицуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MatrixController<int>.GetSubMatrix(this, HoveredCell);
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BufferClass.getInstance().currentCopyObject = this;
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var data = BufferClass.getInstance().currentCopyObject;
            if(data.GetType()==typeof(MatrixGrid))
            {
                MatrixController<int>.Insert(this, (MatrixGrid)data);
            }
        }
    }
    public class CellNeededEventArgs : EventArgs
    {
        public Point Cell { get; private set; }
        public string Value { get; set; }
        public Color BackColor { get; set; }



        public CellNeededEventArgs(Point cell)
        {
            Cell = cell;
        }
    }

    public class CellClickEventArgs : EventArgs
    {
        public Point Cell { get; private set; }
        public Point Point { get; set; }

        public CellClickEventArgs(Point cell, Point point)
        {
            Point = point;
            Cell = cell;
        }
    }


}

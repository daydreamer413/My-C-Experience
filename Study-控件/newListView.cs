using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Study_控件
{
    public partial class newListView : UserControl
    {
        //接口说明
        //导入图片：需要在主窗体设置一个按钮实现选择图片并传入本控件
        //导出图片：在主窗体设置一个按钮调用接口：Selected_Image(bool[] selectpBox)
        #region 变量
        //传入数据
        public List<string> allImagePaths ;//图片路径集合,从0下标开始
        public List<string> allImageFileName ;//图片名称集合，从0下标开始
        int index = 0;//记录当前控件中的图片下标
        private int _lineCount = 6;
        [Description("可自定义一行显示的图片数量")]
        public int LineCount
        {
            get => _lineCount;
            set
            {
                if (value < 2)// 如果输入小于2，设为2
                    _lineCount = 2;
                else if (value > 6)// 如果输入大于6，设为6
                    _lineCount = 6;
                else
                    _lineCount = value;
            }
        }

        List<PictureBox> pictureBoxList = new List<PictureBox>();//图片框集合
       // List<Label> labelList = new List<Label>();//图片框对应的label集合   
        public List<Bitmap> selectedImages = new List<Bitmap>();//存放选中的图片,之后遍历输出
        public bool[] selectPBox = new bool[100];//用于多选，记录被选中的图片

        //底框
        private Rectangle Rect = new Rectangle(0,0,300,300);//蓝色选中框
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(0, 72, 145, 220));//填充的颜色：半透明蓝色
        private Brush selectionBrush2 = new SolidBrush(Color.FromArgb(100, 130, 170, 210));//填充的颜色：半透明淡蓝色
        
        //PictureBox和Label相关属性
        public int KLength = 300;
        public int KHeight = 300;
        public int PBoxWidth = 250;
        public int PBoxHeight = 180;
        private int start = 10;//开始的X位置
        private int start_Y = 10;//开始的Y位置
        private int gap = 5;//两个底框的X间距
        private int gap_Y = 5;//两个底框的Y间距
        private int gapK = 5;//底框与图片的X间距
        private int gapK_Y = 5;//底框与图片的Y间距
        private float fontSize = 12;//label的字体大小                                  
        #endregion

        public newListView()
        {
            InitializeComponent();
            allImagePaths = new List<string>();
            allImageFileName = new List<string>();

            PictureBox p1 = new PictureBox();
            Label lbl1 = new Label();
            pictureBoxList.Add(p1);//把索引为0先占了
          //  labelList.Add(lbl1);
        }

        #region 动态创建pictureBox和Label //备选方式：不推荐
        public void Create_PL(List<string> allImagePaths, List<string> allImageFileName)
        {
            LineCount_set(LineCount);//根据单行显示数量设置参数
            int count = allImagePaths.Count;//图片总数
            int line = count / LineCount;//记录有余数时的索引
            int index = 0;
            for(int j=0;j<line;j++)
            {
                for (int i = 0; i < LineCount; i++)
                {
                    //PictureBox的设置
                    PictureBox PBox = new PictureBox();
                    PBox.Size = new Size(PBoxWidth, PBoxHeight);
                    PBox.Image = Image.FromFile(allImagePaths[i+j*LineCount]);//导入图片
                    PBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    PBox.Name = "PBox_" + (i+j*LineCount + 1);//名称编号
                    //绑定属性
                    PBox.MouseEnter += new EventHandler(PBox_Enter);
                    PBox.Click += new EventHandler(PBox_Click);
                    PBox.MouseLeave += new EventHandler(pictureBox1_MouseLeave);

                    PBox.MouseDoubleClick += new MouseEventHandler(PBox_MouseDoubleClick);

                    PBox.Location = new Point(start + gapK + i * KLength + i * gap, start_Y + gapK_Y + j*KLength+j*gap_Y );//PictureBox的位置
                    pictureBoxList.Add(PBox);//绑定到PBox集合中，按i+1标号
                    this.Controls.Add(PBox);

                    ////Label的设置
                    //Label lbl = new Label();
                    //lbl.MaximumSize = new Size(KLength, 0);// 最大与底框同宽，高度不限制
                    //lbl.MinimumSize = new Size(KLength, 0);
                    //lbl.Name = "lbl" + (i + j * LineCount + 1);
                    //lbl.TextAlign = ContentAlignment.MiddleCenter;
                    //lbl.AutoSize = true;
                    //lbl.BackColor = Color.Transparent;
                    //lbl.Text = allImageFileName[i+j*LineCount];
                    //lbl.Font = new Font(lbl.Font.FontFamily, fontSize, FontStyle.Regular);
                    //using (Graphics g = CreateGraphics())//测量lbl的长度，用于美化布局
                    //{
                    //    SizeF textSize = g.MeasureString(lbl.Text, lbl.Font, lbl.MaximumSize.Width);
                    //    lbl.Height = (int)Math.Ceiling(textSize.Height);
                    //    lbl.Width = (int)Math.Ceiling(textSize.Width);
                    //}
                    //labelList.Add(lbl);//添加到集合中
                    //int x = PBox.Location.X;
                    //int y = PBox.Location.Y;
                    //int lbl_X = 0;
                    //if (lbl.Width > KLength)//根据lbl长度放置
                    //{
                    //    lbl_X = x;
                    //}
                    //else
                    //{
                    //    lbl_X = x - gapK + KLength / 2 - lbl.Width / 2;
                    //}
                    //lbl.Location = new Point(lbl_X, y + (int)(KLength * 0.01) + PBoxHeight);
                    //this.Controls.Add(lbl);
                    //index = j * LineCount + i+1;
                }

            }
            int count_Line = line;
            if (count % LineCount != 0)
            {
                count_Line += 1;
                line = count % LineCount;
                for(int i=0;i<line;i++)
                {
                    //PictureBox
                    PictureBox PBox = new PictureBox();
                    PBox.Size = new Size(PBoxWidth, PBoxHeight);
                    PBox.Image = Image.FromFile(allImagePaths[i + index]);//导入图片
                    PBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    PBox.Name = "PBox_" + (i + index+1);//名称编号
                    //绑定属性
                    PBox.MouseEnter += new EventHandler(PBox_Enter);
                    PBox.Click += new EventHandler(PBox_Click);
                    PBox.MouseLeave += new EventHandler(pictureBox1_MouseLeave);
                    PBox.Visible = true;
                    PBox.Location = new Point(start + gapK + i * KLength + i * gap, start_Y + gapK_Y + count/LineCount * KLength + count / LineCount * gap_Y);//PictureBox的位置
                    pictureBoxList.Add(PBox);//绑定到PBox集合中，按i标号
                    this.Controls.Add(PBox);

                    ////Label
                    //Label lbl = new Label();
                    //lbl.MaximumSize = new Size(KLength, 0);// 最大与底框同宽，高度不限制
                    //lbl.MinimumSize = new Size(KLength, 0);
                    //lbl.Name = "lbl" + (i + index+1);
                    //lbl.TextAlign = ContentAlignment.MiddleCenter;
                    //lbl.AutoSize = true;
                    //lbl.BackColor = Color.Transparent;
                    //lbl.Text = allImageFileName[i + index];
                    //lbl.Font = new Font(lbl.Font.FontFamily, fontSize, FontStyle.Regular);
                    //using (Graphics g = CreateGraphics())
                    //{
                    //    SizeF textSize = g.MeasureString(lbl.Text, lbl.Font, lbl.MaximumSize.Width);
                    //    lbl.Height = (int)Math.Ceiling(textSize.Height);
                    //    lbl.Width = (int)Math.Ceiling(textSize.Width);
                    //}
                    //labelList.Add(lbl);
                    //int x = PBox.Location.X;
                    //int y = PBox.Location.Y;
                    //int lbl_X = 0;
                    //if (lbl.Width > KLength)
                    //{
                    //    lbl_X = x;
                    //}
                    //else
                    //{
                    //    lbl_X = x - gapK + KLength / 2 - lbl.Width / 2;
                    //}
                    //lbl.Location = new Point(lbl_X, y + (int)(KLength * 0.01) + PBoxHeight);
                    //this.Controls.Add(lbl);
                }
            }

           // this.Height = count_Line * KLength + gap_Y * (count_Line - 1) + 2 * start_Y;
        }
        #endregion

        #region 动态创建pictureBox，画文件名  //当前方式
        public void CreateDraw(List<string> allImagePaths, List<string> allImageFileName)
        {
            LineCount_set(LineCount);//根据单行显示数量设置参数
            int count = allImagePaths.Count;//图片总数
            int line = count / LineCount;//行数
            Font font = new Font("Arial", fontSize, FontStyle.Regular);
            Brush brush = Brushes.Black; // 文本颜色
            Graphics g = CreateGraphics();
            for (int j = 0; j < line; j++)
            {
                for (int i = 0; i < LineCount; i++)
                {
                    //PictureBox
                    PictureBox PBox = new PictureBox();
                    PBox.Size = new Size(PBoxWidth, PBoxHeight);
                    PBox.Image = Image.FromFile(allImagePaths[i + j * LineCount]);//导入图片
                    PBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    PBox.Name = "PBox_" + (i + j * LineCount + 1);//名称编号
                    //绑定属性
                    PBox.MouseEnter += new EventHandler(PBox_Enter);
                    PBox.Click += new EventHandler(PBox_Click);
                    PBox.MouseLeave += new EventHandler(pictureBox1_MouseLeave);
                    PBox.Visible = true;
                    PBox.Location = new Point(start + gapK + i * KLength + i * gap, start_Y + gapK_Y + j * KLength + j * gap_Y);//PictureBox的位置
                    pictureBoxList.Add(PBox);//绑定到PBox集合中，按i+1标号
                    this.Controls.Add(PBox);

                    //图片的详情框内容
                    string text = allImageFileName[i];//.Replace("\\", "1");
                    FileInfo fileInfo = new FileInfo(text);
                    long fileSizeInBytes = fileInfo.Length/1024;
                    Bitmap bitmap = new Bitmap(allImagePaths[i + j * LineCount]);
                    int image_width = bitmap.Width;
                    int image_height = bitmap.Height;
                    ImageFormat rawFormat = bitmap.RawFormat;
                    string imageFormat = rawFormat.ToString();
                    if (rawFormat.Equals(ImageFormat.Jpeg))
                    {
                        imageFormat = "JPG";
                    }
                    else if (rawFormat.Equals(ImageFormat.Png))
                    {
                        imageFormat = "PNG";
                    }
                    else if (rawFormat.Equals(ImageFormat.Gif))
                    {
                        imageFormat = "GIF";
                    }
                    else if(rawFormat.Equals(ImageFormat.Bmp))
                    {
                        imageFormat = "BMP";
                    }
                    bitmap.Dispose();//释放资源
                    //绑定详情框内容
                    ToolTip toolTip = new ToolTip();
                    toolTip.SetToolTip(PBox,  $"图片名称：{text}\r\n分辨率：{image_width}×{image_height}\r\n类型: {imageFormat}\r\n文件大小: {fileSizeInBytes} KB");

                    //绘制文本矩形框作为名称
                    int x = PBox.Location.X;
                    int y = PBox.Location.Y + PBoxHeight + 4;
                    SizeF textSize = g.MeasureString(text, font);
                    int Width = (int)Math.Ceiling(textSize.Width);//文本长度
                    int Height = (int)Math.Ceiling(textSize.Height);
                    RectangleF layoutRect = new RectangleF(x, y, PBoxWidth, 2*Height);
                    StringFormat stringFormat = new StringFormat
                    {
                        FormatFlags = StringFormatFlags.LineLimit,//自动换行
                        Trimming = StringTrimming.EllipsisWord,//超出长度就省略符号
                        Alignment = StringAlignment.Center,//居中
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(text, font, brush, layoutRect, stringFormat);
                    index = j * LineCount + i + 1;
                   
                }

            }
            if (count % LineCount != 0)
            {
                line = count % LineCount;
                for (int i = 0; i < line; i++)
                {
                    //PictureBox
                    PictureBox PBox = new PictureBox();
                    PBox.Size = new Size(PBoxWidth, PBoxHeight);
                    PBox.Image = Image.FromFile(allImagePaths[i + index]);//导入图片
                    PBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    PBox.Name = "PBox_" + (i + index + 1);//名称编号
                    //绑定属性
                    PBox.MouseEnter += new EventHandler(PBox_Enter);
                    PBox.Click += new EventHandler(PBox_Click);
                    PBox.MouseLeave += new EventHandler(pictureBox1_MouseLeave);
                    PBox.Visible = true;
                    PBox.Location = new Point(start + gapK + i * KLength + i * gap, start_Y + gapK_Y + count / LineCount * KLength + count / LineCount * gap_Y);//PictureBox的位置
                    pictureBoxList.Add(PBox);//绑定到PBox集合中，按i标号
                    this.Controls.Add(PBox);

                    string text = allImageFileName[i+index];//.Replace("\\", "1");
                    FileInfo fileInfo = new FileInfo(text);
                    long fileSizeInBytes = fileInfo.Length / 1024;
                    Bitmap bitmap = new Bitmap(allImagePaths[i + index]);
                    int image_width = bitmap.Width;
                    int image_height = bitmap.Height;
                    ImageFormat rawFormat = bitmap.RawFormat;
                    string imageFormat = rawFormat.ToString();
                    if (rawFormat.Equals(ImageFormat.Jpeg))
                    {
                        imageFormat = "JPG";
                    }
                    else if (rawFormat.Equals(ImageFormat.Png))
                    {
                        imageFormat = "PNG";
                    }
                    else if (rawFormat.Equals(ImageFormat.Gif))
                    {
                        imageFormat = "GIF";
                    }
                    else if (rawFormat.Equals(ImageFormat.Bmp))
                    {
                        imageFormat = "BMP";
                    }
                    bitmap.Dispose();//释放资源
                    
                    //绑定详情框内容
                    ToolTip toolTip = new ToolTip();
                    toolTip.SetToolTip(PBox, $"图片名称：{text}\r\n分辨率：{image_width}×{image_height}\r\n类型: {imageFormat}\r\n文件大小: {fileSizeInBytes} KB");

                    //  string text = allImageFileName[i+index];
                    int x = PBox.Location.X;
                    int y = PBox.Location.Y + PBoxHeight + 4;
                    SizeF textSize = g.MeasureString(text, font);
                    int Width = (int)Math.Ceiling(textSize.Width);//文本长度
                    int Height = (int)Math.Ceiling(textSize.Height);
                    RectangleF layoutRect = new RectangleF(x, y, PBoxWidth, 2 * Height);
                    StringFormat stringFormat = new StringFormat
                    {
                        FormatFlags = StringFormatFlags.LineLimit,
                        Trimming = StringTrimming.EllipsisWord,//超出长度就省略符号
                        Alignment = StringAlignment.Center,//居中
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(text, font, brush, layoutRect, stringFormat);
                }
            }
        }
        #endregion
       

        #region 根据LineCount初始化PL
        private void LineCount_set(int count)
        {
            start = (int)(this.Width * 0.0075);
            start_Y = start;
            gap = (int)(this.Width * 0.006);
            gap_Y = gap;
            gapK = (int)(this.Width * 0.010);
            switch (count)//根据选择的数量，更改pictureBox和label的属性
            {
                case 6:
                    PBoxWidth = (int)(this.Width * 0.139);
                    PBoxHeight = (int)(PBoxWidth * 0.75);
                    KLength = (int)(this.Width * 0.159);
                    gapK_Y = (KLength - PBoxHeight) / 2;
                    Rect = new Rectangle(0, 0, KLength, KLength);
                    fontSize = 9;
                    break;
                case 5:
                    PBoxWidth = (int)(this.Width * 0.171);
                    PBoxHeight = (int)(PBoxWidth * 0.75);
                    KLength = (int)(this.Width * 0.191);
                    gapK_Y = (KLength - PBoxHeight) / 2;
                    Rect = new Rectangle(0, 0, KLength, KLength);
                    fontSize = 11;
                    break;
                case 4:
                    PBoxWidth = (int)(this.Width * 0.21875);
                    PBoxHeight = (int)(PBoxWidth * 0.75);
                    KLength = (int)(this.Width * 0.23875);
                    gapK_Y = (KLength - PBoxHeight) / 2;
                    Rect = new Rectangle(0, 0, KLength, KLength);
                    fontSize = 13;
                    break;
                case 3:
                    PBoxWidth = (int)(this.Width * 0.2983);
                    PBoxHeight = (int)(PBoxWidth * 0.75);
                    KLength = (int)(this.Width * 0.3183);
                    gapK_Y = (KLength - PBoxHeight) / 2;
                    Rect = new Rectangle(0, 0, KLength, KLength);
                    fontSize = 16;
                    break;
                case 2:
                    PBoxWidth = (int)(this.Width * 0.4575);
                    PBoxHeight = (int)(PBoxWidth * 0.75);
                    KLength = (int)(this.Width * 0.4775);
                    gapK_Y = (KLength - PBoxHeight) / 2;
                    Rect = new Rectangle(0, 0, KLength, KLength);
                    fontSize = 19;
                    break;
            }
        }
        #endregion

        #region 鼠标点击选中
        private void PBox_Click(object sender, EventArgs e)
        {
            PictureBox myPBox = sender as PictureBox;
            string input = myPBox.Name;
            string result = new string(input.Where(char.IsDigit).ToArray());
            int count = int.Parse(result);//根据pictureBox提取标号
            selectPBox[count] = !selectPBox[count];
            int x = myPBox.Location.X;
            int y = myPBox.Location.Y;
            if (selectPBox[count])
            {
                //clear_Reac(x, y);
               // labelList[count].BackColor = Color.FromArgb(164, 96, 157, 217);//变label
                Paint(x, y, count);
            }
            else
            {
               // labelList[count].BackColor = Color.Transparent;
                clear_Reac(x, y, count);//刷新控件，重新绘制
                
            }
        }
        #endregion

        //浅色，鼠标进入但未点击时的底框
        private void Paint2(int x, int y, int index)
        {
           // Color backgroundColor = Color.FromArgb(200, 120, 170, 215);
            Brush brush = Brushes.Black;
            Font font = new Font("Arial", fontSize, FontStyle.Regular);
            string text = allImageFileName[index-1];
            using (Bitmap bitmap = new Bitmap(Width, Height))
            {
                // 双缓冲绘制
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    //  g.FillRectangle(new SolidBrush(backgroundColor), ClientRectangle);
                    g.FillRectangle(selectionBrush2, x - gapK, y - gapK_Y, KLength, KLength);
                }
                // 将内存位图绘制到控件上
                using (Graphics controlGraphics = this.CreateGraphics()) // 使用 this.CreateGraphics()
                {
                    controlGraphics.DrawImage(bitmap, 0, 0);
                    //SizeF textSize = controlGraphics.MeasureString(text, Font);
                    //int Width = (int)Math.Ceiling(textSize.Width);
                    //x = x + PBoxWidth / 2 - Width / 2;
                    //controlGraphics.DrawString(text, font, brush, x, y + PBoxHeight + 4);
                }
            }
        }

        //深色，鼠标点击后的底框
        private void Paint(int x,int y, int index)
        {
            Color backgroundColor = Color.FromArgb(128, 72, 145, 220);
            Brush brush = Brushes.Black;
            Font font = new Font("Arial", fontSize, FontStyle.Regular);
            string text = allImageFileName[index-1];
            using (Bitmap bitmap = new Bitmap(Width, Height))
            {
                // 双缓冲绘制
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    //  g.FillRectangle(new SolidBrush(backgroundColor), ClientRectangle);
                    g.FillRectangle(new SolidBrush(backgroundColor), x - gapK, y - gapK_Y, KLength, KLength);
                }
                // 将内存位图绘制到控件上
                using (Graphics controlGraphics = this.CreateGraphics()) // 使用 this.CreateGraphics()
                {
                    controlGraphics.DrawImage(bitmap, 0, 0);
                    //SizeF textSize = controlGraphics.MeasureString(text, Font);
                    //int Width = (int)Math.Ceiling(textSize.Width);
                    //x = x + PBoxWidth / 2 - Width / 2;
                    //controlGraphics.DrawString(text, font, brush, x, y + PBoxHeight + 4);
                }
            }
        }

        #region 清除选中框
        private void clear_Reac(int x, int y, int index)
        {
            Color backgroundColor = this.BackColor;
            Brush brush = Brushes.Black;
            Font font = new Font("Arial", fontSize, FontStyle.Regular);
            string text = allImageFileName[index-1];
            
            using (Bitmap bitmap = new Bitmap(Width, Height))
            {
                // 双缓冲绘制
                using (Graphics g = Graphics.FromImage(bitmap))
                {

                    g.FillRectangle(new SolidBrush(backgroundColor), x - gapK, y - gapK_Y, KLength, KLength);
                }
                // 将内存位图绘制到控件上
                using (Graphics controlGraphics = this.CreateGraphics()) // 使用 this.CreateGraphics()
                {
                    SizeF textSize = controlGraphics.MeasureString(text, Font);
                    int height = (int)Math.Ceiling(textSize.Height);
                    controlGraphics.DrawImage(bitmap, 0, 0);
                    RectangleF layoutRect = new RectangleF(x, y + PBoxHeight + 4, PBoxWidth, 2 * height);
                    StringFormat stringFormat = new StringFormat
                    {
                        FormatFlags = StringFormatFlags.LineLimit,
                        Trimming = StringTrimming.EllipsisWord,//超出长度就省略符号
                        Alignment = StringAlignment.Center,//居中
                        LineAlignment = StringAlignment.Center
                    };
                    controlGraphics.DrawString(text, font, brush, layoutRect, stringFormat);
                    //SizeF textSize = controlGraphics.MeasureString(text, Font);
                    //int Width = (int)Math.Ceiling(textSize.Width);
                    //x = x + PBoxWidth / 2 - Width / 2;
                    //controlGraphics.DrawString(text, font, brush, x, y + PBoxHeight + 4);
                }
            }
        }

        #endregion

        #region 鼠标进入
        private void PBox_Enter(object sender, EventArgs e)
        {
            PictureBox myPBox = sender as PictureBox;
            string input = myPBox.Name;
            string result = new string(input.Where(char.IsDigit).ToArray());
            int count = int.Parse(result);
            int x = myPBox.Location.X;
            int y = myPBox.Location.Y;
            if (selectPBox[count])
            {
                return;
            }
            else
            {
                Paint2(x, y, count);//刷新控件，重新绘制
              //  labelList[count].BackColor = Color.FromArgb(200, 120, 170, 215);//浅色
            }
        }
        #endregion

        #region 鼠标离开
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            PictureBox myPBox = sender as PictureBox;
            string input = myPBox.Name;
            string result = new string(input.Where(char.IsDigit).ToArray());
            int count = int.Parse(result);
            int x = myPBox.Location.X;
            int y = myPBox.Location.Y;
            if (selectPBox[count])
            {
                return;
            }
            else
            {
                clear_Reac(x, y, count);
               // labelList[count].BackColor = Color.Transparent;//控件背景色
            }
        }
        #endregion

        #region 导出选中的图片接口
        public void Selected_Image(bool[] selectpBox)
        {
            for(int i=1;i<=100;i++)
            {
                if(selectpBox[i])
                {
                    Bitmap bitmap = new Bitmap(allImagePaths[i - 1]);
                    selectedImages.Add(bitmap);
                    bitmap.Dispose();
                }
            }
        }
        #endregion

        #region 鼠标点击（还未定义）
        private void PBox_MouseDoubleClick(object sender, EventArgs e)
        {
            PictureBox myBox = sender as PictureBox;
            
        }
        #endregion
    }
}

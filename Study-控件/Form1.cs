using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Study_控件
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// “选择图片”按钮可以多次导入图片，相当于控件newListView的一个动态显示图片接口；
        /// 如果需要对被选中的图片进行导出或其他操作，可调用newListView控件中的Selected_Image(bool[] selectpBox)接口进行操作
        /// </summary>
        private Color borderColor = Color.Red; // 初始边框颜色
        public Form1()
        {
            InitializeComponent();
        }

        //button相当于调用newListView的接口
        private void button1_Click(object sender, EventArgs e)
        {
            //打开本地文件框
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true, //允许用户选择多个文件
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp", //限制文件类型
                Title = "请选择图片"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    newListView_Test.allImagePaths.Add(System.IO.Path.GetFullPath(openFileDialog.FileNames[i]));//图片完整路径
                    newListView_Test.allImageFileName.Add(openFileDialog.FileNames[i]);//图片名称
                }
            }

            //待改进（暴露太多）：动态创建
            newListView_Test.CreateDraw(newListView_Test.allImagePaths, newListView_Test.allImageFileName);
        }
    }
}

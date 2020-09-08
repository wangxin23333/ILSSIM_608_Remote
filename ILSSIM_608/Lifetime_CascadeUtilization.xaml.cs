using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ILSSIM_608
{
    /// <summary>
    /// Lifetime_CascadeUtilization.xaml 的交互逻辑
    /// </summary>
    public partial class Lifetime_CascadeUtilization : Window
    {
        public Lifetime_CascadeUtilization()
        {
            InitializeComponent();
        }


        #region 公共函数
        //定义一个类对象来实现“提示内容绑定” (实现了INotifyPropertyChanged接口)
        public class TipText : INotifyPropertyChanged
        {
            private string _TipText;//定义私有字段（定义属性的完整写法）
            public string WindowBotton_TipText
            {
                get
                {
                    return _TipText;
                }
                set
                {
                    _TipText = value;
                    if (PropertyChanged != null)//判断是否有人监听这个事件
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("WindowBotton_TipText"));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;//定义监听属性变化事件
        }

        /// <summary>
        /// 填表函数 
        /// </summary>
        /// <param name="sqlConnect"></param>
        /// <returns></returns>
        public DataTable FillTable(string sqlConnect)
        {
            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = connString;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                #region 填表操作
                DataTable dt = new DataTable();
                dt.Clear();

                MySqlCommand cmd = new MySqlCommand(sqlConnect, conn);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                sda.Fill(dt);

                conn.Close();
                return dt;
                #endregion
            }

        }

        /// <summary>
        /// 将给定DataGrid的列名设置为中文 而非设置Table列名 
        /// </summary>
        /// <param name="tmpDataGrid"></param> 给定的DataGrid
        /// <param name="lstChineseColumnName"></param> 对应的中文列名
        public void setChineseColumnName(DataGrid tmpDataGrid, List<string> lstChineseColumnName)
        {
            for (int i = 0; i < tmpDataGrid.Columns.Count; i++)
            {
                tmpDataGrid.Columns[i].Header = lstChineseColumnName[i].ToString();
            }
        }

        /// <summary>
        /// 识别增加列标题字符个数，并为其增加20像素宽度
        /// </summary>
        /// <param name="tmpDataGrid"></param>给定的DataGrid
        public void autoAddColumnWidth(DataGrid tmpDataGrid)
        {
            for (int i = 0; i < tmpDataGrid.Columns.Count; i++)
            {
                int ChineseNum = tmpDataGrid.Columns[i].Header.ToString().Count();
                tmpDataGrid.Columns[i].Width = ChineseNum * 15 + 20;//字符个数 * 15磅字的像素宽度 + 20像素
            }
        }

        /// <summary>
        /// <para> 更新数据库的自定义函数</para> 
        /// <para> 更新成功时返回true，失败时返回false</para> 
        /// </summary>
        /// <param name="tmpTable"></param>
        /// <param name="tmpCmd"></param>
        public bool UpdateDatabase(DataTable tmpTable, string tmpCmd)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = connString;
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    #region 更新数据库
                    MySqlDataAdapter adapter = new MySqlDataAdapter(tmpCmd, conn);
                    MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);
                    commandBuilder.GetUpdateCommand();
                    adapter.Update(tmpTable);
                    tmpTable.AcceptChanges();
                    #endregion
                    return true;
                    //MessageBox.Show("数据保存成功！");
                }
            }
            catch (MySqlException ex)   //不能抓取到输入数据的格式错误？？？？
            {
                MessageBox.Show(ex.Message);
                tmpTable.RejectChanges();//拒绝用户此次修改
                return false;
                //throw;//注释该行语句的目的是：不抛出该异常，只给出错误提示信息，保证程序运行
            }
        }
        
        #endregion

        #region 公共变量
        /// <summary>
        /// 界面窗体下方提示信息
        /// </summary>
        TipText tipText = new TipText();
        
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string connString;
        
        /// <summary>
        /// 模块五的建模公用表格
        /// </summary>
        DataTable Table_Module_5;

        /// <summary>
        /// 查询数据库的公用字符串
        /// </summary>
        public string cmd;

        #endregion

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tipText.WindowBotton_TipText = "请填写梯次使用的四个所需输入参数并保存/r；梯次使用调度结果可在仿真结束后查看；";
            TB_Tip.DataContext = tipText;//设置提示的数据上下文（数据源）
        }

        /// <summary>
        /// 拖动窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Max_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认已完成建模，返回主界面？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 完成按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            //UpdateDatabase(Table_Module_5, cmd);//调用自定义函数保存用户输入信息
            if (MessageBox.Show("确认输入完成梯次使用相关参数，返回主界面？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }
    }
}

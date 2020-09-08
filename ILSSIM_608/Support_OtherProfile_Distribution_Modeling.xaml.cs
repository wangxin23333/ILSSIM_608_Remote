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
    /// Support_OtrerProfile_Modeling.xaml 的交互逻辑
    /// </summary>
    public partial class Support_OtherProfile_Distribution_Modeling : Window
    {
        public Support_OtherProfile_Distribution_Modeling()
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

        /// <summary>
        /// 设置下拉列表框内容及绑定到DataGrid某一列
        /// </summary>
        /// <param name="tmpComboBoxColumn"></param>
        /// <param name="tmp_SQLCmd"></param>
        /// <param name="tmpPropertyName"></param>
        /// <returns></returns>
        public DataGridComboBoxColumn setBoxColumn_FromDB(DataGridComboBoxColumn tmpComboBoxColumn, string tmp_SQLCmd, string tmpPropertyName)
        {
            //tmp_SQLCmd表示需要在函数中执行的SQL语句
            //tmpPropertyName表示当前需要设置为下拉框列的数据表字段名
            tmpComboBoxColumn.Header = tmpPropertyName;

            ObservableCollection<ComboboxItem> lstComboboxItem = new ObservableCollection<ComboboxItem>();
            DataTable tmpDataTable = FillTable(tmp_SQLCmd);
            for (int j = 0; j < tmpDataTable.Rows.Count; j++)
            {
                lstComboboxItem.Add(new ComboboxItem() { Item = tmpDataTable.Rows[j].ItemArray[0].ToString() });//SID在查出来的表的第j行、第0列
            }

            tmpComboBoxColumn.ItemsSource = lstComboboxItem;//设置下拉框内容的数据源
            tmpComboBoxColumn.DisplayMemberPath = "Item";//界面初始化时展示的值
            tmpComboBoxColumn.SelectedValuePath = "Item";//选中后展示的值

            Binding binding = new Binding();     //实例化一个binding对象
            binding.Path = new PropertyPath(tmpPropertyName);     //设置需要绑定到的那一列的列名
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            tmpComboBoxColumn.SelectedValueBinding = binding;

            return tmpComboBoxColumn;
        }

        #endregion

        #region 公共变量
        /// <summary>
        /// 界面窗体下方提示信息
        /// </summary>
        TipText tipText = new TipText();

        /// <summary>
        /// 自定义下拉框项目 类
        /// </summary>
        public class ComboboxItem
        {
            public string Item { get; set; }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string connString;

        /// <summary>
        /// 当前选中的TreeView节点
        /// </summary>
        string tmpNodeName_Module_6;

        /// <summary>
        /// 模块一的建模公用表格
        /// </summary>
        DataTable Table_Module_6;

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
            tipText.WindowBotton_TipText = "请从上到下依次进行建模，完成后即可保存退出";
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
        /// TreeView选项改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_Module_2_Selected(object sender, RoutedEventArgs e)
        {
            //获得当前选中的TreeViewItem,为后续取其Name属性做准备
            TreeViewItem tmpTreeViewItem = e.OriginalSource as TreeViewItem;
            tmpNodeName_Module_6 = tmpTreeViewItem.Name.ToString();

            #region 保存上一节点信息
            //判断当前表格是否为空，如果不是空表的话，说明不是第一次点击TreeView节点，需要在查询新表之前保存前一节点信息
            if (Table_Module_6 != null)
            {
                UpdateDatabase(Table_Module_6, cmd);//调用自定义函数保存用户输入信息
            }
            #endregion

            //节点名与数据库表名一致，可以用节点名去数据库查相应的表；
            //此处先定义不作反应的几个节点名称，以防程序把它当成一个表名去数据库查询数据；
            List<string> TreeViewNode_NoAction = new List<string>() { "Support_Modeling", "Profile_Modeling", "Distribution_Modeling" };

            //如果是不反应节点，则提示相关信息
            if (TreeViewNode_NoAction.Contains(tmpNodeName_Module_6))//如果当前节点名在“无动作节点列表”中，提示框内容设置为：请展开当前节点进行建模
            {
                tipText.WindowBotton_TipText = "请点击下层节点进行建模";
            }
            else
            {
                //当前选中表格节点，不提示
                tipText.WindowBotton_TipText = "请从上到下依次进行建模，完成后即可保存退出";

                cmd = "select * from " + tmpNodeName_Module_6 + ";";//定义查询字符串
                Table_Module_6 = FillTable(cmd);
                DataGrid_Support_OtherProfile_Distribution_Modeling.ItemsSource = Table_Module_6.DefaultView;
                List<string> lstHeader;
                if (tmpNodeName_Module_6 == "PMInformation")
                {
                    lstHeader = new List<string>() { "预防性维修任务名称", "维修方式", "任务对象", "对应产品", "任务时长(h)", "任务频率(次/年)", "维修级别", "维修站点" };
                }
                else if (tmpNodeName_Module_6 == "CMInformation")
                {
                    lstHeader = new List<string>() { "修复性维修任务名称", "维修方式", "任务对象", "对应产品", "任务时长(h)", "任务频率(次/年)", "维修级别", "维修站点" };
                }
                else if (tmpNodeName_Module_6 == "OMInformation")
                {
                    lstHeader = new List<string>() { "使用保障任务名称", "维修方式", "任务对象", "对应产品", "任务时长(h)", "任务频率(次/年)", "维修级别", "维修站点" };
                }
                else if (tmpNodeName_Module_6 == "ResourceProfile")
                {
                    lstHeader = new List<string>() { "资源剖面名称", "剖面时长", "剖面开始时刻", "剖面结束时刻" };
                }
                else if (tmpNodeName_Module_6 == "TransProfile")
                {
                    lstHeader = new List<string>() { "运输剖面名称", "剖面时长", "剖面开始时刻", "剖面结束时刻" };
                }
                else //(tmpNodeName_Module_6 == "TimeDistribution")
                {
                    lstHeader = new List<string>() { "分布标识", "分布类型", "参数一", "参数二", "参数三" };
                }
                

                //设置中文列名，并增加列宽度
                setChineseColumnName(DataGrid_Support_OtherProfile_Distribution_Modeling, lstHeader);
                autoAddColumnWidth(DataGrid_Support_OtherProfile_Distribution_Modeling);
            }
        }

        private void DataGrid_Support_OtherProfile_Distribution_Modeling_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        /// <summary>
        /// 保存并关闭按钮 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            UpdateDatabase(Table_Module_6, cmd);//调用自定义函数保存用户输入信息
            if (MessageBox.Show("确认已完成建模，返回主界面？", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }
    }
}

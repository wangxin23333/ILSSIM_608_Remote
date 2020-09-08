using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace ILSSIM_608
{
    /// <summary>
    /// common function for window
    /// ReserveEngineDemandForecast  Window 界面使用的公共函数集合
    /// </summary>
    public partial class ResourceOptimizing_Window : Window
    {
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
        /// 清空datatable表格
        /// </summary>
        /// <returns></returns>
        public DataTable ClearTable(DataTable tmpTable)
        {
            //倒叙清空表格
            for (int i = tmpTable.Rows.Count; i > 0; i--)
            {
                tmpTable.Rows.Remove(tmpTable.Rows[i - 1]);
            }
            return tmpTable;
        }
        /// <summary>
        /// 设置列标题
        /// </summary>
        /// <param name="tmpDataTable"></param>
        /// <param name="lstColumnName_toSet"></param>
        /// <returns></returns>
        public DataTable setColumnName(DataTable tmpDataTable, List<string> lstColumnName_toSet)
        {
            for (int i = 0; i < lstColumnName_toSet.Count; i++)
            {
                tmpDataTable.Columns[i].ColumnName = lstColumnName_toSet[i].ToString();
            }
            return tmpDataTable;
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
                    MySqlDataAdapter adapter = new MySqlDataAdapter(tmpCmd, conn);//
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
        /// 获取选中行的某一字段的内容
        /// </summary>
        /// <param name="tmpDataGrid">需要查的DataGrid名称</param>
        /// <param name="tmpColumnIndex">要查的字段所在的列</param>
        /// <returns></returns>
        public string getSelectedRowItem(DataGrid tmpDataGrid, int tmpColumnIndex)
        {
            if (tmpDataGrid.SelectedItem.GetType() == typeof(DataRowView))
            {
                var selectedRow = (DataRowView)tmpDataGrid.SelectedItem;//获得tmpDataGrid中被选中的行，且改行默认为DataRowView类型
                return selectedRow[tmpColumnIndex].ToString();
            }
            else
            {
                MessageBox.Show("当前选中行为空白行或该行还未保存!");
                return "null";
            }
        }

        public class ComboboxItem
        {
            public string Item { get; set; }
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
            //tmpPropertyName表示当前需要设置为下拉框列的标题、DataGrid中需要绑定到的那一列的列标题
            tmpComboBoxColumn.Header = tmpPropertyName;

            ObservableCollection<ComboboxItem> lstComboboxItem = new ObservableCollection<ComboboxItem>();
            DataTable tmpDataTable = FillTable(tmp_SQLCmd);
            for (int j = 0; j < tmpDataTable.Rows.Count; j++)
            {
                lstComboboxItem.Add(new ComboboxItem() { Item = tmpDataTable.Rows[j].ItemArray[0].ToString() });//SNAME在查出来的表的第j行、第0列
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

        /// <summary> 
        /// 将两个列不同(结构不同)的DataTable合并成一个新的DataTable 
        /// </summary> 
        /// <param name="DataTable1">表1</param> 
        /// <param name="DataTable2">表2</param> 
        /// <param name="DTName">合并后新的表名</param> 
        /// <returns>合并后的新表</returns> 
        public DataTable UniteDataTable2(DataTable DataTable1, DataTable DataTable2, string DTName)
        {
            DataTable newDataTable = new DataTable();
            if (DataTable1.Rows.Count > DataTable2.Rows.Count)
            {
                newDataTable = FillData(DataTable1, DataTable2);
            }
            else
            {
                newDataTable = FillData(DataTable2, DataTable1);
            }

            newDataTable.TableName = DTName; //设置DT的名字 
            return newDataTable;
        }

        private DataTable FillData(DataTable dt1, DataTable dt2)
        {
            //克隆DataTable1的结构
            DataTable newDataTable = dt1.Clone();
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                //再向新表中加入DataTable2的列结构
                newDataTable.Columns.Add(dt2.Columns[i].ColumnName);
            }
            object[] obj = new object[newDataTable.Columns.Count];
            //添加DataTable1的数据
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                newDataTable.Rows.Add(obj);
            }
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                for (int j = 0; j < dt2.Columns.Count; j++)
                {
                    newDataTable.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                }
            }
            return newDataTable;
        }
    }
}

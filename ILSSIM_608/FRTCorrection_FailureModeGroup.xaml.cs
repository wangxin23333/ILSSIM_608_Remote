using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// 故障率折算、故障模式组合界面 交互逻辑
    /// </summary>
    public partial class FRTCorrection_FailureModeGroup : Window
    {
        public FRTCorrection_FailureModeGroup()
        {
            InitializeComponent();
        }


        #region 全局变量


        public class ComboBoxItem
        {
            public string Item
            {
                get; set;
            }

            public object SelectedValue
            {
                get; set;
            }
        }
        /// <summary>
        /// 默认环境参数表
        /// </summary>
        DataTable Table_TemperatureHumidity;//默认温度湿度参数表
        DataTable Table_Desert;//默认沙尘参数表
        DataTable Table_Plateau;//默认太阳辐射参数表
        DataTable Table_Atomsphere;//默认大气腐蚀参数表

        /// <summary>
        /// 默认环境参数指令
        /// </summary>
        string cmd_temp_hump;
        string cmd_des;
        string cmd_pla;
        string cmd_ate;

        /// <summary>
        /// 上一条选择的产品标识
        /// </summary>
        string LastIID;

        /// <summary>
        /// 产品更换概率信息表
        /// </summary>
        DataTable Table_MaintenanceProbabilityInformation;

        /// <summary>
        /// 批量设置任务标识
        /// </summary>
        DataTable Table_FailureRateInformation;

        /// <summary>
        /// 故障模式（输入）
        /// </summary>
        DataTable tmpTable_FailureMode;



        /// <summary>
        /// 实际环境参数
        /// </summary>
        double TEMP;
        double HUMP;
        double DUP;
        double SRP;
        double ACP;
        string sql;
        #endregion

        #region 数据库连接
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string connString;
        #endregion

        #region 更新数据库
        /// <summary>
        /// <para> 更新数据库</para> 
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

        #region 更新装备任务环境条件信息表
        public bool UpdateMissiontype(string tmpCmd)
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
                    MySqlCommand comm = new MySqlCommand(tmpCmd, conn);
                    comm.ExecuteNonQuery();
                    comm.Clone();
                    #endregion
                    return true;
                }
            }
            catch (MySqlException ex)   //不能抓取到输入数据的格式错误？？？？
            {
                MessageBox.Show(ex.Message);//拒绝用户此次修改
                return false;
                //throw;//注释该行语句的目的是：不抛出该异常，只给出错误提示信息，保证程序运行
            }
        }
        #endregion

        #region 填表函数
        /// <summary>
        /// 填表函数 
        /// </summary>
        /// <param name="tmpCmd"></param>
        /// <returns></returns>
        public DataTable FillTable(string tmpCmd)
        {
            using (MySqlConnection conn = new MySqlConnection())
            {
                conn.ConnectionString = connString;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                //填表操作
                DataTable dt = new DataTable();
                dt.Clear();

                MySqlCommand cmd = new MySqlCommand(tmpCmd, conn);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                sda.Fill(dt);

                conn.Close();
                return dt;

            }
        }
        #endregion

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

        #region 窗口移动
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion

        #region 最小化
        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        #region 最大化
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
        #endregion

        #region 关闭窗口
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否确认关闭", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }
        #endregion

        #region 设置表格列名
        public void setColumnName(DataGrid tmpDataGrid, List<string> lstChineseColumnName)
        {
            for (int i = 0; i < tmpDataGrid.Columns.Count; i++)
            {
                tmpDataGrid.Columns[i].Header = lstChineseColumnName[i].ToString();
            }
        }
        #endregion

        #region 列标题自动增宽
        /// <summary>
        /// 识别增加列标题字符个数，并为其增加20像素宽度
        /// </summary>
        /// <param name="tmpDataGrid"></param>给定的DataGrid
        public void autoAddColumnWidth(DataGrid tmpDataGrid)
        {
            for (int i = 0; i < tmpDataGrid.Columns.Count; i++)
            {
                int ChineseNum = tmpDataGrid.Columns[i].Header.ToString().Count();
                tmpDataGrid.Columns[i].Width = ChineseNum * 20 + 20;//字符个数 * 15磅字的像素宽度 + 20像素
            }
        }
        #endregion

        #region 窗体加载
        /// <summary>
        /// 窗体加载 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region 加载默认环境参数表格
            cmd_temp_hump = "select * from TempANDHump";
            Table_TemperatureHumidity = FillTable(cmd_temp_hump);
            DataTable_TemperatureHumidity.ItemsSource = Table_TemperatureHumidity.DefaultView;
            //数据库读取默认温度湿度参数表

            cmd_des = "select * from DustenPara";
            Table_Desert = FillTable(cmd_des);
            DataTable_Desert.ItemsSource = Table_Desert.DefaultView;
            //数据库读取默认沙尘参数表

            cmd_pla = "select * from SolarraPara";
            Table_Plateau = FillTable(cmd_pla);
            DataTable_Plateau.ItemsSource = Table_Plateau.DefaultView;
            //数据库读取默认太阳辐射参数表

            cmd_ate = "select * from AtmcoPara";
            Table_Atomsphere = FillTable(cmd_ate);
            DataTable_Atomsphere.ItemsSource = Table_Atomsphere.DefaultView;
            //数据库读取默认大气腐蚀表
            #endregion

            #region  设置下拉框数据源
            ObservableCollection<ComboBoxItem> comboBoxItems_Marea = new ObservableCollection<ComboBoxItem>();
            ObservableCollection<ComboBoxItem> comboBoxItems_Msea = new ObservableCollection<ComboBoxItem>();
            ObservableCollection<ComboBoxItem> comboBoxItems_Des = new ObservableCollection<ComboBoxItem>();
            ObservableCollection<ComboBoxItem> comboBoxItems_Sdu = new ObservableCollection<ComboBoxItem>();
            ObservableCollection<ComboBoxItem> comboBoxItems_Pla = new ObservableCollection<ComboBoxItem>();
            ObservableCollection<ComboBoxItem> comboBoxItems_Ate = new ObservableCollection<ComboBoxItem>();
            ObservableCollection<ComboBoxItem> comboBoxItems_Batch_Mtid = new ObservableCollection<ComboBoxItem>();
            #endregion

            #region 任务地区下拉框数据源
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "雷州半岛" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "海南岛" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "台湾南部" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "长江流域" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "四川" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "珠江流域" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "台湾北部" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "福建" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "新疆天山以南" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "戈壁沙漠" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "秦岭以北" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "内蒙南部" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "华北" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "东北南部" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "内蒙中部和西部" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "陕西北部" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "甘肃北部" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "青海" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "新疆天山以北" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "内蒙北部" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "黑龙江" });
            comboBoxItems_Marea.Add(new ComboBoxItem() { Item = "青藏高原地区" });
            Combo_MAREA.ItemsSource = comboBoxItems_Marea;
            Combo_MAREA.DisplayMemberPath = "Item";//界面初始化时展示的值
            Combo_MAREA.SelectedValuePath = "Item";//选中后展示的值
            #endregion

            #region 任务季节下拉框数据源
            comboBoxItems_Msea.Add(new ComboBoxItem() { Item = "春" });
            comboBoxItems_Msea.Add(new ComboBoxItem() { Item = "夏" });
            comboBoxItems_Msea.Add(new ComboBoxItem() { Item = "秋" });
            comboBoxItems_Msea.Add(new ComboBoxItem() { Item = "冬" });
            Combo_MSEA.ItemsSource = comboBoxItems_Msea;
            Combo_MSEA.DisplayMemberPath = "Item";//界面初始化时展示的值
            Combo_MSEA.SelectedValuePath = "Item";//选中后展示的值
            #endregion

            #region 是否沙漠环境下拉框数据源
            comboBoxItems_Des.Add(new ComboBoxItem() { Item = "是" });
            comboBoxItems_Des.Add(new ComboBoxItem() { Item = "否" });
            Combo_DES.ItemsSource = comboBoxItems_Des;
            Combo_DES.DisplayMemberPath = "Item";//界面初始化时展示的值
            Combo_DES.SelectedValuePath = "Item";//选中后展示的值
            #endregion

            #region 沙尘天气下拉框数据源
            comboBoxItems_Sdu.Add(new ComboBoxItem() { Item = "强沙尘暴" });
            comboBoxItems_Sdu.Add(new ComboBoxItem() { Item = "沙尘暴" });
            comboBoxItems_Sdu.Add(new ComboBoxItem() { Item = "扬沙" });
            comboBoxItems_Sdu.Add(new ComboBoxItem() { Item = "浮尘" });
            comboBoxItems_Sdu.Add(new ComboBoxItem() { Item = "否" });
            Combo_SDU.ItemsSource = comboBoxItems_Sdu;
            Combo_SDU.DisplayMemberPath = "Item";//界面初始化时展示的值
            Combo_SDU.SelectedValuePath = "Item";//选中后展示的值
            #endregion

            #region 是否高原下拉框数据源
            comboBoxItems_Pla.Add(new ComboBoxItem() { Item = "是" });
            comboBoxItems_Pla.Add(new ComboBoxItem() { Item = "否" });
            Combo_PLA.ItemsSource = comboBoxItems_Pla;
            Combo_PLA.DisplayMemberPath = "Item";//界面初始化时展示的值
            Combo_PLA.SelectedValuePath = "Item";//选中后展示的值
            #endregion

            #region 大气环境下拉框数据源
            string cmd_Ate = "select ATE from AtmcoPara";
            DataTable TmpTable_ATE = FillTable(cmd_Ate);
            for (int i = 0; i < TmpTable_ATE.Rows.Count; i++)
            {
                comboBoxItems_Ate.Add(new ComboBoxItem() { Item = TmpTable_ATE.Rows[i].ItemArray[0].ToString() });
            }
            Combo_ATE.ItemsSource = comboBoxItems_Ate;
            Combo_ATE.DisplayMemberPath = "Item";//界面初始化时展示的值
            Combo_ATE.SelectedValuePath = "Item";//选中后展示的值
            #endregion

            #region 设置任务标识下拉框数据源
            string cmd_Batch_Mtid = "select MTID from missiontype";
            DataTable TmpTable_BATCH_MTID = FillTable(cmd_Batch_Mtid);
            //string cmd_failureratecon = "selct * from failureratecon";
            //UpdateDatabase(TmpTable_BATCH_MTID, cmd_failureratecon);
            for (int i = 0; i < TmpTable_BATCH_MTID.Rows.Count; i++)
            {
                comboBoxItems_Batch_Mtid.Add(new ComboBoxItem() { Item = TmpTable_BATCH_MTID.Rows[i].ItemArray[0].ToString() });
            }
            Combo_BATCH_MTID.ItemsSource = comboBoxItems_Batch_Mtid;
            Combo_BATCH_MTID.DisplayMemberPath = "Item";//界面初始化时展示的值
            Combo_BATCH_MTID.SelectedValuePath = "Item";//选中后展示的值
            #endregion

        }
        #endregion

        #region 默认参数保存按钮
        private void Btn_Save_FailureRateBasePara_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateDatabase(Table_TemperatureHumidity, cmd_temp_hump) && UpdateDatabase(Table_Desert, cmd_des) && UpdateDatabase(Table_Plateau, cmd_pla) && UpdateDatabase(Table_Atomsphere, cmd_ate))
            {
                MessageBox.Show("保存成功！");
            }

        }
        #endregion

        #region 读取环境参数
        /// <summary>
        /// 根据下拉框选择内容动态读取数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ReadData(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                if (Combo_MAREA.SelectedValue != null && Combo_MSEA.SelectedValue != null && Combo_DES.SelectedValue != null && Combo_SDU.SelectedValue != null && Combo_PLA.SelectedValue != null && Combo_ATE.SelectedValue != null)
                {
                    string tmpcmd_temphump = "SELECT TEMP,HUMP FROM tempandhump WHERE MAREA LIKE '%" + Combo_MAREA.SelectedValue.ToString() + "%'AND MSEA LIKE '%" + Combo_MSEA.SelectedValue.ToString() + "%';";
                    DataTable tmpdatatable_temphump = FillTable(tmpcmd_temphump);
                    TEMP = double.Parse(tmpdatatable_temphump.Rows[0][0].ToString());
                    HUMP = double.Parse(tmpdatatable_temphump.Rows[0][1].ToString());
                    string tmpcmd_dup = "SELECT DUP FROM dustenpara WHERE DES LIKE '%" + Combo_DES.SelectedValue.ToString() + "%'AND SDU LIKE '%" + Combo_SDU.SelectedValue.ToString() + "%';";
                    DataTable tmpdatatable_dup = FillTable(tmpcmd_dup);
                    DUP = double.Parse(tmpdatatable_dup.Rows[0][0].ToString());
                    string tmpcmd_srp = "SELECT SRP FROM solarrapara WHERE PLA LIKE '%" + Combo_PLA.SelectedValue.ToString() + "%'AND MSEA LIKE '%" + Combo_MSEA.SelectedValue.ToString() + "%';";
                    DataTable tmpdatatable_srp = FillTable(tmpcmd_srp);
                    SRP = double.Parse(tmpdatatable_srp.Rows[0][0].ToString());
                    string tmpcmd_acp = "SELECT ACP FROM Atmcopara WHERE ATE LIKE '%" + Combo_ATE.SelectedValue.ToString() + "%';";
                    DataTable tmpdatatable_acp = FillTable(tmpcmd_acp);
                    ACP = double.Parse(tmpdatatable_acp.Rows[0][0].ToString());
                    TB_TEMP.Text = TEMP.ToString();
                    TB_HUMP.Text = HUMP.ToString();
                    TB_DUP.Text = DUP.ToString();
                    TB_SRP.Text = SRP.ToString();
                    TB_ACP.Text = ACP.ToString();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region 批量设置任务标识
        private void Combo_MTID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tmp_mtid = "select*from missionencon WHERE MTID LIKE '%" + Combo_BATCH_MTID.SelectedValue.ToString() + "%' ;";
            DataTable temptable_mtid = FillTable(tmp_mtid);
            string tmp_frcon = "select*from failureratecon WHERE MTID LIKE '%" + Combo_BATCH_MTID.SelectedValue.ToString() + "%' ;";
            DataTable temptable_frcon = FillTable(tmp_frcon);
            foreach (DataRow row in temptable_mtid.Rows)
            {
                Combo_MAREA.SelectedValue = temptable_mtid.Rows[0][1];
                Combo_MSEA.SelectedValue = temptable_mtid.Rows[0][2];
                Combo_DES.SelectedValue = temptable_mtid.Rows[0][3];
                Combo_SDU.SelectedValue = temptable_mtid.Rows[0][4];
                Combo_PLA.SelectedValue = temptable_mtid.Rows[0][5];
                Combo_ATE.SelectedValue = temptable_mtid.Rows[0][6];
                TB_TEMP.Text = temptable_frcon.Rows[0][2].ToString();
                TB_HUMP.Text = temptable_frcon.Rows[0][3].ToString();
                TB_DUP.Text = temptable_frcon.Rows[0][4].ToString();
                TB_SRP.Text = temptable_frcon.Rows[0][5].ToString();
                TB_ACP.Text = temptable_frcon.Rows[0][6].ToString();
            }
        }
        #endregion

        #region 大气环境下拉框选择后文本框读取对应参数并保存至数据库
        private void Combo_ATE_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combo_MAREA.SelectedValue != null && Combo_MSEA.SelectedValue != null && Combo_DES.SelectedValue != null && Combo_SDU.SelectedValue != null && Combo_PLA.SelectedValue != null && Combo_ATE.SelectedValue != null)

            {
                ReadData(sql);
                string tmp_missionencon = "update missionencon set MAREA='" + Combo_MAREA.SelectedValue.ToString() + "',MSEA='" + Combo_MSEA.SelectedValue.ToString() + "',DES='" + Combo_DES.SelectedValue.ToString() + "',SDU='" + Combo_SDU.SelectedValue.ToString() + "',PLA='" + Combo_PLA.SelectedValue.ToString() + "',ATE='" + Combo_ATE.SelectedValue.ToString() + "';";
                UpdateMissiontype(tmp_missionencon);
                string tmp_failureratecon = "update failureratecon set TEMP='" + TEMP + "',HUMP='" + HUMP + "',DUP='" + DUP + "',SRP='" + SRP + "',ACP='" + ACP + "' where MTID ='" + Combo_BATCH_MTID.SelectedValue.ToString() + "';";
                UpdateMissiontype(tmp_failureratecon);
            }
            else
            {
            }
        }
        #endregion

        #region 批量设置形参(本模块未用，为其他模块保留)
        //        /// <summary>
        //        /// 批量设置形参文本框
        //        /// </summary>
        //        /// <param name="sender"></param>
        //        /// <param name="e"></param>
        //        private void TB_SHP_TextChanged(object sender, TextChangedEventArgs e)
        //        {

        //            SHP_validatingResult = SHP_ValidationFlag(sender, e);
        //            if (SHP_validatingResult == true)
        //            {
        //                BATCHSHP_Distribution = TB_SHP.Text.ToString();

        //                for (int i = 0; i < Table_FailureRateInformation.Rows.Count; i++)//修改Table_Spare4355_Outcome的分布类型
        //                {
        //                    DataRow tmpRow = Table_FailureRateInformation.Rows[i];
        //                    tmpRow.BeginEdit();
        //                    //修改表中元素值语句↓  整体上需要BeginEdit\EndEdit\AcceptChanges三个函数结构方可
        //                    tmpRow["基础故障率形参"] = BATCHSHP_Distribution;

        //                    tmpRow.EndEdit();
        //                    tmpRow.AcceptChanges();
        //                }
        //                DataGrid_FailureRateInformation.ItemsSource = Table_FailureRateInformation.DefaultView;

        //            }//第一次进入该模块输入批量分布时，需要判断表格是否为空，为空则不操作
        //            else
        //            {
        //                MessageBox.Show("请先加载产品基本信息！");
        //            }

        //        }
        //        public void SetSHP(object sender, RoutedEventArgs e, double tmpSHP)
        //        {
        //            if (Table_FailureRateInformation != null)
        //            {
        //                //循环遍历修改保障概率
        //                for (int i = 0; i < Table_FailureRateInformation.Rows.Count; i++)
        //                {
        //                    DataRow tmpRow = Table_FailureRateInformation.Rows[i];
        //                    tmpRow.BeginEdit();

        //                    tmpRow["基础故障率形参"] = tmpSHP;

        //                    tmpRow.EndEdit();
        //                    tmpRow.AcceptChanges();
        //                }
        //                DataGrid_FailureRateInformation.ItemsSource = Table_FailureRateInformation.DefaultView;
        //            }//第一次进入该模块输入保障概率要求值时，需要判断表格是否为空，为空则不操作
        //            else
        //            {
        //                MessageBox.Show("请先加载备件基础信息");
        //            }
        //        }
        //        public bool SHP_ValidationFlag(object sender, RoutedEventArgs e)
        //        {
        //            if (Table_FailureRateInformation != null)

        //            {
        //                //数据非空时
        //                if (TB_SHP.Text != "")
        //                {
        //                    //验证数据是否符合double格式
        //                    if (Regex.IsMatch(TB_SHP.Text, @"^[+-]?\d*[.]?\d*$"))
        //                    {
        //                        double tmpData_To_Validate = double.Parse(TB_SHP.Text);//将待验证数据从TextBox中取出来存于临时变量中
        //                                                                               //待验证数据数据大于零
        //                        if ((tmpData_To_Validate >= 0))
        //                        {
        //                            BATCH_SHP = tmpData_To_Validate;
        //                            return true;//数据验证通过
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("形参应大于0！");
        //                            TB_SHP.Text = "";
        //                            return false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("请输入正确的浮点型数据！");
        //                        TB_SHP.Text = "";
        //                        return false;
        //                    }

        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        /// <summary>
        //        /// 批量设置尺参
        //        /// </summary>
        //        /// <param name="sender"></param>
        //        /// <param name="e"></param>
        //        private void TB_SCP_TextChanged(object sender, TextChangedEventArgs e)
        //        {
        //            SCP_validatingResult = SCP_ValidationFlag(sender, e);
        //            if (SCP_validatingResult == true)
        //            {
        //                BATCHSCP_Distribution = TB_SCP.Text.ToString();

        //                for (int i = 0; i < Table_FailureRateInformation.Rows.Count; i++)//修改Table_Spare4355_Outcome的分布类型
        //                {
        //                    DataRow tmpRow = Table_FailureRateInformation.Rows[i];
        //                    tmpRow.BeginEdit();
        //                    //修改表中元素值语句↓  整体上需要BeginEdit\EndEdit\AcceptChanges三个函数结构方可
        //                    tmpRow["基础故障率尺参"] = BATCHSCP_Distribution;

        //                    tmpRow.EndEdit();
        //                    tmpRow.AcceptChanges();
        //                }
        //                DataGrid_FailureRateInformation.ItemsSource = Table_FailureRateInformation.DefaultView;

        //            }//第一次进入该模块输入批量分布时，需要判断表格是否为空，为空则不操作
        //            else
        //            {
        //                MessageBox.Show("请先加载产品基本信息！");
        //            }
        //        }
        //#endregion

        //        #region 形参尺参合法性验证 赋值
        //        /// <summary>
        //        /// 形参合法性验证
        //        /// </summary>
        //        /// <param name="sender"></param>
        //        /// <param name="e"></param>
        //        /// <returns></returns>
        //        public bool SHP_validationFlag(object sender, RoutedEventArgs e)
        //        {
        //            //数据非空时
        //            if (TB_SHP.Text != "")
        //            {
        //                //验证数据是否符合double格式
        //                if (Regex.IsMatch(TB_SHP.Text, @"^[+-]?\d*[.]?\d*$"))
        //                {
        //                    double tmpData_To_Validate_SHP = double.Parse(TB_SHP.Text);//将待验证数据从TextBox中取出来存于临时变量中
        //                    //待验证数据数据在[0.1]区间
        //                    if (tmpData_To_Validate_SHP >= 0)
        //                    {
        //                        SHP = tmpData_To_Validate_SHP;
        //                        return true;//数据验证通过
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("形参应大于0！");
        //                        TB_SHP.Text = "1";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show("请输入正确的浮点型数据！");
        //                    TB_SHP.Text = "1";
        //                    return false;
        //                }

        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }

        //        /// <summary>
        //        /// 尺参合法性验证
        //        /// </summary>
        //        /// <param name="sender"></param>
        //        /// <param name="e"></param>
        //        /// <returns></returns>
        //        public bool SCP_ValidationFlag(object sender, RoutedEventArgs e)
        //        {
        //            if (Table_FailureRateInformation != null)

        //            {//数据非空时

        //                if (TB_SCP.Text != "")
        //                {
        //                    //验证数据是否符合double格式
        //                    if (Regex.IsMatch(TB_SCP.Text, @"^[+-]?\d*[.]?\d*$"))
        //                    {
        //                        double tmpData_To_Validate = double.Parse(TB_SCP.Text);//将待验证数据从TextBox中取出来存于临时变量中
        //                                                                               //待验证数据数据大于零
        //                        if ((tmpData_To_Validate >= 0.5))
        //                        {
        //                            BATCH_SCP = tmpData_To_Validate;
        //                            return true;//数据验证通过
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("尺参应大于0.5！");
        //                            TB_SCP.Text = "";
        //                            return false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("请输入正确的浮点型数据！");
        //                        TB_SCP.Text = "";
        //                        return false;
        //                    }

        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        private void TB_SHP_LostFocus(object sender, RoutedEventArgs e)//设置形参
        //        {
        //            SHP_validationFlag(sender, e);
        //        }
        #endregion

        #region 加载产品更换概率信息表
        private void DataGrid_MaintenanceProbabilityInformation_Loaded(object sender, RoutedEventArgs e)
        {
            string cmd = "select IID from item";
            Table_MaintenanceProbabilityInformation = FillTable(cmd);
            Table_MaintenanceProbabilityInformation.Columns.Add("AF");
            Table_MaintenanceProbabilityInformation.Columns.Add("FDM");
            List<string> DataGrid_MaintenanceProbabilityInformation_ChineseColumnName = new List<string>()//存储DataGrid中文列名的列表，标题顺序应按照前后顺序设置
                         { "产品标识","功能", "故障检测方法"};
            DataGrid_MaintenanceProbabilityInformation.ItemsSource = Table_MaintenanceProbabilityInformation.DefaultView;
            setColumnName(DataGrid_MaintenanceProbabilityInformation, DataGrid_MaintenanceProbabilityInformation_ChineseColumnName);
            autoAddColumnWidth(DataGrid_FailureRateInformation);//引用函数增加列宽度
                                                                //不显示资源编号列

            //string cmd_fm = "select AFM,FMFR,INI,PLA from Failuremodecom";
            //Table_FailureMode = FillTable(cmd_fm);
            //DataTable_FailureMode.ItemsSource = Table_FailureMode.DefaultView;
        }

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

        //private void DataGrid_MaintenanceProbabilityInformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    string CurrentSNAME = getSelectedRowItem(DataGrid_MaintenanceProbabilityInformation, 0);//获取当前选择选中的装备名称
        //    if (LastSNAME == null)
        //    {
        //        LastSNAME = CurrentSNAME;
        //    }

        //    #region 保存当前装备的人员专业数据
        //    //屏蔽第一次运行选择装备时，专业数据表为null的情况
        //    if (tmpTable_FailureMode != null)
        //    {
        //        string cmd_SaveLastFailureModeMessage = "SELECT AFM,FMFR,INI,PLA from Failuremodecom where IID='" + LastSNAME + "';";
        //        UpdateDatabase(tmpTable_FailureMode, cmd_SaveLastFailureModeMessage);
        //    }
        //    #endregion
        //    string cmd_GetCurrentFailureModeMessage = "SELECT AFM,FMFR,INI,PLA from Failuremodecom where IID='" + CurrentSNAME + "';";
        //    tmpTable_FailureMode = FillTable(cmd_GetCurrentFailureModeMessage);
        //    List<string> lstHeader = new List<string>() { "故障模式", "故障模式频数比", "影响强度", "维修方式" };
        //    setColumnName(tmpTable_FailureMode, lstHeader);
        //    DataGrid_FailureMode.ItemsSource = tmpTable_FailureMode.DefaultView;
        //    autoAddColumnWidth(DataGrid_FailureMode);
        //    //如果存在以前的历史数据，把历史数据读入lst
        //    //if (tmpTable_FailureMode.Rows.Count != 0)
        //    //{
        //    //    DataIOTask.ReadData(connString, CalcuTask.T, DataIO.TableName.INPteqPMtask);
        //    //}
        //}
        #endregion

        #region 点击更换概率信息表任意行读取对应故障模式信息表并保存至数据库
        private void DataGrid_MaintenanceProbabilityInformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid_MaintenanceProbabilityInformation.SelectedItem != null)
            {
                string CurrentIID = getSelectedRowItem(DataGrid_MaintenanceProbabilityInformation, 0);//获取当前选择选中的装备名称


                #region 保存当前产品的故障模式数据
                //屏蔽第一次运行选择产品时，专业数据表为null的情况
                if (tmpTable_FailureMode != null)
                {
                    DataTable ChangedTable = tmpTable_FailureMode.GetChanges();//获得用户修改的行
                    if (ChangedTable != null)
                    {
                        foreach (DataRow tmpRow in ChangedTable.Rows)
                        {
                            if (tmpRow.RowState == DataRowState.Added)
                            {
                                string tmpAF;
                                string tmpFDM;

                                for (int i = 0; i < Table_MaintenanceProbabilityInformation.Rows.Count; i++)
                                {
                                    if (Table_MaintenanceProbabilityInformation.Rows[i][0].ToString() == LastIID)
                                    {
                                        tmpAF = Table_MaintenanceProbabilityInformation.Rows[i][1].ToString();
                                        tmpFDM = Table_MaintenanceProbabilityInformation.Rows[i][2].ToString();
                                        string tmpCmd = "select * from failuremodecom";//重新查询一张完整的表
                                        DataTable tmpTable_To_AddNewRow = FillTable(tmpCmd);
                                        DataRow NewDataRow = tmpTable_To_AddNewRow.NewRow();//为完整表新建一行
                                        NewDataRow["IID"] = LastIID;//为新行的各个字段赋值
                                        NewDataRow["AF"] = tmpAF;
                                        NewDataRow["FDM"] = tmpFDM;
                                        NewDataRow["AFM"] = tmpRow[0];
                                        NewDataRow["FMFR"] = tmpRow[1];
                                        NewDataRow["INI"] = tmpRow[2];
                                        NewDataRow["PLA"] = tmpRow[3];
                                        tmpTable_To_AddNewRow.Rows.Add(NewDataRow);

                                        UpdateDatabase(tmpTable_To_AddNewRow, tmpCmd);
                                    }
                                }
                            }
                            else
                            {
                                string cmd_SaveLastHumanMessage = "SELECT AFM,FMFR,INI,PLA FROM failuremodecom where IID='" + LastIID + "';";
                                UpdateDatabase(tmpTable_FailureMode, cmd_SaveLastHumanMessage);
                            }
                        }
                    }



                }
                #endregion
                string cmd_GetCurrentHumanMessage = "SELECT AFM,FMFR,INI,PLA FROM failuremodecom where IID = '" + CurrentIID + "'; ";
                tmpTable_FailureMode = FillTable(cmd_GetCurrentHumanMessage);
                List<string> lstHeader = new List<string>() { "故障模式", "故障模式频数比", "是否产生影响", "维修方式" };
                DataGrid_FailureMode.ItemsSource = tmpTable_FailureMode.DefaultView;
                setColumnName(DataGrid_FailureMode, lstHeader);
                autoAddColumnWidth(DataGrid_FailureMode);

                LastIID = CurrentIID;//保存并更新完数据之后，将当前装备名称存为“上一装备名称”
                                     //如果存在以前的历史数据，把历史数据读入lst
                if (Table_MaintenanceProbabilityInformation.Rows.Count != 0)
                {
                    //DataIOTask.ReadData(connString, CalcuTask.T, DataIO.TableName.INPteqPMtask);
                }
            }
            else { }

        }
        private void DataGrid_FailureMode_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "是否产生影响")
            {
                DataGridComboBoxColumn tmpComboBoxColumn = new DataGridComboBoxColumn();
                tmpComboBoxColumn.Header = "是否产生影响";
                ObservableCollection<ComboBoxItem> lstComboboxItem = new ObservableCollection<ComboBoxItem>();
                lstComboboxItem.Add(new ComboBoxItem() { Item = "是" });
                lstComboboxItem.Add(new ComboBoxItem() { Item = "否" });
                tmpComboBoxColumn.ItemsSource = lstComboboxItem;//设置下拉框内容的数据源
                tmpComboBoxColumn.DisplayMemberPath = "Item";//界面初始化时展示的值
                tmpComboBoxColumn.SelectedValuePath = "Item";//选中后展示的值
                Binding binding = new Binding();     //实例化一个binding对象
                binding.Path = new PropertyPath("是否产生影响");     //设置需要绑定到的那一列的列名
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                tmpComboBoxColumn.SelectedValueBinding = binding;
                e.Column = tmpComboBoxColumn;
            }
            else if (e.PropertyName == "维修方式")
            {
                DataGridComboBoxColumn tmpComboBoxColumn = new DataGridComboBoxColumn();
                tmpComboBoxColumn.Header = "维修方式";
                ObservableCollection<ComboBoxItem> lstComboboxItem = new ObservableCollection<ComboBoxItem>();
                lstComboboxItem.Add(new ComboBoxItem() { Item = "直接维修" });
                lstComboboxItem.Add(new ComboBoxItem() { Item = "更换维修" });
                tmpComboBoxColumn.ItemsSource = lstComboboxItem;//设置下拉框内容的数据源
                tmpComboBoxColumn.DisplayMemberPath = "Item";//界面初始化时展示的值
                tmpComboBoxColumn.SelectedValuePath = "Item";//选中后展示的值
                Binding binding = new Binding();     //实例化一个binding对象
                binding.Path = new PropertyPath("维修方式");     //设置需要绑定到的那一列的列名
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                tmpComboBoxColumn.SelectedValueBinding = binding;
                e.Column = tmpComboBoxColumn;
            }
            else { }
        }


        #endregion

        #region 故障率折算界面计算按钮
        private void Btn_Cal_FailureRate_Click(object sender, RoutedEventArgs e)
        {
            string cmd_mtid = "select MTID,TEMP,HUMP,DUP,SRP,ACP from failureratecon";
            string cmd_iid = "select IID,PARAM1,PARAM2 from item";
            DataTable Table_mtid = FillTable(cmd_mtid);
            DataTable Table_iid = FillTable(cmd_iid);
            double FAR = 0;
            //从数据库查询没有计算结果的输出表
            string cmd_empty = "select * from OUTFailureratecon";
            DataTable tmpout_empty = FillTable(cmd_empty);
            //定义输出结果的存储变量
            List<List<double>> lstOutput_FR = new List<List<double>>();
            Calculation FR_Cal = new Calculation();
            for (int i = 0; i < Table_mtid.Rows.Count; i++)//循环装备任务列表
            {

                for (int j = 0; j < Table_iid.Rows.Count; j++)//循环产品列表
                {
                    double tmpTEMP = double.Parse(Table_mtid.Rows[i][1].ToString());
                    double tmpHUMP = double.Parse(Table_mtid.Rows[i][2].ToString());
                    double tmpDUP = double.Parse(Table_mtid.Rows[i][3].ToString());
                    double tmpSRP = double.Parse(Table_mtid.Rows[i][4].ToString());
                    double tmpACP = double.Parse(Table_mtid.Rows[i][5].ToString());
                    double tmpPARAM1 = double.Parse(Table_iid.Rows[j][1].ToString());
                    double tmpPARAM2 = double.Parse(Table_iid.Rows[j][2].ToString());
                    lstOutput_FR.Add(FR_Cal.Failure_Rate(tmpTEMP, tmpHUMP, tmpDUP, tmpSRP, tmpACP, tmpPARAM1, tmpPARAM2));
                    DataRow dataRow = tmpout_empty.NewRow();
                    dataRow["IID"] = Table_iid.Rows[j][0];
                    dataRow["MTID"] = Table_mtid.Rows[i][0];
                    tmpout_empty.Rows.Add(dataRow);
                    //tmpout_empty.Rows.Add();
                    //tmpout_empty.Rows[i * iid.Rows.Count + j][0] = iid.Rows[j][0];
                    //tmpout_empty.Rows[i * iid.Rows.Count + j][1] = mtid.Rows[i][0];
                    //更新“没有计算结果的输出表”中当前任务、产品对应的一行记录
                    foreach (DataRow tmpRow in tmpout_empty.Rows)
                    {
                        if (tmpRow[0] == Table_iid.Rows[j][0] && tmpRow[1] == Table_mtid.Rows[i][0])
                        {
                            tmpRow["CFAR"] = lstOutput_FR[i * Table_iid.Rows.Count + j][0];
                            tmpRow["MFAR"] = lstOutput_FR[i * Table_iid.Rows.Count + j][1];
                            tmpRow["FAR"] = FAR;
                        }
                    }
                }
            }
            UpdateDatabase(tmpout_empty, cmd_empty);//计算结果保存至数据库
            string tmp_readout = "select * from outfailureratecon";
            DataTable tmptable_FailureRateInformation = FillTable(tmp_readout);
            DataGrid_FailureRateInformation.ItemsSource = tmpout_empty.DefaultView;//计算结果显示在界面产品信息表中

        }
        #endregion

        #region 产品基本信息表加载

        private void DataGrid_FailureRateInformation_Loaded(object sender, RoutedEventArgs e)
        {
            string cmd = "select IID,MTID,CFAR,MFAR from OUTFailureratecon";
            Table_FailureRateInformation = FillTable(cmd);
            List<string> DataGrid_FailureRateInformation_ChineseColumnName = new List<string>()//存储DataGrid中文列名的列表，标题顺序应按照前后顺序设置
                         { "产品标识","任务标识", "折算系数", "故障率均值" };
            DataGrid_FailureRateInformation.ItemsSource = Table_FailureRateInformation.DefaultView;
            setColumnName(DataGrid_FailureRateInformation, DataGrid_FailureRateInformation_ChineseColumnName);
            autoAddColumnWidth(DataGrid_FailureRateInformation);//引用函数增加列宽度
                                                                //不显示资源编号列
        }
        //private void DataGrid_FailureRateInformation_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    if (e.PropertyName == "任务标识")
        //    {
        //        DataGridComboBoxColumn tmpComboBoxColumn = new DataGridComboBoxColumn();
        //        tmpComboBoxColumn.Header = "任务标识";

        //        string cmd = "select MTID from Failureratecon";
        //        DataTable tmpTable_MTID = FillTable(cmd);
        //        ObservableCollection<ComboBoxItem> lstComboboxItem = new ObservableCollection<ComboBoxItem>();
        //        for (int i = 0; i < tmpTable_MTID.Rows.Count; i++)
        //        {
        //            lstComboboxItem.Add(new ComboBoxItem() { Item = tmpTable_MTID.Rows[i].ItemArray[0].ToString() });
        //        }
        //        tmpComboBoxColumn.ItemsSource = lstComboboxItem;//设置下拉框内容的数据源
        //        tmpComboBoxColumn.DisplayMemberPath = "Item";//界面初始化时展示的值
        //        tmpComboBoxColumn.SelectedValuePath = "Item";//选中后展示的值
        //        Binding binding = new Binding();     //实例化一个binding对象
        //        binding.Path = new PropertyPath("任务标识");     //设置需要绑定到的那一列的列名
        //        binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        //        tmpComboBoxColumn.SelectedValueBinding = binding;

        //        e.Column = tmpComboBoxColumn;
        //    }
        //    else { }
        //}

        //private void DataGrid_FailureRateInformation_Outcome_LayoutUpdated(object sender, EventArgs e)
        //{
        //    if (DataGrid_FailureRateInformation.Columns.Count >= 5)
        //    {
        //        this.DataGrid_FailureRateInformation.Columns[0].Visibility = System.Windows.Visibility.Hidden;
        //    }
        //}
        #endregion

        #region 故障模式组合界面计算按钮
        private void Btn_Cal_MaintenanceProbability_Click(object sender, RoutedEventArgs e)
        {
            string cmd_ItemData = "select IID,PARAM1,PARAM2 from item";
            DataTable Table_ItemData = FillTable(cmd_ItemData);
            DataTable fm = new DataTable();
            string cmd_mpout = "select * from outfailuremodecom";
            DataTable Table_Output_IID_MP = FillTable(cmd_mpout);
            List<double> FMFR = new List<double>();
            List<int> INI = new List<int>();
            List<int> PLA = new List<int>();
            Calculation MP_Cal = new Calculation();
            for (int i = 0; i < Table_ItemData.Rows.Count; i++)
            {
                string cmd = "select IID,AFM,FMFR,INI,PLA  from failuremodecom where IID ='" + Table_ItemData.Rows[i][0].ToString() + "';";
                fm = FillTable(cmd);
                double PARAM1 = double.Parse(Table_ItemData.Rows[i][1].ToString());
                double PARAM2 = double.Parse(Table_ItemData.Rows[i][2].ToString());
                foreach (DataRow tmpRow in fm.Rows)
                {
                    for (int j = 0; j < fm.Rows.Count; j++)
                    {
                        FMFR.Add(double.Parse(fm.Rows[j][2].ToString()));
                        INI.Add(int.Parse(fm.Rows[j][3].ToString()));
                        PLA.Add(int.Parse(fm.Rows[j][4].ToString()));
                    }
                }
                //遍历故障模式组合-输出表，判断如果IID一致，则更新改行的MP
                foreach (DataRow tmpRow in Table_Output_IID_MP.Rows)
                {
                    if (tmpRow["IID"].ToString()== Table_ItemData.Rows[i][0].ToString())
                    {
                        tmpRow[1] = MP_Cal.Maintenance_Probability(FMFR, INI, PLA, PARAM1, PARAM2).ToString();//调用计算程序，更新“故障模式组合-输出表”的 MP 列
                    }
                }
            }
            UpdateDatabase(Table_Output_IID_MP, cmd_mpout);
            string tmp_readout = "select m.IID,m.AF,m.FDM,l.MP from failuremodecom m left join outfailuremodecom l ON m.IID=l.IID; ";
            DataTable tmptable_MPInformation = FillTable(tmp_readout);
            DataGrid_MaintenanceProbabilityInformation.ItemsSource = tmptable_MPInformation.DefaultView;//计算结果显示在界面产品维修概率信息表中
            List<string> lstHeader = new List<string>() { "资源标识", "功能说明", "各庄检测方法", "更换概率" };
            setChineseColumnName(DataGrid_MaintenanceProbabilityInformation, lstHeader);
            autoAddColumnWidth(DataGrid_MaintenanceProbabilityInformation);
        }
        #endregion


    }
}

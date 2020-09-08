using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// ResourceOptimizing_Window.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceOptimizing_Window : Window
    {
        public ResourceOptimizing_Window()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>      
        public string connString;


        //声明连接数据库的链接
        MySqlConnection conn;
        #region 实例化计算函数、IO、公共函数
        //实例化计算模块
        // Calculation CalcuTask = new Calculation();
        //实例化一个IO，方便在计算时把用户输入数据读进Ta
        //DataIO DataIOTask = new DataIO();
        #endregion

        #region 标题栏
        
        /// <summary>
        /// 窗口拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        /// <summary>
        /// 备发预计界面_最小化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// 备发预计界面_最大化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_max_Click(object sender, RoutedEventArgs e)
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
        /// 任务预计界面_关闭窗口按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("是否确认关闭备发需求预计预计窗口", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }
        #endregion
        DataTable tmp_REDF_InTable;//全局变量，存放输入数据(REDF=ReserveEngineDemandForecas)
        DataTable tmp_origional_SIM_OUT;//存放原始的仿真结果
        DataTable tmp_REDF_OutTable;//定义全局变量，用于存优化结果
        DataTable tmp_SIM_OutTable;//全局变量表，用于存放仿真结果

        bool Scheme1_Selected = false;//用户选择方案1——标识
        bool Scheme2_Selected = false;//用户选择方案2——标识

        string Last_ColumnHeader1 = "优化方案";
        string Last_ColumnHeader2 = "优化方案";

        /// <summary>
        /// 展示资源输入表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {//要读取三个表，一个是资源汇总表一个是仿真结果表,优化结果表仅展示资源id一列
            string cmd = "SELECT * FROM SpaengineforecastCollect;";//选择数据库中备发预计资源汇总表
            tmp_REDF_InTable = FillTable(cmd);
            //中文表头
            List<string> lstHeader = new List<string>() { "资源名称", "初始储备量", "单价（元）" };

            DtG_Resource_information_table.ItemsSource = tmp_REDF_InTable.DefaultView;//输入表初始设置
            setChineseColumnName(DtG_Resource_information_table, lstHeader);//用这个函数把表头换成中文
            autoAddColumnWidth(DtG_Resource_information_table);//自动调整列宽
            string cmd1 = "SELECT* FROM schemecomparison  where PID='初始方案'; ";//选择数据库中仿真方案对比表
            tmp_origional_SIM_OUT = FillTable(cmd1);
            //中文表头
            List<string> lstHeader1 = new List<string>() { "方案标识", "使用可用度", "战备完好率", "装备完好率", "能执行任务率", "任务成功率", "总费用" };
            DtG_simulation_results_table.ItemsSource = tmp_origional_SIM_OUT.DefaultView;//输入表初始设置
            setChineseColumnName(DtG_simulation_results_table, lstHeader1);//用这个函数把表头换成中文
            autoAddColumnWidth(DtG_simulation_results_table);//自动调整列宽;";//选择数据库中备发预计资源汇总表
                                                             //将输出表中标识id一行先读取在表中
            string cmdREDF = "select RID FROM outspareengineforecast;";
            tmp_REDF_OutTable = FillTable(cmdREDF);
            tmp_REDF_OutTable.Columns["RID"].ColumnName = "资源标识";//第一列列名改为中文


        }

        /// <summary>
        /// 按键：开始优化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Start_Work_Click(object sender, RoutedEventArgs e)
        {
            //需要做三次判断（1）资源信息表是否填写完整（2）空闲率阈值是否填写完整（3）资源优化比是否填写正确
            //判断方法，定义三个布尔变量
            bool JudgeNotEmpty;//表格是否完整判断符
            bool JudgeThreshold;//空闲率阈值是否完整判断符
            bool JudgeResourceRatio;//资源优化比是否完整判断符
            //一、判断资源表是否填完,遍历表中所有空格是否有空
            int p = 1;
            //遍历所有格判断是否有空
            for (int i = 0; i < tmp_REDF_InTable.Rows.Count; i++)
            {
                for (int j = 0; j < tmp_REDF_InTable.Columns.Count; j++)
                {
                    if (tmp_REDF_InTable.Rows[i][j].ToString() == "")
                    {
                        p = 0;//有空，p=0
                    }
                }
            }
            if (p == 0)//有空，表示则表格空判断为假
            {
                JudgeNotEmpty = false;
            }
            else JudgeNotEmpty = true;
            //二、判断资源优化比和空闲率阈值是否填写正确 （是判断资源优化比和空闲率是否是数值形式，是否填写在（0-1）范围内）
            double Threshold;//定义变量用于检查空闲率阈值合法性
            double ResourceRatio;//定义变量用于检查资源优化比合法性
            if (TB_Threshold.Text != "" && double.TryParse(TB_Threshold.Text, out Threshold) && 0 < Threshold && Threshold < 1)
            {
                JudgeThreshold = true;//空闲率判断为真
            }
            else JudgeThreshold = false;//判断为假
            if (TB_Resource_optimization_ratio.Text != "" && double.TryParse(TB_Resource_optimization_ratio.Text, out ResourceRatio) && 0 < ResourceRatio && ResourceRatio < 1)
            {
                JudgeResourceRatio = true;//空闲率判断为真
            }
            else JudgeResourceRatio = false;//判断为假

            //（1）判断全真，更新表格数据库，做计算
            if (JudgeNotEmpty == true && JudgeThreshold == true && JudgeResourceRatio == true)
            {
                //定义DataTable变量，用于存从数据库读出来的Spaengineforecast表
                string cmd = "select * from Spaengineforecast";
                DataTable Table_SpaengineforecastInformation = FillTable(cmd);
                Table_SpaengineforecastInformation.Rows[0][1] = TB_Resource_optimization_ratio.Text;//资源优化比 赋值
                Table_SpaengineforecastInformation.Rows[0][2] = TB_Threshold.Text;//空闲率阈值 赋值
                //更新数据库
                UpdateDatabase(Table_SpaengineforecastInformation, cmd);
                MessageBox.Show("已经开始优化，请稍等！");
                #region 计算函数，并存入表中
                #endregion
                #region 调用仿真，存入表中
                #endregion
                //(1)将两个方案结果存入优化表中
                //判断用户选择了哪种方案
                string SchemeNum;
                string CurrentColumnHeader1 = "";
                string CurrentColumnHeader2 = "";
                //此处做判断选择哪一种优化方案进行继续计算
                if (Scheme1_Selected == false)
                {
                    if (Scheme2_Selected == false)//第一次计算，没有点击过继续优化按钮
                    {
                        SchemeNum = "";
                        CurrentColumnHeader1 = Last_ColumnHeader2 + SchemeNum + "_1";
                        CurrentColumnHeader2 = Last_ColumnHeader2 + SchemeNum + "_2";
                    }
                    else
                    {
                        //点击了”按方案二继续优化“，列名后加2_
                        CurrentColumnHeader1 = Last_ColumnHeader2 + "_1";
                        CurrentColumnHeader2 = Last_ColumnHeader2 + "_2";
                    }
                }
                else
                {
                    //点击了”按方案一继续优化“，列名后加2_1，2_2
                    CurrentColumnHeader1 = Last_ColumnHeader1 + "_1";
                    CurrentColumnHeader2 = Last_ColumnHeader1 + "_2";
                }
                Scheme1_Selected = false;//将方案一二标识清空
                Scheme2_Selected = false;

                string cmd1 = "SELECT * FROM OUTSpareengineForecast ;";//读取优化结果输出表
                DataTable tmp_ThisTime_REDF_OutTable = FillTable(cmd1);

                tmp_REDF_OutTable.Columns.Add(CurrentColumnHeader1);//添加两列，列名由上面写的确定
                tmp_REDF_OutTable.Columns.Add(CurrentColumnHeader2);
                Last_ColumnHeader1 = CurrentColumnHeader1;//将旧的列名保存下来，因为新的列名是在旧列名的基础上加的
                Last_ColumnHeader2 = CurrentColumnHeader2;
                //将plan1,plan2两列存进这个表里
                int REDF_ColumnCount = tmp_REDF_OutTable.Columns.Count;//查询表中现有多少列
                for (int i = 0; i < tmp_REDF_OutTable.Rows.Count; i++)
                {
                    for (int j = 0; j < tmp_ThisTime_REDF_OutTable.Rows.Count; j++)
                    {
                        if (tmp_REDF_OutTable.Rows[i][0].ToString() == tmp_ThisTime_REDF_OutTable.Rows[j][0].ToString())//如果资源标识相同
                        {
                            //将方案一的结果放到输出表的倒数第二列，方案二放到倒数第一列
                            tmp_REDF_OutTable.Rows[i][REDF_ColumnCount - 2] = tmp_ThisTime_REDF_OutTable.Rows[j][1].ToString();
                            tmp_REDF_OutTable.Rows[i][REDF_ColumnCount - 1] = tmp_ThisTime_REDF_OutTable.Rows[j][2].ToString();
                        }
                    }
                }
                //更新数据源增加空列之后不能用Table_PriceEstimatePara.DefaultView的方式设置数据源，该方式不会显示新增的空
                DataView tmpDataView = new DataView(tmp_REDF_OutTable);
                DtG_SEF_OUT_Table.ItemsSource = tmpDataView;
                //DtG_SEF_OUT_Table.ItemsSource = tmp_REDF_OutTable.DefaultView;//输出表初始设置    
                autoAddColumnWidth(DtG_SEF_OUT_Table);//自动调整列宽
                DtG_SEF_OUT_Table.ItemsSource = tmp_REDF_OutTable.DefaultView;
                //(2)将两个方案的优化仿真结果表读取
                string cmd2 = "SELECT * FROM SchemeComparison ;";
                //用这个临时新表读取本次仿真结果
                tmp_SIM_OutTable = FillTable(cmd2);
                tmp_SIM_OutTable.Rows[0].Delete();
                //把新表的结果加入全局表中
                //设置中文表头
                List<string> lstOUTHeader1 = new List<string>() { "优化方案", "使用可用度", "战备完好率", "装备完好率", " 能执行任务率", "任务成功率", "总费用" };
                DtG_SIM_OUT_Table.ItemsSource = tmp_SIM_OutTable.DefaultView;//输出表初始设置
                setChineseColumnName(DtG_SIM_OUT_Table, lstOUTHeader1);//用这个函数把表头换成中文
                autoAddColumnWidth(DtG_SEF_OUT_Table);//自动调整列宽

            }
            #region  //警告信息框组合
            //目前的写法比较繁琐，可以简化.
            else if (JudgeNotEmpty == false && JudgeThreshold == true && JudgeResourceRatio == true)
            {
                MessageBox.Show("请将资源信息表填写完整，不要留空！");
            }
            else if (JudgeNotEmpty == true && JudgeThreshold == false && JudgeResourceRatio == true)
            {
                MessageBox.Show("请正确填写空闲率阈值，范围为（0-1）！");
            }
            else if (JudgeNotEmpty == true && JudgeThreshold == true && JudgeResourceRatio == false)
            {
                MessageBox.Show("请正确填写资源优化比，范围为（0-1）！");
            }
            else if (JudgeNotEmpty == true && JudgeThreshold == false && JudgeResourceRatio == false)
            {
                MessageBox.Show("正确填写空闲率阈值，范围为（0-1）！请正确填写资源优化比，范围为（0-1）！");
            }
            else if (JudgeNotEmpty == false && JudgeThreshold == false && JudgeResourceRatio == true)
            {
                MessageBox.Show("请将资源信息表填写完整，不要留空！正确填写空闲率阈值，范围为（0-1）！");
            }
            else if (JudgeNotEmpty == false && JudgeThreshold == true && JudgeResourceRatio == false)
            {
                MessageBox.Show("请将资源信息表填写完整，不要留空！请正确填写资源优化比，范围为（0-1）！");
            }
            else if (JudgeNotEmpty == false && JudgeThreshold == false && JudgeResourceRatio == false)
            {
                MessageBox.Show("请将资源信息表填写完整，不要留空！请正确填写资源优化比，范围为（0-1）！正确填写空闲率阈值，范围为（0-1）！");
            }
            #endregion

        }

        /// <summary>
        /// 按键：继续优化方案1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Ctu_work1_Click(object sender, RoutedEventArgs e)
        {
            Scheme1_Selected = true;

            if (Scheme2_Selected == false)
            {
                //用户按继续优化，将输出结果的初始储备量一列替换进输入表然后计算
                //选择数据库中备发优化结果表和资源整合输入表
                string cmd1 = "SELECT * FROM spaengineforecastcollect;";//资源配置汇总表
                //DataTable new_spaengineforecastcollect = FillTable(cmd1);
                int Row_Count = tmp_REDF_OutTable.Rows.Count;//读取表一共有多少行,理论上输入输出表的行数相同
                int colum_count = tmp_REDF_OutTable.Columns.Count;//统计现在的优化结果表里一共有多少列
                for (int i = 0; i < Row_Count; i++)
                {
                    for (int j = 0; j < tmp_REDF_InTable.Rows.Count; j++)
                    {
                        //判断汇总表的资源名称是否与输出表当前行名称相同
                        if (tmp_REDF_InTable.Rows[j][0].ToString() == tmp_REDF_InTable.Rows[i][0].ToString())
                        {
                            tmp_REDF_InTable.Rows[j][1] = tmp_REDF_OutTable.Rows[i][colum_count - 2].ToString();
                        }
                    }
                }
                //更新资源整合输出表
                UpdateDatabase(tmp_REDF_InTable, cmd1);

                //do 计算

                MessageBox.Show("继续优化方案1，请点击开始优化");
            }
            else { }

        }

        /// <summary>
        /// 按键：继续优化方案2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Ctu_work2_Click(object sender, RoutedEventArgs e)
        {
            Scheme2_Selected = true;

            //屏蔽方案1 的click事件
            if (Scheme1_Selected == false)
            {
                //用户按继续优化，将输出结果的初始储备量一列替换进输入表然后计算
                //选择数据库中备发优化结果表和资源整合输入表
                string cmd1 = "SELECT * FROM spaengineforecastcollect;";//资源配置汇总表
                //DataTable new_spaengineforecastcollect = FillTable(cmd1);
                int Row_Count = tmp_REDF_OutTable.Rows.Count;//读取表一共有多少行,理论上输入输出表的行数相同
                int colum_count = tmp_REDF_OutTable.Columns.Count;//统计现在的优化结果表里一共有多少列
                for (int i = 0; i < Row_Count; i++)
                {
                    for (int j = 0; j < tmp_REDF_InTable.Rows.Count; j++)
                    {
                        //判断汇总表的资源名称是否与输出表当前行名称相同
                        if (tmp_REDF_InTable.Rows[j][0].ToString() == tmp_REDF_InTable.Rows[i][0].ToString())
                        {
                            tmp_REDF_InTable.Rows[j][1] = tmp_REDF_OutTable.Rows[i][colum_count - 1].ToString();
                        }
                    }
                }
                //更新资源整合输出表
                UpdateDatabase(tmp_REDF_InTable, cmd1);
                //do 计算
                MessageBox.Show("继续优化方案2，请点击开始优化");
            }
            else { }


        }

        private void Btn_Save_final_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("保存成功！");
        }

        
    }


}

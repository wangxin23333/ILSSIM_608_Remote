using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;
using 寿命消耗折算;
using MySqlX.XDevAPI.Common;

namespace ILSSIM_608
{
    /// <summary>
    /// LifetimeCorrection_Window.xaml 的交互逻辑
    /// </summary>
    public partial class LifetimeCorrection_Window : Window
    {
        public LifetimeCorrection_Window()
        {
            InitializeComponent();
        }
        #region 自定义函数

        public string connString;
        /// <summary>
        /// <para> 更新数据库的自定义函数</para> 
        /// <para> 更新成功时返回true，失败时返回false</para> 
        /// </summary>
        /// <param name="tmpTable"></param>
        /// <param name="tmpCmd"></param>



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

                #region 填表操作
                DataTable dt = new DataTable();
                dt.Clear();

                MySqlCommand cmd = new MySqlCommand(tmpCmd, conn);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                sda.Fill(dt);

                conn.Close();
                return dt;
                #endregion
            }

        }

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

        public DataTable GetDataTable(string sql)
        {
            DataTable dataTable = new DataTable();


            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn);


                conn.Open();

                MySqlDataAdapter da = new MySqlDataAdapter();

                da.SelectCommand = cmd;

                da.Fill(dataTable);//

                conn.Close();





            }

            return dataTable;


        }
        
        public string[] Calculation(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataAdapter MydataAdapter = new MySqlDataAdapter(cmd);

                MydataAdapter.TableMappings.Add("Table", "typicalprofile");
                MydataAdapter.TableMappings.Add("Table1", "enginelifecon");
                //MydataAdapter.TableMappings.Add("Table2", "failurerateconInformation");
                DataSet ds = new DataSet();
                MydataAdapter.Fill(ds);//将典型剖面表、装备任务表填充


                int K = 9;//飞行阶段列数

                List<double> taskfp = new List<double>();//典型剖面飞行任务阶段列表Flight phase，包括地面慢车、有地效悬停、起飞增速	、斜向爬升、上升转弯、巡航、盘旋飞行、下滑、着陆

                List<double> typicalfp = new List<double>();//任务剖面飞行任务阶段Flight phase，包括地面慢车、有地效悬停、起飞增速	、斜向爬升、上升转弯、巡航、盘旋飞行、下滑、着陆
                List<double> typicalttt = new List<double>(); //典型剖面任务任务总时间列
                List<double> typicaldl = new List<double>();///典型剖面设计寿命列DesignLife
                List<string> ENGINE = new List<string>();//任务表中发动机型号
                List<string> PN = new List<string>();//任务表中基准剖面名称
                List<string> MTID = new List<string>();//任务表中任务标识
                List<double> SDL = new List<double>();//任务剖面设计寿命列
                List<string> ep = new List<string>();// 环境参数列表，包括地域、季节、海拔、其他4列
                List<string> TEMP = new List<string>();//温度参数
                List<string> HUMP = new List<string>();//湿度参数


                foreach (DataRow row in ds.Tables["enginelifecon"].Rows)//将任务表中的飞行阶段的列添加到列表
                {

                    for (int i = 2; i < 11; i++)
                    {

                        if (row[i] == System.DBNull.Value)//判断任务飞行阶段是否空
                        {


                            ds.Tables["enginelifecon"].Rows[0][i] = "0";//如果为空，飞行阶段时间设为0
                            MySqlCommandBuilder MysqlCommandBuilder = new MySqlCommandBuilder(MydataAdapter);
                            MydataAdapter.Update(ds);


                        }
                        taskfp.Add(double.Parse(row[i].ToString()));

                    }


                }

                //任务剖面飞行任务阶段时间矩阵Ts的创建
                double[] task = taskfp.ToArray();//将list转化为一维数组
                double[,] Ts = new double[K, 1];//将一维数组task转化为矩阵Ts
                for (int i = 0; i < K; i++)
                {
                    Ts[i, 0] = task[i];
                }



                foreach (DataRow row1 in ds.Tables["typicalprofile"].Rows)//将典型剖面表中的飞行阶段的列添加到列表typicalfp
                {
                    for (int i = 2; i < 11; i++)
                    {
                        typicalfp.Add(double.Parse(row1[i].ToString()));
                    }


                }

                //将典型剖面飞行阶段列表typicalf转化为一维数组typical
                double[] typical = typicalfp.ToArray();
                int N = typical.Length / K; //典型剖面表的行数
                double[,] P = new double[N, K];//将一维数组typical转化为矩阵P
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < K; j++)
                    {
                        P[i, j] = typical[i * (K) + j];
                    }
                }


                //提取出典型剖面任务总时间的列添加到列表typicalttt
                foreach (DataRow row2 in ds.Tables["typicalprofile"].Rows)
                {
                    typicalttt.Add(double.Parse(row2["TTD"].ToString()));
                }

                double[] ttt = typicalttt.ToArray();//将典型剖面中任务总时间列表转化为一维数组ttt
                double[,] TTT = new double[N, 1];//将一维数组ttt转化为矩阵TTT
                for (int i = 0; i < N; i++)
                {
                    TTT[i, 0] = ttt[i];
                }

                string tmpcmd = "SELECT TEMP,HUMP FROM tempandhump WHERE MAREA LIKE '%" + ds.Tables["enginelifecon"].Rows[0]["MAREA"].ToString() + "%'AND MSEA='" + ds.Tables["enginelifecon"].Rows[0]["MSEA"].ToString() + "';";
                DataTable tmpdatatable = FillTable(tmpcmd);
                TEMP.Add(tmpdatatable.Rows[0][0].ToString());
                HUMP.Add(tmpdatatable.Rows[0][1].ToString());

                //典型剖面设计寿命列
                foreach (DataRow row3 in ds.Tables["typicalprofile"].Rows)//将典型剖面中设计寿命列添加到typicaldl列表
                {
                    typicaldl.Add(double.Parse(row3["DLF"].ToString()));
                }

                double[] DL = typicaldl.ToArray();//将typicaldl列表转化为一维数组 DL
                string tmpcmdDL = "SELECT DLF FROM typicalprofile WHERE TPNAME ='"+ ds.Tables["enginelifecon"].Rows[0]["DAP"] + "' ";
                DataTable tmpdatatableDL = FillTable(tmpcmdDL);
                SDL.Add(double.Parse(tmpdatatableDL.Rows[0][0].ToString()));
                
                foreach (DataRow row5 in ds.Tables["enginelifecon"].Rows) // //提取出任务表中地域、季节、海拔、其他4列添加到列表ep
                {
                    for (int i = 13; i < 17; i++)
                    {
                        ep.Add(row5[i].ToString());
                    }
                }
                foreach (DataRow row6 in ds.Tables["enginelifecon"].Rows)//提取发动机型号列添加到列表ENGINE
                {
                    ENGINE.Add(row6[0].ToString());
                }
                foreach (DataRow row7 in ds.Tables["enginelifecon"].Rows)//提取任务表中剖面名称列添加到列表PN
                {
                    PN.Add(row7[12].ToString());
                }
                foreach (DataRow row8 in ds.Tables["enginelifecon"].Rows) //提取任务表中任务标识列添加到列表MTID
                {
                    MTID.Add(row8[1].ToString());
                }
                
                
                conn.Close();

                double E = 1;                                      //初始环境系数
                double[] EP = new double[4];//温度，湿度，海拔，人因影响系数4个环境参数系数

                EP[0] = Calculater.TCC(double.Parse(TEMP[0]));//通过温度参数值计算得到温度系数
                EP[1] = Calculater.HCC(double.Parse(HUMP[0]));//通过湿度参数值计算得到湿度系数
                EP[2] = Calculater.ACC(double.Parse(ep[2]));//在环境参数列表ep中提出海拔参数值并计算得到海拔系数
                EP[3] = double.Parse(ep[3]);//在环境参数列表ep中提出人因影响系数



                string[] str1 = new string[5];//建立一个字符串数组，用于接收输出结果，其中第四个元素接收计算结果
                str1[0] = ENGINE[0];//将发动机型号填充到数组
                str1[1] = PN[0];//将基准剖面名称填充到数组
                str1[2] = MTID[0];//将任务标识填充到数组
                str1[4] = Convert.ToString(SDL[0]);//将任务表中设计寿命填充到数组

                double TaskLifeConsumptionResults = 0;//开始计算前寿命消耗设为0


                double[,] TransP = Calculater.Transposition(P);//将P转置
                double[,] PTP = Calculater.Multiplication(P.GetLength(0), P.GetLength(1), TransP.GetLength(1), P, TransP);//P和其转置的乘积

                double[,] PMI = Calculater.MatrixInversion(PTP);//将PTP求逆



                if (PMI == null)//如果矩阵PMI为空即矩阵PTP的逆不存在，否则就计算寿命消耗值
                {
                    MessageBox.Show("求逆错误！寿命消耗折算无法正常计算！");

                    str1[3] = "-99999"; //将寿命计算结果设为 - 99999，说明寿命消耗不能计算；
                    return str1;

                }
                else
                {

                    double[,] MUL = Calculater.Multiplication(PMI.GetLength(0), PMI.GetLength(1), P.GetLength(1), PMI, P);//PMI与P的乘积
                    double[,] L = Calculater.Multiplication(MUL.GetLength(0), MUL.GetLength(1), Ts.GetLength(1), MUL, Ts); //剖面合成权重系数L

                    //飞行剖面折合系数
                    double[,] ccofp = Calculater.CCOFP(DL);

                    //典型剖面第一个剖面为基准剖面时寿命消耗折算结果
                    double CalculationResults = Calculater.LifeConsumptionConversion(TTT, L, ccofp, E, EP);
                    //任务表中基准剖面下寿命消耗折算结果
                    TaskLifeConsumptionResults = (typicaldl[0] * CalculationResults / SDL[0]);
                    str1[3] = TaskLifeConsumptionResults.ToString("0.########");

                }


                return str1;



            }
        }

        public void Save(string sql, string[] vs)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataAdapter MydataAdapter = new MySqlDataAdapter(cmd);

                MydataAdapter.TableMappings.Add("Table", "outenginelifeconsumption");
                DataSet ds = new DataSet();
                ds.Clear();
                MydataAdapter.Fill(ds);
                foreach (DataRow row in ds.Tables["outenginelifeconsumption"].Rows)//将任务表中的飞行阶段的列添加到列表
                {

                    for (int i = 0; i < 4; i++)
                    {

                        if (row[i] == System.DBNull.Value)//判断任务飞行阶段是否空
                        {


                            ds.Tables["outenginelifeconsumption"].Rows[0][i] = vs[i];//如果为空，飞行阶段时间设为0
                            MySqlCommandBuilder MysqlCommandBuilder = new MySqlCommandBuilder(MydataAdapter);
                            MydataAdapter.Update(ds);


                        }


                    }


                }
            }

        }

        #endregion

        #region 全局变量
        /// <summary>
        /// 计算结果数组
        /// </summary>
        string[] result;


        #endregion

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
            if (MessageBox.Show("是否确认关闭", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();

            }
        }

        private void MyGrid_Loaded(object sender, RoutedEventArgs e)
        {
            string sql1 = "select * from enginelifecon ";

            DataTable dataTable1 = GetDataTable(sql1);

            MyGrid.ItemsSource = dataTable1.DefaultView;

            MyGrid1.ItemsSource = dataTable1.DefaultView;
        }

        /// <summary>
        /// 开始计算 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string sql2 = "select * from typicalProfile;select *from enginelifecon;";//读取数据库中的典型剖面表，装备任务表

            result = Calculation(sql2);//根据计算方法得到发动机寿命消耗值

            string str1 = "";


            if (result[3] != "-99999")//即寿命消耗值可计算并显示结果
            {

                str1 = str1 + string.Format("寿命消耗结果为:" + result[3] + "分钟\n");
                EngineBox.Text = result[0];
                MtidBox.Text = result[2];
                ProfileBox.Text = result[1];
                MyText.Text = str1;

                #region 将计算结果保存回数据库
                string cmd_SaveOutputData = "select * from OUTEnginelifeconsumption";
                DataTable Table_SaveOutputData = FillTable(cmd_SaveOutputData);
                if (Table_SaveOutputData.Rows.Count != 0)//当前只支持一个任务的折算，所以返回的result只是一组字符串，待改成兼容两种任务的情况
                {
                    foreach (DataRow tmpRow in Table_SaveOutputData.Rows)
                    {
                        if (tmpRow["ENGINE"].ToString()== result[0])//result的各位与数据库字段顺序不对应？？？？？
                        {
                            tmpRow["MTID"] = result[2];
                            tmpRow["DDL"] = Convert.ToDouble(result[4]);
                            tmpRow["EFC"] = Convert.ToDouble(result[3]);
                        }
                    }
                }
                UpdateDatabase(Table_SaveOutputData, cmd_SaveOutputData);
                #endregion
            }
        }
    }
}

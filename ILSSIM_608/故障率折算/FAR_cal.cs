//using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ILSSIM_608
{
    public partial class Calculation
    {
        //private static string connString = "server=localhost;database=enginecbm_db;uid=root;pwd=123456;";
        //public static DataTable GetDataTable(string sql)
        //{
        //    DataTable dataTable = new DataTable();


        //    using (MySqlConnection conn = new MySqlConnection(connString))
        //    {
        //        MySqlCommand cmd = new MySqlCommand(sql, conn);


        //        conn.Open();

        //        MySqlDataAdapter da = new MySqlDataAdapter();

        //        da.SelectCommand = cmd;

        //        da.Fill(dataTable);//

        //        conn.Close();





        //    }

        //    return dataTable;


        //}
        /// <summary>
        /// <para>成附件故障率</para>
        /// <para>输入：任务执行地区,任务执行季节,是否处于沙漠地区,沙尘天气,是否处于高原地区,大气环境,任务时长</para>
        /// <para>输出：备件故障率FAR,备件折算系数CFAR,备件故障率均值MFAR</para>
        /// </summary>
        /// <returns></returns>

        ////环境参数后续改为数据库读取
        //#region 温度参数和湿度参数数据
        //public class Environ
        //{
        //    public Environ(String marea, string msea, double temp, double hump)
        //    {
        //        this.MAREA = marea;
        //        this.MSEA = msea;
        //        this.TEMP = temp;
        //        this.HUMP = hump;
        //    }

        //    public string MAREA { get; set; }

        //    public string MSEA { get; set; }

        //    public double TEMP { get; set; }

        //    public double HUMP { get; set; }
        //}
        //public class EnvironList : List<Environ>
        //{
        //    public EnvironList()
        //    {
        //        this.Add(new Environ("雷州半岛", "春", -0.016, -0.07));
        //        this.Add(new Environ("雷州半岛", "夏", -0.02, -0.1));
        //        this.Add(new Environ("雷州半岛", "秋", -0.016, -0.07));
        //        this.Add(new Environ("雷州半岛", "冬", -0.018, -0.09));
        //        this.Add(new Environ("海南岛", "春", -0.016, -0.07));
        //        this.Add(new Environ("海南岛", "夏", -0.02, -0.1));
        //        this.Add(new Environ("海南岛", "秋", -0.016, -0.07));
        //        this.Add(new Environ("海南岛", "冬", -0.018, -0.09));
        //        this.Add(new Environ("台湾南部", "春", -0.016, -0.07));
        //        this.Add(new Environ("台湾南部", "夏", -0.02, -0.1));
        //        this.Add(new Environ("台湾南部", "秋", -0.016, -0.07));
        //        this.Add(new Environ("台湾南部", "冬", -0.018, -0.09));
        //        this.Add(new Environ("秦岭以南", "春", -0.012, -0.05));
        //        this.Add(new Environ("秦岭以南", "夏", -0.02, -0.09));
        //        this.Add(new Environ("秦岭以南", "秋", -0.012, -0.05));
        //        this.Add(new Environ("秦岭以南", "冬", -0.005, -0.06));
        //        this.Add(new Environ("长江流域", "春", -0.012, -0.05));
        //        this.Add(new Environ("长江流域", "夏", -0.02, -0.09));
        //        this.Add(new Environ("长江流域", "秋", -0.012, -0.05));
        //        this.Add(new Environ("长江流域", "冬", -0.005, -0.06));
        //        this.Add(new Environ("四川", "春", -0.012, -0.05));
        //        this.Add(new Environ("四川", "夏", -0.02, -0.09));
        //        this.Add(new Environ("四川", "秋", -0.012, -0.05));
        //        this.Add(new Environ("四川", "冬", -0.005, -0.06));
        //        this.Add(new Environ("珠江流域", "春", -0.012, -0.05));
        //        this.Add(new Environ("珠江流域", "夏", -0.02, -0.09));
        //        this.Add(new Environ("珠江流域", "秋", -0.012, -0.05));
        //        this.Add(new Environ("珠江流域", "冬", -0.005, -0.06));
        //        this.Add(new Environ("台湾北部", "春", -0.012, -0.05));
        //        this.Add(new Environ("台湾北部", "夏", -0.02, -0.09));
        //        this.Add(new Environ("台湾北部", "秋", -0.012, -0.05));
        //        this.Add(new Environ("台湾北部", "冬", -0.005, -0.06));
        //        this.Add(new Environ("福建", "春", -0.012, -0.05));
        //        this.Add(new Environ("福建", "夏", -0.02, -0.09));
        //        this.Add(new Environ("福建", "秋", -0.012, -0.05));
        //        this.Add(new Environ("福建", "冬", -0.005, -0.06));
        //        this.Add(new Environ("新疆天山以南", "春", -0.012, 0));
        //        this.Add(new Environ("新疆天山以南", "夏", -0.02, 0));
        //        this.Add(new Environ("新疆天山以南", "秋", -0.012, 0));
        //        this.Add(new Environ("新疆天山以南", "冬", -0.005, 0));
        //        this.Add(new Environ("戈壁沙漠", "春", -0.012, 0));
        //        this.Add(new Environ("戈壁沙漠", "夏", -0.02, 0));
        //        this.Add(new Environ("戈壁沙漠", "秋", -0.012, 0));
        //        this.Add(new Environ("戈壁沙漠", "冬", -0.005, 0));
        //        this.Add(new Environ("秦岭以北", "春", -0.006, 0));
        //        this.Add(new Environ("秦岭以北", "夏", -0.012, -0.01));
        //        this.Add(new Environ("秦岭以北", "秋", -0.006, 0));
        //        this.Add(new Environ("秦岭以北", "冬", -0.008, -0.008));
        //        this.Add(new Environ("内蒙南部", "春", -0.006, 0));
        //        this.Add(new Environ("内蒙南部", "夏", -0.012, -0.01));
        //        this.Add(new Environ("内蒙南部", "秋", -0.006, 0));
        //        this.Add(new Environ("内蒙南部", "冬", -0.008, -0.008));
        //        this.Add(new Environ("华北", "春", -0.006, 0));
        //        this.Add(new Environ("华北", "夏", -0.012, -0.01));
        //        this.Add(new Environ("华北", "秋", -0.006, 0));
        //        this.Add(new Environ("华北", "冬", -0.008, -0.008));
        //        this.Add(new Environ("东北南部", "春", -0.006, 0));
        //        this.Add(new Environ("东北南部", "夏", -0.012, -0.01));
        //        this.Add(new Environ("东北南部", "秋", -0.006, 0));
        //        this.Add(new Environ("东北南部", "冬", -0.008, -0.008));
        //        this.Add(new Environ("内蒙中部和西部", "春", -0.006, 0));
        //        this.Add(new Environ("内蒙中部和西部", "夏", -0.012, -0.003));
        //        this.Add(new Environ("内蒙中部和西部", "秋", -0.006, 0));
        //        this.Add(new Environ("内蒙中部和西部", "冬", -0.008, -0.003));
        //        this.Add(new Environ("陕西北部", "春", -0.006, 0));
        //        this.Add(new Environ("陕西北部", "夏", -0.012, -0.003));
        //        this.Add(new Environ("陕西北部", "秋", -0.006, 0));
        //        this.Add(new Environ("陕西北部", "冬", -0.008, -0.003));
        //        this.Add(new Environ("甘肃北部", "春", -0.006, 0));
        //        this.Add(new Environ("甘肃北部", "夏", -0.012, -0.003));
        //        this.Add(new Environ("甘肃北部", "秋", -0.006, 0));
        //        this.Add(new Environ("甘肃北部", "冬", -0.008, -0.003));
        //        this.Add(new Environ("青海", "春", -0.006, 0));
        //        this.Add(new Environ("青海", "夏", -0.012, -0.003));
        //        this.Add(new Environ("青海", "秋", -0.006, 0));
        //        this.Add(new Environ("青海", "冬", -0.008, -0.003));
        //        this.Add(new Environ("新疆天山以北", "春", -0.006, 0));
        //        this.Add(new Environ("新疆天山以北", "夏", -0.012, -0.003));
        //        this.Add(new Environ("新疆天山以北", "秋", -0.006, 0));
        //        this.Add(new Environ("新疆天山以北", "冬", -0.008, -0.003));
        //        this.Add(new Environ("内蒙北部", "春", -0.006, -0.008));
        //        this.Add(new Environ("内蒙北部", "夏", -0.012, 0));
        //        this.Add(new Environ("内蒙北部", "秋", -0.006, -0.008));
        //        this.Add(new Environ("内蒙北部", "冬", -0.02, -0.01));
        //        this.Add(new Environ("黑龙江", "春", -0.006, -0.008));
        //        this.Add(new Environ("黑龙江", "夏", -0.012, 0));
        //        this.Add(new Environ("黑龙江", "秋", -0.006, -0.008));
        //        this.Add(new Environ("黑龙江", "冬", -0.02, -0.01));
        //        this.Add(new Environ("青藏高原地区", "春", -0.008, 0));
        //        this.Add(new Environ("雷州半岛", "夏", -0.012, 0));
        //        this.Add(new Environ("雷州半岛", "秋", -0.008, 0));
        //        this.Add(new Environ("雷州半岛", "冬", -0.01, 0));

        //    }
        //}
        //#endregion
        //#region 沙尘参数数据
        //public class Dust
        //{
        //    public Dust(String des, string sdu, double dup)
        //    {
        //        this.DES = des;
        //        this.SDU = sdu;
        //        this.DUP = dup;
        //    }

        //    public string DES { get; set; }

        //    public string SDU { get; set; }

        //    public double DUP { get; set; }
        //}
        //public class DustList : List<Dust>
        //{
        //    public DustList()
        //    {
        //        this.Add(new Dust("是", "强沙尘暴", -0.3));
        //        this.Add(new Dust("是", "沙尘暴", -0.28));
        //        this.Add(new Dust("是", "扬沙", -0.23));
        //        this.Add(new Dust("是", "浮尘", -0.2));
        //        this.Add(new Dust("是", "否", -0.18));
        //        this.Add(new Dust("否", "强沙尘暴", -0.25));
        //        this.Add(new Dust("否", "沙尘暴", -0.2));
        //        this.Add(new Dust("否", "扬沙", -0.15));
        //        this.Add(new Dust("否", "浮尘", -0.09));
        //        this.Add(new Dust("否", "否", 0));
        //    }
        //}
        //#endregion
        //#region 太阳辐射参数数据
        //public class Sun
        //{
        //    public Sun(String pla, string msea, double srp)
        //    {
        //        this.PLA = pla;
        //        this.MSEA = msea;
        //        this.SRP = srp;
        //    }

        //    public string PLA { get; set; }

        //    public string MSEA { get; set; }

        //    public double SRP { get; set; }
        //}
        //public class SunList : List<Sun>
        //{
        //    public SunList()
        //    {
        //        this.Add(new Sun("是", "春", -0.06));
        //        this.Add(new Sun("是", "夏", -0.16));
        //        this.Add(new Sun("是", "秋", -0.06));
        //        this.Add(new Sun("是", "冬", -0.09));
        //        this.Add(new Sun("否", "春", 0));
        //        this.Add(new Sun("否", "夏", 0));
        //        this.Add(new Sun("否", "秋", 0));
        //        this.Add(new Sun("否", "冬", 0));
        //    }
        //}
        //#endregion
        //#region 大气腐蚀参数数据
        //public class Air
        //{
        //    public Air(String ate, double acp)
        //    {
        //        this.ATE = ate;
        //        this.ACP = acp;
        //    }

        //    public string ATE { get; set; }

        //    public double ACP { get; set; }
        //}
        //public class AirList : List<Air>
        //{
        //    public AirList()
        //    {
        //        this.Add(new Air("海洋大气环境", -0.1));
        //        this.Add(new Air("工业大气环境", -0.05));
        //        this.Add(new Air("乡村大气环境", 0));
        //        this.Add(new Air("城市大气环境", -0.01));
        //        this.Add(new Air("其他", 0));
        //    }
        //}
        //#endregion
        public List<double> Failure_Rate(double TEMP, double HUMP, double DUP, double SRP, double ACP, double PARAM1, double PARAM2)//计算故障率
        //张浩田2020.07.30
        {
            #region 临时数据
            //double FAR;//定义备件故障率
            double CFAR;//定义状态描述函数
            double MFAR;//定义故障率均值
            List<double> FAR_out = new List<double> { 0, 0 };//定义列表接收返回值
            #endregion
            #region 数据库读取
            //string MAREA;//任务执行地区
            //string MSEA;//任务执行季节
            //string DES;//是否处于沙漠地区
            //string SDU;//是否有沙尘天气
            //string PLA;//是否处于高原地区
            //string ATE;//大气环境//来源：用户输入 //用户输入后保存至数据库
            //#region 读取温度参数和湿度参数
            //EnvironList environ = new EnvironList();
            //Predicate<Environ> findValue1 = delegate (Environ p)
            //{
            //    return p.MAREA.Equals(Marea) && p.MSEA.Equals(Msea);
            //};
            //Environ firstEnviron = environ.Find(findValue1);
            //#endregion
            //#region 读取沙尘参数
            //DustList dust = new DustList();
            //Predicate<Dust> findValue2 = delegate (Dust p)
            //{
            //    return p.DES.Equals(Des) && p.SDU.Equals(Sdu);
            //};
            //Dust firstDust = dust.Find(findValue2);
            //#endregion
            //#region 读取太阳辐射参数
            //SunList sun = new SunList();
            //Predicate<Sun> findValue3 = delegate (Sun p)
            //{
            //    return p.PLA.Equals(Pla) && p.MSEA.Equals(Msea);
            //};
            //Sun firstSun = sun.Find(findValue3);
            //#endregion
            //#region 读取沙尘参数
            //AirList air = new AirList();
            //Predicate<Air> findValue4 = delegate (Air p)
            //{
            //    return p.ATE.Equals(Ate);
            //};
            //Air firstAir = air.Find(findValue4);
            //#endregion

            #region 协系数数据
            double TEMC = -0.209;//温度协系数
            double HUMC = -0.236;//湿度协系数
            double DUC = -0.5;//沙尘协系数
            double SRC = -0.105;//太阳辐射协系数
            double ACC = -0.158;//大气腐蚀协系数
                                //来源：系统内置（暂未考虑用户输入）
            #endregion
            #region 备件基础故障率数据
            //double ShapePara = 4.3;//成附件基础故障率形状参数
            //double LifePara = 18908;//成附件基础故障率寿命参数//来源：用户输入
            #endregion
            #endregion
            #region 计算故障率
            double GammaX;
            GammaX = TEMP * TEMC + HUMP * HUMC + DUP * DUC + SRP * SRC + ACP * ACC;//计算环境参数与协系数的乘积和
            CFAR = Math.Exp(GammaX);//计算折算参数
            //double Baseline;//定义基础故障率函数
            //Baseline = Math.Pow((APMT/ PARAM2), (PARAM1-1)) * (PARAM1/PARAM2);//计算基础故障率
            //FAR = Baseline * CFAR;//求出故障率
            double Base_MFAR;//定义基础故障率均值
            MyMath cal = new MyMath();//调用伽马函数
            double MTTF = PARAM2 * cal.gamma(1 + 1 / PARAM1);//折算前MTTF
            Base_MFAR = 1 / (MTTF);//计算基础故障率均值（折算前故障率均值）
            MFAR = Base_MFAR * CFAR;//计算折算后故障率均值
            FAR_out[0] = CFAR;
            FAR_out[1] = MFAR;//计算结果存入表内进行输出//张浩田2020.08.06增加了CFAR,MFAR两个输出
            Console.WriteLine("折算参数={0},故障率均值={1},",CFAR, MFAR);
            //Console.WriteLine("{0},{1},{2},{3},{4}", firstEnviron.TEMP, firstEnviron.HUMP, firstDust.DUP, firstSun.SRP, firstAir.ACP);
            return FAR_out;
            #endregion
        }
    }
}

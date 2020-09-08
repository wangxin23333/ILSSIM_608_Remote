using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSSIM_608
{
    public partial class Calculation
    {
        /// <summary>
        /// 备件维修概率
        /// </summary>
        /// <para>输入：故障模式表(List),故障模式频数比表（List),维修方式表(List),故障模式影响强度表（List)</para>
        /// <para>输出：维修概率MP</para>
        /// <returns></returns>
        public double Maintenance_Probability(List<double> FMFR, List<int> INI, List<int> PLA,double PARAM1,double PARAM2)//计算备件维修概率
        ////张浩田2020.07.30
        {
            #region 临时数据
            List<string> AF = new List<string> { };//功能说明
            List<string> FDM = new List<string> { };//故障检测方式//计算中没用到
            #endregion
            #region 计算备件更换概率
            double MP = 0;//定义成附件维修概率
            MyMath cal = new MyMath();
            double MTTF = PARAM2 * cal.gamma(1 + 1 / PARAM1);//折算前MTTF
            double Base_MFAR = 1 / (MTTF);//计算基础故障率均值（折算前故障率均值）
            for (int i = 0; i < FMFR.Count; i++)
            {
                MP += Base_MFAR * FMFR[i] * INI[i] * PLA[i];//计算更换概率
            }

            return MP;
            #endregion

        }
    }
}

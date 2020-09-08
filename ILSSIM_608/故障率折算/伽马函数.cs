using System;
using System.Collections.Generic;
using System.Text;

namespace ILSSIM_608
{

    public class MyMath//伽马函数计算
    {
        private static readonly byte[] RawData =
   {
        53, 92, 152, 66, 185, 2, 173, 194, 224, 28, 192, 65, 166, 169, 157, 191, 153, 107, 158, 58, 193, 8, 181, 182,
        10, 23, 118, 159, 134, 11, 83, 64, 86, 54, 20, 43, 87, 160, 85, 192, 198, 76, 59, 241, 155, 3, 56, 64, 250, 176,
        158, 182, 52, 181, 243, 191, 216, 18, 236, 25, 115, 205, 83, 63, 136, 152, 14, 28, 24, 161, 214, 190
    };
        private static readonly float[] LgammaCoffF;
        private static readonly double[] LgammaCoffD;
        static unsafe MyMath()
        {
            LgammaCoffF = new float[6];
            LgammaCoffD = new double[6];
            fixed (byte* data = &RawData[0])
            {
                fixed (float* f = LgammaCoffF)
                {
                    var data2 = (byte*)f;
                    for (var i = 0; i < 24; i++) data2[i] = data[i];
                }
            }

            fixed (byte* data = &RawData[24])
            {
                fixed (double* d = LgammaCoffD)
                {
                    var data2 = (byte*)d;
                    for (var i = 0; i < 48; i++) data2[i] = data[i];
                }
            }
        }

        public float gamma(float x)
        {
            var val1 = 1.000000000190015F;
            var y = x + 1.0F;
            for (var i = 0; i < 6; ++i, y += 1.0F)
                val1 += LgammaCoffF[i] / y;
            return 2.5066282746310005F * val1 / x / (float)Math.Exp(x + 5.5F) * (float)Math.Pow(x + 5.5F, x + 0.5F);
        }

        public double gamma(double x)
        {
            var val1 = 1.000000000190015;
            var y = x + 1.0;
            for (var i = 0; i < 6; ++i, y += 1.0)
                val1 += LgammaCoffD[i] / y;
            return 2.5066282746310005 * val1 / x / Math.Exp(x + 5.5) * Math.Pow(x + 5.5, x + 0.5);
        }
        // 参考数据
        // gamma(0.1)=9.513507698668731836292487177265402192550578626088377343050
        // gamma(0.2)=4.590843711998803053204758275929152003434109998293403017788
        // gamma(0.3)=2.991568987687590628312516515904917791112806024921715112744
        // gamma(0.4)=2.218159543757688223059054021907679450770566501771469582241
        // gamma(0.5)=1.772453850905516027298167483341145182797549456122387128213
        // gamma(0.6)=1.489192248812817102394333388321342281320599038759924735338
        // gamma(0.7)=1.298055332647557785681171179152811617784141170553946247921
        // gamma(0.8)=1.164229713725303373636320938268458693141961768891187752984
        // gamma(0.9)=1.068628702119319354897305335694480778169838785060973179049
        // gamma(1.0)=1
        // gamma(0.227814641242585501358616865)=4
    }
}
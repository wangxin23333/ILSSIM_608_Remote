using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace 寿命消耗折算
{
    class Calculater
    {
        public static double[,] Transposition(double[,] A)      //矩阵转置
        {
            double[,] Trans = new double[A.GetLength(1), A.GetLength(0)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    Trans[j, i] = A[i, j];
                }

            }
            return Trans;
        }

        public static double[,] MatrixInversion(double[,] rr)//矩阵求逆
        {

            int n;
            n = rr.GetLength(0);
            if (n != rr.GetLength(1)) return null;//A
            double[,] C = new double[n, 2 * n];
            double[,] D = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    C[i, j] = rr[i, j];
            for (int i = 0; i < n; i++)
                C[i, i + n] = 1;
            for (int k = 0; k < n; k++)
            {
                double max = Math.Abs(C[k, k]);
                int ii = k;
                for (int m = k + 1; m < n; m++)
                    if (max < Math.Abs(C[m, k]))
                    {
                        max = Math.Abs(C[m, k]);
                        ii = m;
                    }
                for (int m = k; m < 2 * n; m++)
                {
                    if (ii == k) break;
                    double c;
                    c = C[k, m];
                    C[k, m] = C[ii, m];
                    C[ii, m] = c;
                }
                if (C[k, k] != 1)
                {
                    double bs = C[k, k];
                    if (bs == 0)
                    {
                        return null;
                        //throw new MyException("求逆错误！结果可能不正确！");


                    }

                    C[k, k] = 1;
                    for (int p = k + 1; p < n * 2; p++)
                    {
                        C[k, p] /= bs;
                    }
                }
                for (int q = k + 1; q < n; q++)
                {
                    double bs = C[q, k];
                    for (int p = k; p < n * 2; p++)
                    {
                        C[q, p] -= bs * C[k, p];
                    }
                }
            }
            for (int q = n - 1; q > 0; q--)
            {
                for (int k = q - 1; k > -1; k--)
                {
                    double bs = C[k, q];
                    for (int m = k + 1; m < 2 * n; m++)
                    {
                        C[k, m] -= bs * C[q, m];
                    }
                }

            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    D[i, j] = C[i, j + n];
                }
            }
            return D;

        }

        public static double[,] CCOFP(double[] dl)   //计算飞行剖面折合系数K(Conversion Coefficient Of Flight Profile)
        {
            double[,] K = new double[dl.Length, 1];
            for (int i = 0; i < dl.Length; i++)
            {

                K[i, 0] = dl[0] / dl[i];
                //Console.Write("{0}\t", K[i, 0]);
            }
            return K;
        }

        public static double TCC(double Af1)         //计算温度折合系数 Temperature conversion coefficient
        {
            double result = Math.Pow(Math.E, -Af1);
            return result;
        }
        public static double HCC(double Af2)         //计算湿度折合系数 Humidity conversion coefficient
        {
            double result = Math.Pow(Math.E, -Af2); ;
            return result;
        }
        public static double ACC(double Af3)         //计算海拔折合系数Altitude conversion coefficient
        {
            double result = 1 + (Af3 / 1000 * 0.1);
            return result;
        }

        public static double LifeConsumptionConversion(double[,] M, double[,] N, double[,] X, double e, double[] ei)//计算寿命消耗计算
        {
            double[,] result1 = new double[M.GetLength(0), M.GetLength(1)];
            double[,] result2 = new double[result1.GetLength(1), result1.GetLength(0)];

            for (int i = 0; i < M.GetLength(0); i++)            //对应项相乘
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    result1[i, j] = M[i, j] * N[i, j];
                }
            }
            for (int i = 0; i < result1.GetLength(0); i++)       //进行转置
            {
                for (int j = 0; j < result1.GetLength(1); j++)
                {
                    result2[j, i] = result1[i, j];
                }

            }

            for (int i = 0; i < ei.Length; i++)
            {
                e = e * ei[i];
            }
            double[,] Tmrl = Multiplication(result2.GetLength(0), result2.GetLength(1), X.GetLength(1), result2, X);
            double tmrl = Tmrl[0, 0];
            double Result = tmrl * e;
            return Result;



        }


        public static double[,] Multiplication(int x1, int x2, int x3, double[,] A, double[,] B)  //矩阵乘法
        {
            double[,] M = new double[x1, x2];         //定义一个矩阵
            double[,] N = new double[x2, x3];        //定义一个矩阵
            double[,] R = new double[x1, x3];       //定义一个矩阵，并保存两矩阵的乘积即剖面合成权重系数L
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    M[i, j] = A[i, j];
                }
            }
            for (int i = 0; i < x2; i++)
            {
                for (int j = 0; j < x3; j++)
                {
                    N[i, j] = B[i, j];
                }

            }
            for (int i = 0; i < x1; i++)                  //n是第一个矩阵的行数
            {
                for (int j = 0; j < x3; j++)       //w是第二个矩阵的列数
                {
                    double sum1 = 0;
                    for (int k = 0; k < x2; k++)    //n是第二个矩阵的行数
                    {
                        sum1 = sum1 + M[i, k] * N[k, j];
                    }
                    R[i, j] = sum1;

                }

            }
            return R;

        }


    }

}

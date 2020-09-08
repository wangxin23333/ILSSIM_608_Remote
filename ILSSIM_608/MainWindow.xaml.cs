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

namespace ILSSIM_608
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region 全局变量
        static string connString = "server=localhost;database=enginecbm_db;uid=ilssim001;pwd=ilssim709";

        #endregion


        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Lable_StationEquipmentMission_ModelingFinish.Visibility = Visibility.Collapsed;
            Lable_FRTCorrection_FailureModeGroup_Finish.Visibility = Visibility.Collapsed;
            Lable_ResourcePredict_Modeling_Finish.Visibility = Visibility.Collapsed;
            Lable_EngineLifetimeCorrection_Finish.Visibility = Visibility.Collapsed;
            Lable_Lifetime_CascadeUtilization_Finish.Visibility = Visibility.Collapsed;
            Lable_SupportScheme_Modeling_Finish.Visibility = Visibility.Collapsed;
            Lable_ResourceOptimizing_Finish.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 鼠标点击边框拖动 事件
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
            if (MessageBox.Show("是否确认关闭", "提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// 模块一 保障组织产品树及装备任务建模 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_StationEquipmentMission_Modeling_Click(object sender, RoutedEventArgs e)
        {
            StationEquipmentMissionModeling_Window stationEquipmentMissionModeling_Window = new StationEquipmentMissionModeling_Window();
            stationEquipmentMissionModeling_Window.connString = connString;
            stationEquipmentMissionModeling_Window.ShowDialog();
            
            //一切设置均完成后，将按钮的背景色调为绿色；并显示：已完成
            Color color = Color.FromRgb(48, 175, 173);
            Border_StationEquipmentMission_Modeling.Background =new SolidColorBrush(color) ;
            Lable_StationEquipmentMission_ModelingFinish.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 模块二 故障率折算及故障模式组合关系建模 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_FRTCorrection_FailureModeGroup_Click(object sender, RoutedEventArgs e)
        {
            FRTCorrection_FailureModeGroup fRTCorrection_FailureModeGroup = new FRTCorrection_FailureModeGroup();
            fRTCorrection_FailureModeGroup.connString = connString;
            fRTCorrection_FailureModeGroup.ShowDialog();

            //一切设置均完成后，将按钮的背景色调为绿色；并显示：已完成
            Color color = Color.FromRgb(48, 175, 173);
            Border_FRTCorrection_FailureModeGroup.Background = new SolidColorBrush(color);
            Lable_FRTCorrection_FailureModeGroup_Finish.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 模块三 保障资源数量预测 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ResourcePredict_Modeling_Click(object sender, RoutedEventArgs e)
        {
            Color color = Color.FromRgb(48, 175, 173);
            Border_ResourcePredict_Modeling.Background = new SolidColorBrush(color);
            Lable_ResourcePredict_Modeling_Finish.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 模块四 发动机寿命折算 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_EngineLifetimeCorrection_Click(object sender, RoutedEventArgs e)
        {
            LifetimeCorrection_Window lifetimeCorrection_Window = new LifetimeCorrection_Window();
            lifetimeCorrection_Window.connString = connString;
            lifetimeCorrection_Window.ShowDialog();
            Color color = Color.FromRgb(48, 175, 173);
            Border_EngineLifetimeCorrection.Background = new SolidColorBrush(color);
            Lable_EngineLifetimeCorrection_Finish.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 模块五 梯次使用 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Lifetime_CascadeUtilization_Click(object sender, RoutedEventArgs e)
        {
            Lifetime_CascadeUtilization lifetime_CascadeUtilization = new Lifetime_CascadeUtilization();
            lifetime_CascadeUtilization.connString = connString;
            lifetime_CascadeUtilization.ShowDialog();

            Color color = Color.FromRgb(48, 175, 173);
            Border_Lifetime_CascadeUtilization.Background = new SolidColorBrush(color);
            Lable_Lifetime_CascadeUtilization_Finish.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 模块六 保障方案建模 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SupportScheme_Modeling_Click(object sender, RoutedEventArgs e)
        {
            Support_OtherProfile_Distribution_Modeling support_OtherProfile_Distribution_Modeling = new Support_OtherProfile_Distribution_Modeling();
            support_OtherProfile_Distribution_Modeling.connString = connString;
            support_OtherProfile_Distribution_Modeling.ShowDialog();

            Color color = Color.FromRgb(48, 175, 173);
            Border_SupportScheme_Modeling.Background = new SolidColorBrush(color);
            Lable_SupportScheme_Modeling_Finish.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 启动仿真 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_StartSIM_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 模块七 备发备件动态储备优化 按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ResourceOptimizing_Click(object sender, RoutedEventArgs e)
        {
            ResourceOptimizing_Window resourceOptimizing_Window = new ResourceOptimizing_Window();
            resourceOptimizing_Window.connString = connString;
            resourceOptimizing_Window.ShowDialog();

            Color color = Color.FromRgb(48, 175, 173);
            Border_ResourceOptimizing.Background = new SolidColorBrush(color);
            Lable_ResourceOptimizing_Finish.Visibility = Visibility.Visible;
        }
        
    }
}

   M S B A M L   `   `   `  ����E  AILSSIM_608, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:ILSSIM_608
ILSSIM_608  5   	       ILSSIM_608.Login   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:ILSSIM_608     5   	   -   5        $	��450��5   	   $	��800��6     � WindowStyle$
  None=�6"    � WindowStartupLocation$ CenterScreen=�6f   .��5	      � ��5
   
   �� 5       �� ImageSource$# resource/loginbackground1.png��6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      .+�5   
   � $��0��6   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�63   .+�5      �� $��/resource/feiji_4.png��6   $��25��67   $��10 0 0 0q�6C   �� 5      $?��9涡轴发动机维护保障智能辅助管理决策软件��6   $��14(�67   
�������$��Center=�6]   $��6 0 0 0q�6x   5      �� 5      $��Center=�6   .+�5      �� $��用户登录��6   
�������$��Center=�6A   $��Center=�6\   5      �� 5      $ 
Horizontal=�6   $��Right=�62   .+�5      �� -   5        $��Btn_Min��5      $��20��6>   $��40��6J   
������ 
������ $��0 6 4 0q�6�   .��5      �� $��/resource/zuixiaohua.png��6   5      �� 5      -   5        $��Btn_Max��5      $��18��6>   $��40��6J   
������ 
������ $��2 2 4 2q�6�   .��5      �� $��/resource/zuidahua.png��6   5       �� 5!      -   5        $��	Btn_Close��5!      $��20��6B   $��40��6N   
������ 
������ $��2 2 10 2q�6�   .��5"      �� $��/resource/guanbi.png��6   5#      5$      5%      � 5'   
   $��1��6   .+�5(      �  5)      �� 5*      $
��0.8* �6$   �� 5+      $��45 �6$   �� 5,      $��45 �6$   �� 5-       � ColumnDefinitions 5/      �� 50      $
��0.8* �6'   �� 51      .+�54      �� $��0��6   $��用户名：��6%   $��36��64   $	��100��6@   $��16(�6L   ���#FFFFFF$��Bottom=�6o   $��Right=�6�   $��Center=�6�   $��Right=�6�   �� 55      $��1��6   $��	密码：��6%   $��36��63   $	��100��6?   $��16(�6K   ���#FFFFFF$��Bottom=�6n   $��Right=�6�   $��Center=�6�   $��Right=�6�   �� 56      $��2��6   $��军种类型：��6%   $��36��65   $	��100��6A   $��16(�6M   ���#FFFFFF$��Bottom=�6p   $��Right=�6�   $��Center=�6�   $��Right=�6�   �� 58      -   5        $��TB_Login_UserName��58   7   $��0��6   $��1��6'   $��30��6P   $	��180��6\   $��16(�6h   $��Bottom=�6v   $
��Left=�6�   $��Center=�6�   $
��Left=�6�    J� Opacity$
 0.84Q�6�   �� 59      -   5        $��TB_Login_Password��59   7   $��1��6   $��1��6'   $��30��6P   $	��180��6\   $��16(�6h   $��Bottom=�6v   $
��Left=�6�   $��Center=�6�   $
��Left=�6�   $
 0.84Q�6�   �� 5:      -   5        $%��ComboBox_Login_MilitaryServices��5:   8   $��2��6   $��1��6(   $��30��6_   $	��180��6k   $��16(�6w   $��Bottom=�6�   $
��Left=�6�   $	 0.9Q�6�   �� 5<      -   5        $��	Btn_Login��5<   6   $��3��6   $��0��6&   $��36��6G   $	��120��6S   $��Right=�6_   $��0 0 0 20q�6{   ���#FFFFFF$
 0.75Q�6�   .��5=      �� $ 
Horizontal=�6!   $��0 2 0 0q�6:   .+�5>      �� $��/resource/baocun1.png��6    $��20��6?   $��26��6K   �� 5?      $��	  登 录��6$   $��16(�61   $��24��6?   $��Center=�6K   $��Center=�6f   5@      5A      �� 5B      $��3��6   $��1��6&   $��36��66   $	��120��6B   $
��Left=�6N   $��
100 0 0 20q�6i   ���#FFFFFF$
 0.75Q�6�   .��5C      �� $ 
Horizontal=�6!   $��0 2 0 0q�6:   .+�5D      �� $��/resource/tuichu.png��6    $��20��6>   $��26��6J   �� 5E      $��	  退 出��6$   $��16(�61   $��24��6?   $��Center=�6K   $��Center=�6f   5F      5G      5H      5L      5N      5O      
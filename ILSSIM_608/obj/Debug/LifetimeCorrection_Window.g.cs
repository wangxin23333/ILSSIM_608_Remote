﻿#pragma checksum "..\..\LifetimeCorrection_Window.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "34ED072EF876D3D59EBC082E81A4E202FC38D6AE"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using ILSSIM_608;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ILSSIM_608 {
    
    
    /// <summary>
    /// LifetimeCorrection_Window
    /// </summary>
    public partial class LifetimeCorrection_Window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 95 "..\..\LifetimeCorrection_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Min;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\LifetimeCorrection_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Max;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\LifetimeCorrection_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Close;
        
        #line default
        #line hidden
        
        
        #line 168 "..\..\LifetimeCorrection_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid MyGrid;
        
        #line default
        #line hidden
        
        
        #line 205 "..\..\LifetimeCorrection_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid MyGrid1;
        
        #line default
        #line hidden
        
        
        #line 224 "..\..\LifetimeCorrection_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border Border_NowProject;
        
        #line default
        #line hidden
        
        
        #line 244 "..\..\LifetimeCorrection_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EngineBox;
        
        #line default
        #line hidden
        
        
        #line 246 "..\..\LifetimeCorrection_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MtidBox;
        
        #line default
        #line hidden
        
        
        #line 248 "..\..\LifetimeCorrection_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ProfileBox;
        
        #line default
        #line hidden
        
        
        #line 251 "..\..\LifetimeCorrection_Window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MyText;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ILSSIM_608;component/lifetimecorrection_window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\LifetimeCorrection_Window.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\LifetimeCorrection_Window.xaml"
            ((ILSSIM_608.LifetimeCorrection_Window)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Window_MouseMove);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Btn_Min = ((System.Windows.Controls.Button)(target));
            
            #line 95 "..\..\LifetimeCorrection_Window.xaml"
            this.Btn_Min.Click += new System.Windows.RoutedEventHandler(this.Btn_Min_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Btn_Max = ((System.Windows.Controls.Button)(target));
            
            #line 98 "..\..\LifetimeCorrection_Window.xaml"
            this.Btn_Max.Click += new System.Windows.RoutedEventHandler(this.Btn_Max_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Btn_Close = ((System.Windows.Controls.Button)(target));
            
            #line 101 "..\..\LifetimeCorrection_Window.xaml"
            this.Btn_Close.Click += new System.Windows.RoutedEventHandler(this.Btn_Close_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.MyGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 168 "..\..\LifetimeCorrection_Window.xaml"
            this.MyGrid.Loaded += new System.Windows.RoutedEventHandler(this.MyGrid_Loaded);
            
            #line default
            #line hidden
            return;
            case 6:
            this.MyGrid1 = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.Border_NowProject = ((System.Windows.Controls.Border)(target));
            return;
            case 8:
            this.EngineBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.MtidBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.ProfileBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.MyText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            
            #line 257 "..\..\LifetimeCorrection_Window.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


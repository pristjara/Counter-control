﻿#pragma checksum "..\..\..\Views\ReadoutsManagement.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "662C9C98D7CCB208C187585D31AA3712B42ACFCE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Counter_Control.Views;
using MahApps.Metro.Controls;
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


namespace Counter_Control.Views {
    
    
    /// <summary>
    /// ReadoutsManagement
    /// </summary>
    public partial class ReadoutsManagement : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 22 "..\..\..\Views\ReadoutsManagement.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAdd;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Views\ReadoutsManagement.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMeterFilter;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\Views\ReadoutsManagement.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid tbl_Meters;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\Views\ReadoutsManagement.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox gbReadouts;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\Views\ReadoutsManagement.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblReadoutsHeader;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\Views\ReadoutsManagement.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtReadoutFilter;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\Views\ReadoutsManagement.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid tbl_Readouts;
        
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
            System.Uri resourceLocater = new System.Uri("/Counter Control;component/views/readoutsmanagement.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ReadoutsManagement.xaml"
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
            this.btnAdd = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\Views\ReadoutsManagement.xaml"
            this.btnAdd.Click += new System.Windows.RoutedEventHandler(this.btnAdd_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtMeterFilter = ((System.Windows.Controls.TextBox)(target));
            
            #line 42 "..\..\..\Views\ReadoutsManagement.xaml"
            this.txtMeterFilter.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtMeterFilter_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tbl_Meters = ((System.Windows.Controls.DataGrid)(target));
            
            #line 50 "..\..\..\Views\ReadoutsManagement.xaml"
            this.tbl_Meters.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.ViewReadouts);
            
            #line default
            #line hidden
            return;
            case 6:
            this.gbReadouts = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 7:
            this.lblReadoutsHeader = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.txtReadoutFilter = ((System.Windows.Controls.TextBox)(target));
            
            #line 80 "..\..\..\Views\ReadoutsManagement.xaml"
            this.txtReadoutFilter.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtReadoutFilter_TextChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.tbl_Readouts = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 4:
            
            #line 60 "..\..\..\Views\ReadoutsManagement.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ViewReadouts);
            
            #line default
            #line hidden
            break;
            case 5:
            
            #line 67 "..\..\..\Views\ReadoutsManagement.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddReadout);
            
            #line default
            #line hidden
            break;
            case 10:
            
            #line 99 "..\..\..\Views\ReadoutsManagement.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.EditReadout);
            
            #line default
            #line hidden
            break;
            case 11:
            
            #line 107 "..\..\..\Views\ReadoutsManagement.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteReadout);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}


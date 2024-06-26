﻿using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace ExampleApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    
    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HT_CAPTION = 0x2;
    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    public MainWindow()
    {
        InitializeComponent();
        
        DataContext = this;
    }

    private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if(e.LeftButton != MouseButtonState.Pressed)
            return;
        
        ReleaseCapture();
        SendMessage(GetForegroundWindow(), WM_NCLBUTTONDOWN, HT_CAPTION, 0);
    }

    private void MinimizeBtn_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void SafeThreadInvoker(Action a)
    {
        if (!CheckAccess())
            Dispatcher.Invoke(a);
        else
            a();
    }
}
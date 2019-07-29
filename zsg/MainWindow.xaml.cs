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
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Windows.Threading;

namespace zsg
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Page
    {
        
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                
                //if(bool.Parse(ConfigurationManager.AppSettings["nv1_c"]))
                {
                    add_Button();
                    BtnPop5.Content = ConfigurationManager.AppSettings["bt_nv1"];
                    //BtnPop5.Visibility = Visibility.Visible;
                }
                //else
                {
                    //BtnPop5.Visibility = Visibility.Hidden;
                }
                //if (bool.Parse(ConfigurationManager.AppSettings["nv2_c"]))
                {
                    BT_video.Content = ConfigurationManager.AppSettings["bt_nv2"];
                }
                //else
                {
                    //BT_video.Visibility = Visibility.Hidden;
                }
                bgpic.Source = new BitmapImage(new Uri(ConfigurationManager.AppSettings["app_bg"]));
            }
            catch
            {
                
            }
            //dp1.Visibility = Visibility.Hidden;
        }
        public static DataTable dt1 = new DataTable();
        public void bindlist1()
        {
            dt1 = dbop1("select * from lm ").Tables[0];
            //this.listView.DataContext = dt1;
            //this.listView.SetBinding(ListView.ItemsSourceProperty, new Binding());
        }
        public OleDbConnection getConn()
        {
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0 ;Data Source=database1.mdb";
            OleDbConnection tempconn = new OleDbConnection(connstr);
            return (tempconn);
        }
        public DataSet dbop1(string sqlstr)
        {
            DataSet ds = new DataSet();
            string oleconnectionString = "Provider=Microsoft.Jet.OleDb.4.0;";
            oleconnectionString += @"Data Source =Database1.mdb";
            OleDbConnection connection = new OleDbConnection(oleconnectionString);
            //string SQLString = "select * from 入党";
            OleDbCommand cmd = new OleDbCommand(sqlstr, connection);
            cmd.CommandTimeout = 2000;
            OleDbDataAdapter oda = new OleDbDataAdapter(cmd);

            try
            {
                connection.Open();
                //int rows = cmd.ExecuteNonQuery();
                oda.Fill(ds);
                oda = null;
                return ds;
            }
            catch (System.Data.OleDb.OleDbException E)
            {
                connection.Close();
                throw new Exception(E.Message);
            }

        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (v1 == 0)
            {
                v1 = 1;
                dp1.Visibility = Visibility.Visible;
                pop1 = 1;
            }
            else
            {
                v1 = 0;
                dp1.Visibility = Visibility.Hidden;
            }
        }
        private void BT_EXT_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void BT_SHOW_PIC_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Page2.xaml", UriKind.Relative));
        }

        private void BT_SET_Click(object sender, RoutedEventArgs e)
        {
            Settings st1 = new Settings();
            st1.Closed += new EventHandler(st1_Closed);//注册关闭事件
            st1.ShowDialog();
        }
        public void st1_Closed(object sender, EventArgs e)
        {
            try
            {
                //DoEvents();
                //if (!bool.Parse(ConfigurationManager.AppSettings["nv1_c"]))
                //{
                //    BtnPop5.Visibility = Visibility.Hidden;
                //}
                //else
                {
                    add_Button();
                    BtnPop5.Content = ConfigurationManager.AppSettings["bt_nv1"];
                    //BtnPop5.Visibility = Visibility.Visible;
                }
                //if (!bool.Parse(ConfigurationManager.AppSettings["nv2_c"]))
                {
                    //BT_video.Visibility = Visibility.Hidden;
                }
                //else
                {
                    BT_video.Content = ConfigurationManager.AppSettings["bt_nv2"];
                    //BT_video.Visibility = Visibility.Visible;
                }
                pop1 = 1;
            }
            catch { }
        }
       

        private void BT_video_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Page1.xaml", UriKind.Relative));
        }

        private void add_Button()
        {
            bindlist1();
            if (dt1 != null)
            {
                int n = dt1.Rows.Count;
                if (n > 0)
                {
                    pop_c.Children.Clear();
                    for (int i = 0; i < n; i++)
                    {

                        Button btn = new Button
                        {
                            Name = "Button_" + i.ToString(),
                            Content = dt1.Rows[i][1].ToString(),
                            Height = 50,
                            //Width = 64,
                            //HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(5, 5, 5, 5),
                            //VerticalAlignment = VerticalAlignment.Top,
                            //Visibility = Visibility.Visible
                        };

                        btn.Click += new RoutedEventHandler(btn_click);
                       pop_c.Children.Add(btn);
                    }
                }
            }
            

        }
        private void btn_click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int i = int.Parse(btn.Name.Split('_')[1]);
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["pic_path"].Value = dt1.Rows[i][3].ToString();
            cfa.Save();
            ConfigurationManager.RefreshSection("appSettings");
            NavigationService.Navigate(new Uri("Page2.xaml", UriKind.Relative));
        }

        int v1 = 1;
        TouchPoint tp1, tp2;
        private void Grd1_TouchMove(object sender, TouchEventArgs e)
        {
            tp2 = e.GetTouchPoint(grd1);
        }

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {
            tp1 = e.GetTouchPoint(grd1);
        }

        int pop1 = 1;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Page2.xaml", UriKind.Relative));
           // if (pop1==0)
           //{
           //     Pop5.IsOpen = false;
           //     pop1 = 1;
           // }
           // else
           // {
           //     Pop5.IsOpen = true;
           //     pop1 = 0;
           // }
            
        }

        private void BT_tip_Click(object sender, RoutedEventArgs e)
        {
            Window2 w2 = new Window2();
            w2.ShowDialog();
        }

        private void Grd1_TouchUp(object sender, TouchEventArgs e)
        {
            
            if ((System.Math.Abs(tp2.Position.X - tp1.Position.X) <= 100) && (tp2.Position.Y - tp1.Position.Y) >= 100)
            {
                dp1.Visibility = Visibility.Hidden;
            }
            if ((System.Math.Abs(tp2.Position.X - tp1.Position.X) <= 100) && (tp1.Position.Y - tp2.Position.Y) >= 100)
            {
                //if (tm1.IsEnabled) { tm1.Start(); }

                this.Dispatcher.Invoke(new Action(() =>
                {
                    dp1.Visibility = Visibility.Visible;
                    //if (!bool.Parse(ConfigurationManager.AppSettings["nv1_c"]))
                    //{
                    //    BtnPop5.Visibility = Visibility.Hidden;
                    //}
                    //else
                    //{
                    //    BtnPop5.Visibility = Visibility.Visible;
                    //}
                    //if (!bool.Parse(ConfigurationManager.AppSettings["nv2_c"]))
                    //{
                    //    BT_video.Visibility = Visibility.Hidden;
                    //}
                    //else
                    //{
                     //   BT_video.Visibility = Visibility.Visible;
                    //}
                    pop1 = 1;
                }));
            }
        }

    }
}


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
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
using System.Windows.Shapes;

namespace zsg
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Window
    {

        public Settings()
        {
            InitializeComponent();
            bindlist1();
            video_path.Text = ConfigurationManager.AppSettings["video_path"].ToString();
            app_bg.Text = ConfigurationManager.AppSettings["app_bg"].ToString();
            video_bg.Text = ConfigurationManager.AppSettings["video_bg"].ToString();
            bt_nv1.Text= ConfigurationManager.AppSettings["bt_nv1"].ToString();
            bt_nv2.Text = ConfigurationManager.AppSettings["bt_nv2"].ToString();
            l_bgmusic.Text= ConfigurationManager.AppSettings["pic_bgsound"].ToString();
            l_face.Text = ConfigurationManager.AppSettings["pic_bg"].ToString();
            l_path.Text = ConfigurationManager.AppSettings["pic_path"].ToString();
            //nv1_c.IsChecked = bool.Parse(ConfigurationManager.AppSettings["nv1_c"]);
            //nv2_c.IsChecked = bool.Parse(ConfigurationManager.AppSettings["nv2_c"]);
        }
        public static DataTable dt1 = new DataTable();
        public void bindlist1()
        {
            dt1 = dbop1("select * from lm ").Tables[0];
            this.listView.DataContext = dt1;
            this.listView.SetBinding(ListView.ItemsSourceProperty, new Binding());
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
        public Boolean dbop(string sql1)
        {
            Boolean tempvalue = false;
            //string sqlstr = ""; //当时在这里定义，是为了在出现异常的时候看看我的SQL语句是否正确
            try
            {
                //用到了我前面写的那个得到数据库连接的函数
                OleDbConnection conn = getConn(); //getConn():得到连接对象，
                conn.Open();

                //确定我们需要执行的SQL语句，本处是UPDATE语句！
                //sqlstr = "UPDATE notes SET ";
                //sqlstr += "title='" + note.title + "',";
                //sqlstr += "content='" + DealString(note.content) + "',";
                //sqlstr += "author='" + note.author + "',";
                //sqlstr += "email='" + note.email + "',";
                //sqlstr += "http='" + note.http + "'";
                //sqlstr += "pic='" +note.pic +"'";
                //sqlstr += " where id=" + note.id;

                //定义command对象，并执行相应的SQL语句
                OleDbCommand myCommand = new OleDbCommand(sql1, conn);
                myCommand.ExecuteNonQuery(); //执行SELECT的时候我们是用的ExecuteReader()
                conn.Close();


                //假如执行成功，则，返回TRUE，否则，返回FALSE
                tempvalue = true;
                return (tempvalue);
            }
            catch (Exception e)
            {
                throw (new Exception("数据库更新出错:" + sql1 + "\r" + e.Message));
            }
        }
        private void btn_click(object sender, RoutedEventArgs e)

        {
            Button btn = sender as Button;
            if ("Button1" == btn.Name)
            {
                System.Windows.MessageBox.Show("hello");
            }
            else if ("Button2" == btn.Name)
            {
                System.Windows.MessageBox.Show("World.");
            }
        }

        private void Txb1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool shiftKey = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;//判断shifu键是否按下
            if (shiftKey == true)                  //当按下shift
            {
                e.Handled = true;//不可输入
            }
            else                           //未按shift
            {
                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Delete || e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Enter))
                {
                    e.Handled = true;//不可输入
                }
            }
        }

        private void Bt_mod_Click(object sender, RoutedEventArgs e)
        {
             int si = listView.SelectedIndex;
            if (si >= 0)
            {
                dbop("update lm set 栏目='" + l_name.Text + "',封面='" + l_face.Text + "',文件夹='" + l_path.Text + "' where id=" + dt1.Rows[listView.SelectedIndex][0].ToString());
                bindlist1();
                listView.SelectedIndex = si;
                //MessageBox.Show("修改成功");
            }
        }

        private void Bt_add_Click(object sender, RoutedEventArgs e)
        {
            dbop("insert into lm(栏目,封面,文件夹) values('" + l_name.Text + "','" + l_face.Text + "','" + l_path.Text + "')");
            bindlist1();
            //MessageBox.Show("添加成功");
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listView.SelectedIndex >= 0)
            {
                l_name.Text = dt1.Rows[listView.SelectedIndex][1].ToString();
                l_face.Text = dt1.Rows[listView.SelectedIndex][2].ToString();
               // image.Source = bmp(@"Reource/Image/" + dt1.Rows[listView.SelectedIndex][4].ToString());
                l_path.Text = dt1.Rows[listView.SelectedIndex][3].ToString();
                //textBox3_Copy12.Text = dt1.Rows[listView.SelectedIndex][3].ToString();
            }
            //MessageBox.Show(listView.SelectedIndex.ToString());
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.Filter = "Picture File(*.jpg;*.gif;*.png;*.bmp)|*.jpg;*.gif;*.png;*.bmp|All File(*.*)|*.*";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                l_face.Text = dialog.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = m_Dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string m_Dir = m_Dialog.SelectedPath.Trim();
            this.l_path.Text = m_Dir;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = m_Dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string m_Dir = m_Dialog.SelectedPath.Trim();
            this.video_path.Text = m_Dir;
        }
        private void Bt_del_Click(object sender, RoutedEventArgs e)
        {
            int si = listView.SelectedIndex;
            if (si >= 0)
            {
                dbop("delete from lm  where id=" + dt1.Rows[listView.SelectedIndex][0].ToString());
                bindlist1();
                if (si < listView.Items.Count - 1)
                {
                    listView.SelectedIndex = si;
                }
                else
                {
                    listView.SelectedIndex = listView.Items.Count - 1;
                }
                //MessageBox.Show("删除成功");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["video_path"].Value = video_path.Text;
            cfa.AppSettings.Settings["app_bg"].Value = app_bg.Text;
            cfa.AppSettings.Settings["video_bg"].Value = video_bg.Text;
            cfa.AppSettings.Settings["pic_path"].Value = l_path.Text;
            cfa.AppSettings.Settings["pic_bg"].Value = l_face.Text;
            cfa.AppSettings.Settings["pic_bgsound"].Value = l_bgmusic.Text;
            cfa.AppSettings.Settings["bt_nv1"].Value = bt_nv1.Text;
            cfa.AppSettings.Settings["bt_nv2"].Value = bt_nv2.Text;
            //cfa.AppSettings.Settings["nv1_c"].Value = nv1_c.IsChecked.ToString();
            //cfa.AppSettings.Settings["nv2_c"].Value = nv2_c.IsChecked.ToString();
            cfa.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.Filter = "Picture File(*.jpg;*.gif;*.png;*.bmp)|*.jpg;*.gif;*.png;*.bmp|All File(*.*)|*.*";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                app_bg.Text = dialog.FileName;
            }
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.Filter = "Video File(*.jpg;*.gif;*.png;*.bmp)|*.jpg;*.gif;*.png;*.bmp|All File(*.*)|*.*";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                video_bg.Text = dialog.FileName;
            }
        }


        private void Button_bgmusic_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.Filter = "Sound File(*.mp3;*.wma;*.wav)|*.mp3;*.wma;*.wav|All File(*.*)|*.*";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                l_bgmusic.Text = dialog.FileName;
            }
        }
    }
}

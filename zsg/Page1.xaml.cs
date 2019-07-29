using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Threading;
using Microsoft.Win32;

namespace zsg
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {
        DataTable data = new DataTable();
        public Page1()
        {
            InitializeComponent();
            try
            {
                string[] vd_list = SearchFolder(System.Configuration.ConfigurationManager.AppSettings["video_path"]);
                if (vd_list.Length > 0)
                {
                    //list_video.ItemsSource = vd_list;
                    DataColumn ID = new DataColumn("ID");//第一列

                    ID.DataType = System.Type.GetType("System.Int32");

                    //ID.AutoIncrement = true; //自动递增ID号 
                    data.Columns.Add(ID);
                    //设置主键
                    DataColumn[] keys = new DataColumn[1];
                    keys[0] = ID;
                    data.PrimaryKey = keys;
                    data.Columns.Add(new DataColumn("Name", typeof(string)));//第二列
                    data.Columns.Add(new DataColumn("path", typeof(string)));//第三列
                    int i = 1;
                    foreach (string vd in vd_list)
                    {
                        System.IO.DirectoryInfo drf = new System.IO.DirectoryInfo(vd);
                        data.Rows.Add(i, drf.Name, drf.FullName);
                        i++;
                        //list_video.Items.Add(vd.ToString());
                        //list_video.Items[list_video.Items.Count - 1]
                    }
                    list_video.DataContext = data;
                    bgpic.Source = new BitmapImage(new Uri(System.Configuration.ConfigurationManager.AppSettings["video_bg"]));
                }
            }
            catch { }
        }
        
        private string _videoPath;
               /// <summary>        
        /// 搜出路径下 文件路径      
        /// </summary>       
        /// <param name="dir"></param>       
        /// <returns></returns>     
        public static string[] SearchFolder(string dir)
        {
            //string str1 = System.Environment.CurrentDirectory;
            //Directory.SetCurrentDirectory(System.Environment.CurrentDirectory);
            string[] images = null;
            if (System.IO.Directory.Exists(dir))   //如果存在这个文件夹       
            {
                int image_sum = 0;
                foreach (string files in System.IO.Directory.GetFiles(dir))
                {
                    if (files.Contains(".avi") || files.Contains(".mp4") || files.Contains(".mkv") || files.Contains(".rmvb"))
                    {
                        image_sum++;
                    }
                }
                images = new string[image_sum];
                image_sum = 0;
                foreach (string files in System.IO.Directory.GetFiles(dir))
                {
                    if (files.Contains(".avi") || files.Contains(".mp4") || files.Contains(".mkv") || files.Contains(".rmvb"))
                    {
                        images[image_sum] = files;
                        image_sum++;
                    }
                }
            }
            return images;
        }
        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Video File(*.avi;*.mp4;*.mkv;*.wav;*.rmvb)|*.avi;*.mp4;*.mkv;*.wav;*.rmvb|All File(*.*)|*.*";

            if (dialog.ShowDialog().GetValueOrDefault())
            {
                _videoPath = dialog.FileName;
            }
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            //MediaPlayer.Source = new Uri(list_video.SelectedItem.ToString());
            MediaPlayer.Play();
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Stop(); 

        }
        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Pause();
        }
        private void ButtonForward_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Position = MediaPlayer.Position + TimeSpan.FromSeconds(20);
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Position = MediaPlayer.Position - TimeSpan.FromSeconds(20);
        }

        DispatcherTimer timer = null;
        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Get the lenght of the video
            int duration = MediaPlayer.NaturalDuration.TimeSpan.Seconds;
            sliderPosition.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            //媒体文件打开成功
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }
        private bool userIsDraggingSlider = false;
        private void timer_tick(object sender, EventArgs e)
        {
            if ((MediaPlayer.Source != null) && (MediaPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliderPosition.Value = MediaPlayer.Position.TotalSeconds;
            }
        }
        //控制视频的位置
        private void sliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                //mediaElement.Stop();
                MediaPlayer.Position = TimeSpan.FromSeconds(sliderPosition.Value);
                l_time.Content = TimeSpan.FromSeconds(sliderPosition.Value).ToString(@"hh\:mm\:ss") + "/" + MediaPlayer.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
                //MediaPlayer.Position.Minutes.ToString() + ":" + MediaPlayer.Position.Seconds.ToString() + "/" + MediaPlayer.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + MediaPlayer.NaturalDuration.TimeSpan.Seconds.ToString();
                //lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
                //mediaElement.Play();
            }
            catch (Exception ei) { MessageBox.Show(ei.ToString()); }
        }
        int vd_c = 1;
        private void fullscreen(object sender, MouseButtonEventArgs e)
        {
            if(vd_c==1)
            {
                MediaPlayer.Margin = new Thickness(0, 0, 0, 0);
                MediaPlayer.Width = 1920;
                MediaPlayer.Height = 1080;
                MediaPlayer.Stretch = Stretch.Fill;
                list_video.Visibility = Visibility.Hidden;
                sp1.Visibility = Visibility.Hidden;
                l_time.Visibility = Visibility.Hidden;
                pbVolume.Visibility = Visibility.Hidden;
                vd_c = 0;
            }
            else
            {
                MediaPlayer.Margin = new Thickness(220, 202, 500, 202);
                MediaPlayer.Width = 1200;
                MediaPlayer.Height = 675;
                list_video.Visibility = Visibility.Visible;
                sp1.Visibility = Visibility.Visible;
                l_time.Visibility = Visibility.Visible;
                pbVolume.Visibility = Visibility.Visible;
                vd_c = 1;
            }
            

           
        }

        private void Buttonhome_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Stop();
            if(timer!=null)
            {
                timer.Stop();
                timer = null;
            }
            NavigationService.GoBack();
        }

        private void List_video_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer = null;
                }
                MediaPlayer.Stop();
                //MediaPlayer.Source = null;
                MediaPlayer.Source = new Uri(data.Rows[list_video.SelectedIndex]["path"].ToString());
                MediaPlayer.Play();
            }
            catch (Exception ei) { MessageBox.Show(ei.ToString()); }
        }

        private void Buttonfull_Click(object sender, RoutedEventArgs e)
        {
            if (vd_c == 1&& MediaPlayer.Source !=null)
            {
                MediaPlayer.Margin = new Thickness(0, 0, 0, 0);
                MediaPlayer.Width = 1920;
                MediaPlayer.Height = 1080;
                MediaPlayer.Stretch = Stretch.Fill;
                list_video.Visibility = Visibility.Hidden;
                sp1.Visibility = Visibility.Hidden;
                //l_time.Visibility = Visibility.Hidden;
                //pbVolume.Visibility = Visibility.Hidden;
                vd_c = 0;
            }
            else
            {
                MediaPlayer.Margin = new Thickness(220, 202, 500, 202);
                MediaPlayer.Width = 1200;
                MediaPlayer.Height = 675;
                list_video.Visibility = Visibility.Visible;
                sp1.Visibility = Visibility.Visible;
                //l_time.Visibility = Visibility.Visible;
                //pbVolume.Visibility = Visibility.Visible;
                vd_c = 1;
            }
        }

        private void Slidervol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaPlayer.Volume = pbVolume.Value;

        }

        private void sliProgress_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            MediaPlayer.Position = TimeSpan.FromSeconds(sliderPosition.Value);
        }

    }
}

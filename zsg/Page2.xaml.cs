using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace zsg
{
    /// <summary>
    /// Page2.xaml 的交互逻辑
    /// </summary>
    public partial class Page2 : Page
    {
        BitmapImage[] BitmapImage_Source;
        int pic_now = 0, pic_num = 0;
        public Page2()
        {
            InitializeComponent();
            try
            {
                dp1.Visibility = Visibility.Hidden;
                //bt_next.Visibility = Visibility.Hidden;
                //bt_back.Visibility = Visibility.Hidden;
                //bt_mute.Visibility = Visibility.Hidden;
                string[] s = SearchFolder(System.Configuration.ConfigurationManager.AppSettings["pic_path"]);
                if (s != null)
                {
                    pic_num = s.Length;
                    BitmapImage_Source = new BitmapImage[s.Length];
                    int i = 0;
                    foreach (string item in s)
                    {
                        BitmapImage_Source[i] = new BitmapImage(new Uri(@"" + item, UriKind.Absolute));

                        i++;
                    }
                    img1.Source = BitmapImage_Source[0];
                }
                //SoundPlayer sp = new SoundPlayer();
                //sp.SoundLocation = @"D:\\pic\\中国空军进行曲.mp3";
                //sp.PlayLooping();
                //MediaPlayer.Source = new Uri(@"D:\\资料\\中国空军进行曲.mp3");
                //MediaPlayer.Play();
                playSound();
            }
            catch { }
        }
        public class BgMusic
        {
            public string BgMusicPath { get; set; }
        }
        private void playSound()
        {
            BgMusic bm = new BgMusic();
            bm.BgMusicPath = SearchFoldermusic(System.Configuration.ConfigurationManager.AppSettings["pic_path"])[0];
            mySoundPlayer.DataContext = bm;
        }
        private void Bt_next_Click(object sender, RoutedEventArgs e)
        {
            if (pic_num > 0)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {

                    
                    pic_now++;
                    if (pic_now > BitmapImage_Source.Length - 1) { pic_now = 0; }
                    //img1.BeginInit();
                    img1.Source = BitmapImage_Source[pic_now];
                    img1.RenderTransform = new MatrixTransform(1, 0, 0, 1, 0, 0);
                    img1.Height = 1080;
                    img1.HorizontalAlignment = HorizontalAlignment.Center;
                    img1.Stretch = Stretch.Uniform;


                }));
            }

        }

        private void Bt_pre_Click(object sender, RoutedEventArgs e)
        {
            if (pic_num > 0)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {

                    
                    pic_now--;
                    if (pic_now < 0) { pic_now = BitmapImage_Source.Length - 1; }
                    //img1.BeginInit();
                    img1.Source = BitmapImage_Source[pic_now];
                    img1.RenderTransform = new MatrixTransform(1, 0, 0, 1, 0, 0);
                    img1.Height = 1080;
                    img1.HorizontalAlignment = HorizontalAlignment.Center;
                    img1.Stretch = Stretch.Uniform;
                    


                }));
            }
        }


        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dp1.Visibility = Visibility.Visible;
            //bt_pre.Visibility = Visibility.Visible;
            //bt_back.Visibility = Visibility.Visible;
            //bt_mute.Visibility = Visibility.Visible;
        }

        private void Img1_ManipulationDelta(object sender, ManipulationDeltaEventArgs args)
        {

            UIElement element = args.Source as UIElement;
            if (element != null)
            {
                try
                {
                    MatrixTransform xform = element.RenderTransform as MatrixTransform;
                    Matrix matrix = xform.Matrix;
                    ManipulationDelta delta = args.DeltaManipulation;
                    Point center = args.ManipulationOrigin;
                    matrix.ScaleAt(delta.Scale.X, delta.Scale.Y, center.X, center.Y);
                    //matrix.RotateAt(delta.Rotation, center.X, center.Y);
                    if (img1.RenderTransform.Value.M11 > 1)
                    {
                        matrix.Translate(delta.Translation.X, delta.Translation.Y);
                    }
                    xform.Matrix = matrix;
                    args.Handled = true;
                  
                    //base.OnManipulationDelta(args);
                }
                catch (Exception ei)
                {
                    MessageBox.Show(ei.ToString());
                }

            }
        }

 

   
        TouchPoint tp1, tp2;

        private void Grd1_TouchMove(object sender, TouchEventArgs e)
        {
            tp2 = e.GetTouchPoint(grd1);
        }

        private void Grd1_TouchUp(object sender, TouchEventArgs e)
        {
            if (img1.RenderTransform.Value.M11 > 1&&movingEllipses.Count<2)
            {
                i += 1;
                timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
                timer.IsEnabled = true;
                if (i % 2 == 0)
                {
                    timer.IsEnabled = false;
                    i = 0;
                    img1.RenderTransform = new MatrixTransform(1, 0, 0, 1, 0, 0);
                }
            }
            movingEllipses.Remove(e.TouchDevice.Id);


            if (img1.RenderTransform.Value.M11 <= 1)
            {
                if ((tp1.Position.X - tp2.Position.X >= 150) && (System.Math.Abs(tp2.Position.Y - tp1.Position.Y) <= 100))
                {

                    this.Dispatcher.Invoke(new Action(() =>
                    {

                        pic_now++;
                        if (pic_now > BitmapImage_Source.Length - 1) { pic_now = 0; }
                        img1.Source = BitmapImage_Source[pic_now];
                        img1.RenderTransform = new MatrixTransform(1, 0, 0, 1, 0, 0);
                        img1.Height = 1080;
                        img1.HorizontalAlignment = HorizontalAlignment.Center;
                        img1.Stretch = Stretch.Uniform;

                    }));
                }
                if ((tp2.Position.X - tp1.Position.X >= 150) && (System.Math.Abs(tp2.Position.Y - tp1.Position.Y) <= 100))
                {

                    this.Dispatcher.Invoke(new Action(() =>
                    {

                        pic_now--;
                        if (pic_now < 0) { pic_now = BitmapImage_Source.Length - 1; }
                        img1.Source = BitmapImage_Source[pic_now];
                        img1.RenderTransform = new MatrixTransform(1, 0, 0, 1, 0, 0);
                        img1.Height = 1080;
                        img1.HorizontalAlignment = HorizontalAlignment.Center;
                        img1.Stretch = Stretch.Uniform;

                    }));
                }
                if ((System.Math.Abs(tp2.Position.X - tp1.Position.X) <= 100) && (tp2.Position.Y - tp1.Position.Y) >= 150)
                {


                    dp1.Visibility = Visibility.Hidden;
                    //bt_pre.Visibility = Visibility.Hidden;
                    //bt_back.Visibility = Visibility.Hidden;
                    //bt_mute.Visibility = Visibility.Hidden;
                    //button_Copy.Visibility = Visibility.Hidden;

                }
                if ((System.Math.Abs(tp2.Position.X - tp1.Position.X) <= 100) && (tp1.Position.Y - tp2.Position.Y) >= 150)
                {
                    //if (tm1.IsEnabled) { tm1.Start(); }

                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        dp1.Visibility = Visibility.Visible;
                    //bt_pre.Visibility = Visibility.Visible;
                    //bt_back.Visibility = Visibility.Visible;
                    //bt_mute.Visibility = Visibility.Visible;
                    //button_Copy.Visibility = Visibility.Visible;

                }));
                }
            }
        }

        private void Bt_back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        int ff = 1;
        private void Bt_mute_Click(object sender, RoutedEventArgs e)
        {
            
            if (ff == 1)
            {
                mySoundPlayer.IsMuted = true;
                bt_mute.Content = "SOUND";
                ff = 0;
            }
            else
            {
                mySoundPlayer.IsMuted = false;
                bt_mute.Content = "MUTE";
                ff = 1;
            }
        }

        private void Bt_resize_Click(object sender, RoutedEventArgs e)
        {
            img1.RenderTransform = new MatrixTransform(1, 0, 0, 1, 0, 0);
            //img1.Height =1080;
            //img1.HorizontalAlignment = HorizontalAlignment.Center;
            //img1.Stretch = Stretch.Uniform;
            //MessageBox.Show(img1.ActualHeight + "|" + img1.ActualWidth);

        }


        private Dictionary<int, Ellipse> movingEllipses = new Dictionary<int, Ellipse>();
        Random rd = new Random();
        int i = 0;
        DispatcherTimer timer = new DispatcherTimer();

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {
           
            Ellipse ellipse = new Ellipse();
            movingEllipses[e.TouchDevice.Id] = ellipse;
            tp1 = e.GetTouchPoint(grd1);

        }

 





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
                    if (files.Contains(".JPG") || files.Contains(".jpg") || files.Contains(".JPEG") || files.Contains(".jpeg") || files.Contains(".bmp") || files.Contains(".BMP") || files.Contains(".GIF") || files.Contains(".gif") || files.Contains(".PNG") || files.Contains(".png"))
                    {
                        image_sum++;
                    }
                }
                images = new string[image_sum];
                image_sum = 0;
                foreach (string files in System.IO.Directory.GetFiles(dir))
                {
                    if (files.Contains(".JPG") || files.Contains(".jpg") || files.Contains(".JPEG") || files.Contains(".jpeg") || files.Contains(".bmp") || files.Contains(".BMP") || files.Contains(".GIF") || files.Contains(".gif") || files.Contains(".PNG") || files.Contains(".png"))
                    {
                        images[image_sum] = files; image_sum++;
                    }
                }
            }
            return images;
        }
        /// <summary>        
        /// 搜出路径下 文件路径      
        /// </summary>       
        /// <param name="dir"></param>       
        /// <returns></returns>     
        public static string[] SearchFoldermusic(string dir)
        {
            //string str1 = System.Environment.CurrentDirectory;
            //Directory.SetCurrentDirectory(System.Environment.CurrentDirectory);
            string[] images = null;
            if (System.IO.Directory.Exists(dir))   //如果存在这个文件夹       
            {
                int image_sum = 0;
                foreach (string files in System.IO.Directory.GetFiles(dir))
                {
                    if (files.Contains(".MP3") || files.Contains(".mp3") || files.Contains(".WAV") || files.Contains(".wav") || files.Contains(".WMA") || files.Contains(".wma") )
                    {
                        image_sum++;
                    }
                }
                images = new string[image_sum];
                image_sum = 0;
                foreach (string files in System.IO.Directory.GetFiles(dir))
                {
                    if (files.Contains(".MP3") || files.Contains(".mp3") || files.Contains(".WAV") || files.Contains(".wav") || files.Contains(".WMA") || files.Contains(".wma"))
                    {
                        images[image_sum] = files; image_sum++;
                    }
                }
            }
            return images;
        }
    }
}

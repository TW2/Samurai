using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Samurai
{
    public partial class WaveSpectrum : UserControl
    {
        private List<VideoFrame> videoFrames = new List<VideoFrame>();
        private long msPrecision = 10_000L, msWanted = 0L;
        private bool spectrum = false;
        DirectoryInfo directoryInfo = null;
        private string media = null;

        public WaveSpectrum()
        {
            InitializeComponent();

            Paint += WaveSpectrum_Paint;
        }

        private void WaveSpectrum_Paint(object sender, PaintEventArgs e)
        {
            // On remplit cette zone avec une couleur par défaut
            e.Graphics.FillRectangle(Brushes.Azure, e.ClipRectangle);

            if (media != null)
            {
                Bitmap m = DisplayImageAt();
                if (m != null) e.Graphics.DrawImage(m, e.ClipRectangle);
            }
        }

        public void setMedia(string media)
        {
            this.media = media;

            directoryInfo = new DirectoryInfo("temp");
            if (!directoryInfo.Exists) directoryInfo.Create();
            getFrames(new FileInfo(media));
        }

        private void getFrames(FileInfo f)
        {
            videoFrames.Clear();

            string ffprobeWish = "-select_streams v:0 " +
                    "-show_entries packet=pts_time,flags " +
                    "-of csv=print_section=0 \"" + f.FullName + "\"";

            List<VideoFrame> vfs = new List<VideoFrame>();

            Process p = new Process();
            p.StartInfo.FileName = @"lib\ffprobe.exe";
            p.StartInfo.Arguments = ffprobeWish;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            List<string> data = output.Split(new char[] { '\n' }).ToList<string>();

            int index = 0;
            foreach(string line in data)
            {
                try
                {
                    // Console.WriteLine("OUT: " + line);
                    int indexLastComma = line.IndexOf('\u002C');
                    string strtime = line.Substring(0, indexLastComma).Replace('.', ',');
                    double parsing = double.Parse(strtime);
                    double ms = parsing * 1000d;
                    bool keyframe = line.Contains(",K");

                    vfs.Add(VideoFrame.Create(ms, keyframe, index));

                    index++;
                }
                catch (Exception)
                {

                }
            }
        }

        private Bitmap CreateSpectrogram(long msAreaStart, long msAreaStop)
        {
            string start = Convert.ToString(msAreaStart) + "ms";
            string stop = Convert.ToString(msAreaStop) + "ms";

            DirectoryInfo folder = new DirectoryInfo(directoryInfo.FullName + "\\s");
            if (folder.Exists == false) folder.Create();
            FileInfo imgFile = new FileInfo(folder + "\\" + Convert.ToString(msAreaStart) + ".png");
            if (imgFile.Exists == true) imgFile.Delete();

            string ffmpegWish = "-ss " + start + " -to " + stop + " -i \"" + media + "\" " +
                "-lavfi showspectrumpic=s=" + Width + "x" + Height + ":legend=0:scale=cbrt \"" +
                imgFile.FullName + "\"";

            Process p = new Process();
            p.StartInfo.FileName = @"lib\ffmpeg.exe";
            p.StartInfo.Arguments = ffmpegWish;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();

            return new Bitmap(imgFile.FullName);
        }
        private Bitmap CreateWaveForm(long msAreaStart, long msAreaStop)
        {
            string start = Convert.ToString(msAreaStart) + "ms";
            string stop = Convert.ToString(msAreaStop) + "ms";

            DirectoryInfo folder = new DirectoryInfo(directoryInfo.FullName + "\\w");
            if (folder.Exists == false) folder.Create();
            FileInfo imgFile = new FileInfo(folder + "\\" + Convert.ToString(msAreaStart) + ".png");
            if (imgFile.Exists == true) imgFile.Delete();

            string ffmpegWish = "-ss " + start + " -to " + stop + " -i \"" + media + "\" " +
                "-filter_complex showwavespic=s=" + Width + "x" + Height + 
                ":colors=Blue|BlueViolet -frames:v 1 \"" + imgFile.FullName + "\"";

            Process p = new Process();
            p.StartInfo.FileName = @"lib\ffmpeg.exe";
            p.StartInfo.Arguments = ffmpegWish;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();

            return new Bitmap(imgFile.FullName);
        }

        public List<VideoFrame> Frames   // property
        {
            get { return videoFrames; }   // get method
        }

        public long MsPrecision   // property
        {
            get { return msPrecision; }   // get method
            set { msPrecision = value; Refresh(); }  // set method
        }

        public bool Spectrum   // property
        {
            get { return spectrum; }   // get method
            set { spectrum = value; Refresh(); }  // set method
        }

        public long MsWanted   // property
        {
            get { return msWanted; }   // get method
            set { msWanted = value; Refresh(); }  // set method
        }

        private Bitmap DisplayImageAt()
        {
            long res = msWanted / (msPrecision + 1);

            return spectrum ? 
                CreateSpectrogram(res, res + msPrecision) :
                CreateWaveForm(res, res + msPrecision);
        }

    }
}

using Mpv.NET.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Samurai
{
    public partial class Player : UserControl
    {
        private MpvPlayer mpvPlayer;
        private bool mediaExists = false;

        public Player()
        {
            InitializeComponent();

            mpvPlayer = new MpvPlayer(this.Handle)
            {
                Volume = 50
            };
        }

        public void dispose()
        {
            mpvPlayer.Dispose();
        }

        public void setMsPosition(long ms)
        {
            mpvPlayer.Position = TimeSpan.FromMilliseconds(ms);
        }

        public void setMsPosition(string rawTime)
        {
            setMsPosition(GetTime(rawTime));
        }

        private long GetTime(string rawTime)
        {
            string pat = @"(\d+):(\d+):(\d+).(\d+)";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            int hour = 0, min = 0, sec = 0, ms = 0;

            Match m = r.Match(rawTime);
            if (m.Success)
            {
                hour = Convert.ToInt32(m.Groups[1].Value);
                min = Convert.ToInt32(m.Groups[2].Value);
                sec = Convert.ToInt32(m.Groups[3].Value);
                ms = Convert.ToInt32(m.Groups[4].Value) * 10;
            }

            long time = hour * 3600000 + min * 60000 + sec * 1000 + ms;

            return time;
        }

        public void setVideo(string media)
        {
            if(mpvPlayer.IsPlaying) mpvPlayer.Stop();
            mpvPlayer.Load(media);
            mediaExists = true;
        }

        public void changeSubtitles(String url)
        {
            if(mediaExists == true)
            {
                if (mpvPlayer.IsPlaying) mpvPlayer.Stop();
                mpvPlayer.API.Command(new string[] { "sub-add", url });
            }            
        }

        public void play()
        {
            if (mediaExists != false) mpvPlayer.Resume();
        }

        public void pause()
        {
            if (mediaExists != false) mpvPlayer.Pause();
        }

        public void stop()
        {
            if(mediaExists != false)
            {
                mpvPlayer.Pause();
                setMsPosition(0L);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samurai
{
    public class VideoFrame
    {
        private string name;
        private double msTime;
        private bool keyframe;
        public int index;

        private VideoFrame()
        {

        }

        public static VideoFrame Create(double ms, bool keyframe, int index)
        {
            VideoFrame frame = new VideoFrame();

            frame.msTime = ms;
            frame.keyframe = keyframe;
            frame.name = keyframe ? "Keyframe" : "Normal frame";
            frame.index = index;

            return frame;
        }

        public String getName()
        {
            return name;
        }

        public double getMsTime()
        {
            return msTime;
        }

        public bool isKeyframe()
        {
            return keyframe;
        }

        public int getIndex()
        {
            return index;
        }
    }
}

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
    public partial class Indication : UserControl
    {
        public enum IndiceType
        {
            CPL, CPS
        }

        private IndiceType indiceType = IndiceType.CPL;
        private float indice = -1f;

        private string text = string.Empty;
        private long msTime = -1L;

        private string cpl = "CPL";
        private string cps = "CPS";

        public Indication()
        {
            InitializeComponent();

            Paint += Indication_Paint;
        }

        private void Indication_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            string i = Convert.ToString(indice);
            float s1 = e.Graphics.MeasureString(i, new Font("Arial", 8f)).Width;

            switch (indiceType)
            {
                case IndiceType.CPL:
                    CPL(text);
                    float s2 = e.Graphics.MeasureString(cpl, new Font("Arial", 6f)).Width;
                    e.Graphics.DrawString(cpl, new Font("Arial", 6f),
                        Brushes.Black, new PointF((Width - s2) / 2, 0));
                    e.Graphics.DrawLine(Pens.Black, new Point(4, 10), new Point(Width - 4, 10));
                    e.Graphics.DrawString(i, new Font("Arial", 8f),
                        Brushes.Black, new PointF((Width - s1) / 2, 12));
                    break;
                case IndiceType.CPS:
                    CPS(text, msTime);
                    float s3 = e.Graphics.MeasureString(cps, new Font("Arial", 6f)).Width;
                    e.Graphics.DrawString(cps, new Font("Arial", 6f),
                        Brushes.Black, new PointF((Width - s3) / 2, 0));
                    e.Graphics.DrawLine(Pens.Black, new Point(4, 10), new Point(Width - 4, 10));
                    e.Graphics.DrawString(i, new Font("Arial", 8f),
                        Brushes.Black, new PointF((Width - s1) / 2, 12));
                    break;
            }
            
        }

        private void CPL(string text)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                foreach (Match match in Regex.Matches(text, "\\{*[^\\}]*\\}([A-Za-z0-9\\s]+)"))
                {
                    sb.AppendLine(match.Value);
                }

                string s = sb.ToString().Length == 0 ? text : sb.ToString();

                if (s.Contains("\\N"))
                {
                    string[] t = s.Split(new[] { "\\N" }, StringSplitOptions.None);
                    int count = 0, lastCount = 0;

                    for (int i = 0; i < t.Length; i++)
                    {
                        count = Math.Max(count, t[i].Length);

                        if (count > lastCount) s = t[i];

                        lastCount = count;
                    }
                }

                indice = s.ToCharArray().Length;
            }
            catch (RegexMatchTimeoutException)
            {
                indice = 0;
            }
        }

        private void CPS(string text, long msTime)
        {
            if (msTime == 0)
            {
                indice = 0;
                return;
            }

            try
            {
                StringBuilder sb = new StringBuilder();

                foreach (Match match in Regex.Matches(text, "\\{*[^\\}]*\\}([A-Za-z0-9\\s]+)"))
                {
                    sb.AppendLine(match.Value);
                }

                string line = sb.ToString();
                line = line.Replace("\\N", "");
                line = line.Replace(" ", "");
                text = text.Replace("\\N", "");
                text = text.Replace(" ", "");

                double length = line.Length == 0 ?
                    text.ToCharArray().Length : line.ToCharArray().Length;
                double time = msTime / 1000L;

                if (time == 0)
                {
                    indice = 0;
                    return;
                }

                double result = length / time;

                indice = (float)Math.Round(result, 2);
            }
            catch (RegexMatchTimeoutException)
            {
                indice = 0;
            }
        }

        public IndiceType IType   // property
        {
            get { return indiceType; }   // get method
            set { indiceType = value; Refresh(); }  // set method
        }

        public float Indice   // property
        {
            get { return indice; }   // get method
            set { indice = value; Refresh(); }  // set method
        }

        public string RawText   // property
        {
            get { return text; }   // get method
            set { text = value; Refresh(); }  // set method
        }

        public long MsTime   // property
        {
            get { return msTime; }   // get method
            set { msTime = value; Refresh(); }  // set method
        }

        public string LangCPL   // property
        {
            set { cpl = value; }  // set method
        }

        public string LangCPS   // property
        {
            set { cps = value; }  // set method
        }
    }
}

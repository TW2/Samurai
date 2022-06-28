using CoolTable;
using CoolTable.Core;
using Samurai.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Samurai
{
    public partial class Form1 : Form
    {
        private Table table1;
        private Player player;
        private WaveSpectrum waveSpectrum;

        private Button btnPlay;
        private Button btnPause;
        private Button btnStop;

        // Contrôles de la courbe wave / spectrogramme
        // (Spectrum/Waveform controls)
        private Button btnChangeWaveSpectrum;
        private Button btnPlayBefore; // Play |-->[ (before area as [ ])
        private Button btnPlayBegin; // Play [-->|
        private Button btnPlayArea; // Play [-->]
        private Button btnPlayEnd; // Play |-->]
        private Button btnPlayAfter; // Play ]-->|
        private Button btnPlayBeforeKaraoke; // Play |-->[ (on syllable karaoke)
        private Button btnPlayAreaKaraoke; // Play [-->] (on syllable karaoke)
        private Button btnPlayAfterKaraoke; // Play ]-->| (on syllable karaoke)

        // Contrôles de la zone cliente
        // (User area controls)
        private PlaceholderTextBox txtLineType;
        private PlaceholderTextBox txtLayer;
        private PlaceholderTextBox txtStart;
        private PlaceholderTextBox txtEnd;
        private PlaceholderTextBox txtDuration;
        private PlaceholderTextBox txtMarginL;
        private PlaceholderTextBox txtMarginR;
        private PlaceholderTextBox txtMarginV;
        private PlaceholderTextBox txtStyle;
        private PlaceholderTextBox txtNameOrActor;
        private PlaceholderTextBox txtEffect;
        private PlaceholderTextBox txtLanguage;
        private RichTextBox txtText;
        private Indication indCPL;
        private Indication indCPS;

        private Size videoSize;

        public Form1()
        {
            //Thread t = new Thread(new ThreadStart(StartForm));
            //t.Start();

            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            InitializeComponent();
            Init();

            //t.Abort();            
        }

        public void StartForm()
        {
            Application.Run(new Splash());
        }

        private void Init()
        {
            mnuFile.Text = Languages.MenuFile;
            mnuFileOpenVideo.Text = Languages.SubMenuOpenVideo;
            mnuFileOpenASS.Text = Languages.SubMenuOpenASS;

            table1 = new Table();
            player = new Player();
            waveSpectrum = new WaveSpectrum();            

            player.Location = new Point(0, 24);
            player.Size = new Size(Width / 2, Height / 2 - 24 - 40);

            table1.Location = new Point(0, Height / 2);
            table1.Size = new Size(Width - 16, Height / 2 - 38);

            waveSpectrum.Location = new Point(Width / 2 + 1, 24);
            waveSpectrum.Size = new Size(Width / 2, Height / 4 - 24 - 28);

            this.Controls.Add(table1);
            this.Controls.Add(player);
            this.Controls.Add(waveSpectrum);

            //==============================================================
            // Video Controls
            //==============================================================

            btnPlay = new Button();
            btnPause = new Button();
            btnStop = new Button();

            btnPlay.Image = Properties.Resources._32_timer_stuffs_play_v;
            btnPlay.Location = new Point(40*0, Height / 2 - 40);
            btnPlay.Size = new Size(40, 40);
            btnPlay.Click += BtnPlay_Click;
            this.Controls.Add(btnPlay);

            btnPause.Image = Properties.Resources._32_timer_stuffs_pause_j;
            btnPause.Location = new Point(40*1, Height / 2 - 40);
            btnPause.Size = new Size(40, 40);
            btnPause.Click += BtnPause_Click;
            this.Controls.Add(btnPause);

            btnStop.Image = Properties.Resources._32_timer_stuffs_red;
            btnStop.Location = new Point(40*2, Height / 2 - 40);
            btnStop.Size = new Size(40, 40);
            btnStop.Click += BtnStop_Click;
            this.Controls.Add(btnStop);

            //==============================================================
            // Spectrum/Waveform Controls
            //==============================================================

            btnChangeWaveSpectrum = new Button();
            btnPlayBefore = new Button();
            btnPlayBegin = new Button();
            btnPlayArea = new Button();
            btnPlayEnd = new Button();
            btnPlayAfter = new Button();
            btnPlayBeforeKaraoke = new Button();
            btnPlayAreaKaraoke = new Button();
            btnPlayAfterKaraoke = new Button();
            txtLineType = new PlaceholderTextBox();
            txtLayer = new PlaceholderTextBox();
            txtStart = new PlaceholderTextBox();
            txtEnd = new PlaceholderTextBox();
            txtDuration = new PlaceholderTextBox();
            txtMarginL = new PlaceholderTextBox();
            txtMarginR = new PlaceholderTextBox();
            txtMarginV = new PlaceholderTextBox();
            txtStyle = new PlaceholderTextBox();
            txtNameOrActor = new PlaceholderTextBox();
            txtEffect = new PlaceholderTextBox();
            txtLanguage = new PlaceholderTextBox();
            txtText = new RichTextBox();
            indCPL = new Indication();
            indCPS = new Indication();

            btnChangeWaveSpectrum.Image = Properties.Resources._20_wavespectro;
            btnChangeWaveSpectrum.Location = new Point(28 * 0 + Width / 2 + 1, Height / 4 - 24);
            btnChangeWaveSpectrum.Size = new Size(28, 28);
            btnChangeWaveSpectrum.Click += BtnChangeWaveSpectrum_Click; ;
            this.Controls.Add(btnChangeWaveSpectrum);

            btnPlayBefore.Image = Properties.Resources._20_timer_stuffs_play_out_01;
            btnPlayBefore.Location = new Point(28 * 1 + Width / 2 + 1, Height / 4 - 24);
            btnPlayBefore.Size = new Size(28, 28);
            btnPlayBefore.Click += BtnPlayBefore_Click;
            this.Controls.Add(btnPlayBefore);

            btnPlayBegin.Image = Properties.Resources._20_timer_stuffs_play_in_01;
            btnPlayBegin.Location = new Point(28 * 2 + Width / 2 + 1, Height / 4 - 24);
            btnPlayBegin.Size = new Size(28, 28);
            btnPlayBegin.Click += BtnPlayBegin_Click;
            this.Controls.Add(btnPlayBegin);

            btnPlayArea.Image = Properties.Resources._20_timer_stuffs_in;
            btnPlayArea.Location = new Point(28 * 3 + Width / 2 + 1, Height / 4 - 24);
            btnPlayArea.Size = new Size(28, 28);
            btnPlayArea.Click += BtnPlayArea_Click;
            this.Controls.Add(btnPlayArea);

            btnPlayEnd.Image = Properties.Resources._20_timer_stuffs_play_in_02;
            btnPlayEnd.Location = new Point(28 * 4 + Width / 2 + 1, Height / 4 - 24);
            btnPlayEnd.Size = new Size(28, 28);
            btnPlayEnd.Click += BtnPlayEnd_Click;
            this.Controls.Add(btnPlayEnd);

            btnPlayAfter.Image = Properties.Resources._20_timer_stuffs_play_out_02;
            btnPlayAfter.Location = new Point(28 * 5 + Width / 2 + 1, Height / 4 - 24);
            btnPlayAfter.Size = new Size(28, 28);
            btnPlayAfter.Click += BtnPlayAfter_Click;
            this.Controls.Add(btnPlayAfter);

            btnPlayBeforeKaraoke.Image = Properties.Resources._20_timer_stuffs_play_out_05;
            btnPlayBeforeKaraoke.Location = new Point(28 * 6 + Width / 2 + 1, Height / 4 - 24);
            btnPlayBeforeKaraoke.Size = new Size(28, 28);
            btnPlayBeforeKaraoke.Click += BtnPlayBeforeKaraoke_Click;
            this.Controls.Add(btnPlayBeforeKaraoke);

            btnPlayAreaKaraoke.Image = Properties.Resources._20_timer_stuffs_area_yellow;
            btnPlayAreaKaraoke.Location = new Point(28 * 7 + Width / 2 + 1, Height / 4 - 24);
            btnPlayAreaKaraoke.Size = new Size(28, 28);
            btnPlayAreaKaraoke.Click += BtnPlayAreaKaraoke_Click;
            this.Controls.Add(btnPlayAreaKaraoke);

            btnPlayAfterKaraoke.Image = Properties.Resources._20_timer_stuffs_play_out_06;
            btnPlayAfterKaraoke.Location = new Point(28 * 8 + Width / 2 + 1, Height / 4 - 24);
            btnPlayAfterKaraoke.Size = new Size(28, 28);
            btnPlayAfterKaraoke.Click += BtnPlayAfterKaraoke_Click;
            this.Controls.Add(btnPlayAfterKaraoke);

            txtLineType.Location = new Point(Width / 2 + 1, Height / 4 - 24 + 28);
            txtLineType.Size = new Size(100, 30);
            txtLineType.Font = new Font(txtLineType.Font.FontFamily, 12f);
            txtLineType.Placeholder = Languages.TableLineType;
            this.Controls.Add(txtLineType);

            txtLayer.Location = new Point(Width / 2 + 1 + 100, Height / 4 - 24 + 28);
            txtLayer.Size = new Size(60, 30);
            txtLayer.Font = new Font(txtLayer.Font.FontFamily, 12f);
            txtLayer.Placeholder = Languages.TableLayer;
            this.Controls.Add(txtLayer);

            txtStart.Location = new Point(Width / 2 + 1 + 160, Height / 4 - 24 + 28);
            txtStart.Size = new Size(80, 30);
            txtStart.Font = new Font(txtStart.Font.FontFamily, 12f);
            txtStart.Placeholder = Languages.TableStartTime;
            this.Controls.Add(txtStart);

            txtEnd.Location = new Point(Width / 2 + 1 + 240, Height / 4 - 24 + 28);
            txtEnd.Size = new Size(80, 30);
            txtEnd.Font = new Font(txtEnd.Font.FontFamily, 12f);
            txtEnd.Placeholder = Languages.TableEndTime;
            this.Controls.Add(txtEnd);

            txtDuration.Location = new Point(Width / 2 + 1 + 320, Height / 4 - 24 + 28);
            txtDuration.Size = new Size(80, 30);
            txtDuration.Font = new Font(txtDuration.Font.FontFamily, 12f);
            txtDuration.Placeholder = Languages.TableLayer;
            this.Controls.Add(txtDuration);

            txtMarginL.Location = new Point(Width / 2 + 1 + 400, Height / 4 - 24 + 28);
            txtMarginL.Size = new Size(60, 30);
            txtMarginL.Font = new Font(txtMarginL.Font.FontFamily, 12f);
            txtMarginL.Placeholder = Languages.TableMarginL;
            this.Controls.Add(txtMarginL);

            txtMarginR.Location = new Point(Width / 2 + 1 + 460, Height / 4 - 24 + 28);
            txtMarginR.Size = new Size(60, 30);
            txtMarginR.Font = new Font(txtMarginR.Font.FontFamily, 12f);
            txtMarginR.Placeholder = Languages.TableMarginR;
            this.Controls.Add(txtMarginR);

            txtMarginV.Location = new Point(Width / 2 + 1 + 520, Height / 4 - 24 + 28);
            txtMarginV.Size = new Size(60, 30);
            txtMarginV.Font = new Font(txtMarginV.Font.FontFamily, 12f);
            txtMarginV.Placeholder = Languages.TableMarginV;
            this.Controls.Add(txtMarginV);

            indCPL.Location = new Point(Width / 2 + 1, Height / 4 - 24 + 28 + 30);
            indCPL.Size = new Size(50, 30);
            indCPL.IType = Indication.IndiceType.CPL;
            indCPL.LangCPL = Languages.TableCPL;
            this.Controls.Add(indCPL);

            indCPS.Location = new Point(Width / 2 + 1 + 50, Height / 4 - 24 + 28 + 30);
            indCPS.Size = new Size(50, 30);
            indCPS.IType = Indication.IndiceType.CPS;
            indCPS.LangCPS = Languages.TableCPS;
            this.Controls.Add(indCPS);

            txtStyle.Location = new Point(Width / 2 + 1 + 100, Height / 4 - 24 + 28 + 30);
            txtStyle.Size = new Size(160, 30);
            txtStyle.Font = new Font(txtStyle.Font.FontFamily, 12f);
            txtStyle.Placeholder = Languages.TableStyle;
            this.Controls.Add(txtStyle);

            txtNameOrActor.Location = new Point(Width / 2 + 1 + 260, Height / 4 - 24 + 28 + 30);
            txtNameOrActor.Size = new Size(160, 30);
            txtNameOrActor.Font = new Font(txtNameOrActor.Font.FontFamily, 12f);
            txtNameOrActor.Placeholder = Languages.TableNameOrActor;
            this.Controls.Add(txtNameOrActor);

            txtEffect.Location = new Point(Width / 2 + 1 + 420, Height / 4 - 24 + 28 + 30);
            txtEffect.Size = new Size(160, 30);
            txtEffect.Font = new Font(txtEffect.Font.FontFamily, 12f);
            txtEffect.Placeholder = Languages.TableEffect;
            this.Controls.Add(txtEffect);

            txtLanguage.Location = new Point(Width / 2 + 1, Height / 4 - 24 + 28 + 30 * 2);
            txtLanguage.Size = new Size(Width / 2 - 20, 30);
            txtLanguage.Font = new Font(txtLanguage.Font.FontFamily, 12f);
            txtLanguage.Placeholder = Languages.TableText;
            this.Controls.Add(txtLanguage);

            // RichTextBox
            txtText.Location = new Point(Width / 2 + 1, Height / 4 - 24 + 28 + 30 * 3);
            txtText.Size = new Size(Width / 2 - 20, Height / 4 - 135);
            txtText.Font = new Font(txtText.Font.FontFamily, 12f);
            this.Controls.Add(txtText);

            //==============================================================

            videoSize = new Size(0, 0);

            //==============================================================

            table1.AddLineNumberColumn(Languages.TableLineNumber);         
            table1.AddColumn(Column.Create(typeof(string), Languages.TableLineType, 80f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableLayer, 60f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableStartTime, 80f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableEndTime, 80f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableMarginL, 35f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableMarginR, 35f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableMarginV, 35f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableStyle, 80f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableNameOrActor, 80f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableEffect, 80f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableCPL, 45f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableCPS, 45f));
            table1.AddColumn(Column.Create(typeof(string), Languages.TableText));
            table1.ResizeLastColumn(Width - 38);

            FormClosing += Form1_FormClosing;
            table1.MouseClick += Table1_MouseClick;
            player.MouseMove += Player_MouseMove;
        }

        private void Player_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void Table1_MouseClick(object sender, MouseEventArgs e)
        {
            List<object> selData = table1.SelectedData;
            if(selData != null && selData.Count > 0 && selData[0] != null)
            {
                txtLineType.Text = selData[1].ToString().Length > 0 ? selData[1].ToString() : txtLineType.Text;
                txtLayer.Text = selData[2].ToString().Length > 0 ? selData[2].ToString() : txtLayer.Text;
                txtStart.Text = selData[3].ToString().Length > 0 ? selData[3].ToString() : txtStart.Text;
                txtEnd.Text = selData[4].ToString().Length > 0 ? selData[4].ToString() : txtEnd.Text;
                txtMarginL.Text = selData[5].ToString().Length > 0 ? selData[5].ToString() : txtMarginL.Text;
                txtMarginR.Text = selData[6].ToString().Length > 0 ? selData[6].ToString() : txtMarginR.Text;
                txtMarginV.Text = selData[7].ToString().Length > 0 ? selData[7].ToString() : txtMarginV.Text;
                txtStyle.Text = selData[8].ToString().Length > 0 ? selData[8].ToString() : txtStyle.Text;
                txtNameOrActor.Text = selData[9].ToString().Length > 0 ? selData[9].ToString() : txtNameOrActor.Text;
                txtEffect.Text = selData[10].ToString().Length > 0 ? selData[10].ToString() : txtEffect.Text;
                txtLanguage.Text = selData[13].ToString().Length > 0 ? selData[13].ToString() : txtLanguage.Text;
                txtText.Text = selData[13].ToString();

                long duration = GetTime(selData[4].ToString()) - GetTime(selData[3].ToString());
                txtDuration.Text = SetTime(duration);

                indCPL.Indice = Convert.ToSingle(selData[11].ToString());
                indCPS.Indice = Convert.ToSingle(selData[12].ToString());

                txtLineType.ForeColor = txtLineType.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtLayer.ForeColor = txtLayer.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtStart.ForeColor = txtStart.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtEnd.ForeColor = txtEnd.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtMarginL.ForeColor = txtMarginL.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtMarginR.ForeColor = txtMarginR.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtMarginV.ForeColor = txtMarginV.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtStyle.ForeColor = txtStyle.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtNameOrActor.ForeColor = txtNameOrActor.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtEffect.ForeColor = txtEffect.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtLanguage.ForeColor = txtLanguage.TextLength > 0 ? Color.LimeGreen : Color.Salmon;
                txtDuration.ForeColor = txtDuration.TextLength > 0 ? Color.LimeGreen : Color.Salmon;

            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //--- big components ---
            player.Location = new Point(0, 24);
            player.Size = new Size(Width / 2, Height / 2 - 24 - 40);

            table1.Location = new Point(0, Height / 2);
            table1.Size = new Size(Width - 16, Height / 2 - 38);
            table1.ResizeLastColumn(Width - 38);
            table1.Refresh();

            waveSpectrum.Location = new Point(Width / 2 + 1, 24);
            waveSpectrum.Size = new Size(Width / 2, Height / 4 - 24 - 28);

            //--- small components ---
            btnPlay.Location = new Point(40 * 0, Height / 2 - 40);
            btnPlay.Size = new Size(40, 40);

            btnPause.Location = new Point(40 * 1, Height / 2 - 40);
            btnPause.Size = new Size(40, 40);

            btnStop.Location = new Point(40 * 2, Height / 2 - 40);
            btnStop.Size = new Size(40, 40);

            btnChangeWaveSpectrum.Location = new Point(28 * 0 + Width / 2 + 1, Height / 4 - 24);
            btnChangeWaveSpectrum.Size = new Size(28, 28);

            btnPlayBefore.Location = new Point(28 * 1 + Width / 2 + 1, Height / 4 - 24);
            btnPlayBefore.Size = new Size(28, 28);
            
            btnPlayBegin.Location = new Point(28 * 2 + Width / 2 + 1, Height / 4 - 24);
            btnPlayBegin.Size = new Size(28, 28);
            
            btnPlayArea.Location = new Point(28 * 3 + Width / 2 + 1, Height / 4 - 24);
            btnPlayArea.Size = new Size(28, 28);
            
            btnPlayEnd.Location = new Point(28 * 4 + Width / 2 + 1, Height / 4 - 24);
            btnPlayEnd.Size = new Size(28, 28);
            
            btnPlayAfter.Location = new Point(28 * 5 + Width / 2 + 1, Height / 4 - 24);
            btnPlayAfter.Size = new Size(28, 28);
            
            btnPlayBeforeKaraoke.Location = new Point(28 * 6 + Width / 2 + 1, Height / 4 - 24);
            btnPlayBeforeKaraoke.Size = new Size(28, 28);
            
            btnPlayAreaKaraoke.Location = new Point(28 * 7 + Width / 2 + 1, Height / 4 - 24);
            btnPlayAreaKaraoke.Size = new Size(28, 28);
            
            btnPlayAfterKaraoke.Location = new Point(28 * 8 + Width / 2 + 1, Height / 4 - 24);
            btnPlayAfterKaraoke.Size = new Size(28, 28);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            player.dispose();
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            player.play();
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            player.pause();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            player.stop();
        }

        private void BtnChangeWaveSpectrum_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnPlayAfterKaraoke_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnPlayAreaKaraoke_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnPlayBeforeKaraoke_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnPlayAfter_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnPlayEnd_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnPlayArea_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnPlayBegin_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnPlayBefore_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuFileOpenVideo_Click(object sender, EventArgs e)
        {
            ofdVideo = new OpenFileDialog();
            ofdVideo.Filter = "Matroska file (*.mkv)|*.mkv|All files (*.*)|*.*";
            ofdVideo.Title = Languages.DialogOpenVideo;
            DialogResult dr = ofdVideo.ShowDialog();
            if (dr == DialogResult.OK && player != null)
            {
                player.setVideo(ofdVideo.FileName);
                waveSpectrum.setMedia(ofdVideo.FileName);
                waveSpectrum.Refresh();


            }
        }

        private void mnuFileOpenASS_Click(object sender, EventArgs e)
        {
            ofdASS = new OpenFileDialog();
            ofdASS.Filter = "ASS file (*.ass)|*.ass";
            ofdASS.Title = Languages.DialogOpenASS;
            DialogResult dr = ofdASS.ShowDialog();
            if (dr == DialogResult.OK && player != null)
            {
                player.changeSubtitles(ofdASS.FileName);
                AddEventsToTable(ofdASS.FileName);
            }
        }

        private void ClearTable()
        {
            try
            {
                table1.RemoveAll();
            }
            catch { }            
        }

        private void AddEventsToTable(string path)
        {
            CSASS.Csass ass = new CSASS.Csass();
            ass.LoadASS(path);

            ClearTable();

            foreach(CSASS.CA_Event ev in ass.Events)
            {
                long duration = ev.End - ev.Start;

                table1.AddRow(new object[] {
                    "",                                             // Line number
                    ev.Comment == true ? "Comment" : "Dialogue",    // Line type
                    ev.Layer,                                       // Layer
                    ev.StartString,                                 // Start
                    ev.EndString,                                   // End
                    ev.MarginL,                                     // MarginL
                    ev.MarginR,                                     // MarginR
                    ev.MarginV,                                     // MarginV
                    ev.Style,                                       // Style
                    ev.NameOrActor,                                 // Name (actor)
                    ev.Effect,                                      // Effect
                    CPL(ev.Text),                                   // CPL
                    CPS(ev.Text, duration),                         // CPS
                    ev.Text                                         // Text
                });
            }
        }

        private string CPL(string text)
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

                return Convert.ToString(s.ToCharArray().Length);
            }
            catch (RegexMatchTimeoutException)
            {
                return "0";
            }
        }

        private string CPS(string text, long msTime)
        {
            if (msTime == 0) return "0";

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
                double time = msTime / 1000L; if(time == 0d) return "0";
                double result = length / time;

                return Convert.ToString(Math.Round(result, 2));
            }
            catch (RegexMatchTimeoutException)
            {
                return "0";
            }
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

        private string SetTime(long time)
        {
            int hour = (int)(time / 3600000);
            int min = (int)((time - 3600000 * hour) / 60000);
            int sec = (int)((time - 3600000 * hour - 60000 * min) / 1000);
            int mSec = (int)(time - 3600000 * hour - 60000 * min - 1000 * sec);

            int centi = mSec / 10;

            string h = Convert.ToString(hour);
            string m = min > 9 ? Convert.ToString(min) : "0" + Convert.ToString(min);
            string s = sec > 9 ? Convert.ToString(sec) : "0" + Convert.ToString(sec);
            string n = centi > 9 ? Convert.ToString(centi) : "0" + Convert.ToString(centi);

            return h + ":" + m + ":" + s + "." + n;
        }

        private void GetVideoSize(string media)
        {
            string ffprobeWish = "-v error -select_streams v:0 " +
                "-show_entries stream=width,height -of csv=s=x:p=0" +
                " \"" + media + "\"";

            Process p = new Process();
            p.StartInfo.FileName = @"lib\ffprobe.exe";
            p.StartInfo.Arguments = ffprobeWish;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            string[] t = output.Split('x');

            videoSize = new Size(Convert.ToInt32(t[0]), Convert.ToInt32(t[1]));
        }
    }
}

using Evoker;
using Evoker.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Graph
{
    public partial class MainForm : Form
    {
        int scr_x;
        int scr_y;

        string layoutPath = Path.Combine(Application.StartupPath, "layout.bmp");
        string backgroundPath = Path.Combine(Application.StartupPath, "background.bmp");
        string defPath = Path.Combine(Application.StartupPath, "def.bmp");
        string notesPath = Path.Combine(Application.StartupPath, "notesDb.txt");
        string[] notesDb;

        List<string> NoteText = new List<string>();
        PointF[] NoteLocations = new PointF[9];
        StreamWriter sw;
        

        public MainForm()
        {
            InitializeComponent();
            scr_x = Screen.PrimaryScreen.Bounds.Width;
            scr_y = Screen.PrimaryScreen.Bounds.Height;
            Setup();
        }

        void Setup()
        {
            #region Set Note Locations
            int x = scr_x / 8;
            int y = scr_y / 6;

            NoteLocations[0] = new PointF(5 * x, scr_y / 2); //Middle Notes
            NoteLocations[1] = new PointF(7 * x, scr_y / 2);
            NoteLocations[2] = new PointF(3 * x, scr_y / 2);

            NoteLocations[3] = new PointF(5 * x, y ); //Upper Notes
            NoteLocations[4] = new PointF(7 * x, y );
            NoteLocations[5] = new PointF(3 * x, y );

            NoteLocations[6] = new PointF(5 * x, 5 * y); //Downer Notes
            NoteLocations[7] = new PointF(7 * x, 5 * y);
            NoteLocations[8] = new PointF(3 * x, 5 * y);

            #endregion                
            notesDb = File.ReadAllLines(notesPath);
            comboBox1.DataSource = notesDb;

            List<string> fonts = new List<string>();
            foreach (FontFamily font in FontFamily.Families) { fonts.Add(font.Name); }
            cmbFont.DataSource = fonts;

            frontColour.Color = Color.Black;
            backColour.Color = Color.White;
            txtMetin.Text = "Example";
            txtPunto.Text = "56";
            cmbFont.SelectedIndex = 1;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            radTek.Checked = true;
        }

        public void saveAndSet(Image img)
        {
            try {
                if (File.Exists(backgroundPath)) {
                    try
                    {
                        File.Delete(backgroundPath);
                        img.Save(backgroundPath);
                        Wallpaper.Set(new Uri(backgroundPath), Wallpaper.Style.Centered);
                        img.Dispose();
                        File.Delete(defPath);
                    }
                    catch
                    {
                        img.Save(defPath);
                        Wallpaper.Set(new Uri(defPath), Wallpaper.Style.Centered);
                        img.Dispose();
                        //File.Delete(backgroundPath);
                    }
                }
                else
                {
                    img.Save(backgroundPath);
                    Wallpaper.Set(new Uri(defPath), Wallpaper.Style.Centered);
                    File.Delete(defPath);
                }
            }
            catch { MessageBox.Show("Unstability error.", "Err"); }
        }
        

        public void UpdateNotes()
        {
            Image backgroundImg = Image.FromFile(layoutPath);
            Graphics graphics = Graphics.FromImage(backgroundImg);
            SolidBrush solidBrush = new SolidBrush(Color.Black);

            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            NoteText.Clear();
            NoteText.AddRange(File.ReadAllLines(notesPath));
            
            for(int i = 0; i < 9; i++)
            {
                graphics.DrawString(NoteText[i], new Font("Arial", 24), solidBrush, NoteLocations[i], stringFormat);
            }

            saveAndSet(backgroundImg);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = notesDb[comboBox1.SelectedIndex];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.Delete(notesPath);
            File.AppendAllLines(notesPath, notesDb);
            UpdateNotes();
            comboBox1.DataSource = notesDb;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            notesDb[comboBox1.SelectedIndex] = textBox1.Text;
            comboBox1.DataSource = notesDb;
        }

        private void btnSSV_Click(object sender, EventArgs e)
        {
            Font font = new Font(cmbFont.SelectedItem.ToString(), Convert.ToInt32(txtPunto.Text));
            Pen pen = new Pen(frontColour.Color);

            font = new Font(cmbFont.SelectedItem.ToString(), Convert.ToInt32(txtPunto.Text));
            List<string> lines = new List<string>();
            foreach (string text in txtMetin.Lines) { lines.Add(text); }

            Image img = new Bitmap(scr_x, scr_y);
            Graphics graphics = Graphics.FromImage(img);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            graphics.Clear(backColour.Color);

            pen = new Pen(frontColour.Color);
            pen.Width = Convert.ToInt32(txtPunto.Text) / 8;

            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            if (radTek.Checked)
            {
                graphics.DrawString(lines[0], font, new SolidBrush(frontColour.Color), scr_x / 2, scr_y / 2, sf);
            }
            else if (radCift.Checked)
            {
                try
                {
                    graphics.DrawLine(pen, scr_x / 2, 0, scr_x / 2, scr_y);
                    graphics.DrawString(lines[0], font, new SolidBrush(frontColour.Color), scr_x / 4, scr_y / 2, sf);
                    graphics.DrawString(lines[1], font, new SolidBrush(frontColour.Color), scr_x * 3 / 4, scr_y / 2, sf);
                }
                catch
                {
                    MessageBox.Show("Insufficient input.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (radDort.Checked)
            {
                try
                {
                    graphics.DrawLine(pen, scr_x / 2, 0, scr_x / 2, scr_y);
                    graphics.DrawLine(pen, 0, scr_y / 2, scr_x, scr_y / 2);
                    graphics.DrawString(lines[0], font, new SolidBrush(frontColour.Color), scr_x / 4, scr_y / 4, sf);
                    graphics.DrawString(lines[1], font, new SolidBrush(frontColour.Color), scr_x * 3 / 4, scr_y / 4, sf);
                    graphics.DrawString(lines[2], font, new SolidBrush(frontColour.Color), scr_x / 4, scr_y * 3 / 4, sf);
                    graphics.DrawString(lines[3], font, new SolidBrush(frontColour.Color), scr_x * 3 / 4, scr_y * 3 / 4, sf);
                }
                catch
                {
                    MessageBox.Show("Insufficient input.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            graphics.Dispose();


            saveAndSet(img);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Wallpaper.Set(new Uri(Application.StartupPath + "\\layout.bmp"), Wallpaper.Style.Centered);
        }
    }
}

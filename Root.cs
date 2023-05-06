using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Inteferente_ECO
{
    public partial class Root : Form
    {

        Pen WhitePen = new Pen(Color.WhiteSmoke);
        OpenFileDialog Ofd = new OpenFileDialog();

        Bitmap EntityBitmap;
        TextureBrush EntityTextureBrush;
       

        Entity[,] Entities;

        public Root()
        {
            InitializeComponent();
        }

        private void Root_Load(object sender, EventArgs e)
        {
            new Settings();
            Settings.CellSizeX = MainPictureBox.Width / 10;
            Settings.CellSizeY = MainPictureBox.Height / 20;

            Ofd.Filter = "Game Map|*.txt";
            Ofd.FileName = string.Empty;
        }

        private void MainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (GridCheckbox.Checked)
            {
                for(int Linie = 0; Linie <= 20; Linie++)
                {
                    e.Graphics.DrawLine(WhitePen,0,Linie*Settings.CellSizeY, MainPictureBox.Width,Linie*Settings.CellSizeY);
                }
                for (int Coloana = 0; Coloana <= 10; Coloana++)
                {
                    e.Graphics.DrawLine(WhitePen, Coloana*Settings.CellSizeX, MainPictureBox.Height , Coloana*Settings.CellSizeX, -MainPictureBox.Height);
                }
            }

            if (Ofd.FileName != string.Empty)
            {
                // ADD ENTITIES IF NONE
                if (Entities == null) // LOAD ENTITIES INTO MATRIX
                {
                    Console.WriteLine("Registering entities...");

                    Entities = new Entity[20, 10];
                    foreach (string TextLine in File.ReadAllLines(Ofd.FileName))
                    {
                        string[] LineSplit = TextLine.Split(' ');
                        int LinieEntitate = Convert.ToInt32(LineSplit[1]) - 1;
                        int ColoanaEntitate = Convert.ToInt32(LineSplit[2]) - 1;

                        Entities[LinieEntitate, ColoanaEntitate] = new Entity 
                        {
                            Name = LineSplit[0],
                            X =  ColoanaEntitate * Settings.CellSizeX,
                            Y = LinieEntitate * Settings.CellSizeY,
                        };
                    }
                }

                // DRAW ENTITIES
                for(int Linie = 0; Linie < 20; Linie++)
                {
                    for(int Coloana = 0;Coloana < 10; Coloana++)
                    {
                        if (Entities[Linie,Coloana] != null)
                        {
                            EntityBitmap = new Bitmap(Image.FromFile(Settings.ResourcesPath + Entities[Linie,Coloana].Name + ".png") , Settings.CellSizeX , Settings.CellSizeY);
                            EntityTextureBrush = new TextureBrush(EntityBitmap);
                            e.Graphics.FillRectangle(EntityTextureBrush, Entities[Linie,Coloana].X , Entities[Linie, Coloana].Y, Settings.CellSizeX , Settings.CellSizeY);

                            EntityBitmap = null;
                            EntityTextureBrush = null;
                        }
                    }
                }

                //DRAW DEFLECTOR PREVIEW

                if(Settings.PlacingDeflector == true)
                {
                    Bitmap DeflectorBitmap = new Bitmap(Image.FromFile(Settings.ResourcesPath + "Deflector.png"), Settings.CellSizeX, Settings.CellSizeY);
                    for (int __IndRotIncr = 1; __IndRotIncr <= Settings.DeflectorIncrement; __IndRotIncr++)
                    {
                        DeflectorBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    TextureBrush DeflectorTextureBrush = new TextureBrush(DeflectorBitmap);
                    e.Graphics.FillRectangle(DeflectorTextureBrush, Settings.PlacementColumn * Settings.CellSizeX, Settings.PlacementLine * Settings.CellSizeY, Settings.CellSizeX, Settings.CellSizeY);
                }

            }

            
            Settings.GCTick += Math.Max(1, Update.Interval / 1000);
            if(Settings.GCTick >= 10)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Settings.GCTick = 0;
            }

        }

        private void StartButton_Click(object sender, EventArgs e)
        {

        }

        private void Update_Tick(object sender, EventArgs e)
        {
            MainPictureBox.Invalidate();
            DeflectorPanel.Invalidate();
        }

        private void LoadmapButton_Click(object sender, EventArgs e)
        {
            Ofd.ShowDialog();
        }

        private void DeflectorPanel_Paint(object sender, PaintEventArgs e)
        {
            Bitmap DeflectorPanelBitmap = new Bitmap(Image.FromFile(Settings.ResourcesPath + "Deflector.png"), DeflectorPanel.Width, DeflectorPanel.Height);
            for(int __IndRotIncr = 1; __IndRotIncr <= Settings.DeflectorIncrement; __IndRotIncr++)
            {
                DeflectorPanelBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            TextureBrush DeflectorPanelTextureBrush = new TextureBrush(DeflectorPanelBitmap);
            e.Graphics.FillRectangle(DeflectorPanelTextureBrush, 0, 0, DeflectorPanel.Width, DeflectorPanel.Height);
        }

        private void RotateDeflectorButton_Click(object sender, EventArgs e)
        {
            if(Settings.DeflectorIncrement + 1 == 4) Settings.DeflectorIncrement = 0; else Settings.DeflectorIncrement++;
        }

        private void DeflectorPanel_Click(object sender, EventArgs e)
        {
            Settings.PlacingDeflector = !Settings.PlacingDeflector;
        }

        private void MainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(Settings.PlacingDeflector == true)
            {
                int IncrementedLocationX = e.Location.X / Settings.CellSizeX * Settings.CellSizeX;
                int IncrementedLocationY = e.Location.Y / Settings.CellSizeY * Settings.CellSizeY;
                Settings.PlacementLine = IncrementedLocationY / Settings.CellSizeY;
                Settings.PlacementColumn = IncrementedLocationX / Settings.CellSizeX;
            }
        }
    }
}

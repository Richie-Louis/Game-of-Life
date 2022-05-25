﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

// Try adding audio to the project and choices

namespace GOLStartUpTemplate
{
    public partial class GameOfLife : Form
    {   // Global variable for finite or toriodal view
        // Finite = 0
        // Toroidal = 1
        int s = 0;

        // The universe array
        bool[,] universe = new bool[Properties.Settings.Default.CellWidthCount, Properties.Settings.Default.CellHeightCount];
        //bool[,] universe = new bool[10, 10];
        // The scratchpad array
        bool[,] scratchpad = new bool[Properties.Settings.Default.CellWidthCount, Properties.Settings.Default.CellHeightCount];
        //bool[,] scratchpad = new bool[10, 10];
        int xa;
        int ya;
        int maxWidth = 0;
        int maxHeight = 0;

        // Color
        Color countColor;
        Color countColor2;
        Color hudColor;
        Color hudColor2;

        // Drawing colors
        Color gridColor;
        Color cellColor;
        Color temp = Color.Red;

        // Global Variable
        Brush brush;
        int seed;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        // Adding audio
        SoundPlayer music1 = new SoundPlayer(@"C:\Users\robor\Music\WAV\Diamond no Ace Opening 3 full.wav");
        SoundPlayer music2 = new SoundPlayer(@"C:\Users\robor\Music\WAV\Savage Genius- Watashi wo mitsukete (Pandora Hearts-ED-2).wav");
        SoundPlayer music3 = new SoundPlayer(@"C:\Users\robor\Music\WAV\Baton.wav");
        SoundPlayer music4 = new SoundPlayer(@"C:\Users\robor\Music\WAV\Minami-Ke Okaeri - Merry Christmas S.wav");
        string m1 = "Diamond no Ace Opening 3 full";
        string m2 = "Savage Genius- Watashi wo mitsukete (Pandora Hearts-ED-2)";
        string m3 = "Baton";
        string m4 = "Minami-Ke Okaeri - Merry Christmas";

        public GameOfLife()
        {
            InitializeComponent();

            // Reading the property
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            countColor = Properties.Settings.Default.CountColor;
            countColor2 = Properties.Settings.Default.CountColor;
            hudColor = Properties.Settings.Default.HudColor;
            hudColor2 = Properties.Settings.Default.HudColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            xa = Properties.Settings.Default.CellWidthCount;
            ya = Properties.Settings.Default.CellHeightCount;
            //seed = Properties.Settings.Default.Seed;
            // Setup the timer
            timer.Interval = Properties.Settings.Default.TimerInterval;
            //timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            scratchpad = new bool[xa, ya];

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (s == 0)
                    {
                        int count = CountNeighborsFinite(x, y);

                        if (universe[x, y] == true && count < 2)
                        {
                            scratchpad[x, y] = false;
                        }
                        else if (universe[x, y] == true && count > 3)
                        {
                            scratchpad[x, y] = false;
                        }
                        else if (universe[x, y] == true && count == 2)
                        {
                            scratchpad[x, y] = true;
                        }
                        else if (universe[x, y] == true && count == 3)
                        {
                            scratchpad[x, y] = true;
                        }
                        else if (universe[x, y] == false && count == 3)
                        {
                            scratchpad[x, y] = true;
                        }

                    }

                    if (s == 1)
                    {
                        int count = CountNeighborsToroidal(x, y);

                        if (universe[x, y] == true && count < 2)
                        {
                            scratchpad[x, y] = false;
                        }
                        else if (universe[x, y] == true && count > 3)
                        {
                            scratchpad[x, y] = false;
                        }
                        else if (universe[x, y] == true && count == 2)
                        {
                            scratchpad[x, y] = true;
                        }
                        else if (universe[x, y] == true && count == 3)
                        {
                            scratchpad[x, y] = true;
                        }
                        else if (universe[x, y] == false && count == 3)
                        {
                            scratchpad[x, y] = true;
                        }

                    }
                }
            }
            bool[,] temp = universe;
            universe = scratchpad;
            scratchpad = temp;

            // Increment generation count
            generations++;

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

            // Invalidate graphics panel
            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {   // Brush for finite and toroidal view
            brush = new SolidBrush(countColor);

            // Use float to make window precise

            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            FontStyle fontStyle = FontStyle.Regular;
            Font font = new Font("Arial", 14, fontStyle);

            int alive = 0;
            string boundary = "";

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                        alive++;
                    }
                    int count = CountNeighborsFinite(x, y);

                    int finite = CountNeighborsFinite(x, y);

                    int toroidal = CountNeighborsToroidal(x, y);

                    if (s == 0)
                    {
                        boundary = "Finite";
                        if (finite > 0)
                        {
                            //graphicsPanel1.Invalidate();
                            e.Graphics.DrawString($"{finite}", font, brush, cellRect, format);
                        }
                    }
                    if (s == 1)
                    {
                        boundary = "Toroidal";
                        if (toroidal > 0)
                        {
                            e.Graphics.DrawString($"{toroidal}", font, brush, cellRect, format);
                            //graphicsPanel1.Invalidate();
                        }
                    }
                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }

            StringFormat nformat = new StringFormat();
            nformat.Alignment = StringAlignment.Near;
            nformat.LineAlignment = StringAlignment.Far;
            FontStyle nfontStyle = FontStyle.Regular;
            Font nfont = new Font("Arial", 12, nfontStyle);
            Brush nbrush = new SolidBrush(hudColor);
            e.Graphics.DrawString($"Generations: {generations}\nCell Count: {alive}\nBoundary Type: {boundary}\nUniverse Size: {{Width={xa}, Height={ya}}}", nfont, nbrush, graphicsPanel1.ClientRectangle, nformat);

            // Update status strip 
            aliveCellsNumber.Text = "Alive: " + alive.ToString();
            SeedLabel.Text = "Seed: " + seed.ToString();

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
            //brush.Dispose();
            font.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                //universe = new bool[xa, ya];
                //Floats
                // Calculate the width and height of each cell in pixels
                float cellWidth = (float)graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                float cellHeight = (float)graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[(int)x, (int)y] = !universe[(int)x, (int)y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Play_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            PlayerDialog player = new PlayerDialog();
            player.Player = "Which song do you want to listen to? ";
            player.Music = "Music 1";
            if(DialogResult.OK == player.ShowDialog())
            {

            }
            //if (s == 0)
            //{
            //List<string> musicList1 = new List<string>();
            //musicList1.Add(m1);
            //musicList1.Add(m3);
            //    int i = musicList1.Count - 1;
            //    while (i >= 0)
            //    {
            //        Console.WriteLine((i+1) + ") " + musicList1[i]);
            //        i--;
            //    }
            //    int n = Int32.Parse(Console.ReadLine());
            //    switch (n)
            //    {
            //        case 1:
            //            music1.PlayLooping();
            //            break;
            //        case 2:
            //            music3.PlayLooping();
            //            break;

            //        default:
            //            break;
            //    }
            //}
            //else
            //{
            //    List<string> musicList2 = new List<string>();
            //    musicList2.Add(m2);
            //    musicList2.Add(m4);
            //    for (int i = 0; i < musicList2.Count; i++)
            //    {
            //        Console.WriteLine((i+1) + ") " + musicList2[i]);
            //    }
            //    int m = Int32.Parse(Console.ReadLine());
            //    switch (m)
            //    {
            //        case 1:
            //            music2.PlayLooping();
            //            break;
            //        case 2:
            //            music4.PlayLooping();
            //            break;

            //        default:
            //            break;
            //    }
            //}
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            music1.Stop();
            music2.Stop();
            music3.Stop();
            music4.Stop();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal to xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal to yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }
                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xCheck = xLen - 1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }
                    // if xCheck is greater than or equal to xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        //xLen = 0;
                        xCheck = 0;
                    }

                    // if yCheck is greater than or equal to yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        //yLen = 0;
                        yCheck = 0;
                    }

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }
        private void newfile_Click(object sender, EventArgs e)
        {
            generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            seed = 0;

            float cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            float cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

            Pen gridPen = new Pen(gridColor, 1);

            Brush cellBrush = new SolidBrush(cellColor);

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    RectangleF cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;
                    universe[x, y] = false;
                    scratchpad[x, y] = false;
                }
            }
            gridPen.Dispose();
            cellBrush.Dispose();
            graphicsPanel1.Invalidate();
        }

        private void viewNeighborCount_Click(object sender, EventArgs e)
        {
            if (viewNeighborCount.Checked == false)
            {
                neighborCountToolStripMenuItem.Checked = false;
                countColor = Color.Empty;
            }
            else
            {
                neighborCountToolStripMenuItem.Checked = true;
                countColor = countColor2;
            }
            graphicsPanel1.Invalidate();
        }

        private void finiteview_Click(object sender, EventArgs e)
        {
            s = 0;
            if (finiteview.Checked == true)
            {
                toroidalview.Checked = false;
            }
            graphicsPanel1.Invalidate();
        }

        private void toroidalview_Click(object sender, EventArgs e)
        {
            s = 1;
            if (toroidalview.Checked == true)
            {
                finiteview.Checked = false;
            }
            graphicsPanel1.Invalidate();
        }

        private void gridColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = gridColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
                temp = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }


        private void cellColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = cellColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }

        private void backColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = graphicsPanel1.BackColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
            }
                graphicsPanel1.Invalidate();
        }

        private void countColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = countColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                countColor = dlg.Color;
                countColor2 = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }

        private void gridView_Click(object sender, EventArgs e)
        {
            if (gridView.Checked == false)
            {
                gridToolStripMenuItem.Checked = false;
                gridColor = Color.Empty;
                gridColorToolStripMenuItem.Enabled = false;
                gridColorToolStripMenuItem1.Enabled = false;
            }

            else
            {
                gridToolStripMenuItem.Checked = true;
                gridColor = temp;
                gridColorToolStripMenuItem.Enabled = true;
                gridColorToolStripMenuItem1.Enabled = true;
            }

            graphicsPanel1.Invalidate();
        }

        private void fromSeedRandomize_Click(object sender, EventArgs e)
        {
            FromSeedDialog dlg = new FromSeedDialog();      

            // Seed
            dlg.Seed = seed;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                Random rng = new Random(dlg.Seed);
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Iterate through the universe in the x, left to right
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        int n = rng.Next(0, 2);
                        
                        if (n == 0)
                        {
                            universe[x, y] = true;
                        }
                        else
                        {
                            universe[x, y] = false;
                        }
                    }
                }
                seed = dlg.Seed;
                graphicsPanel1.Invalidate();
            }
        }

        private void optionsSettings_Click(object sender, EventArgs e)
        {
            
            OptionsSettingsDialog dlg = new OptionsSettingsDialog();
            if (maxWidth != 0)
            {
                dlg.WidthX = maxWidth;
                dlg.HeightY = maxHeight;
            }
            else
            {
                dlg.WidthX = xa;
                dlg.HeightY = ya;
            }
            dlg.Timer = timer.Interval;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                xa = dlg.WidthX;
                ya = dlg.HeightY;
                timer.Interval = dlg.Timer;
                universe = new bool[xa, ya];
                scratchpad = new bool[xa, ya];
                graphicsPanel1.Invalidate();
            }
        }

        private void GameOfLife_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Updating the property
            Properties.Settings.Default.PanelColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.CountColor = countColor;
            Properties.Settings.Default.CountColor = countColor2;
            Properties.Settings.Default.GridColor = gridColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.CellWidthCount = xa;
            Properties.Settings.Default.CellHeightCount = ya;
            Properties.Settings.Default.TimerInterval = timer.Interval;
            Properties.Settings.Default.Seed = seed;
            Properties.Settings.Default.HudColor = hudColor;
            Properties.Settings.Default.HudColor = hudColor2;
            Properties.Settings.Default.Save();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Reseting the property
            Properties.Settings.Default.Reset();
            generations = 0;
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            countColor = Properties.Settings.Default.CountColor;
            countColor2 = Properties.Settings.Default.CountColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            xa = Properties.Settings.Default.CellWidthCount;
            ya = Properties.Settings.Default.CellHeightCount;
            universe = new bool[xa, ya];
            timer.Interval = Properties.Settings.Default.TimerInterval;
            seed = Properties.Settings.Default.Seed;
            hudColor = Properties.Settings.Default.HudColor;
            hudColor2 = Properties.Settings.Default.HudColor;
            graphicsPanel1.Invalidate();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Reloading the property
            Properties.Settings.Default.Reload();
            graphicsPanel1.BackColor = Properties.Settings.Default.PanelColor;
            countColor = Properties.Settings.Default.CountColor;
            countColor2 = Properties.Settings.Default.CountColor;
            gridColor = Properties.Settings.Default.GridColor;
            cellColor = Properties.Settings.Default.CellColor;
            xa = Properties.Settings.Default.CellWidthCount;
            ya = Properties.Settings.Default.CellHeightCount;
            timer.Interval = Properties.Settings.Default.TimerInterval;
            seed = Properties.Settings.Default.Seed;
            hudColor = Properties.Settings.Default.HudColor;
            hudColor2 = Properties.Settings.Default.HudColor;
            universe = new bool[xa, ya];
            scratchpad = new bool[xa, ya];
            graphicsPanel1.Invalidate();
        }

        private void fromTime_Click(object sender, EventArgs e)
        {
            // Time
            Random time = new Random(DateTime.Now.Minute);

            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int o = time.Next(0, 2);
                    if (o == 0)
                    {
                        universe[x, y] = true;
                    }
                    else
                    {
                        universe[x, y] = false;
                    }
                }
            }
            graphicsPanel1.Invalidate();
        }

        private void fromCurrentSeed_Click(object sender, EventArgs e)
        {
            // Seed
            Random rng = new Random(seed);
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int n = rng.Next(0, 2);
                    if (n == 0)
                    {
                        universe[x, y] = true;
                    }
                    else
                    {
                        universe[x, y] = false;
                    }
                }
            }
            graphicsPanel1.Invalidate();
        }

        private void save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);
                writer.WriteLine(seed);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!" + System.DateTime.Now.ToString());

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        if (universe[x, y] == true)
                        {
                            currentRow += 'O';
                        }

                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        else
                        {
                            currentRow += '.';
                        }
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        private void open_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);
                generations = 0;
                toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                maxWidth = 0;
                maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (row.StartsWith("!"))
                    {
                        continue;
                    }
                    if (row.Any(char.IsDigit))
                    {
                        continue;
                    }

                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.

                    if (!row.StartsWith("!") || !row.Any(char.IsDigit))
                    {
                        maxHeight++;
                    }

                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    maxWidth = row.Length;
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                universe = new bool[maxWidth, maxHeight];
                scratchpad = new bool[maxWidth, maxHeight];

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Y position
                int yPos = 0;

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    if (row.StartsWith("!"))
                    {
                        continue;
                    }
                    if (row.Any(char.IsDigit))
                    {
                        seed = Convert.ToInt32(row);
                        continue;
                    }

                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    for (int xPos = 0; xPos < row.Length; xPos++)
                    {
                        // If row[xPos] is a 'O' (capital O) then
                        // set the corresponding cell in the universe to alive.
                        if (row[xPos] == 'O')
                        {
                            universe[xPos, yPos] = true;
                        }

                        // If row[xPos] is a '.' (period) then
                        // set the corresponding cell in the universe to dead.
                        if (row[xPos] == '.')
                        {
                            universe[xPos, yPos] = false;
                        }
                    }
                    yPos++;
                }
                xa = maxWidth;
                ya = maxHeight;
                graphicsPanel1.Invalidate();
                // Close the file.
                reader.Close();
            }
        }

        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (neighborCountToolStripMenuItem.Checked == false)
            {
                viewNeighborCount.Checked = false;
                countColor = Color.Empty;
            }
            else
            {
                viewNeighborCount.Checked = true;
                countColor = countColor2;
            }
            graphicsPanel1.Invalidate();
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridToolStripMenuItem.Checked == false)
            {
                gridView.Checked = false;
                gridColor = Color.Empty;
                gridColorToolStripMenuItem.Enabled = false;
                gridColorToolStripMenuItem1.Enabled = false;
            }
            else
            {
                gridView.Checked = true;
                gridColor = temp;
                gridColorToolStripMenuItem.Enabled = true;
                gridColorToolStripMenuItem1.Enabled = true;
            }
            graphicsPanel1.Invalidate();
        }

        private void hudToolMenu_Click(object sender, EventArgs e)
        {
            if (hudToolMenu.Checked == false)
            {
                hudToolStrip.Checked = false;
                hudColor = Color.Empty;
                hudColors.Enabled = false;
                hudColors2.Enabled = false;
            }
            else
            {
                hudToolStrip.Checked = true;
                hudColors.Enabled = true;
                hudColors2.Enabled = true;
                hudColor = hudColor2;
            }
            graphicsPanel1.Invalidate();
        }

        private void hudToolStrip_Click(object sender, EventArgs e)
        {
            if (hudToolStrip.Checked == false)
            {
                hudToolMenu.Checked = false;
                hudColor = Color.Empty;
                hudColors.Enabled = false;
                hudColors2.Enabled = false;
            }
            else
            {
                hudToolMenu.Checked = true;
                hudColors.Enabled = true;
                hudColors2.Enabled = true;
                hudColor = hudColor2;
            }
            graphicsPanel1.Invalidate();
        }

        private void hudColors_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = hudColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                hudColor = dlg.Color;
                hudColor2 = dlg.Color;
                graphicsPanel1.Invalidate();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName; ;
            System.Diagnostics.Process.Start($@"{path}\Richie Game of Life About File.docx");
        }
    }
}

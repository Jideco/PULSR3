using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DarkPulsr
{
    public partial class Form1 : Form
    {
        int levelStart = 0;
        int cycle = 0;
        int score = 0;
        //
        //private float angle = 0;
        //private const float orbitRadius = 100;
        //private const float centerOffset = 150;
        //
        private int centerX;
        private int centerY;
        private int largeCircleRadius = 250;
        private int smallCircleRadius = 10;
        private double angle = 270;
        private double angularSpeed = 5;
        private int cycleCount = 0;
        public Form1()
        {
            InitializeComponent();
            //panel12.Paint += panel12_Paint;
            //timer.Tick += timer_Tick;
            //timer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Start();

            //this.KeyPreview = true;       // Enable keyevents for the move
            //this.KeyDown += keyisdown;   // Register the KeyDown event handler
        }

        /////// Timer //////////////
        private void timer_Tick(object sender, EventArgs e)
        {
            angle -= angularSpeed;
            if (angle == -90)
            {
                timer.Stop();
                cycleCount += 1;
                button1.Text = cycleCount.ToString();
                //button1.ForeColor = Color.White;
                //angle = 0;
            }
            panel12.Invalidate(); // Trigger a redraw of the panel

            //// Calculate and display the coordinates
            //float centerX = panel12.Width / 2;
            //float centerY = panel12.Height / 2;
            //float orbitingX = centerX + (float)(orbitRadius * Math.Cos(angle)) - centerOffset;
            //float orbitingY = centerY + (float)(orbitRadius * Math.Sin(angle));

            //string coordinates = $"Orbiting Circle: ({orbitingX}, {orbitingY})";
            //Console.WriteLine(coordinates); // Print to the command line

        }

        ////  Close Maximize Minimize  ////
        private void closeButton(object sender, MouseEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you want to close this window", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes) { Close(); }
            else if (dr == DialogResult.No) { }
            else { }
        }
        private void maximizeButton(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        private void minimizeButton(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal ;
        }

        //////// START Button //////
        private void startMouseEnter(object sender, EventArgs e)
        {
            panel8.BackgroundImage = global::DarkPulsr.Properties.Resources.hoverButton;
        }

        private void startMouseLeave(object sender, EventArgs e)
        {
            panel8.BackgroundImage = global::DarkPulsr.Properties.Resources.IdleButton;
        }

        private void startClick(object sender, MouseEventArgs e)
        {
            timer.Stop(); // Stop the timer if it is already running

            angle = 270;
            //cycleCount += 1;
            //textBox1.Text = cycleCount.ToString();
            timer.Start(); // Start the timer to begin the animation
        }

        ////////// RESET Button///
        private void resetMouseEnter(object sender, EventArgs e)
        {
            panel9.BackgroundImage = global::DarkPulsr.Properties.Resources.hoverButton;
        }

        private void resetMouseLeave(object sender, EventArgs e)
        {
            panel9.BackgroundImage = global::DarkPulsr.Properties.Resources.IdleButton;
        }

        private void resetClick(object sender, MouseEventArgs e)
        {
            button1.ResetText();  // Clear the textbox
            angle = 270;
            cycleCount = 0;
            timer.Stop();
        }

        ////// INCREASE Button ////
        private void increaseMouseEnter(object sender, EventArgs e)
        {
            panel10.BackgroundImage = global::DarkPulsr.Properties.Resources.hoverButton;
        }

        private void increaseMouseLeave(object sender, EventArgs e)
        {
            panel10.BackgroundImage = global::DarkPulsr.Properties.Resources.IdleButton;
        }

        private void increaseClick(object sender, MouseEventArgs e)
        {
                
            button2.Text = Convert.ToString(levelStart += 100);
            

        }

        ////// DECREASE Button /////
        private void decreaseMouseEnter(object sender, EventArgs e)
        {
            panel11.BackgroundImage = global::DarkPulsr.Properties.Resources.hoverButton;
        }
        private void decreaseMouseLeave(object sender, EventArgs e)
        {
            panel11.BackgroundImage = global::DarkPulsr.Properties.Resources.IdleButton;
        }
        private void decreaseClick(object sender, MouseEventArgs e)
        {
            if (levelStart > 100)
            {
                
                button2.Text = Convert.ToString(levelStart -= 100);
               
            }
        }
        ///////// Orbiting Circle /////////////
        private void panel12_Paint(object sender, PaintEventArgs e)
        {
            Pen largeCirclePen = new Pen(Color.FromArgb(0xB0, 0x80, 0x2E), 5.0f);
            float centerX = panel12.Width / 2;
            float centerY = panel12.Height / 2;
            //float orbitingX = centerX + (float)(orbitRadius * Math.Cos(angle)) - centerOffset;
            //float orbitingY = centerY + (float)(orbitRadius * Math.Sin(angle));

            /// Draw the larger circle
            float largeCircleX = centerX - largeCircleRadius;
            float largeCircleY = centerY - largeCircleRadius;
            float largeCircleDiameter = 2 * largeCircleRadius;

            e.Graphics.DrawEllipse(largeCirclePen, largeCircleX, largeCircleY, largeCircleDiameter, largeCircleDiameter);


            /// Draw Small Obiting Circle
            double radians = angle * Math.PI / 180;
            int smallCircleX = (int)(centerX + largeCircleRadius * Math.Cos(radians));
            int smallCircleY = (int)(centerY + largeCircleRadius * Math.Sin(radians));

            int smallCircleDiameter = 2 * smallCircleRadius;

            int smallCircleXPos = smallCircleX - smallCircleRadius;
            int smallCircleYPos = smallCircleY - smallCircleRadius;

            e.Graphics.FillEllipse(Brushes.Green, smallCircleXPos, smallCircleYPos, smallCircleDiameter, smallCircleDiameter);

            /// Display the coordinates in the terminal
            //Console.WriteLine("Smaller Circle Coordinates: X = {0}, Y = {1}", smallCircleXPos, smallCircleYPos);

            /// Draw the small rectangle
            //float rectWidth = 40;
            //float rectHeight = 20;
            //float rectX = centerX - rectWidth / 2;
            //float rectY = centerY - orbitingCircleRadius - rectHeight;
            //e.Graphics.FillRectangle(Brushes.Blue, rectX, rectY, rectWidth, rectHeight);
            //e.Graphics.FillRectangle(Brushes.Blue, rectX, rectY, 10, 10);
        }


    }
}

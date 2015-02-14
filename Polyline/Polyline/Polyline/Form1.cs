using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Point> hexPts = new List<Point>();

        protected override void OnPaint(PaintEventArgs e)
        {
        
            float[] hexX =new float[] {1.5F, 0F, -1.5F, -1.5F, 0.1F, 1.5F, 1.5F};
            float[] hexY=new float[] {0.5F,1F,0.5F,-0.5F,-1F,-0.5F,0.5F};
            float[] newhexX, newhexY;

            //can't multiply by array with *. Does this need a matrix?
            float[] genT = new float[] { 3.0F };
            float[] genS = new float[] { 1.5F, (float)(0.5F * Math.Sqrt(3)) };
            float[] trans;
            int k, l, kmax, lmax;

            //set y vals
            for (int c = 0; c < 7; c++)
            {
                hexY[c] = (float)(hexY[c] * Math.Sqrt(3));
            }

            kmax = 5;
            lmax = 5;

            for (k = -kmax; k <= kmax; k++)
            {
                for (l = -lmax; l <= lmax; l++)
                {
                    //trans = (float)k * genT + (float)l * genS);
                    //newhexX = hexX + trans[1];
                    //newhexY = hexY + trans[2];
                    //polyline[newhexX,newhexY];

                }
            }

            // Create pen.
            Pen blackPen = new Pen(Color.Black, 2);

            // Draw polygon to screen.
            //e.Graphics.DrawPolygon(blackPen, hexPts);


        }
    }
}

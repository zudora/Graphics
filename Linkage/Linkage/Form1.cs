using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Linkage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            drawSys();
            //rectRun();
  
        }

        List<PointF> sysPts = new List<PointF>();
        List<LineCoord> lines = new List<LineCoord>();
        //beam newBeam = new beam();

        protected override void OnPaint(PaintEventArgs e)
        {
            // Create pen.
            Pen blackPen = new Pen(Color.Black, 2);

            foreach (LineCoord line in lines)
            {
                e.Graphics.DrawLine(blackPen, line.start, line.end);
                //Debug.WriteLine(lines.IndexOf(line));
            }

#if false            
            e.Graphics.DrawRectangle(blackPen,newBeam.Left,newBeam.Right,newBeam.Height,newBeam.Width);
#endif
        }
        public void drawSys()
        {
            PointF fixLeft = new PointF(80, 200);
            PointF freeTop = new PointF();
            PointF freeBott = new PointF();
            PointF fixRt = new PointF(500, 300);

            PointF rtSweep = new PointF();

            int lRad = 140;
            int rRad = 140;
            int freeLen = 200;

            double ctrDist = Math.Sqrt((fixLeft.X - fixRt.X) * (fixLeft.X - fixRt.X) + (fixLeft.Y - fixRt.Y) + (fixLeft.Y - fixRt.Y));
            Debug.WriteLine("ctrDist=" + ctrDist);

            double topY = new double();
            double topX = new double();
            bool mFound = false;        //found first match
            double matchInput=0, matchEnd=0, matchDeg=0;

            //sweeps chosen arbitrarily
            for (double degrees = -100; degrees <= 120; degrees++)
            {
                double radAngle = degrees * Math.PI / 180;
                //Debug.WriteLine(radAngle);
                topX = fixLeft.X + lRad * (Math.Cos(radAngle));
                topY = fixLeft.Y + lRad * (Math.Sin(radAngle));
                freeTop.X = (float)topX;
                freeTop.Y = (float)topY;

                //find first match
                if (mFound == false)
                {
                    matchInput = 90;
                    matchEnd = 300;
                }

                for (matchDeg = matchInput-10; matchDeg <= matchEnd; matchDeg++)
                {
                    //convert to radians
                    double matchAng = matchDeg * Math.PI / 180;
                    //Debug.WriteLine(matchAng);
                    double bottX = fixRt.X + rRad * (Math.Cos(matchAng));
                    double bottY = fixRt.Y + rRad * (Math.Sin(matchAng));

                    double dist = Math.Sqrt((bottX - topX) * (bottX - topX) + (bottY - topY) * (bottY - topY));
                    if (Math.Abs(dist - freeLen) < 1)
                    {
                        freeBott.X = (float)bottX;
                        freeBott.Y = (float)bottY;
                        lines.Add(new LineCoord() { start = freeBott, end = fixRt });
                        lines.Add(new LineCoord() { start = freeTop, end = freeBott });
                        mFound = true;
                        matchInput = matchDeg;
                        matchEnd = matchDeg + 10; 
                        Debug.WriteLine("Left circle angle: "+ degrees +" Right circle angle: " + matchInput);
                    }

                    //draw the right arc circle
                    rtSweep.X = (float)bottX;
                    rtSweep.Y = (float)bottY;
                    lines.Add(new LineCoord() { start = fixRt, end = rtSweep });
                    
                }
                
                sysPts.Add(fixLeft);
                sysPts.Add(freeTop);
                sysPts.Add(rtSweep);

                //sysPts.Add(freeBott);
                sysPts.Add(fixRt);

                lines.Add(new LineCoord() { start = fixLeft, end = freeTop });
            }

        }

#if false
        public void rectRun()
        {
            PointF st = new PointF(100, 100);
            PointF en = new PointF(300, 300);

            //newBeam = beamRect(st, en);
            newBeam = new beam(st, en);
        }

        public RectangleF beamRect(PointF leftEnd,PointF rtEnd)
        {
            RectangleF rect = new RectangleF(leftEnd.X,rtEnd.X,100.0F,10.0F);
            return rect;
        }

        public struct beam
        {
            public RectangleF beamRect;
            public PointF leftCtr, rtCtr;

            public beam(PointF st, PointF en)
            { 
                
            }
        }

#endif
        public struct LineCoord
        {
            public PointF start;
            public PointF end;
        }
        

    }
}

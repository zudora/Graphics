using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int m_program = new int();
        RectangleF m_rect = new RectangleF();
        PointF m_corner = new PointF();
        PointF m_origPoint = new PointF();
        PointF m_invPoint = new PointF();
        List<lineCoord> m_lines = new List<lineCoord>();    //all connections between sets of m_origPoint,m_invPoint

        float m_side = new float();
        float m_stPtX = new float();
        float m_stPtY = new float();
        float m_offset = 5;

        List<lineCoord> m_origShape = new List<lineCoord>();  //set of points for initial shape
        List<lineCoord> m_refShape = new List<lineCoord>();   //set of points for inverted shape
        List<List<lineCoord>> m_listList = new List<List<lineCoord>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs eargs)
        {
            if (m_offset < 100)
            {
                m_side = 200;
                m_stPtX = 100;
                m_stPtY = 100;
                m_corner.X = m_stPtX;
                m_corner.Y = m_stPtY;
                m_rect = new RectangleF(m_corner.X, m_corner.Y, m_side, m_side);

                if (m_program == 1)
                {
                    invertLine(ref m_origPoint, ref m_invPoint, m_offset, m_rect);
                    m_lines.Add(new lineCoord(m_origPoint, m_invPoint));
                    m_offset = m_offset + 5;
                    this.Invalidate();
                }

                if (m_program == 2)
                {
                    //m_origShape, m_refShape are lineCoord lists
                    //set up vertices of original shape
                    PointF UL = new PointF(150, 150);
                    PointF UR = new PointF(150, 200);
                    PointF LR = new PointF(200, 200);
                    PointF LL = new PointF(200, 150);
                    m_origShape.Add(new lineCoord(UL, UR));
                    m_origShape.Add(new lineCoord(UR, LR));
                    m_origShape.Add(new lineCoord(LR, LL));
                    m_origShape.Add(new lineCoord(LL, UL));

                    shapeInvert(ref m_origShape, ref m_refShape, m_rect);
                    m_listList.Add(new List<lineCoord>(m_origShape));
                    m_listList.Add(new List<lineCoord>(m_refShape));

                    this.Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen myPen = new Pen(Brushes.SaddleBrown);
            myPen.Width = 1.0F;
            if (m_program == 1 | m_program == 2) { g.DrawEllipse(myPen, m_rect); }
            drawList(m_lines, myPen, g);

            //foreach (lineCoord line in m_lines)
            //{
            //    g.DrawLine(myPen, line.startPoint, line.endPoint);
            //}
            if (m_listList.Count > 1)
            {
                Debug.Write ("Run");
            }
            
            foreach (List<lineCoord> list in m_listList)
            {
                drawList(list, myPen, g);
            }
        }
        public static void drawList(List<lineCoord> lineList, Pen myPen, Graphics g)
        {
            foreach (lineCoord line in lineList)
            {
                g.DrawLine(myPen, line.startPoint, line.endPoint);
            }
        }
        public static void shapeInvert(ref List<lineCoord> m_origShape, ref List<lineCoord> m_refShape, RectangleF m_rect)
        {
            PointF ctr = new PointF(m_rect.Left + m_rect.Width / 2, m_rect.Top + m_rect.Height / 2);

            Matrix transl = invMatrix(ctr);

            foreach (lineCoord line in m_origShape)
            {
                PointF origSt = new PointF(line.startPoint.X, line.startPoint.Y);
                PointF origEnd = new PointF(line.startPoint.X, line.startPoint.Y);

                PointF invSt = new PointF();
                PointF invEnd = new PointF();
                invSt = pointInv(origSt, transl, m_rect.Height / 2);
                invEnd = pointInv(origEnd, transl, m_rect.Height / 2);

                m_refShape.Add(new lineCoord(invSt, invEnd));

            }

            transl.Reset();

        }

        public static void invertLine(ref PointF m_origPoint, ref PointF m_invPoint, float m_offset, RectangleF m_rect)
        {
            PointF ctr = new PointF(m_rect.Left + m_rect.Width / 2, m_rect.Top + m_rect.Height / 2);

            //express point to translate as an offset from ctr
            m_origPoint.X = ctr.X + m_offset * 1.2F;
            m_origPoint.Y = ctr.Y * 1.19F;

            Matrix transl = invMatrix(ctr);
            //Matrix revTrans = transl.Clone();
            //revTrans.Invert();

            PointF invSt = pointInv(m_origPoint, transl, m_rect.Height / 2);
            //Debug.WriteLine("orig: " + m_origPoint.X);
            //PointF origShift = pointMove(transl, m_origPoint);
            //Debug.WriteLine("Orig: " + m_origPoint.X + ", trans: " + origShift.X);

            //float rad = m_rect.Height/2;
            //m_invPoint.X = (origShift.X * rad * rad) / (float)(Math.Sqrt((origShift.X * origShift.X) + (origShift.Y * origShift.Y)));
            //m_invPoint.Y = (origShift.Y * rad * rad) / (float)(Math.Sqrt((origShift.X * origShift.X) + (origShift.Y * origShift.Y)));

            //Debug.WriteLine("Before revTrans: invPointX=" + m_invPoint.X + ", origPointX=" + m_origPoint.X);
            //m_invPoint = pointMove(revTrans, m_invPoint);
            //m_origPoint = pointMove(revTrans, origShift);
            //Debug.WriteLine("After revTrans: invPointX=" + m_invPoint.X + ", origPointX=" + m_origPoint.X);

            transl.Reset();
        }

        public static Matrix invMatrix(PointF ctr)
        {
            Matrix transl = new Matrix();
            transl.Translate(-(ctr.X), -(ctr.Y));
            return transl;
        }

        public static PointF pointInv(PointF origPoint, Matrix trMatrix, float rad)
        {
            Matrix revTrans = trMatrix.Clone();
            revTrans.Invert();
            PointF invPoint = new PointF();

            Debug.WriteLine("orig: " + origPoint.X);
            PointF origShift = pointMove(trMatrix, origPoint);
            Debug.WriteLine("Orig: " + origPoint.X + ", trans: " + origShift.X);

            invPoint.X = (origShift.X * rad * rad) / (float)(Math.Sqrt((origShift.X * origShift.X) + (origShift.Y * origShift.Y)));
            invPoint.Y = (origShift.Y * rad * rad) / (float)(Math.Sqrt((origShift.X * origShift.X) + (origShift.Y * origShift.Y)));

            Debug.WriteLine("Before revTrans: invPointX=" + invPoint.X + ", origPointX=" + origPoint.X);
            invPoint = pointMove(revTrans, invPoint);
            origPoint = pointMove(revTrans, origShift);
            Debug.WriteLine("After revTrans: invPointX=" + invPoint.X + ", origPointX=" + origPoint.X);

            revTrans.Reset();

            return invPoint;
        }
        public static PointF pointMove(Matrix matrix, PointF p)
        {
            //define new array of PointF containing one member and assign incoming point p to it.
            PointF[] points = new PointF[1];
            points[0] = p;

            //now we can apply transform to coords of one point "p" without changing the original point
            matrix.TransformPoints(points);
            return points[0];
        }

        public struct lineCoord
        {
            public PointF startPoint, endPoint;
            public lineCoord(PointF start, PointF end)
            {
                startPoint = start;
                endPoint = end;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                cartesian();
            }

            if (radioButton4.Checked)
            {
                sinGraph();
            }

            if (radioButton1.Checked)
            {
                System.Windows.Forms.Timer drawTimer = new System.Windows.Forms.Timer();
                drawTimer.Interval = 1000;
                drawTimer.Start();
                drawTimer.Tick += new EventHandler(Timer_Tick);
                m_program = 1;
            }
            if (radioButton2.Checked)
            {
                System.Windows.Forms.Timer drawTimer = new System.Windows.Forms.Timer();
                drawTimer.Interval = 1000;
                drawTimer.Start();
                drawTimer.Tick += new EventHandler(Timer_Tick);
                m_program = 2;
            }
}
        
        public void cartesian()
        {
            float xpt = new float();
            float ypt = new float();
            float xmult = new float();

            List<PointF> pointL = new List<PointF>();

            for (float c = 1.0F; c <= 2.0F; c += .2F)
            {    
                for (xpt = 0; xpt <= 3; xpt++) 
                {
                    ypt = 40*(float)Math.Pow((double)xpt, (double)c);
                    xmult = 40 * xpt;
                    pointL.Add(new PointF(xmult, ypt));
                }
            
                foreach (PointF point in pointL)
                {
                    int ind = new int();
                    ind = pointL.IndexOf(point);
                    if (ind < pointL.Count-1)
                    {
                        m_lines.Add(new lineCoord() {startPoint = point, endPoint=pointL[ind + 1]});
                    }
                }
                m_listList.Add(new List<lineCoord>(m_lines));
                m_lines.Clear();
                pointL.Clear();
            }
            
            this.Invalidate();

        }
        public void sinGraph()
        {
            float xpt = new float();
            float ypt = new float();
            float xMult = new float();
            float minY = new float();
            minY = 0;

            List<PointF> pointL = new List<PointF>();

            for (xpt = -10; xpt <= 10; xpt++)
            {
                ypt = (float)Math.Sin((double)xpt);
                ypt = 8*(ypt+50);
                xMult = 8*(xpt+50);
                if (ypt < minY) { minY = ypt; } 
                pointL.Add(new PointF(xMult, ypt));
            }

            foreach (PointF point in pointL)
            {
                //point.X = point.X + minY + 50;
                int ind = new int();
                ind = pointL.IndexOf(point);
                if (ind < pointL.Count - 1)
                {
                    m_lines.Add(new lineCoord() { startPoint = point, endPoint = pointL[ind + 1] });
                }
            }
            m_listList.Add(new List<lineCoord>(m_lines));
            m_lines.Clear();
            pointL.Clear();
            
            this.Invalidate();

        }

    }
}

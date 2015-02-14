using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Timers;
using System.Threading;


namespace Shape
{
    public partial class Form1 : Form
    {
        PointF[] m_pointArray = new PointF[]{};
        List<PointF> m_pointL = new List<PointF>();
        List<PointF> m_pointTrans = new List<PointF>();     //temporary home for the transformed points as they are added as lines
        List<lineCoord> m_lines = new List<lineCoord>();
        Matrix m_transMatrix = new Matrix();
        int m_prevCount = new int();
        int m_seedStart = new int();
        int m_prevSide = new int();
        float m_prevLen = new float();

        public Form1()
        {
            InitializeComponent();
            //set seedStart to 0 to mark first run
            m_seedStart = 0;
            System.Windows.Forms.Timer drawTimer = new System.Windows.Forms.Timer();
            drawTimer.Interval = 1000;
            drawTimer.Start();
            drawTimer.Tick += new EventHandler(Timer_Tick);
        }

        private void Timer_Tick(object sender, EventArgs eArgs)
        {
            
            if (m_seedStart == 0)
            {
                //this is the first run
                //define seed, convert to list, and then draw, skipping transform
                //Debug.Print("First run");

                //these are used to connect shapes after transformations. Each transformed shape will have a new figStart/figEnd combo
                PointF figStart;
                PointF figEnd;
                figStart = new PointF(50, 50);
                figEnd = new PointF(250, 50);
                
                m_pointL.Add(new PointF(150,150));
                m_pointL.Add(new PointF(150, 350));
                m_pointL.Add(new PointF(350, 350));
                m_pointL.Add(new PointF(350,150));

                //add coordinates as lines

                for (int pointID=0; pointID < m_pointL.Count-1; pointID++) {
                    m_lines.Add(new lineCoord(m_pointL[pointID], m_pointL[pointID+1]));
                }
            }
            else
            {
                //apply transform to line list object, then draw     
                //clear the list of lines and the transformed list
                m_lines.Clear();
                m_pointTrans.Clear();
                m_transMatrix.Reset();

                PointF figStart = new PointF();
                PointF figEnd = new PointF();
                List<PointF> figConn=new List<PointF>();
                
                Matrix ULTrans = new Matrix();
                Matrix URTrans = new Matrix();
                Matrix LLTrans = new Matrix();
                Matrix LRTrans = new Matrix();

                List<Matrix> transforms = new List<Matrix>();
                transforms.Add(ULTrans);
                transforms.Add(URTrans);
                transforms.Add(LLTrans);
                transforms.Add(LRTrans);

                //reduce scaling each time
                //float scale = (float)(.5-(m_seedStart*.02));

                //scale depends on length of boundary box compared to length of "side"
                //boundary is 300
                //after seed, round 2: 3 sides
                //round 3: 7 sides
                //round 4: 15 sides
                //round n = 2(n.sides) + 1
                //round n = (n^2)-1

                int sides = new int();
                if (m_seedStart == 1)
                {
                    sides = 3;
                }
                else
                {
                    sides = (m_prevSide) * 2 + 1;
                    
                }
                float scale = new float();
                m_prevSide = sides;
                float sideLen = 300 / sides;
                
                if (m_seedStart == 1)
                {
                    scale = .5F;
                    m_prevLen = 100F;
                }
                else
                {
                    scale = sideLen / m_prevLen;
                    m_prevLen = sideLen;
                }
                
                for (int i = 0; i < transforms.Count; i++) {
                
                    transforms[i].Translate(-250, -250, MatrixOrder.Append);
                    transforms[i].Scale(.5F, .5F, MatrixOrder.Append);
                    //transforms[i].Translate(150, 150, MatrixOrder.Append);
                    PointF ctr = new PointF(250, 250);
                    if (i == 0)     //Upper left
                    {
                        transforms[i].Rotate(-90, MatrixOrder.Append);
                        //transforms[i].RotateAt(-90, ctr, MatrixOrder.Append);
                        //translate (-100,-100) from old center 
                        transforms[i].Scale(1, -1, MatrixOrder.Append);
                        transforms[i].Translate(150, 150, MatrixOrder.Append);
                    }

                    if (i == 1) 
                    {
                        //translate (-100,100) from old center
                        transforms[i].Translate(150, 350, MatrixOrder.Append);
                    }
                    
                    if (i == 2) 
                    {
                        //translate (100,100) from old center
                        transforms[i].Translate(350, 350, MatrixOrder.Append);
                    }
                    
                    if (i == 3)
                    {
                        transforms[i].Rotate(90, MatrixOrder.Append);
                        //translate (100,-100) from old center 
                        transforms[i].Scale(1, -1, MatrixOrder.Append);
                        transforms[i].Translate(350, 150, MatrixOrder.Append);
                    }

                    m_transMatrix = transforms[i];

                    for (int j = 0; j < m_pointL.Count; j++) {
                        //send each point to transform method with matrix

                        PointF origPoint = m_pointL[j];
                        PointF transPoint = pointMove(m_transMatrix, origPoint);
                        m_pointTrans.Add(transPoint);
                        
                        if (m_seedStart == 2)
                        {
                            if (j == 2) 
                            { 
                                //MessageBox.Show("Round 2, point 2"); 
                            }
                        }

                        if (j == 0) {
                            
                            //invert order on 3
                            if (i == 3)
                            {
                                figEnd = transPoint;
                                PointF endPt = new PointF();
                                endPt = figEnd;
                                figConn.Add(endPt); 
                            }

                            else
                            {
                                figStart = transPoint;
                                PointF stPt = new PointF();
                                stPt = figStart;
                                figConn.Add(stPt);
                            }
                        }
                        if (j == m_pointL.Count - 1)
                        {
                            if (i == 3)
                            {
                                figStart = transPoint;
                                PointF stPt = new PointF();
                                stPt = figStart;
                                figConn.Add(stPt);
                            }
                            else
                            {
                                figEnd = transPoint;
                                PointF endPt = new PointF();
                                endPt = figEnd;
                                figConn.Add(endPt);
                            }
                        }
                    }
                }

                //add coordinates as lines
                for (int pointID = 0; pointID < m_pointTrans.Count - 1; pointID++) {
                    int a = (pointID+1) % m_pointL.Count;
                    if (a != 0) {
                        m_lines.Add(new lineCoord(m_pointTrans[pointID], m_pointTrans[pointID + 1]));
                        }
                }

                m_lines.Insert((m_pointL.Count)-1, new lineCoord(figConn[1], figConn[2]));
                m_lines.Insert((m_pointL.Count * 2)-1, new lineCoord(figConn[3], figConn[4]));
                m_lines.Insert((m_pointL.Count * 3)-1, new lineCoord(figConn[5], figConn[6]));

                //get points out of line list as input for next round
                m_pointL.Clear();
                
                foreach (lineCoord lineC in m_lines)
                {
                    //only add unique points
                    lineCoord prevLine = new lineCoord();
                    //bool hasPointX = (m_pointL.Any(p => p.X == lineC.startPoint.X));
                    //bool hasPointY = (m_pointL.Any(p => p.Y == lineC.startPoint.Y));
                    //if (hasPointX==false && hasPointY==false)
                    //{
                    //        m_pointL.Add(lineC.startPoint);
                    //        m_pointL.Add(lineC.endPoint);
                    //}

                    if (lineC.startPoint != prevLine.startPoint && lineC.endPoint != prevLine.endPoint)
                    {
                        m_pointL.Add(lineC.startPoint);
                        m_pointL.Add(lineC.endPoint);
                        prevLine.startPoint = lineC.startPoint;
                        prevLine.endPoint = lineC.endPoint;
                    }

                }

                //for (int n = 0; n < 4; n++) {
                //    //insert figConn
                //    //start1-start2
                //    //end2-start3
                //    //end3-end4
                //    for (int connCount = 0; connCount < figConn.Count; connCount+=2) {
                //        m_lines.Insert(ptCount, new lineCoord(figConn[connCount], figConn[connCount+1]));
                //    }
                //}
                //for (int i = 0; i < transforms.Count; i++) 
                //{
                //    m_transMatrix = transforms[i];
                //    foreach (PointF origPoint in m_pointL) {
                //        //send each point to transform method with matrix
                //        PointF transPoint = new PointF();
                //        transPoint = pointMove(m_transMatrix, origPoint);
                //        m_pointTrans.Add(transPoint);

                //        //add coordinates as lines
                //        for (int pointID = 0; pointID < m_pointTrans.Count - 1; pointID++) {
                //            m_lines.Add(new lineCoord(m_pointTrans[pointID], m_pointTrans[pointID + 1]));
                //        }
                //    }

            }

            m_seedStart++;
            this.Invalidate();
            
        }

        public static PointF pointMove(Matrix matrix, PointF p) 
        { 
            //define new array of PointF containing one member and assign incoming point p to it.
            PointF[] points= new PointF[1];
            points[0] = p;

            //now we can apply transform to coords of one point "p" without changing the original point
            matrix.TransformPoints(points);
            return points[0];
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {
            //Debug.WriteLine("New Round");
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(BackColor);

            Pen myPen = new Pen(Color.Red);
            myPen.Width = 3;
            Pen connPen = new Pen(Color.Blue);
            myPen.Width = 3;

            //Pen gridPen = new Pen(Color.Black);
            //gridPen.Width = 1;
            //PointF gridSt = new PointF(100, 100);
            //PointF gridXLB = new PointF(400, 100);
            //PointF gridYLB = new PointF(100, 400);
            //PointF gridEnd = new PointF(400,400);
   
            //g.DrawLine(gridPen, gridSt, gridXLB);
            //g.DrawLine(gridPen, gridSt, gridYLB);
            //g.DrawLine(gridPen, gridXLB, gridEnd);
            //g.DrawLine(gridPen, gridYLB, gridEnd);

            int c=new int();
            int pos2 = (m_pointL.Count * 2);
            int pos3 = (m_pointL.Count * 3);

            for (c=0; c<m_lines.Count; c++)
            {
                //(lineCoord coords in m_lines)
                //Debug.WriteLine(coords.startPoint.X + " " + coords.startPoint.Y);
                
                if (c==m_prevCount-1|c==(m_prevCount*2)-1|c==(m_prevCount*3)-1)
                {
                    g.DrawLine(connPen, m_lines[c].startPoint, m_lines[c].endPoint);
                }
                else
                {
                    g.DrawLine(myPen, m_lines[c].startPoint, m_lines[c].endPoint);
                }
            }

            m_prevCount = m_pointL.Count;

        }

       
        public void transTest(int m_pointArray)
        {
            //start with seed

            //int[,] m_pointArray = new int[1000, 2];
            //List<PointF> m_pointL = new List<PointF>();
            //List<lineCoord> m_lines = new List<lineCoord>();
            //List<PointF> pointL = new List<PointF>();
            //List<lineCoord> Lines = new List<lineCoord>();
        }


        public static void lineAdd(List<PointF> m_pointL, PointF startPoint, PointF endPoint, int linePos)
        {
            int index = linePos * 2;
            m_pointL.Insert(index, startPoint);
            m_pointL.Insert(index + 1, endPoint);
        }

        public struct lineCoord
        {
            public PointF startPoint, endPoint;
            public lineCoord(PointF start, PointF end)
            {
                startPoint=start;
                endPoint=end;
            }
        }
        
        //testing TransformPoints method
        //public void TransformPointsExample(PaintEventArgs e)
        //{
        //    Pen myPen = new Pen(Color.Blue, 1);
        //    Pen myPen2 = new Pen(Color.Red, 1);

        //    //create an array of PointFs
        //    PointF[] myArray =
        //        {
        //            new PointF(20,20),
        //            new PointF(120,20),
        //            new PointF(120,120),
        //            new PointF(20,120)
        //        };

        //    //draw the PointFs to the screen before applying transform
        //    e.Graphics.DrawLines(myPen, myArray);

        //    //create matrix and scale it
        //    Matrix myMatrix = new Matrix();
        //    myMatrix.Scale(3, 2, MatrixOrder.Append);
        //    myMatrix.TransformPoints(myArray);

        //    //draw the PointFs to the screen again after applying the transform
        //    e.Graphics.DrawLines(myPen2, myArray);
        //}

    }
}

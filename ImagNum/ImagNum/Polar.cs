using System;

namespace ImagNum
{
    public struct polCoord
    {
        public float dist, theta;

        public polCoord(float inX, float inY)
        {
            //How do zero vaules work?
            //Determine quadrant

            if (inX > 0)
            {
                //+,+
                if (inY > 0)
                {
                    dist = (float)Math.Sqrt((double)(inX * inX) + (inY * inY));
                    float angle = (float)Math.Atan((double)inY / inX);
                    theta = (float)((double)angle * 180 / Math.PI);
                }
                //+,0
                if (inY == 0)
                {
                    dist = (float)Math.Sqrt((double)(inX * inX) + (inY * inY));
                    float angle = (float)Math.Atan((double)inY / inX);
                    theta = (float)((double)angle * 180 / Math.PI);

                }
                //+,-
                else
                {
                    dist = (float)Math.Sqrt((double)(inX * inX) + (inY * inY));
                    float angle = (float)Math.Atan((double)inY / inX);
                    theta = 360+(float)((double)angle * 180 / Math.PI);
                }
            }
            if (inX == 0)
            {
                //0,+
                if (inY > 0)
                {
                    dist = (float)Math.Sqrt((double)(inX * inX) + (inY * inY));
                    float angle = (float)Math.Atan((double)inY / inX);
                    theta = 180+(float)((double)angle * 180 / Math.PI);
                }
                //0,0
                if (inY == 0)
                {
                    dist = (float)Math.Sqrt((double)(inX * inX) + (inY * inY));
                    float angle = (float)Math.Atan((double)inY / inX);
                    theta = (float)((double)angle * 180 / Math.PI);
                }
                //0,-
                else
                {
                    dist = (float)Math.Sqrt((double)(inX * inX) + (inY * inY));
                    float angle = (float)Math.Atan((double)inY / inX);
                    theta = 360+(float)((double)angle * 180 / Math.PI);
                }
            }
            else
            {
                //-,-
                if (inY > 0)
                {
                    dist = (float)Math.Sqrt((double)(inX * inX) + (inY * inY));
                    float angle = (float)Math.Atan((double)inY / inX);
                    theta = 180+(float)((double)angle * 180 / Math.PI);
                }
                //-,+
                if (inY == 0)
                {
                    dist = (float)Math.Sqrt((double)(inX * inX) + (inY * inY));
                    float angle = (float)Math.Atan((double)inY / inX);
                    theta = 180+(float)((double)angle * 180 / Math.PI);
                }
                //-,0
                else
                {
                    dist = (float)Math.Sqrt((double)(inX * inX) + (inY * inY));
                    float angle = (float)Math.Atan((double)inY / inX);
                    theta = 180+(float)((double)angle * 180 / Math.PI);
                }
            }
        }
    }

    public struct cartCoord
    {
        public float outX, outY;

        public cartCoord(float dist, float theta)
        {
        
            float angle=(float)((double)theta*Math.PI/180);
            
            //outX=(float)Math.Cos((double)angle/dist);
            outX = (float)(dist*Math.Cos((double)angle));
            //outY/sin theta=dist/sin 90
            outY = (float)(dist / Math.Sin(90) * Math.Sin((double)angle));
        }
    }

}

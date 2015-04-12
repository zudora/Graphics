using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImagNum
{
    public struct complex
    {
        public double real, imag;

        public complex(double realPart, double imagPart)
        {
            real = realPart;
            imag = imagPart;
        }

        public static complex operator +(complex z, complex w)
        {
            complex u;
            u.real = (z.real + w.real);
            u.imag = (z.imag + w.imag);
            
            return u;
        }

        public static complex operator *(complex z, complex w)
        {
            complex u;
            u.real = (z.real * w.real) - (z.imag * w.imag);
            u.imag = (z.real * w.imag) + (z.imag * w.real);
            return u;
        }

        public static complex operator /(complex z, complex w)
        {
            complex u;
            u.real = ((z.real * w.real) + (z.imag * w.imag))/((w.real*w.real)+(w.imag*w.imag));
            u.imag = (((z.imag * w.real) - (z.real * w.imag)) / ((w.real * w.real) + (w.imag * w.imag)));
            return u;
        }


        //public static void getDispVal(complex x, out string sign, out string imagVal)
        //{
        //    //abs of double component is 1: don't show the number before i
        //    //double component is 0: don't show the sign, number before i, or i
        //    //double component is <0: sign is -, val is -imag
        //    //double component is >0: sign is +, val is imag

        //    imagVal = "";
        //    double negVal = -x.imag;

        //    if (x.imag < 0)
        //    {
        //        sign = " - ";
        //        imagVal = negVal.ToString();
        //    }
        //    else if (x.imag > 0)
        //    {
        //        sign = " + ";
        //        imagVal = x.imag.ToString();
        //    }
        //    else
        //    {
        //        sign = "";
        //        imagVal = "";
        //    }

        //    if (Math.Abs(x.imag) == 1)
        //    {
        //        imagVal = "";
        //    }

        //}
    }
}

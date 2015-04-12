using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace ImagNum
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void GuiMain()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void Main()
        {
            complex z = new complex(1.0, -2.0);
            complex w = new complex(2.0, 3.0);
            complex r = new complex();
            string zDisp = "", wDisp = "", rDisp = "";

            zDisp = getDispVal(z);
            wDisp = getDispVal(w);

            r = z + w;
            rDisp = getDispVal(r);
            Debug.WriteLine(zDisp+" + " + wDisp+" = " + rDisp);

            r = z * w;
            rDisp = getDispVal(r);
            Debug.WriteLine(zDisp + " * " + wDisp + " = " + rDisp);

            r = z / w;
            rDisp = getDispVal(r);
            Debug.WriteLine(zDisp + " / " + wDisp + " = " + rDisp);

            //Identify radian values for angles. I think: Can be negative but negative=opposite direction of rotation.
            //Polar output here uses degrees

            

            float polInX = -12F;
            float polInY = 5F;
            polCoord pol = new polCoord(polInX, polInY);
            float cartInDist = 13F;
            float cartInAng = 22.6F;
            cartCoord cart = new cartCoord(cartInAng,cartInDist);
            Debug.WriteLine("Cartesian ("+polInX+ ","+polInY+") is (" + pol.dist + "," + pol.theta + ") in polar coordinates.");
            Debug.WriteLine("Polar ("+cartInDist+","+cartInAng+") is (" + cart.outX + "," + cart.outY + ") in Cartesian coordinates.");
        }

        public static string getDispVal(complex x) 
        {
            //abs of double component is 1: don't show the number before i
            //double component is 0: don't show the sign, number before i, or i
            //double component is <0: sign is -, val is -imag
            //double component is >0: sign is +, val is imag
            string imagVal="", sign="";
            double negVal = -x.imag;
            
            if (x.imag < 0)
            {
                sign = " - ";
                imagVal=negVal.ToString();
            }
            else if (x.imag > 0)
            {
                sign = " + ";
                imagVal=x.imag.ToString();
            }
            else
            {
                sign = "";
                imagVal = "";
            }

            if (Math.Abs(x.imag) == 1)
            {
                imagVal = "";
            }

            string disp2 = "(" + x.real + sign + imagVal + "i)";
            return disp2;
        }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelLib
{
    public class Pixel
    {
        //Atributs
         private byte R, G, B;

        //Mètodes


        public Pixel(byte R, byte G, byte B)
        {
             this.R = R;
             this.G = G;
             this.B = B;
        }

        public Pixel()
        {
            this.R = 0;
            this.G = 0;
            this.B = 0;
        }
        public int GetR()
        {
            return this.R;
        }

        public void SetR(byte newR)
        {
            this.R = newR;
        }

        public int GetG()
        {
            return this.G;
        }

        public void SetG(byte newG)
        {
            this.G = newG;
        }

        public int GetB()
        {
            return B;
        }

        public void SetB(byte newB)
        {
            this.B = newB;
        }
    }
}

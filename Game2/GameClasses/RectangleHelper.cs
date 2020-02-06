using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR.GameClasses
{
    static class RectangleHelper
    {
        const int TopZone = 5;
        const int SideZone = 4;
        const int Pixel = 1;

        public static bool TouchTopOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Bottom + Pixel >= r2.Top - Pixel && 
                    r1.Bottom <= r2.Top + (r2.Height / 2 ) &&
                    r1.Right >= r2.Left + (r2.Width / TopZone) &&
                    r1.Left <= r2.Right - (r2.Width / TopZone));

        }
        public static bool TouchBottomOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Top <= r2.Bottom + (r2.Height / TopZone) &&
                    r1.Top >= r2.Bottom - (r2.Height / TopZone) &&
                    r1.Right >= r2.Left + (r2.Width / TopZone) &&
                    r1.Left <= r2.Right -(r2.Width / TopZone));
        }
        public static bool TouchLeftOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Right <= r2.Right &&
                    r1.Right >= r2.Left - TopZone &&
                    r1.Top <= r2.Bottom - (r2.Width / SideZone) &&
                    r1.Bottom >= r2.Top + (r2.Width / SideZone));
        }
        public static bool TouchRightOf(this Rectangle r1, Rectangle r2)
        {
            return (r1.Left >= r2.Left &&
                    r1.Left <= r2.Right + TopZone &&
                    r1.Top <= r2.Bottom - (r2.Width / SideZone) &&
                    r1.Bottom >= r2.Top + (r2.Width / SideZone));
        }
    }
}

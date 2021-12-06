using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EventHandling.Object
{
    internal class Bubble : BaseObject
    {
        public int time = 0;
        public Bubble(float x, float y, float angle) : base(x, y, angle)
        {
            
        }

        public override void Render(Graphics g)
        {

             g.FillEllipse(new SolidBrush(Color.YellowGreen), -time / 2, -time / 2, time, time);
            g.DrawString($"{time}", new Font("Verdana", 8), new SolidBrush(Color.Black), time / 2, time / 2);

        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
                path.AddEllipse(-time / 2, -time / 2, time, time);
            return path;
        }
        

    }
}

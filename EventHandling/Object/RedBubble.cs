using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EventHandling.Object
{
    internal class RedBubble : BaseObject
    {
        public int time = 0;
        public RedBubble(float x, float y, float angle) : base(x, y, angle)
        {

        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(50, 255, 0, 0)), -time/2, -time / 2, time, time);
        }
        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-time / 2, -time / 2, time, time);
            return path;
        }

    }
}

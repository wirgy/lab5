using EventHandling.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventHandling
{
    public partial class Form1 : Form
    {
        public int score = 0;
   
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        Bubble bubble;
        Bubble bubble2;
        RedBubble redbubble;
        public Form1()
        {
            InitializeComponent();
            
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            bubble = new Bubble(0, 0, 0);
            bubble2 = new Bubble(0, 0, 0);
            redbubble = new RedBubble(100, 150, 0);
            // добавляю реакцию на пересечение
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            // добавил реакцию на пересечение с маркером
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnBubbleOverlap += (b) =>
            {
                GenerateCircle(b);
                score++;
                ScoreOutput();
            };
            player.OnRedBubbleOverlap += (rb) =>
            {
                GenerateCircle2(rb);
                score--;
                ScoreOutput();
                redbubble.time = 0;
            };

           
            marker = new Marker(pbMain.Width / 2 + 1, pbMain.Height / 2 + 1, 0);

            objects.Add(marker);
            objects.Add(player);
            objects.Add(bubble);
            objects.Add(bubble2);
            objects.Add(redbubble);

        }
        private void ScoreOutput()
        {
            label1.Text = "очки: " + score;
        }
        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            updatePlayer();

            foreach (var obj in objects.ToList())
            {

                // проверяю было ли пересечение с игроком
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj); // то есть игрок пересекся с объектом
                    obj.Overlap(player); // и объект пересекся с игроком
                }
            }
            // рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
                
            }
        }

        private void pbMain_Click(object sender, EventArgs e)
        {

        }

        private void updatePlayer()
        {
            // тут добавляем проверку на marker не нулевой
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                // по сути мы теперь используем вектор dx, dy
                // как вектор ускорения, точнее даже вектор притяжения
                // который притягивает игрока к маркеру
                // 0.5 просто коэффициент который подобрал на глаз
                // и который дает естественное ощущение движения
                player.vX += dx * 0.8f;
                player.vY += dy * 0.8f;

                // расчитываем угол поворота игрока 
                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            // тормозящий момент,
            // нужен чтобы, когда игрок достигнет маркера произошло постепенное замедление
            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;

        }
        private void GenerateCircle2(RedBubble сircle)
        {
            Random random = new Random();
            сircle.X = random.Next() % 600 + 40;
            сircle.Y = random.Next() % 300 + 40;
        }
        private void GenerateCircle(Bubble сircle)
        {
            Random random = new Random();
            сircle.X = random.Next() % 630 + 40;
            сircle.Y = random.Next() % 360 + 40;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            var r = new Random();
            foreach (var obj in objects.ToList())
            {
                if (obj is RedBubble redbubble)
                redbubble.time++;
                
               if (obj is Bubble bubble)
                {
                    bubble.time--;
                    if (bubble.time <= 0)
                    {
                        GenerateCircle(bubble);
                        bubble.time = 110 + r.Next() % 50;
                    }
                }

            }
           
            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {

            // тут добавил создание маркера по клику если он еще не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }

            marker.X = e.X;
            marker.Y = e.Y;
        }

    }
}

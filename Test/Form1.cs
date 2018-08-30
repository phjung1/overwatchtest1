using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ScreenStateLogger screenStateLogger = new ScreenStateLogger();

        Timer t = new Timer();

        private void Form1_Load(object sender, EventArgs e)
        {



            int width = 1024;
            int height = 768;
            Color[,] colorArray = new Color[width, height];


            

            screenStateLogger.ScreenRefreshed += (s, data) =>
            {

              

                    MyMouse mouse = new MyMouse();
                    Bitmap bmp;
                    LockBitmap lockBitmap;

                    double moveX = 0;
                    double moveY = 0;
                    int areaX = width / 2;
                    int areaY = height / 2;

                    int offset = 100;

                    int startX = areaX - offset;
                    int startY = areaY - offset;
                    int endX = areaX + offset;
                    int endY = areaY + offset;
                    Color c;

                    Point p;


                    using (var ms = new MemoryStream(data))
                    {

                        bmp = new Bitmap(Image.FromStream(ms));
                        lockBitmap = new LockBitmap(bmp);
                        lockBitmap.LockBits();



                        for (int y = startY; y < endY - 1; y++)
                        {
                            for (int x = startX; x < endX - 1; x++)
                            {
                                c = lockBitmap.GetPixel(x, y);
                                bool b1 = c.R > 200 && c.B < 160 && c.G < 160;
                                bool b2 = c.R > 150 && c.B < 80 && c.G < 95;

                                bool bb1 = c.R > 180 && c.G > 140;

                                if (bb1)
                                {
                                    continue;
                                }

                                if (b1 || b2)
                                {
                                    moveX = (x - width / 2);
                                    moveY = (y - height / 2) - 30;

                                    break;
                                }


                            }
                        }
                        lockBitmap.UnlockBits();
                        p = new Point(moveX, moveY);
                        mouse.Move(p);

                    }
              
            };
            screenStateLogger.Start();


            //globalKeyboardHook gkh = new globalKeyboardHook();
            //gkh.HookedKeys.Add(Keys.F7);
            //gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            //gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
            //gkh.hook();
        }



        private void gkh_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void gkh_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleWindows
{
    public partial class Form1 : Form
    {
        List<Image> imgList = new List<Image>();
        int imgWidth = 100;
        int imgHeight = 100;
        PuzzleGameEngine pge;
        private int theGameTick;
        private int theTick;
        private Font theFont;
        private Brush theBrush;
        private Pen thePen;

        public Form1()
        {
            InitializeComponent();
            //image를 가져와서 list에 넣자
            for (int i = 0; i < 16; i++)
            {
                string fileName = string.Format("pic_{0}.png", (char)('a'+i));
                //MessageBox.Show(fileName);
                Image tmp1 = Image.FromFile(fileName);
                imgList.Add(tmp1);

            }
            //PuzzleGameEngine 생성하자
            pge = new PuzzleGameEngine();
            //timer
            //기준 타이머 = 0;
            //움직이는 타이머 = 0;
            //타이머 시작하자
            theGameTick = 0;
            theTick = 0;
            timer1.Start();

            theFont = new Font("굴림", 15); //굴림체에 15포인트
            theBrush = new SolidBrush(Color.Green); //폰트와 브러쉬는 생성자에서 초기화 해주는거임
            thePen = new Pen(Color.Red);
          
           
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            /* 더블버퍼드 트루로하는건 버퍼를 두개 만들어놓고 둘중 완료된 버퍼를 출력. 그래서 안깜빡임(다 완성된걸 출력하니ㅣ까)
             * 그런데 1초에 게임오버 메세지박스뜨는이유는 우리가 보는 창은 최신 그림이 아니기 때문. 즉 데이터는 0인데 화면을 그리는 버퍼는
             * 아직 그 그림을 그리지 못함
             */

            //timer표시하자
            int time = 100 - (theTick - theGameTick) / (1000 / timer1.Interval);//1초씩.
            //theGameTick을 빼는 이유는 다음 스테이지가 있을 수 있다는 가정때문임
            // 만약 theTick이 40이 됐을때 깼음. theGameTick에 theTick을 넣어두면 다음 레벨때 theTick을 계속 증가시키니까
            //theTick - theGameTick하면 다음 스테이지 하는데 얼마나 걸렸는지 알 수 있음 
            string stringTime = string.Format("Time : {0:D3}", time);

            e.Graphics.DrawString(stringTime, theFont, theBrush, 0, 10);
           //펜은 선을 브러쉬는 면을 그림 
            e.Graphics.FillRectangle(theBrush, 0, 0, time * 3, 10);
      

            if (time == -1) //더블버퍼드때문에 -1로 하는것. 좀 편법 ㅎ
            {
                timer1.Stop();
                MessageBox.Show("게임오버");
            }

            //그릴 때 퍼즐 조각 image 그리자 
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //e.Graphics.DrawImage(imgList[i+j*4], i * imgWidth+50, j* imgHeight+50, imgWidth, imgHeight);
                    int index = pge.GetViewIndex(i + j * 4);
                    if (index != 16 - 1) //15지만 16으로 해야 나중에 바꿀때 편함 P인덱스가 15니까 P 안그리기 위함.
                    {
                        e.Graphics.DrawImage(imgList[index], i * imgWidth + 50, j * imgHeight + 50, imgWidth, imgHeight);
                    }
                }
            }
        }

        //클릭하면 클릭한곳과 빈곳을 교체하자
        private void Form1_MouseDown(object sender, MouseEventArgs e) //마우스 다운했을때 호출됨
        {
            int index = 0;
            int tmpX = e.X;
            int tmpY = e.Y;
            
            /*MessageBox.Show(((tmpX - 50) / imgWidth).ToString());
            MessageBox.Show(((tmpY - 50) / imgHeight).ToString());*/
            tmpX -= 50;
            tmpY -= 50;
            tmpX /= imgWidth;
            tmpY /= imgHeight;
            //MessageBox.Show(tmpX + ", " + tmpY);
            index = tmpX + tmpY * 4;
            pge.Change(index);
            Invalidate(); //새로 그려
            if (pge.IsEnd())
            {
                MessageBox.Show("ㅊㅋㅊㅋ");
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            theTick++;
            //50ms마다 1씩 증가.
            //그러면 theTick이 100이면 5000ms 즉 5초가 지난것
            Invalidate(); //새로 그려나 
        }
    }
}

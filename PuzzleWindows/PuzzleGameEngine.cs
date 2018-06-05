using System;

namespace PuzzleWindows
{
    internal class PuzzleGameEngine
    {
        int[] theViewIndices; //indices는 index의 복수
        private static int puzzleSize = 16;
        public PuzzleGameEngine()
        {
            //보여지는 이미지의 인덱스를 담자
            theViewIndices = new int[puzzleSize];
            for (int i = 0; i < theViewIndices.Length; i++)
            {
                theViewIndices[i] = i;
            }

            //랜덤으로 섞자
            Random rand = new Random();
            for (int i = 0; i < 10000; i++)
            {
                int i1 = rand.Next(puzzleSize); //0이상 16미만 
                int i2 = rand.Next(puzzleSize);

                //Swap(i1, i2);
                //그냥 마구잡이로 Swap하면 풀리지 않는 퍼즐이 될수도 있음.
                Change(i1); 
            }


        }

        private void Swap(int i1, int i2)
        {
            int tmp = theViewIndices[i1];
            theViewIndices[i1] = theViewIndices[i2];
            theViewIndices[i2] = tmp;
        }

        //보여지는 이미지의 인덱스를 반환하자
        internal int GetViewIndex(int i)
        {
            return theViewIndices[i];
        }

        internal void Change(int touchedIndex)
        {
           //터치한 인덱스 상하좌우에 빈칸이 있는지 확인하고 있으면 교체
           if((touchedIndex/4 == GetEmptyIndex()/4) && //같은 줄에 있는지 체크 
                (touchedIndex - 1 == GetEmptyIndex() || touchedIndex +1 == GetEmptyIndex())
                || touchedIndex -4 == GetEmptyIndex() || touchedIndex + 4 == GetEmptyIndex())
            {
                Swap(touchedIndex, GetEmptyIndex());
            }
        }

        //빈칸 인덱스를 반환하자
        private int GetEmptyIndex()
        {
            for(int i = 0; i < puzzleSize; i++)
            {
                if(theViewIndices[i] == puzzleSize - 1)
                {
                    return i;
                }
            }
            return -1;
        }

        internal bool IsEnd()
        {
            int cnt = 0;
            //theViewIndices에 순서대로 잘 배열되어있으면 end
            for(int i = 0; i < puzzleSize; i++)
            {
                if (theViewIndices[i] == i) cnt++;
            }
            return cnt == puzzleSize;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassConstants;
using TurtleWalk.ClassGround;

namespace TurtleWalk.ClassGroundMover
{
    static class GroundMover
    {
        private static int _first = 0;
        private static int _second = 0;

        private static double _initialX1, _initialX2;
        private static double _initialY1, _initialY2;

        private static double _finalX1, _finalX2;

        private static List<Ground> _groundsToMove;

        private static DispatcherTimer _timer;

        public static bool AreMoving = false;

        public static void StartMovingGrounds(Image btnClicked)
        {
            _groundsToMove = new List<Ground>();

            switch (btnClicked.Tag)
            {
                case 1:
                    _first = 0;
                    _second = 2;
                    break;

                case 2:
                    _first = 2;
                    _second = 3;
                    break;

                case 3:
                    _first = 1;
                    _second = 3;
                    break;

                case 4:
                    _first = 0;
                    _second = 2;
                    break;
            }

            _groundsToMove.Add(Ground.MovableGroundsList[_first]);
            _groundsToMove.Add(Ground.MovableGroundsList[_second]);

            _initialX1 = _groundsToMove[0].X;
            _initialY1 = _groundsToMove[0].Y;

            _finalX1 = _initialX1 + 136;

            _initialX2 = _groundsToMove[1].X;
            _initialY2 = _groundsToMove[1].Y;

            _finalX2 = _initialX2 + 136;

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 3);
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        private async static void OnTimerTick(object sender, EventArgs e)
        {
            MoveFront();
            await GroundsMovingBackAnimation();
        }

        private static void MoveFront()
        {
            if (_groundsToMove[0].X < _finalX1)
            {
                _groundsToMove[0].X += 5;
                _groundsToMove[0].HitBox = CollisionElement.GetHitBox(_groundsToMove[0].Body);
                _groundsToMove[0].HitBox.Inflate(50, 50);
            }

            if (_groundsToMove[1].X < _finalX2)
            {
                _groundsToMove[1].X += 5;
                _groundsToMove[1].HitBox = CollisionElement.GetHitBox(_groundsToMove[1].Body);
                _groundsToMove[1].HitBox.Inflate(50, 50);
            }

            MovingFrontCheck();
        }

        private static void MoveBack()
        {
            if (_groundsToMove[0].X > _initialX1)
            {
                _groundsToMove[0].X -= 5;
                _groundsToMove[0].HitBox = CollisionElement.GetHitBox(_groundsToMove[0].Body);
            }

            if (_groundsToMove[1].X > _initialX2)
            {
                _groundsToMove[1].X -= 5;
                _groundsToMove[1].HitBox = CollisionElement.GetHitBox(_groundsToMove[1].Body);
            }

            MovingBackCheck();
        }

        private static void MovingFrontCheck()
        {
            if (_groundsToMove[0].X < _finalX1 && _groundsToMove[1].X < _finalX2)
            {
                if (!AreMoving)
                {
                    AreMoving = true;
                }
            }
        }

        private static void MovingBackCheck()
        {
            if (_groundsToMove[0].X > _initialX1 && _groundsToMove[1].X > _initialX2)
            {
                if (!AreMoving)
                {
                    AreMoving = true;
                }
            }
            else
            {
                if (AreMoving)
                {
                    AreMoving = false;
                }
            }
        }

        private static async Task GroundsMovingBackAnimation()
        {
            await Task.Delay(2000);

            MoveBack();

            _timer.Stop();
        }
    }
}

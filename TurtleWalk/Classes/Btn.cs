using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TurtleWalk.ClassCollisionElement;
using TurtleWalk.ClassConstants;
using TurtleWalk.ClassGround;

namespace TurtleWalk.ClassBtn
{
    class Btn
    {
        private Image _btnImage;

        private List<Ground> _groundsToMove;

        private DispatcherTimer _timer;

        private double _initialX1, _initialX2;
        private double _initialY1, _initialY2;

        private double _finalX1, _finalX2;

        public Btn(Image btnImage)
        {
            _btnImage = btnImage;
            _btnImage.Cursor = Constants.CURSOR_HAND;
            _btnImage.MouseUp += BtnClick;
        }

        private async void BtnClick(object sender, MouseButtonEventArgs e)
        {
            await CursorAnimation();

            _groundsToMove = new List<Ground>();

            switch (_btnImage.Tag)
            {
                case 1:
                    _groundsToMove.Add(Ground.MovableGrounds[0]);
                    _groundsToMove.Add(Ground.MovableGrounds[1]);
                    break;

                case 2:
                    _groundsToMove.Add(Ground.MovableGrounds[2]);
                    _groundsToMove.Add(Ground.MovableGrounds[3]);
                    break;

                case 3:
                    _groundsToMove.Add(Ground.MovableGrounds[1]);
                    _groundsToMove.Add(Ground.MovableGrounds[3]);
                    break;

                case 4:
                    _groundsToMove.Add(Ground.MovableGrounds[0]);
                    _groundsToMove.Add(Ground.MovableGrounds[2]);
                    break;
            }

            _initialX1 = _groundsToMove[0].X;
            _initialY1 = _groundsToMove[0].Y;

            _finalX1 = _initialX1 + 136;

            _initialX2 = _groundsToMove[1].X;
            _initialY2 = _groundsToMove[1].Y;

            _finalX2 = _initialX2 + 136;

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 3);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private async void _timer_Tick(object sender, EventArgs e)
        {
            MoveFront();
            await GroundsFadeAnimation();
        }

        private void MoveFront()
        {
            if (_groundsToMove[0].X < _finalX1)
            {
                _groundsToMove[0].X += 5;
                _groundsToMove[0].HitBox = CollisionElement.GetHitBox(_groundsToMove[0].Body);
            }

            if (_groundsToMove[1].X < _finalX2)
            {
                _groundsToMove[1].X += 5;
                _groundsToMove[1].HitBox = CollisionElement.GetHitBox(_groundsToMove[1].Body);
            }
        }

        private void MoveBack()
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
        }

        private async Task CursorAnimation()
        {
            _btnImage.Cursor = Constants.CURSOR_GRABBED;
            await Task.Delay(250);
            _btnImage.Cursor = Constants.CURSOR_HAND;
        }


        // HITBOX PROBLEMS - INFLATION HEIGHT, WIDTH MAYBE? 
        private async Task GroundsFadeAnimation()
        {
            await Task.Delay(20000);

            MoveBack();

            _timer.Stop();

            _groundsToMove[0].HitBox = Rect.Empty;
            _groundsToMove[1].HitBox = Rect.Empty;
        }
    }
}

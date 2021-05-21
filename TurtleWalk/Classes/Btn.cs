using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private double _initialX;
        private double _initialY;

        private double _finalX;
        private const double _finalY = 312;

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
                    break;

                case 3:
                    break;

                case 4:
                    break;
            }

            _initialX = _groundsToMove[0].X;
            _initialY = _groundsToMove[0].Y;

            _finalX = _initialX + 136;
            _groundsToMove[0].Y = _finalY;

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 3);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private async void _timer_Tick(object sender, EventArgs e)
        {
            MoveFront();
            GroundsFadeAnimation();
        }

        private void MoveFront()
        {
            if (_groundsToMove[0].X < _finalX)
            {
                _groundsToMove[0].X += 5;
                _groundsToMove[0].HitBox = CollisionElement.GetHitBox(_groundsToMove[0].Body);
            }
        }

        private void MoveBack()
        {
            if (_groundsToMove[0].X > _initialX)
            {
                _groundsToMove[0].X -= 5;
                _groundsToMove[0].HitBox = CollisionElement.GetHitBox(_groundsToMove[0].Body);
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
            await Task.Delay(1500);

            MoveBack();

            //_groundsToMove[0].X = _initialX;
            //_groundsToMove[0].Y = _initialY;

            _timer.Stop();
        }
    }
}

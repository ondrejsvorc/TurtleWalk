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
using TurtleWalk.ClassGroundMover;

namespace TurtleWalk.ClassBtn
{
    class Btn
    {
        private Image _btnImage;

        public Btn(Image btnImage)
        {
            _btnImage = btnImage;
            _btnImage.Cursor = Constants.CURSOR_HAND;
            _btnImage.MouseUp += BtnClick;
        }

        private async void BtnClick(object sender, MouseButtonEventArgs e)
        {
            await CursorAnimation();

            if (!GroundMover.AreMoving)
            {
                GroundMover.StartMovingGrounds(_btnImage);
            }
        }

        private async Task CursorAnimation()
        {
            _btnImage.Cursor = Constants.CURSOR_GRABBED;
            await Task.Delay(250);
            _btnImage.Cursor = Constants.CURSOR_HAND;
        }
    }
}

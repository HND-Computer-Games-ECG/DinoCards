using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DinoCards
{
    #region Card enums
    enum FacingState
    {
        Up = 0,
        Down
    }
    #endregion

    class Card
    {
        // Front and back art of the card
        private Texture2D _frontTex;
        private Texture2D _backTex;
        private Texture2D _shadowTex;

        private Vector2 _pos;

        public Rectangle Rect { get; private set; }

        private Vector2 _shadowOffset;

        private bool _isHeld;
        private FacingState _facing;
        private bool _highlight;

        public int Value { get; private set; }

        public Card(Vector2 position, Texture2D frontTex, Texture2D backTex, Texture2D shadowTex, int value, FacingState facing = FacingState.Up)
        {
            _frontTex = frontTex;
            _backTex = backTex;
            _shadowTex = shadowTex;

            Value = value;

            _pos = position;
            Rect = new Rectangle(position.ToPoint(), frontTex.Bounds.Size);
            _shadowOffset = new Vector2(8);

            _isHeld = false;
            _highlight = false;

            _facing = facing;
        }

        public void Update(MouseState msCurr, MouseState msOld)
        {
            if (msCurr.LeftButton == ButtonState.Released)
                _isHeld = false;
            else if (msOld.LeftButton == ButtonState.Released && Rect.Contains(msCurr.Position))
                _isHeld = true;

            if (_isHeld)
            {
                _pos += (msCurr.Position - msOld.Position).ToVector2();
            }

            Rect = new Rectangle(_pos.ToPoint(), Rect.Size);

            _highlight = Rect.Contains(msCurr.Position);
        }

        public void Draw(SpriteBatch sb)
        {
            if (_isHeld)
                sb.Draw(_shadowTex, _pos + _shadowOffset, _highlight ? Color.White : Color.LightGray);

            sb.Draw(_facing == FacingState.Up ? _frontTex : _backTex, _pos, _highlight ? Color.White : Color.LightGray);
        }
    }
}

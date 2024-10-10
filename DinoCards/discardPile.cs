using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DinoCards
{
    class DiscardPile
    {
        public bool Accepting { get; set; }
        private int _currentValid;

        private List<Texture2D> _possibleTextures;
        private List<SoundEffect> _sfx;

        private Vector2 _pos;
        public Rectangle Rect { get => rect; private set => rect = value; }

        private float _shuffleTime;
        private float _shuffleTimer;

        private int _yMin, _yMax;
        private int _startY, _targetY;
        private Rectangle rect;

        public DiscardPile(int xPos, int yMin, int yMax, List<Texture2D> textures, List<SoundEffect> sfx)
        {
            _possibleTextures = textures;
            _sfx = sfx;

            _pos = new Vector2(xPos, yMin);
            Rect = new Rectangle(_pos.ToPoint(), (textures[0].Bounds.Size.ToVector2() * 1.2f).ToPoint());
            Accepting = false;
            _currentValid = 0;

            _shuffleTimer = _shuffleTime = 6f;
            _yMin = yMin;
            _yMax = yMax;
            _startY = _targetY = (int) _pos.Y;
        }

        public void TestCard(Card cardToTest)
        {
            if (Accepting && cardToTest.Value == _currentValid * 2 + 4)
            {
                _sfx[2].Play();
            }
            else
            {
                _sfx[1].Play();
            }
            Accepting = false;
            _shuffleTimer = _shuffleTime;
        }

        public void Update(float deltaTime)
        {
            _shuffleTimer -= deltaTime;

            if (_shuffleTimer > 0)  // Phase toggle timer
            {
                if (!Accepting) // Shuffling
                {
                    _currentValid = Game1.RNG.Next(_possibleTextures.Count);
                    _pos.Y = MathHelper.Lerp(_targetY, _startY, _shuffleTimer / _shuffleTime);
                }
            }
            else    // Time's up
            {
                _shuffleTimer = _shuffleTime;
                if (!Accepting) 
                {
                    _sfx[0].Play();
                    _startY = (int)_pos.Y;

                    if (_pos.Y < _yMin + 64)
                        _targetY = Game1.RNG.Next(_yMax - 128, _yMax);
                    else if (_pos.Y > _yMax - 64)
                        _targetY = Game1.RNG.Next(_yMin, _yMin + 128);
                    else
                        _targetY = Game1.RNG.Next(_yMin, _yMax);
                }
                Accepting = !Accepting;
            }

            rect.Location = _pos.ToPoint();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_possibleTextures[_currentValid], Rect, Color.White * 0.5f);
        }

    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace DinoCards
{
    class DiscardPile
    {
        public bool Accepting { get; set; }
        private int _currentValid;

        private List<Texture2D> _possibleTextures;
        private List<SoundEffect> _sfx;

        private Vector2 _pos;
        public Rectangle Rect { get; private set; }

        private float _shuffleTime;
        private float _shuffleTimer;

        public DiscardPile(Vector2 position, List<Texture2D> textures, List<SoundEffect> sfx)
        {
            _possibleTextures = textures;
            _sfx = sfx;

            _pos = position;
            Rect = new Rectangle(_pos.ToPoint(), (textures[0].Bounds.Size.ToVector2() * 1.2f).ToPoint());
            Accepting = false;
            _currentValid = 0;

            _shuffleTimer = _shuffleTime = 6f;
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
        }

        public void Update(float deltaTime)
        {
            _shuffleTimer -= deltaTime;

            if (_shuffleTimer > 0)
            {
                if (!Accepting)
                    _currentValid = Game1.RNG.Next(_possibleTextures.Count);
            }
            else
            {
                _shuffleTimer = _shuffleTime;
                if (!Accepting)
                    _sfx[0].Play();
                Accepting = !Accepting;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_possibleTextures[_currentValid], Rect, Color.White * 0.5f);
        }

    }
}

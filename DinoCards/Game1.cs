using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DinoCards
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static readonly Random RNG = new Random();

        private MouseState ms_curr, ms_old;

        private Point scR;

        private List<Texture2D> cardFronts;
        private List<SoundEffect> gameFX;

        // --

        private List<Card> dinoCards;
        private DiscardPile discards;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
        }

        protected override void Initialize()
        {

            scR = _graphics.GraphicsDevice.Viewport.Bounds.Size;

            dinoCards = new List<Card>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cardFronts = new List<Texture2D>
            {
                Content.Load<Texture2D>("Fronts//4_Ankylosaurus"),
                Content.Load<Texture2D>("Fronts//6_Parasaurolophus"),
                Content.Load<Texture2D>("Fronts//8_Diplodocus"),
                Content.Load<Texture2D>("Fronts//10_SabretoothedTiger"),
                Content.Load<Texture2D>("Fronts//12_Stegosaurus"), 
                Content.Load<Texture2D>("Fronts//14_Pterodactylus"),
                Content.Load<Texture2D>("Fronts//16_WoollyMammoth"), 
                Content.Load<Texture2D>("Fronts//18_Plesiosaurus"), 
                Content.Load<Texture2D>("Fronts//20_Tyrannosaurus"), 
                Content.Load<Texture2D>("Fronts//22_Triceratops"), 
                Content.Load<Texture2D>("Fronts//24_Spinosaurus")
            };
            gameFX = new List<SoundEffect>
            {
                Content.Load<SoundEffect>("sfx//question_004"),
                Content.Load<SoundEffect>("sfx//error_005"),
                Content.Load<SoundEffect>("sfx//confirmation_003")
            };

            discards = new DiscardPile(8, 8, scR.Y - 150, cardFronts, gameFX);

            for (var i = 0; i < cardFronts.Count; i++)
            {
                dinoCards.Add(
                    new Card(new Vector2(RNG.Next(150, scR.X - 60), RNG.Next(scR.Y - 100)),
                        cardFronts[i],
                        Content.Load<Texture2D>("0_Back"),
                        Content.Load<Texture2D>("0_LiftShadow"),
                        i*2 + 4)
                    );
            }

        }

        protected override void Update(GameTime gameTime)
        {
            ms_curr = Mouse.GetState();

            discards.Update((float) gameTime.ElapsedGameTime.TotalSeconds);

            foreach (var dino in dinoCards)
                dino.Update(ms_curr, ms_old);

            for (int i = dinoCards.Count - 1; i >= 0; i--)
            {
                if (discards.Accepting
                    && discards.Rect.Intersects(dinoCards[i].Rect)
                    && ms_curr.LeftButton == ButtonState.Released)
                {
                    discards.TestCard(dinoCards[i]);
                    dinoCards.RemoveAt(i);
                }
            }

            ms_old = ms_curr;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            _spriteBatch.Begin();
            discards.Draw(_spriteBatch);

            foreach (var dino in dinoCards)
                dino.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

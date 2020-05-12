using AtomBuilder.DomainModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AtomBuilder
{
	internal class WorkshopState : GameState
	{
        enum ElementState
        {
            Hidden,
            Showing,
            Visible,
            Hiding
        }

		private double fLastInputUpdate = 0.0;

		private Model fBaryon;

		private Model fElectron;

		private float aspectRatio;

		private Texture2D fTopLeftCornerTexture;

		private Texture2D fPencilTexture;

		private SpriteFont fElementSymbolFont;

		private SpriteFont fElementNameFont;

		private SpriteFont fAtomicNumberFont;

		private Texture2D fUnknownElementGroupBackground;

		private Texture2D fActinideSeriesBackground;

		private Texture2D fAddButton;

		private Texture2D fAlkaliMetalsBackground;

		private Texture2D fAlkalineEarthMetalsBackground;

		private Texture2D fLanthanideSeriesBackground;

		private Texture2D fMinusButton;

		private Texture2D fNobleGasesBackground;

		private Texture2D fNonmetalsBackground;

		private Texture2D fPoorMetalsBackground;

		private Texture2D fTransitionMetalsBackground;

		private SpriteFont fConfigurationFont;

		private float fViewRotation = 0f;

		private float fCameraDistance = 1000f;

		private SpriteBatch fSpriteBatch;

		private int fElementOffset = 0;

		private ElementState fElementState = ElementState.Hidden;

		private Isotope fCurrentIsotope;

		private int fUpdateSpeed = 250;

		private float modelRotation = 0f;

		private Vector3 cameraPosition = new Vector3(0f, 5000f, 50f);

		private readonly Atom fAtom;

		private readonly float fSpeed = 50f;

		private List<Vector3> fNucleusObjectPositions = new List<Vector3>();

		public WorkshopState(string pName, StatefulGame pGame, GraphicsDeviceManager pGraphics)
			: base(pName, pGame, pGraphics)
		{
			fAtom = new Atom();
		}

		public override void LoadContent()
		{
            fBaryon = ParentGame.Content.Load<Model>("Models/sphere");
			fElectron = ParentGame.Content.Load<Model>("Models/sphere");
			fConfigurationFont = ParentGame.Content.Load<SpriteFont>("Fonts/ConfigurationFont");
			fElementSymbolFont = ParentGame.Content.Load<SpriteFont>("Fonts/ElementSymbolFont");
			fElementNameFont = ParentGame.Content.Load<SpriteFont>("Fonts/ElementNameFont");
			fAtomicNumberFont = ParentGame.Content.Load<SpriteFont>("Fonts/AtomicNumberFont");
			fTopLeftCornerTexture = ParentGame.Content.Load<Texture2D>("Textures/TopLeftCorner");
			fActinideSeriesBackground = ParentGame.Content.Load<Texture2D>("Textures/ActinideSeries");
			fAlkaliMetalsBackground = ParentGame.Content.Load<Texture2D>("Textures/AlkaliMetals");
			fAlkalineEarthMetalsBackground = ParentGame.Content.Load<Texture2D>("Textures/AlkalineEarthMetals");
			fLanthanideSeriesBackground = ParentGame.Content.Load<Texture2D>("Textures/LanthanideSeries");
			fNobleGasesBackground = ParentGame.Content.Load<Texture2D>("Textures/NobleGases");
			fNonmetalsBackground = ParentGame.Content.Load<Texture2D>("Textures/Nonmetals");
			fPoorMetalsBackground = ParentGame.Content.Load<Texture2D>("Textures/PoorMetals");
			fTransitionMetalsBackground = ParentGame.Content.Load<Texture2D>("Textures/TransitionMetals");
			fUnknownElementGroupBackground = ParentGame.Content.Load<Texture2D>("Textures/UnknownElementGroup");
			fAddButton = ParentGame.Content.Load<Texture2D>("Textures/AddButton");
			fMinusButton = ParentGame.Content.Load<Texture2D>("Textures/MinusButton");
			fPencilTexture = ParentGame.Content.Load<Texture2D>("Textures/Pencil");
			fSpriteBatch = new SpriteBatch(graphics.GraphicsDevice);
		}

		public override void UnloadContent() { }

		public override void Update(GameTime gameTime)
		{
			fViewRotation = (float)gameTime.TotalGameTime.TotalMilliseconds / 50f;
			double num = 0.0;
			foreach (Vector3 fNucleusObjectPosition in fNucleusObjectPositions)
			{
				num = Math.Max(num, fNucleusObjectPosition.Length());
			}
			num += 25.0;
			double num2 = (num + 25 * fAtom.GetShellCount()) / Math.Tan(MathHelper.ToRadians(5f));
			if (fCameraDistance < num2)
			{
				fCameraDistance += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 5f;
			}
			if (fCameraDistance > num2)
			{
				fCameraDistance -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 5f;
			}
			int num3 = 250;
			if (gameTime.TotalGameTime.TotalMilliseconds - fLastInputUpdate > fUpdateSpeed)
			{
				MouseState state = Mouse.GetState();
				if ((int)state.LeftButton == 1)
				{
					fLastInputUpdate = gameTime.TotalGameTime.TotalMilliseconds;
					num3 = fUpdateSpeed - 5;
					if (15 <= state.Y && state.Y <= 35)
					{
						if (185 <= state.X && state.X <= 205) fAtom.GetNucleus().RemoveProton();
						if (210 <= state.X && state.X <= 230) fAtom.GetNucleus().AddProton();
					}
					if (45 <= state.Y && state.Y <= 65)
					{
						if (185 <= state.X && state.X <= 205) fAtom.GetNucleus().RemoveNeutron();
						if (210 <= state.X && state.X <= 230) fAtom.GetNucleus().AddNeutron();
					}
					if (75 <= state.Y && state.Y <= 95)
					{
						if (185 <= state.X && state.X <= 205) fAtom.RemoveElectron();
						if (210 <= state.X && state.X <= 230) fAtom.AddElectron();
					}
				}
				//KeyboardState state2 = Keyboard.GetState();
				//Keys[] pressedKeys = state2.GetPressedKeys();
				//foreach (Keys val in pressedKeys)
				//{
				//}
				fUpdateSpeed = num3;
			}
			cameraPosition = new Vector3(fCameraDistance * (float)Math.Sin(MathHelper.ToRadians(fViewRotation)), 0f, fCameraDistance * (float)Math.Cos(MathHelper.ToRadians(fViewRotation)));
			if (fNucleusObjectPositions.Count < fAtom.GetNucleus().GetNeutrons().Count + fAtom.GetNucleus().GetProtons().Count)
			{
				fNucleusObjectPositions = CalculateNucleusPositions(fAtom.GetNucleus().GetSize(), 5f, 1f);
			}
			if (fCurrentIsotope == fAtom.GetCurrentIsotope())
			{
				if (fCurrentIsotope != null && fElementState != ElementState.Visible)
				{
					fElementState = ElementState.Showing;
				}
			}
			else if (fElementState == ElementState.Hidden)
			{
				fCurrentIsotope = fAtom.GetCurrentIsotope();
				if (fCurrentIsotope != null)
				{
					fElementState = ElementState.Showing;
				}
			}
			else
			{
				fElementState = ElementState.Hiding;
			}
			if (fElementState == ElementState.Showing)
			{
				if (fElementOffset < 250)
				{
					fElementOffset += 25;
				}
				else
				{
					fElementState = ElementState.Visible;
				}
			}
			else if (fElementState == ElementState.Hiding)
			{
				if (fElementOffset > 0)
				{
					fElementOffset -= 25;
				}
				else
				{
					fElementState = ElementState.Hidden;
				}
			}
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			Viewport viewport = graphics.GraphicsDevice.Viewport;
			aspectRatio = (float)viewport.Width / viewport.Height;
			graphics.GraphicsDevice.Clear(Color.Black);
            Matrix[] array = new Matrix[fBaryon.Bones.Count];
            fBaryon.CopyAbsoluteBoneTransformsTo(array);
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            double num5 = 0.0;
            float num6 = 0.25f;
            if (fCurrentIsotope != null && fCurrentIsotope.IsRadioactive())
            {
                num6 = 1f;
            }
			Vector3 val;
            ModelMeshCollection.Enumerator enumerator;
            ModelEffectCollection.Enumerator enumerator2;
            Color val3;
            while (num4 < fAtom.GetNucleus().GetSize())
            {
                Vector3 val5;
                if (num2 < fAtom.GetNucleus().GetNeutrons().Count)
                {
                    float num7 = MathHelper.ToRadians((int)gameTime.TotalGameTime.TotalMilliseconds + num4 * (int)(720f / fNucleusObjectPositions.Count));
                    val = new Vector3(num6 * (float)Math.Sin(num7), num6 * (float)Math.Cos(num7), num6 * (float)Math.Sin(num7));
                    enumerator = fBaryon.Meshes.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            ModelMesh current = enumerator.Current;
                            enumerator2 = current.Effects.GetEnumerator();
                            try
                            {
                                while (enumerator2.MoveNext())
                                {
                                    BasicEffect val2 = (BasicEffect)enumerator2.Current;
                                    val2.EnableDefaultLighting();
                                    val2.World = array[current.ParentBone.Index] * Matrix.CreateRotationY(modelRotation) * Matrix.CreateTranslation(fNucleusObjectPositions[num4] + val) * Matrix.CreateScale(10f);
                                    val2.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                                    val2.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), aspectRatio, 1f, 10000f);
                                    val2.AmbientLightColor = Color.Blue.ToVector3();
                                }
                            }
                            finally
                            {
                                enumerator2.Dispose();
                            }
                            current.Draw();
                        }
                    }
                    finally
                    {
                        enumerator.Dispose();
                    }
                    val5 = fNucleusObjectPositions[num4];
                    num5 = Math.Max(num4, val5.Length());
                    num2++;
                    num4++;
                }
                if (num3 < fAtom.GetNucleus().GetProtons().Count)
                {
                    float num7 = MathHelper.ToRadians(((int)gameTime.TotalGameTime.TotalMilliseconds + num4 * (int)(720f / (float)fNucleusObjectPositions.Count)));
                    val = new Vector3(num6 * (float)Math.Sin(num7), num6 * (float)Math.Cos(num7), num6 * (float)Math.Sin(num7));
                    enumerator = fBaryon.Meshes.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            ModelMesh current = enumerator.Current;
                            enumerator2 = current.Effects.GetEnumerator();
                            try
                            {
                                while (enumerator2.MoveNext())
                                {
                                    BasicEffect val2 = (BasicEffect)enumerator2.Current;
                                    val2.EnableDefaultLighting();
                                    val2.World = array[current.ParentBone.Index] * Matrix.CreateRotationY(modelRotation) * Matrix.CreateTranslation(fNucleusObjectPositions[num4] + val) * Matrix.CreateScale(10f);
                                    val2.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                                    val2.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), aspectRatio, 1f, 10000f);
                                    val2.AmbientLightColor = Color.Red.ToVector3();
                                }
                            }
                            finally
                            {
                                enumerator2.Dispose();
                            }
                            current.Draw();
                        }
                    }
                    finally
                    {
                        enumerator.Dispose();
                    }
                    val5 = fNucleusObjectPositions[num4];
                    num5 = Math.Max(num5, val5.Length());
                    num3++;
                    num4++;
                }
            }
            foreach (ElectronShell shell in fAtom.GetShells())
            {
                int num8 = 0;
                if (fAtom.GetShellCount() > 0)
                {
                    num8 = 360 / fAtom.GetShellCount();
                    num8 *= fAtom.GetShells().IndexOf(shell);
                }
                double num9 = MathHelper.ToRadians(num8 + (int)gameTime.TotalGameTime.TotalMilliseconds / 100);
                if (fAtom.GetShells().IndexOf(shell) % 2 == 1)
                {
                    num9 *= -1.0;
                }
                foreach (Electron electron in shell.GetElectrons())
                {
                    float num10 = (float)(2f * shell.GetDistance() * Math.PI / fSpeed);
                    int num11 = (int)(electron.GetOffsetDegree() + gameTime.TotalGameTime.TotalMilliseconds / num10);
                    float num12 = MathHelper.ToRadians(num11);
                    if (fAtom.GetShells().IndexOf(shell) % 2 == 1)
                    {
                        num12 *= -1f;
                    }
                    enumerator = fElectron.Meshes.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            ModelMesh current = enumerator.Current;
                            float num13 = (float)Math.Sin(num9) * ((float)num5 + shell.GetDistance()) * (float)Math.Sin(num12);
                            float num14 = ((float)num5 + shell.GetDistance()) * (float)Math.Cos(num12);
                            float num15 = (float)Math.Cos(num9) * ((float)num5 + shell.GetDistance()) * (float)Math.Sin(num12);
                            enumerator2 = current.Effects.GetEnumerator();
                            try
                            {
                                while (enumerator2.MoveNext())
                                {
                                    BasicEffect val2 = (BasicEffect)enumerator2.Current;
                                    val2.EnableDefaultLighting();
                                    val2.World = array[current.ParentBone.Index] * Matrix.CreateTranslation(num13, num14, num15) * Matrix.CreateScale(5f);
                                    val2.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                                    val2.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), aspectRatio, 1f, 10000f);
                                    val3 = Color.Green;
                                    val2.AmbientLightColor = val3.ToVector3();
                                }
                            }
                            finally
                            {
                                enumerator2.Dispose();
                            }
                            current.Draw();
                        }
                    }
                    finally
                    {
                        enumerator.Dispose();
                    }
                }
            }
            base.Draw(gameTime);
			fSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			fSpriteBatch.Draw(fTopLeftCornerTexture, new Rectangle(0, 0, 325, 135), Color.White);
            float x = fConfigurationFont.MeasureString("" + fAtom.GetNucleus().GetProtons().Count).X;
            float x2 = fConfigurationFont.MeasureString("" + fAtom.GetNucleus().GetNeutrons().Count).X;
            float x3 = fConfigurationFont.MeasureString("" + fAtom.GetElectronCount()).X;
            fSpriteBatch.DrawString(fConfigurationFont, "" + fAtom.GetNucleus().GetProtons().Count, new Vector2(50f - x, 10f), Color.Red);
            fSpriteBatch.DrawString(fConfigurationFont, "Protons", new Vector2(55f, 10f), Color.Red);
            fSpriteBatch.DrawString(fConfigurationFont, "" + fAtom.GetNucleus().GetNeutrons().Count, new Vector2(50f - x2, 40f), Color.Blue);
            fSpriteBatch.DrawString(fConfigurationFont, "Neutrons", new Vector2(55f, 40f), Color.Blue);
            fSpriteBatch.DrawString(fConfigurationFont, "" + fAtom.GetElectronCount(), new Vector2(50f - x3, 70f), Color.Green);
            fSpriteBatch.DrawString(fConfigurationFont, "Electrons", new Vector2(55f, 70f), Color.Green);
            fSpriteBatch.Draw(fMinusButton, new Rectangle(185, 15, 20, 20), Color.White);
			fSpriteBatch.Draw(fAddButton, new Rectangle(210, 15, 20, 20), Color.White);
			fSpriteBatch.Draw(fMinusButton, new Rectangle(185, 45, 20, 20), Color.White);
			fSpriteBatch.Draw(fAddButton, new Rectangle(210, 45, 20, 20), Color.White);
			fSpriteBatch.Draw(fMinusButton, new Rectangle(185, 75, 20, 20), Color.White);
			fSpriteBatch.Draw(fAddButton, new Rectangle(210, 75, 20, 20), Color.White);
			Texture2D val7 = fUnknownElementGroupBackground;
			if (fCurrentIsotope != null)
			{
				if (fCurrentIsotope.Element.Classification == Element.ClassActinideSeries)
				{
					val7 = fActinideSeriesBackground;
				}
				else if (fCurrentIsotope.Element.Classification == Element.ClassAlkaliMetals)
				{
					val7 = fAlkaliMetalsBackground;
				}
				else if (fCurrentIsotope.Element.Classification == Element.ClassAlkalineEarthMetals)
				{
					val7 = fAlkalineEarthMetalsBackground;
				}
				else if (fCurrentIsotope.Element.Classification == Element.ClassLanthanideSeries)
				{
					val7 = fLanthanideSeriesBackground;
				}
				else if (fCurrentIsotope.Element.Classification == Element.ClassNobleGases)
				{
					val7 = fNobleGasesBackground;
				}
				else if (fCurrentIsotope.Element.Classification == Element.ClassNonmetals)
				{
					val7 = fNonmetalsBackground;
				}
				else if (fCurrentIsotope.Element.Classification == Element.ClassPoorMetals)
				{
					val7 = fPoorMetalsBackground;
				}
				else if (fCurrentIsotope.Element.Classification == Element.ClassTransitionMetals)
				{
					val7 = fTransitionMetalsBackground;
				}
			}
		    fSpriteBatch.Draw(val7, new Rectangle(viewport.Width - fElementOffset, 0, 250, 250), Color.White);
			if (fCurrentIsotope != null)
			{
                Vector2 val8 = fElementSymbolFont.MeasureString(fCurrentIsotope.Symbol);
                Vector2 val9 = fElementNameFont.MeasureString(fCurrentIsotope.Name());
                //Vector2 val10 = fElementNameFont.MeasureString("" + (int)Math.Round(fCurrentIsotope.RelativeAtomicMass));
                string text = "" + fCurrentIsotope.Element.AtomicNumber;
                fSpriteBatch.DrawString(fAtomicNumberFont, text, new Vector2(viewport.Width - fElementOffset + (150f - val9.X / 2f) + 2f, 12f), Color.White);
                string text2 = "" + fCurrentIsotope.Element.AtomicNumber;
                fSpriteBatch.DrawString(fAtomicNumberFont, text2, new Vector2(viewport.Width - fElementOffset + (150f - val9.X / 2f), 10f), Color.Black);
                Color val11 = Color.Black;
                if ((fCurrentIsotope.Element.Attributes & Element.StateGas) == Element.StateGas)
                {
                    val11 = Color.DarkGreen;
                }
                if ((fCurrentIsotope.Element.Attributes & Element.StateLiquid) == Element.StateLiquid)
                {
                    val11 = Color.DarkRed;
                }
                if ((fCurrentIsotope.Element.Attributes & Element.AttrSynthetic) == Element.AttrSynthetic)
                {
                    val11 = Color.LightGray;
                }
                string symbol = fCurrentIsotope.Symbol;
				string text3 = fCurrentIsotope.Name();
                fSpriteBatch.DrawString(fElementSymbolFont, symbol, new Vector2(viewport.Width - fElementOffset + (150f - val8.X / 2f) + 2f, 52f), Color.White);
                fSpriteBatch.DrawString(fElementSymbolFont, symbol, new Vector2(viewport.Width - fElementOffset + (150f - val8.X / 2f), 50f), val11);
                fSpriteBatch.DrawString(fElementNameFont, text3, new Vector2(viewport.Width - fElementOffset + (150f - val9.X / 2f) + 2f, 117f), Color.White);
                fSpriteBatch.DrawString(fElementNameFont, text3, new Vector2(viewport.Width - fElementOffset + (150f - val9.X / 2f), 115f), Color.Black);
                double a = Math.Round(fCurrentIsotope.RelativeAtomicMass * 1000.0) / 1000.0;
                string text5 = "" + a;
                fSpriteBatch.DrawString(fElementNameFont, text5, new Vector2(viewport.Width - fElementOffset + (150f - val9.X / 2f) + 2f, 137f), Color.White);
                string text6 = "" + a;
                fSpriteBatch.DrawString(fElementNameFont, text6, new Vector2(viewport.Width - fElementOffset + (150f - val9.X / 2f), 135f), Color.Black);
            }
			MouseState state = Mouse.GetState();
			fSpriteBatch.Draw(fPencilTexture, new Rectangle(state.X, state.Y - 27, 27, 27), Color.White);
			fSpriteBatch.End();
		}

		private List<Vector3> CalculateNucleusPositions(int pCount, float pSize, float pOverlap)
		{
			List<Vector3> list = new List<Vector3>();
			float num = 0f;
			float num2 = pSize - pOverlap;
			Vector3 item;
			while (list.Count < pCount)
			{
				list = new List<Vector3>();
				num += num2 / 1000f;
				float num3 = num * 2f;
				float num4 = -num3;
				float num5 = -num3;
				float num6 = -num3;
				float num7 = 0f;
				float num8 = 0f;
				while (num6 < num3)
				{
					item = new Vector3(num4, num5, num6);
					if (item.Length() < num3)
					{
						list.Add(item);
					}
					num4 += num2;
					if (num4 > num3)
					{
						num7 = (num7 != 0f) ? 0f : (num2 / 2f);
						num4 = -num3 - num7;
						num5 += num2;
					}
					if (num5 > num3)
					{
						num8 = (num8 != 0f) ? 0f : (num2 / 2f);
						num5 = -num3 - num8;
						num6 += num2;
					}
				}
			}
			list.Sort(new VectorComparer());
			return list;
		}
	}
}

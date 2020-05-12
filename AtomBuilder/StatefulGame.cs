using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AtomBuilder
{
	public class StatefulGame : Game
	{
		public GraphicsDeviceManager graphics;

		private string fGameStateName;

		private List<GameState> fGameStates;

		public StatefulGame()
		{
			graphics = (GraphicsDeviceManager)(object)new GraphicsDeviceManager((Game)(object)this);
			base.Content.RootDirectory = "Content";
		}

		protected virtual List<GameState> LoadGameStates()
		{
			return new List<GameState>();
		}

		private List<GameState> GetAllGameStates()
		{
			if (fGameStates == null)
			{
				fGameStates = LoadGameStates();
			}
			return fGameStates;
		}

		public string GetCurrentGameStateName()
		{
			return fGameStateName;
		}

		public void SetCurrentGameState(string pName)
		{
			fGameStateName = pName;
		}

		public GameState GetCurrentGameState()
		{
			foreach (GameState allGameState in GetAllGameStates())
			{
				if (allGameState.Name.Equals(GetCurrentGameStateName()))
				{
					return allGameState;
				}
			}
			return null;
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			base.LoadContent();
			foreach (GameState allGameState in GetAllGameStates())
			{
				allGameState.LoadContent();
			}
		}

		protected override void UnloadContent()
		{
			foreach (GameState allGameState in GetAllGameStates())
			{
				allGameState.UnloadContent();
			}
			base.UnloadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			if (GetCurrentGameState() != null)
			{
				GetCurrentGameState().Update(gameTime);
			}
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (GetCurrentGameState() != null)
			{
				GetCurrentGameState().Draw(gameTime);
			}
			base.Draw(gameTime);
		}
	}
}

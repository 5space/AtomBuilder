using System;
using Microsoft.Xna.Framework;

namespace AtomBuilder
{
	public class GameState
	{
		public readonly StatefulGame ParentGame;

		public readonly GraphicsDeviceManager graphics;

		public readonly string Name;

		public GameState(string pName, StatefulGame pGame, GraphicsDeviceManager pGraphics)
		{
			ParentGame = pGame;
			graphics = pGraphics;
			Name = pName;
		}

		public virtual void LoadContent()
		{
		}

		public virtual void UnloadContent()
		{
		}

		public virtual void Draw(GameTime pGameTime)
		{
		}

		public virtual void Update(GameTime pGameTime)
		{
		}
	}
}

using System.Collections.Generic;

namespace AtomBuilder
{
	public class AtomBuilderGame : StatefulGame
	{
		public AtomBuilderGame()
		{
			SetCurrentGameState("Workshop");
		}

		protected override List<GameState> LoadGameStates()
		{
			List<GameState> list = new List<GameState>();
			list.Add(new WorkshopState("Workshop", this, graphics));
			return list;
		}
	}
}

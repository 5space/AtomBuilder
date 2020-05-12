using System;

namespace AtomBuilder
{
	internal class Proton
	{
		private Nucleus fNucleus;

		public Proton(Nucleus pNucleus)
		{
			fNucleus = pNucleus;
		}

		public Nucleus GetNucleus()
		{
			return fNucleus;
		}
	}
}

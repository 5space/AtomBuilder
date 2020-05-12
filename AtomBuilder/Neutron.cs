using System;

namespace AtomBuilder
{
	internal class Neutron
	{
		private Nucleus fParent;

		public Neutron(Nucleus pNucleus)
		{
			fParent = pNucleus;
		}

		public Nucleus GetNucleus()
		{
			return fParent;
		}
	}
}

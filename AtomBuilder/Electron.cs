using System;

namespace AtomBuilder
{
	internal class Electron
	{
		private ElectronShell fShell;

		public Electron(ElectronShell pShell)
		{
			fShell = pShell;
		}

		public ElectronShell GetShell()
		{
			return fShell;
		}

		public int GetOffsetDegree()
		{
			int num = 360 / GetShell().GetElectrons().Count;
			return num / 2 + GetShell().GetElectrons().IndexOf(this) * num;
		}
	}
}

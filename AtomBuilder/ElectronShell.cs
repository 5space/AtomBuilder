using System.Collections.Generic;
namespace AtomBuilder
{
	internal class ElectronShell
	{
		private Atom fAtom;

		private List<Electron> fElectrons = new List<Electron>();

		public ElectronShell(Atom pAtom)
		{
			fAtom = pAtom;
		}

		public float GetDistance()
		{
			return (GetAtom().GetShells().IndexOf(this) + 1) * 25;
		}

		public Atom GetAtom()
		{
			return fAtom;
		}

		public void SetElectronCount(int pElectrons)
		{
			while (GetElectronCount() < pElectrons)
			{
				AddElectron();
			}
			while (GetElectronCount() > pElectrons)
			{
				RemoveElectron();
			}
		}

		public int GetElectronCount()
		{
			return fElectrons.Count;
		}

		public void AddElectron()
		{
			Electron item = new Electron(this);
			fElectrons.Add(item);
		}

		public void RemoveElectron()
		{
			if (fElectrons.Count > 0)
			{
				fElectrons.RemoveAt(0);
			}
		}

		public List<Electron> GetElectrons()
		{
			return fElectrons;
		}
	}
}

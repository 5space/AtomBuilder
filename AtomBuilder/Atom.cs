using AtomBuilder.DomainModel;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AtomBuilder
{
	internal class Atom
	{
		public Vector3 Location = new Vector3(0f, 0f, 0f);

		private Nucleus fNucleus;

		private List<ElectronShell> fShells;

		public Isotope GetCurrentIsotope()
		{
			foreach (Isotope isotope in AtomLookup.GetIsotopes())
			{
				if (GetElectronCount() == isotope.GetElectronCount() && GetNucleus().GetNeutrons().Count == isotope.GetNeutronCount() && GetNucleus().GetProtons().Count == isotope.GetProtonCount())
				{
					return isotope;
				}
			}
			return null;
		}

		public int GetElectronCount()
		{
			int num = 0;
			foreach (ElectronShell shell in GetShells())
			{
				num += shell.GetElectrons().Count;
			}
			return num;
		}

		public int GetShellCount()
		{
			int i;
			for (i = 0; i < GetShells().Count && GetShells()[i].GetElectrons().Count > 0; i++)
			{
			}
			return i;
		}

		private void GetElectronCount(int pElectronCount)
		{
			foreach (ElectronShell shell in GetShells())
			{
				shell.SetElectronCount(0);
			}
			foreach (Isotope isotope in AtomLookup.GetIsotopes())
			{
				if (isotope.GetElectronCount() == pElectronCount)
				{
					for (int i = 0; i < isotope.Element.Electrons.Length; i++)
					{
						GetShells()[i].SetElectronCount(isotope.Element.Electrons[i]);
					}
				}
			}
		}

		public void RemoveElectron()
		{
			GetElectronCount(GetElectronCount() - 1);
		}

		public void AddElectron()
		{
			if (GetElectronCount() < AtomLookup.GetMaxElectrons())
			{
				GetElectronCount(GetElectronCount() + 1);
			}
		}

		public Nucleus GetNucleus()
		{
			if (fNucleus == null)
			{
				fNucleus = new Nucleus(this);
			}
			return fNucleus;
		}

		public List<ElectronShell> GetShells()
		{
			if (fShells == null)
			{
				fShells = new List<ElectronShell>();
				for (int i = 0; i < 10; i++)
				{
					ElectronShell item = new ElectronShell(this);
					fShells.Add(item);
				}
			}
			return fShells;
		}
	}
}

using AtomBuilder.DomainModel;
using System.Collections.Generic;

namespace AtomBuilder
{
	internal class Nucleus
	{
		private List<Proton> fProtons = new List<Proton>();

		private List<Neutron> fNeutrons = new List<Neutron>();

		private Atom fParent;

		public Nucleus(Atom pAtom)
		{
			fParent = pAtom;
		}

		public int GetSize()
		{
			return GetProtons().Count + GetNeutrons().Count;
		}

		public List<Proton> GetProtons()
		{
			return fProtons;
		}

		public List<Neutron> GetNeutrons()
		{
			return fNeutrons;
		}

		public void AddNeutron()
		{
			if (fNeutrons.Count < AtomLookup.GetMaxNeutrons())
			{
				fNeutrons.Add(new Neutron(this));
			}
		}

		public void RemoveNeutron()
		{
			if (fNeutrons.Count > 0)
			{
				fNeutrons.RemoveAt(0);
			}
		}

		public void AddProton()
		{
			if (fProtons.Count < AtomLookup.GetMaxProtons())
			{
				fProtons.Add(new Proton(this));
			}
		}

		public void RemoveProton()
		{
			if (fProtons.Count > 0)
			{
				fProtons.RemoveAt(0);
			}
		}

		public Atom GetAtom()
		{
			return fParent;
		}
	}
}

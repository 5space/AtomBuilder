using System.Collections.Generic;

namespace AtomBuilder.DomainModel
{
	internal class Element
	{
		private List<Isotope> fIsotopes;

		public static int StateGas = 1;

		public static int StateSolid = 2;

		public static int StateLiquid = 4;

		public static int AttrSynthetic = 8;

		public static int AttrRadioactive = 16;

		public static int ClassUnknown = 0;

		public static int ClassAlkaliMetals = 1;

		public static int ClassAlkalineEarthMetals = 2;

		public static int ClassTransitionMetals = 3;

		public static int ClassPoorMetals = 4;

		public static int ClassNonmetals = 5;

		public static int ClassNobleGases = 6;

		public static int ClassActinideSeries = 7;

		public static int ClassLanthanideSeries = 8;

		public readonly int AtomicNumber;

		public readonly string Symbol;

		public readonly string Name;

		public readonly int Group;

		public readonly int Classification;

		public readonly double StandardAtomicWeight;

		public readonly int Attributes;

		public readonly int[] Electrons;

		public Element(int pAtomicNumber, string pSymbol, string pName, int pGroup, int pClassification, double pStandardAtomicWeight, int pAttributes, int[] pElectrons)
		{
			AtomicNumber = pAtomicNumber;
			Symbol = pSymbol;
			Name = pName;
			Group = pGroup;
			Classification = pClassification;
			StandardAtomicWeight = pStandardAtomicWeight;
			Attributes = pAttributes;
			Electrons = pElectrons;
		}

		public Isotope GetCommonIsotope()
		{
			Isotope isotope = null;
			foreach (Isotope isotope2 in GetIsotopes())
			{
				if (isotope2.RelativeAtomicMass > 0.0)
				{
					if (isotope == null || isotope2.Abundance > isotope.Abundance)
					{
						isotope = isotope2;
					}
				}
			}
			return isotope;
		}

		public List<Isotope> GetIsotopes()
		{
			if (fIsotopes == null)
			{
				fIsotopes = new List<Isotope>();
				foreach (Isotope isotope in AtomLookup.GetIsotopes())
				{
					if (isotope.Element.AtomicNumber == AtomicNumber)
					{
						fIsotopes.Add(isotope);
					}
				}
			}
			return fIsotopes;
		}

		public int GetElectronCount()
		{
			int num = 0;
			for (int i = 0; i < Electrons.Length; i++)
			{
				num += Electrons[i];
			}
			return num;
		}
	}
}
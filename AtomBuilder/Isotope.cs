using System;
namespace AtomBuilder.DomainModel
{
	internal class Isotope
	{
		private string fName;

		public readonly Element Element;

		public readonly string Symbol;

		public readonly double RelativeAtomicMass;

		public readonly double Abundance;

		public readonly int Attributes;

		public Isotope(Element pElement, string pSymbol, string pName, double pRelativeAtomicMass, double pAbundance, int pAttributes)
		{
			Element = pElement;
			if (pSymbol == null || pSymbol.Length == 0)
			{
				pSymbol = pElement.Symbol;
			}
			Symbol = pSymbol;
			fName = pName;
			RelativeAtomicMass = pRelativeAtomicMass;
			Abundance = pAbundance;
			Attributes = pAttributes;
		}

		public string Name()
		{
			if (fName == null || fName.Length == 0)
			{
				if (Element.GetCommonIsotope() == this)
				{
					fName = Element.Name;
				}
				else
				{
					fName = Element.Name + "-" + Math.Round(RelativeAtomicMass);
				}
			}
			return fName;
		}

		public int GetProtonCount()
		{
			return Element.AtomicNumber;
		}

		public int GetNeutronCount()
		{
			return (int)Math.Round(RelativeAtomicMass) - GetProtonCount();
		}

		public int GetElectronCount()
		{
			return Element.GetElectronCount();
		}

		public bool IsRadioactive()
		{
			if ((Attributes & Element.AttrRadioactive) == Element.AttrRadioactive)
			{
				return true;
			}
			if ((Element.Attributes & Element.AttrRadioactive) == Element.AttrRadioactive)
			{
				return true;
			}
			return false;
		}
	}
}
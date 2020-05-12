using System;
using System.Collections.Generic;
using System.IO;

namespace AtomBuilder.DomainModel
{
	internal class AtomLookup
	{
		private static int fMaxProtons = -1;

		private static int fMaxNeutrons = -1;

		private static int fMaxElectrons = -1;

		private static List<Element> fElements;

		private static List<Isotope> fIsotopes;

		public static List<Element> GetElements()
		{
			if (fElements == null)
			{
				InitializeElements();
			}
			return fElements;
		}

		public static List<Isotope> GetIsotopes()
		{
			if (fIsotopes == null)
			{
				InitializeIsotopes();
			}
			return fIsotopes;
		}

		public static int GetMaxProtons()
		{
			if (fMaxProtons == -1)
			{
				foreach (Isotope isotope in GetIsotopes())
				{
					fMaxProtons = Math.Max(fMaxProtons, isotope.GetProtonCount());
				}
			}
			return fMaxProtons;
		}

		public static int GetMaxNeutrons()
		{
			if (fMaxNeutrons == -1)
			{
				foreach (Isotope isotope in GetIsotopes())
				{
					fMaxNeutrons = Math.Max(fMaxNeutrons, isotope.GetNeutronCount());
				}
			}
			return fMaxNeutrons;
		}

		public static int GetMaxElectrons()
		{
			if (fMaxElectrons == -1)
			{
				foreach (Isotope isotope in GetIsotopes())
				{
					fMaxElectrons = Math.Max(fMaxElectrons, isotope.GetElectronCount());
				}
			}
			return fMaxElectrons;
		}

		public static Element FindElement(int pAtomicNumber)
		{
			foreach (Element element in GetElements())
			{
				if (element.AtomicNumber == pAtomicNumber)
				{
					return element;
				}
			}
			return null;
		}

		private static void InitializeElements()
		{
			fElements = new List<Element>();
			FileStream fileStream = new FileStream("Elements.txt", FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			streamReader.ReadLine();
			while (!streamReader.EndOfStream)
			{
				string text = streamReader.ReadLine();
				string[] array = text.Split(new char[1]
				{
					'\t'
				});
				if (array.Length <= 6)
				{
					continue;
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].Length > 0)
					{
						while (array[i].Substring(0, 1) == "\"")
						{
							array[i] = array[i].Substring(1);
						}
						while (array[i].EndsWith("\""))
						{
							array[i] = array[i].Substring(0, array[i].Length - 1);
						}
					}
				}
				string[] array2 = array[7].Split(new char[1]
				{
					','
				});
				int[] array3 = new int[array2.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array3[i] = Convert.ToInt32(array2[i]);
				}
				int pClassification = Element.ClassUnknown;
				if (array[4].ToUpper().StartsWith("ALKALI METALS"))
				{
					pClassification = Element.ClassAlkaliMetals;
				}
				else if (array[4].ToUpper().StartsWith("ALKALINE"))
				{
					pClassification = Element.ClassAlkalineEarthMetals;
				}
				else if (array[4].ToUpper().StartsWith("TRANSITION"))
				{
					pClassification = Element.ClassTransitionMetals;
				}
				else if (array[4].ToUpper().StartsWith("POOR"))
				{
					pClassification = Element.ClassPoorMetals;
				}
				else if (array[4].ToUpper().StartsWith("NONMETAL"))
				{
					pClassification = Element.ClassNonmetals;
				}
				else if (array[4].ToUpper().StartsWith("NOBLE"))
				{
					pClassification = Element.ClassNobleGases;
				}
				else if (array[4].ToUpper().StartsWith("ACTINIDE"))
				{
					pClassification = Element.ClassActinideSeries;
				}
				else if (array[4].ToUpper().StartsWith("LANTHANIDE"))
				{
					pClassification = Element.ClassLanthanideSeries;
				}
				if (array[3] == "")
				{
					array[3] = "-1";
				}
				int num = 0;
				if (array[6].IndexOf("g") > -1)
				{
					num |= Element.StateGas;
				}
				if (array[6].IndexOf("s") > -1)
				{
					num |= Element.StateSolid;
				}
				if (array[6].IndexOf("l") > -1)
				{
					num |= Element.StateLiquid;
				}
				if (array[6].IndexOf("r") > -1)
				{
					num |= Element.AttrRadioactive;
				}
				if (array[6].IndexOf("y") > -1)
				{
					num |= Element.AttrSynthetic;
				}
				Element item = new Element(Convert.ToInt32(array[0]), array[1], array[2], Convert.ToInt32(array[3]), pClassification, Convert.ToDouble(array[5]), num, array3);
				fElements.Add(item);
			}
			streamReader.Close();
			fileStream.Close();
		}

		private static void InitializeIsotopes()
		{
			fIsotopes = new List<Isotope>();
			FileStream fileStream = new FileStream("Isotopes.txt", FileMode.Open);
			StreamReader streamReader = new StreamReader(fileStream);
			streamReader.ReadLine();
			while (!streamReader.EndOfStream)
			{
				string text = streamReader.ReadLine();
				string[] array = text.Split(new char[1]
				{
					'\t'
				});
				if (array.Length <= 4)
				{
					continue;
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].Length > 0)
					{
						while (array[i].Substring(0, 1) == "\"")
						{
							array[i] = array[i].Substring(1);
						}
						while (array[i].EndsWith("\""))
						{
							array[i] = array[i].Substring(0, array[i].Length - 1);
						}
					}
				}
				Element pElement = FindElement(Convert.ToInt32(array[0]));
				Isotope item = new Isotope(pElement, array[1], array[2], Convert.ToDouble(array[3]), Convert.ToDouble(array[4]), 0);
				fIsotopes.Add(item);
			}
			streamReader.Close();
			fileStream.Close();
		}
	}
}

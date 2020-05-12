using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AtomBuilder
{
	internal class VectorComparer : Comparer<Vector3>
	{
		public override int Compare(Vector3 x, Vector3 y)
		{
			if (x.Length() < y.Length())
			{
				return -1;
			}
			if (x.Length() > y.Length())
			{
				return 1;
			}
			return 0;
		}
	}
}

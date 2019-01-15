using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Ex5
{
	public class PrivateKey
	{
		public int[] pi;
		public BigInteger m, w;
		public BigInteger[] b;
		public PrivateKey(int[] a,BigInteger x, BigInteger y, BigInteger[] z)
		{
			pi = a;
			m = x;
			w = y;
			b = z;
		}
	}
}

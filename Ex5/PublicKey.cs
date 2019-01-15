using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Ex5
{
	public class PublicKey
	{
		public BigInteger[] a;
		public PublicKey(BigInteger[] x)
		{
			a=x;
		}
	}
}

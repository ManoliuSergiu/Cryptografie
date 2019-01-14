using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex4
{
    class PublicKey
    {
		public BigInteger n;
		public BigInteger e;
		public PublicKey(BigInteger a, BigInteger b)
		{
			n = a;
			e = b;
		}
    }
}

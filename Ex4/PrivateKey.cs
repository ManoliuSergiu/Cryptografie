using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex4
{
    class PrivateKey
    {
		public BigInteger d;
		public PrivateKey(BigInteger a)
		{
			d = a;
		}
    }
}

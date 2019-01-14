using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Numerics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ex4
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		PublicKey publicKey;
		PrivateKey privateKey;
		BigInteger n;
		
		int b = 32;
		Random random = new Random();

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			statusTxt.Content = "Generating Keys...";
			await Task.Run(()=>Load());
			statusTxt.Content = "Done!";
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Load()
		{
			BigInteger p,q,phi;
			p = GenRandLargePrime();
			q = GenRandLargePrime();
			BigInteger e;
			n = p * q;
			phi = (p - 1) * (q - 1);
			int k = 0;
			do
			{
				k++;
				e = RandomIntegerBelow(phi, 1);
			} while(BigInteger.GreatestCommonDivisor(e,phi)!=1);

			bool a = e >= phi;
			//a = k == k;
			//a = BigInteger.GreatestCommonDivisor(e, phi) == 1;
			ExtEucl(e, phi, out BigInteger d, out BigInteger disp);
			a = BigInteger.Remainder((e*d-1),phi)==0;
			publicKey = new PublicKey(n, e);
			privateKey = new PrivateKey(d);
			if (!a)
				Load();
		}

		private BigInteger ExtEucl(BigInteger a, BigInteger b,out BigInteger x,out BigInteger y)
		{

			BigInteger x2 = 1, x1 = 0, y2 = 0, y1 = 1,q,r;
			while (b>0)
			{
				q = a / b;
				r = a - q * b;
				x = x2 - q * x1;
				y = y2 - q * y1;
				a = b;
				b = r;
				x2 = x1;
				x1 = x;
				y2 = 1;
				y1 = y;
			}
			x = (x2%n+n)%n;
			y = y2;
			return a;
		}

		private BigInteger GenRandLargePrime()
		{
			byte[] aux = new byte[b];
			while (true)
			{
				random.NextBytes(aux);
				if (aux[0] % 2 == 0) aux[0] += 1;
				BigInteger aux2 = new BigInteger(aux);
				aux2 = BigInteger.Abs(aux2);
				if (Fermat(aux2, 100))
				{
					return aux2;
				}
			}
			
		}

		

		private bool Fermat(BigInteger n, int t)
		{
			for (int i = 0; i < t; i++)
			{
				BigInteger a = RandomIntegerBelow(n,2);
				BigInteger r = BigInteger.ModPow(a, n - 1, n);
				if (r != 1)
					return false;
			}
			return true;
		}

		

		public BigInteger RandomIntegerBelow(BigInteger N,int t)
		{
			byte[] bytes = N.ToByteArray();
			BigInteger R;
			do
			{
				random.NextBytes(bytes);
				bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
				R = new BigInteger(bytes);
			} while (!(R < N - t && R > t));

			return R;
		}

		private async void Button_Click_1(object sender, RoutedEventArgs e)
		{
			statusTxt.Content = "Encrypting...";
			string a = tb.Text + "";
			List<BigInteger> cryptedMessage = await Task.Run(()=>Encrypt(a, publicKey));
			string aux = "";
			for (int i = 0; i < cryptedMessage.Count; i++)
			{
				
					aux += cryptedMessage[i]+"|";
				
				
			}
			tb.Text = aux;
			File.Delete("../../out.txt");
			statusTxt.Content = "Done!";

		}

		private List<BigInteger> Encrypt(string text, PublicKey p)
		{
			List<BigInteger> message = MessageNormalization(text);
			List<BigInteger> cryptedMessage = new List<BigInteger>();
			for (int i = 0; i < message.Count; i++)
			{
				BigInteger a = BigInteger.ModPow(message[i], p.e, p.n);
				cryptedMessage.Add(BigInteger.ModPow(message[i], p.e, p.n));
			}
			return cryptedMessage;
		}

		private List<BigInteger> MessageNormalization(string text)
		{
			byte[] b = Encoding.ASCII.GetBytes(text);
			List<BigInteger> message = new List<BigInteger>();
			BigInteger m = new BigInteger(b);
			byte[] prev = null;
			if (m > n)
			{
				int k = 0;
				for (int i = 0; i < b.Length; i++)
				{
					byte[] aux = new byte[i + 1 - k];
					for (int j = k; j < i ; j++)
					{
						aux[j - k] = b[j];
					}
					BigInteger x = new BigInteger(aux);
					if (x > n)
					{
						k = i-1;
						message.Add(new BigInteger(prev));
					}
					if (i == b.Length - 1) message.Add(x);
					else prev = aux;
				}
			}
			else message.Add(m);
			return message;
		}

		private async void Button_Click_2(object sender, RoutedEventArgs e)
		{
			statusTxt.Content = "Decrypting...";
			string a = tb.Text + "";
			string mesaj = await Task.Run(() => Decrypt(a,privateKey));
			tb.Text = mesaj;
			statusTxt.Content = "Done!";
		}

		private string Decrypt(string text, PrivateKey p)
		{
			List<BigInteger> cryptedMessage = MessageNormalization2(text);
			List<BigInteger> decryptedMessage1 = new List<BigInteger>();
			string decryptedMessage = "";
			for (int i = 0; i < cryptedMessage.Count; i++)
			{
				BigInteger aux = BigInteger.ModPow(BigInteger.Abs(cryptedMessage[i]), p.d, n);
				decryptedMessage += Encoding.ASCII.GetString(aux.ToByteArray());
			}
			return decryptedMessage;
		}

		private List<BigInteger> MessageNormalization2(string text)
		{
			char[] split = { '|' };
			string[] aux = text.Split(split,StringSplitOptions.RemoveEmptyEntries);
			List<BigInteger> message = new List<BigInteger>();
			if (aux.Length > 1)
			{
				for (int i = 0; i < aux.Length; i++)
				{
					message.Add(BigInteger.Parse(aux[i]));
				}
			}
			else message.Add(BigInteger.Parse(aux[0]));
			return message;
		}
	}
	
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ex4;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ex5
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Random random = new Random();
		PublicKey publicKey;
		PrivateKey privateKey;
		int messageLength = 70;
		public MainWindow()
		{
			InitializeComponent();
			Load();
		}

		private void Load()
		{
			KeyGeneration();
		}

		private void KeyGeneration()
		{
			int n = messageLength;
			BigInteger m=0;
			BigInteger[] b = GetSuperincreasingSequence(n,out m);
			BigInteger w;
			do
			{
				w = RandomIntegerBelow(m, 1);
			} while (BigInteger.GreatestCommonDivisor(w,m)!=1);
			int[] pi = RandomIntegerPermutation(n);
			BigInteger[] a = Compute6(b, w, m,pi);
			publicKey = new PublicKey(a);
			privateKey = new PrivateKey(pi, m, w, b);
		}

		private BigInteger[] Compute6(BigInteger[] b, BigInteger w, BigInteger m,int[] pi)
		{
			BigInteger[] a = new BigInteger[b.Length];
			for (int i = 0; i < b.Length; i++)
			{
				a[i] = (w * b[pi[i]]) % m;
			}
			return a;
		}

		public BigInteger RandomIntegerBelow(BigInteger N, int t)
		{
			byte[] bytes = N.ToByteArray();
			BigInteger R;
			do
			{
				random.NextBytes(bytes);
				bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
				R = new BigInteger(bytes);
			} while (!(R <= N - t && R >= t));
			return R;
		}

		private int[] RandomIntegerPermutation(int n)
		{
			int[] aux = new int[n];
			for (int i = 0; i < n; i++)
				aux[i] = i;
			for (int i = 0; i < n; i++)
				for (int j = 0; j < n; j++)
				{
					int x = random.Next(n);
					int aux2 = aux[j];
					aux[j] = aux[x];
					aux[x] = aux2;
				}
			return aux;
		}

		private BigInteger[] GetSuperincreasingSequence(int n,out BigInteger m)
		{
			BigInteger[] rez = new BigInteger[n];
			rez[0] = random.Next(1,10);
			BigInteger sum = rez[0];
			for (int i = 1; i < n; i++)
			{
				rez[i] = rez[i - 1] * 2 + random.Next(1, 10);
				sum += rez[i];
			}
			m = sum + random.Next(1, 100);
			return rez;
		}

		private int[] Superincreasing(BigInteger[] b,BigInteger s)
		{
			int[] x = new int[b.Length];
			int i = b.Length - 1;
			while (i>=1)
			{
				if (s >= b[i])
				{
					x[i] = 1;
					s -= b[i];
				}
				else x[i] = 0;
				i--;
			}
			if (s == 0)
				return x;
			else
			{
				x[0] = -1;
				return x;
			}
				
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			statusTxt.Content = "Generating Keys...";
			await Task.Run(() => KeyGeneration());
			statusTxt.Content = "Done!";

		}

		private async void Button_Click_1(object sender, RoutedEventArgs e)
		{
			string a = tb.Text + "";
			var encryptedMessage = await Task.Run(()=> Encrypt(a));
			tb.Text = "";
			for (int i = 0; i < encryptedMessage.Count; i++)
			{
				tb.Text += encryptedMessage[i] + "|";
			}
		}

		private List<BigInteger> Encrypt(string a)
		{
			byte[] aux = Encoding.ASCII.GetBytes(a);
			int n = publicKey.a.Length;
			string aux2="";

			List<string> aux3 = new List<string>();
			for (int i = 0; i < aux.Length; i++)
			{
				string au = Convert.ToString(aux[i], 2);
				if(au.Length<7)
				{
					for (int j = 0; j < 7-au.Length; j++)
					{
						au = "0" + au;
					}
				}
				aux2 += au;
				if (aux2.Length== n)
				{
					aux3.Add(aux2);
					aux2 = "";
				}
				else if(i==aux.Length-1) aux3.Add(aux2+"");
				
			}
;

			List<BigInteger> encryptedMessage = new List<BigInteger>();
			for (int i = 0; i < aux3.Count; i++)
			{
				encryptedMessage.Add(0);
				for (int j = 0; j < n; j++)
				{
					if(j<aux3[i].Length)
					{
						encryptedMessage[i] += aux3[i][j] * publicKey.a[j];
					}
					
				}
			}
			return encryptedMessage;
		}

		private async void Button_Click_2(object sender, RoutedEventArgs e)
		{
			string a = tb.Text + "";
			var decryptedMessage = await Task.Run(() => Decrypt(a));
			tb.Text = decryptedMessage;
		}

		private string Decrypt(string a)
		{
			char[] split = { '|' };
			string[] aux = a.Split(split);
			string rez="";
			for (int i = 0; i < aux.Length; i++)
			{
				BigInteger d = BigInteger.Parse(aux[i]);
				d = BigInteger.Divide(d, privateKey.w)%privateKey.m;
				
				int[] r = Superincreasing(privateKey.b, d);
				string mes = "";
				for (int j = 0; j < r.Length; j++)
				{
					mes += r[privateKey.pi[j]];
				}
				byte[] x = new byte[messageLength / 7];
				int k = 0;
				for (int j = 0; j < mes.Length-7; j+=7)
					x[k++]=Convert.ToByte(mes.Substring(j, 7));
				rez += Encoding.ASCII.GetString(x);

			}
			return rez;


		}
	}
}

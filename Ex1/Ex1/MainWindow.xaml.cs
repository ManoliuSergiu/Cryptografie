using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Ex1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		int selected=-1;
		int[] MonoAlpha = { 4, 22, 5, 9, 7, 25, 10, 1, 17, 15, 8, 13, 2, 24, 11, 3, 6, 21, 20, 12, 16, 14, 18, 19, 0, 23 };
		public MainWindow()
		{
			InitializeComponent();
			Load();
			Output.Text = MonoAlpha.Length+" "+-1%26;
		}

		private void Load()
		{
			CezarNTextBox.Text = "";
			CezarNLabel.Visibility = Visibility.Hidden;
			CezarNTextBox.Visibility = Visibility.Hidden;
			PlayfairLabel.Visibility = Visibility.Hidden;
			PlayfairTextBox.Visibility = Visibility.Hidden;
			PolyTextBox.Visibility = Visibility.Hidden;
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selected = ETypesMenu.SelectedIndex;

			if (selected == 1)
			{
				CezarNLabel.Visibility = Visibility.Visible;
				CezarNTextBox.Visibility = Visibility.Visible;
				PlayfairLabel.Visibility = Visibility.Hidden;
				PlayfairTextBox.Visibility = Visibility.Hidden;
				PolyTextBox.Visibility = Visibility.Hidden;

			}
			else if(selected == 3)
			{
				PlayfairLabel.Visibility = Visibility.Visible;
				PlayfairTextBox.Visibility = Visibility.Visible;
				CezarNLabel.Visibility = Visibility.Hidden;
				CezarNTextBox.Visibility = Visibility.Hidden;
				PolyTextBox.Visibility = Visibility.Hidden;

			}
			else if (selected == 5)
			{
				PlayfairLabel.Visibility = Visibility.Visible;
				PolyTextBox.Visibility = Visibility.Visible;
				CezarNLabel.Visibility = Visibility.Hidden;
				CezarNTextBox.Visibility = Visibility.Hidden;
				PlayfairTextBox.Visibility = Visibility.Hidden;
			}
			else 
			{
				CezarNLabel.Visibility = Visibility.Hidden;
				CezarNTextBox.Visibility = Visibility.Hidden;
				PlayfairLabel.Visibility = Visibility.Hidden;
				PlayfairTextBox.Visibility = Visibility.Hidden;
				PolyTextBox.Visibility = Visibility.Hidden;
			}
		}

		private void buttonEncrypt_Click(object sender, RoutedEventArgs e)
		{
			if (selected == 0) Cezar(3);
			if (selected == 1)  
			{
				int n;
				if (int.TryParse(CezarNTextBox.Text, out n))
				{
					if (n < 26&&n>0)
						Cezar(n);
					else if(n >= 26)
						MessageBox.Show("N is greater than 26, it should be less than that");
					else
						MessageBox.Show("N is negative, it has to be positive");
				}
				else
					MessageBox.Show("Please input a number");
			
			}
			if (selected == 2) Cezar(13);
			if (selected == 3) Playfair();
			if (selected == 4) MonoAlphabetEncryption();
			if (selected == 5) PolyAlphabetEncryption();
		}


		private void buttonDecrypt_Click(object sender, RoutedEventArgs e)
		{
			if (selected == 0) Cezar(26 - 3);
			if (selected == 1)
			{
				int n;
				if (int.TryParse(CezarNTextBox.Text, out n))
				{
					if (n < 26 && n > 0)
						Cezar(26-n);
					else if (n >= 26)
						MessageBox.Show("N is greater than 26, it should be less than that");
					else
						MessageBox.Show("N is negative, it has to be positive");
				}
				else
					MessageBox.Show("Please input a number");
			}
			if (selected == 2) Cezar(13);
			if (selected == 3) PlayfairDe();
			if (selected == 4) MonoAlphabetDecryption();
			if (selected == 5) PolyAlphabetDecryption();

		}

		private void PolyAlphabetDecryption()
		{
			string aux = PolyTextBox.Text.ToUpper();
			int[] key = new int[aux.Length];
			for (int i = 0; i < PolyTextBox.Text.Length; i++)
			{
				key[i] = aux[i] - 65;
			}
			string sub = "";
			aux = Input.Text.ToUpper();
			for (int i = 0; i < aux.Length; i++)
			{
				if (char.IsLetter(aux[i]))
				{
					int a= aux[i] - 65 +(26- key[i % key.Length]);
					sub += (char)(65 +a%26);
				}
				else sub += aux[i];
			}
			Output.Text = sub;
		}

		private void PolyAlphabetEncryption()
		{
			string aux = PolyTextBox.Text.ToUpper();
			int[] key=new int[aux.Length];
			for (int i = 0; i < PolyTextBox.Text.Length; i++)
			{
				key[i] = aux[i]-65;
			}
			string sub = "";
			aux = Input.Text.ToUpper();
			for (int i = 0; i < aux.Length; i++)
			{
				if (char.IsLetter(aux[i]))
					sub += (char)(65 + (aux[i] - 65 + key[i % key.Length]) % 26);
				else sub += aux[i];
			}
			Output.Text = sub;
		}

		private void MonoAlphabetDecryption()
		{
			char[] sup = new char[Input.Text.Length];
			string aux = Input.Text.ToUpper();
			for (int i = 0; i < sup.Length; i++)
			{
				if (char.IsLetter(aux[i]))
				{
					char a='☺';
					for (int j = 0; j < 26; j++)
					{
						if (MonoAlpha[j] == aux[i] - 65)
						{
							a =(char)(65 + j);
						}
					}
					sup[i] = a;

				}
				else sup[i] = aux[i];
			}
			Output.Text = new string(sup);
		}

		private void MonoAlphabetEncryption()
		{
			char[] sup = new char[Input.Text.Length];
			string aux = Input.Text.ToUpper();
			for (int i = 0; i < sup.Length; i++)
			{
				if (char.IsLetter(aux[i]))
				{
					sup[i] = Convert.ToChar(65 + MonoAlpha[aux[i]-65]);

				}
				else sup[i] = aux[i];
			}
			Output.Text = new string(sup);
		}

		private void Playfair()
		{
			char[,] PF = CreateMatrix();
			string inp = ReadString();
			//Output.Text = inp;
			Output.Text = PlayfairEncryption(PF,inp);
		}

		private string PlayfairEncryption(char[,] PF, string inp)
		{
			char[] split = { ' ' };
			string[] aux = inp.Split(split, StringSplitOptions.RemoveEmptyEntries);
			string a = "";
			foreach (var item in aux)
			{
				string sub = "";
				int poz1 = GetPoint(item[0],PF);
				int poz2 = GetPoint(item[1],PF);
				if (poz1 / 5 != poz2 / 5 && poz1 % 5 != poz2 % 5)
				{
					sub = PF[poz1 / 5, poz2 % 5] + "" + PF[poz2 / 5, poz1 % 5];
				}
				else if(poz1/5==poz2/5)
				{
					sub = PF[poz1 / 5, (poz1%5==4)?0:(poz1%5+1)] + "" + PF[poz2 / 5, (poz2 % 5 == 4) ? 0 : (poz2 % 5+1)];
				}
				else if(poz1%5==poz2%5)
				{
					sub = PF[(poz1/5==4)?0:(poz1/5+1),poz1%5] + "" + PF[ (poz2 / 5 == 4) ? 0 : (poz2 / 5+1),poz2%5];
				}
				a += sub + " ";
			}
			return a;
		}

		private int GetPoint(char v,char[,] PF)
		{
			if (v == 'J') v = 'I';
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (PF[i, j] == v)
						return i * 5 + j;
				}
			}
			return 0;
		}

		private string ReadString()
		{
			string aux = Input.Text.ToUpper();
			char[] split = { ' ' };
			string[] aux2 = aux.Split(split, StringSplitOptions.RemoveEmptyEntries);
			aux = "";
			for (int i = 0; i < aux2.Length; i++)
			{
				aux += aux2[i];
			}
			List<string> au = new List<string>();
			for (int i = 0; i < aux.Length; i+=2)
			{
				string sub;
				if (i + 1 < aux.Length)
				{
					if (aux[i] == aux[i + 1])
					{
						sub = aux[i] + "X";
						i--;
					}
					else sub = aux[i] +""+ aux[i + 1] + "";
				}
				else
				{
					sub = aux[i] + "X";
				}
				au.Add(sub);
			}
			string a = "";
			for (int i = 0; i < au.Count; i++)
			{
				a += au[i] + " ";
			}
			return a;
		}

		private char[,] CreateMatrix()
		{
			char[,] PF = new char[5, 5];
			string aux = PlayfairTextBox.Text.ToUpper();
			char[] aux2 = new char[aux.Length];
			int len = aux.Length;
			int k = 0;
			for (int i = 0; i < aux.Length; i++)
			{
				bool ok = true;
				for (int j = 0; j < i; j++)
				{
					if (aux[i] == aux[j])
					{
						ok = false;
					}
				}
				if (ok)
					aux2[k++] = aux[i];
				else
					len--;
			}
			k = 0;
			char b = 'A';
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (k < len)
						PF[i, j] = ((aux[k] == 'J') ? 'I' : aux2[k++]);
					else
					{
						if (b == 'J') b++;
						bool ok = true;
						for (int l = 0; l <= i && ok; l++)
							for (int m = 0; m < 5 && ok; m++)
								if (l == i && j < m || l < i)
									if (PF[l, m] == b)
										ok = false;
						if (ok)
							PF[i, j] = b;
						else
						{
							if (j == 0)
							{
								i--;
								j = 4;
							}
							else j--;
						}
						b++;
					}
				}
			}
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					Debug.Write(PF[i, j]+" ");
				}
				Debug.WriteLine("");
			}
			return PF;
		}

		private void Cezar(int v)
		{
			char[] aux = new char[Input.Text.Length];
			string aux2 = Input.Text.ToUpper();
			for (int i = 0; i < Input.Text.Length; i++)
			{
				if (Char.IsLetter(aux2[i]))
					aux[i] = Convert.ToChar(65 + (aux2[i] - 65 + v) % 26);
				else
					aux[i] = aux2[i];
			}
			Output.Text = new string(aux);
		}

		private void PlayfairDe()
		{
			char[,] PF = CreateMatrix();
			string inp = ReadString();
			Output.Text = PlayfairDecryption(PF, inp);
		}

		private string PlayfairDecryption(char[,] PF, string inp)
		{
			char[] split = { ' ' };
			string[] aux = inp.Split(split, StringSplitOptions.RemoveEmptyEntries);
			string a = "";
			foreach (var item in aux)
			{
				string sub = "";
				int poz1 = GetPoint(item[0], PF);
				int poz2 = GetPoint(item[1], PF);
				if (poz1 / 5 != poz2 / 5 && poz1 % 5 != poz2 % 5)
				{
					sub = PF[poz1 / 5, poz2 % 5] + "" + PF[poz2 / 5, poz1 % 5];
				}
				else if (poz1 / 5 == poz2 / 5)
				{
					sub = PF[poz1 / 5, (poz1 % 5 == 0) ? 4 : (poz1 % 5 - 1)] + "" + PF[poz2 / 5, (poz2 % 5 == 0) ? 4 : (poz2 % 5 - 1)];
				}
				else if (poz1 % 5 == poz2 % 5)
				{
					sub = PF[(poz1 / 5 == 0) ? 4 : (poz1 / 5 - 1), poz1 % 5] + "" + PF[(poz2 / 5 == 0) ? 4 : (poz2 / 5 - 1), poz2 % 5];
				}
				a += sub + " ";
			}
			return a;
		}
	}
}

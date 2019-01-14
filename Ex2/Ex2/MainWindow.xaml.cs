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
using System.Security.Cryptography;
using System.IO;

namespace Ex2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public enum Alg
	{
		DES, Aes, RC2, Rijndael, TripleDes
	}
	public partial class MainWindow : Window
	{
		static Random rnd = new Random();
		byte[] Key;
		byte[] IV;
		bool ok=true;
		public MainWindow()
		{
			InitializeComponent();
			MainWindowLoad();

		}
		private void MainWindowLoad()
		{
			for (int i = 0; i < Enum.GetValues(typeof(Alg)).Length; i++)
			{
				comboBox1.Items.Add((Alg)i);
			}
			comboBox1.Text = comboBox1.Items[0].ToString();

			for (int i = 1; i <= Enum.GetValues(typeof(CipherMode)).Length; i++)
			{
				comboBox2.Items.Add((CipherMode)i);
			}
			comboBox2.Text = comboBox2.Items[0].ToString();

			for (int i = 1; i <= Enum.GetValues(typeof(PaddingMode)).Length; i++)
			{
				comboBox3.Items.Add((PaddingMode)i);
			}
			comboBox3.Text = comboBox3.Items[0].ToString();
			IV = new byte[16];
			OutputTB.Text = Convert.ToByte('a')+"";
		}
		private void Encrypt_Click(object sender, RoutedEventArgs e)
		{
			Alg alg = (Alg)Enum.Parse(typeof(Alg), comboBox1.Text);
			CipherMode cm = (CipherMode)Enum.Parse(typeof(CipherMode), comboBox2.Text);
			PaddingMode pm = (PaddingMode)Enum.Parse(typeof(PaddingMode), comboBox3.Text);
			GetPreReq();
			EncryptData(Key, IV, alg, cm, pm);
			
		}

		private void GetPreReq()
		{
			if (comboBox1.Text == "DES")
				Key = new byte[8];
			else
				Key = new byte[16];

			string buffer = ivTB.Text;
			char[] Separator = { ' ', ',', '.' };
			char[] bytes = ivTB.Text.ToCharArray();
			for (int i = 0; i < IV.Length; i++)
			{
				IV[i] = Convert.ToByte(bytes[i]);
			}
			buffer = keyTB.Text;
			bytes = keyTB.Text.ToCharArray();
			for (int i = 0; i < Key.Length; i++)
			{
				Key[i] = Convert.ToByte(bytes[i]);
			}
		}

		private void Decrypt_Click(object sender, RoutedEventArgs e)
		{
			Alg alg = (Alg)Enum.Parse(typeof(Alg), comboBox1.Text);
			CipherMode cm = (CipherMode)Enum.Parse(typeof(CipherMode), comboBox2.Text);
			PaddingMode pm = (PaddingMode)Enum.Parse(typeof(PaddingMode), comboBox3.Text);
			GetPreReq();
			DecryptData(Key, IV, alg, cm, pm);
		}

		private void DecryptData(byte[] key, byte[] iv, Alg SymAlg, CipherMode cm, PaddingMode pm)
		{
			if(ok) Task.Run(() => Warning());
			FileStream fin = new FileStream("../../out.txt", FileMode.OpenOrCreate, FileAccess.Read);
			//File.Create("../../in.txt");
			FileStream fout = new FileStream("../../in.txt", FileMode.OpenOrCreate, FileAccess.Write);
			byte[] bin = new byte[100];
			char[] ban = new char[100];
			long rdlen = 0;
			long totlen = InputTB.Text.Length;
			int len;
			try
			{
				SymmetricAlgorithm rijn = SymmetricAlgorithm.Create(SymAlg.ToString());
				rijn.Mode = cm;
				rijn.Padding = pm;
				CryptoStream cryptoStream;
				
					cryptoStream = new CryptoStream(fout, rijn.CreateDecryptor(key, iv), CryptoStreamMode.Write);
				while (rdlen < totlen)
				{
					len = fin.Read(bin, 0, 100);
					cryptoStream.Write(bin, 0, len);
					rdlen = rdlen + len;
				}

				cryptoStream.Close();
				fout.Close();
				fin.Close();
				OutputTB.Text = File.ReadAllText("../../in.txt");
				File.Delete("../../in.txt");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			finally
			{
				fout.Close();
				fin.Close();
				File.Delete("../../in.txt");
			}
		}

		private void Warning()
		{
			MessageBox.Show("If you didn't change the 'out.txt' file the program will decrypt the last encrypted message.");
			ok = false;
		}

		private void EncryptData(byte[] EnKey, byte[] EnIV, Alg SymAlg, CipherMode Cm, PaddingMode Pm)
		{

			File.Delete("../../out.txt");
			FileStream fout = new FileStream("../../out.txt", FileMode.OpenOrCreate, FileAccess.Write);
			StringReader stringReader = new StringReader(InputTB.Text);
			byte[] bin = new byte[100]; 
			char[] ban = new char[100];
			long rdlen = 0;              
			long totlen=InputTB.Text.Length;  
			int len;                  
			try
			{
				SymmetricAlgorithm rijn = SymmetricAlgorithm.Create(SymAlg.ToString()); 
				rijn.Mode = Cm;
				rijn.Padding = Pm;
				CryptoStream cryptoStream;
				
				cryptoStream = new CryptoStream(fout, rijn.CreateEncryptor(EnKey, EnIV), CryptoStreamMode.Write);
				
				while (rdlen < totlen)
				{
					len = stringReader.Read(ban,0,100);
					for (int i = 0; i < len; i++)
					{
						bin[i] = Convert.ToByte(ban[i]);
					}
					cryptoStream.Write(bin, 0, len);
					rdlen = rdlen + len;
				}

				cryptoStream.Close();
				fout.Close();
				OutputTB.Text = File.ReadAllText("../../out.txt");
				
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			finally
			{
				fout.Close();
				//File.Delete("../../out.txt");
			}
		}
	}
}

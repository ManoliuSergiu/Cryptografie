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

namespace Ex3
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	public enum Hashes
	{
		MD5, RIPEMD160, SHA1, SHA256, SHA384, SHA512
	}
	public partial class MainWindow : Window
	{
		Hashes hash;
		byte[] message;
		string sourcefile;
		FileStream fin;
		public MainWindow()
		{
			InitializeComponent();
			Load();
		}
		private void Load()
		{
			for (int i = 0; i < Enum.GetValues(typeof(Hashes)).Length; i++)
				HashTypesTB.Items.Add((Hashes)i);
			HashTypesTB.Text = HashTypesTB.Items[0].ToString();
			hash = 0; 
			
		}

		private void SourceButton_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
			openFileDialog1.DefaultExt = ".txt";
			openFileDialog1.Filter = "TXT Files (*.txt)|*.txt";
			bool? result = openFileDialog1.ShowDialog();
			if (result == true)
			{
				sourcefile = openFileDialog1.FileName;
				SourceTB.Text = sourcefile;
			}
		}

		private async void StartButton_Click(object sender, RoutedEventArgs e)
		{
			string h = await Task.Run(() => GetHash());
			Output.Text = h;
		}

		private string GetHash()
		{
			HashAlgorithm hashAlgorithm = Algorithm();
			try
			{
				fin = new FileStream(sourcefile, FileMode.Open, FileAccess.Read);
				fin.Position = 0;
				message = hashAlgorithm.ComputeHash(fin);
				fin.Close();
				StringBuilder sub = new StringBuilder(message.Length * 2);
				foreach (var item in message)
					sub.AppendFormat("{0:x2}", item);
				return sub.ToString();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			return "";
		}
		private void HashTypesTB_DropDownClosed(object sender, EventArgs e)
		{
			hash = (Hashes)HashTypesTB.SelectedIndex;
		}
		private HashAlgorithm Algorithm()
		{
			
			switch (hash)
			{
				case (Hashes.MD5):
					return MD5.Create();
				case (Hashes.RIPEMD160):
					return  RIPEMD160.Create();
				case (Hashes.SHA1):
					return  SHA1.Create();
				case (Hashes.SHA256):
					return  SHA256.Create();
				case (Hashes.SHA384):
					return  SHA384.Create();
				case (Hashes.SHA512):
					return  SHA512.Create();
				default:
					return MD5.Create();
			}
		}

		
	}
}

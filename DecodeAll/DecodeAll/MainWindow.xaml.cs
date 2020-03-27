using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace decode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string Key { get; set; } = "";
        public MainWindow()
        {
            InitializeComponent();
        }
        // Events clicks
        private void Button_Decode(object sender, RoutedEventArgs e)
        {
            Key = key_Simetric.Text;
            string date = Date_Decode.Text = Decrypt(dateEncode.Text);


        }
        private void Button_Decode_Async(object sender, RoutedEventArgs e)
        {

            System.Windows.MessageBox.Show("Remember to use your correct PRIVATE key.");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Private Key in xml (*.xml)|*.xml";


                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
                {
                    Stream tempStream;
                        if ( (tempStream = openFileDialog.OpenFile()) != null ) 
                        {
                            string xml = new StreamReader(tempStream).ReadToEnd();
                            byte[] encriptedData = Decrypt_Async(dateEncode.Text, xml);
                            Date_Decode.Text = Encoding.ASCII.GetString(encriptedData);
                        }
                }


            
        }

        //Methods of Decode

        public string Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;


                    using (var transform = tdes.CreateDecryptor())
                    {
                        try
                        {
                            byte[] cipherBytes = Convert.FromBase64String(cipher);
                            byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                            return UTF8Encoding.UTF8.GetString(bytes);
                        }
                        catch
                        {
                            String text = "incorrect";
                            var result = System.Windows.MessageBox.Show("Review your code inputed.");
                            return text;
                        }

                    }

                }
            }
        }
        public static byte[] Decrypt_Async(string date, string key)
        {
            
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024);
            RSA.FromXmlString(key);
            byte[] decryptData = RSA.Decrypt(Convert.FromBase64String(date), false);
            return decryptData;
        }



        //inputs

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void key_Simetric_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }
}


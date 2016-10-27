using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace MyRSA
{
    public partial class Form1 : Form
    {

        String publicKey, privateKey;
        UnicodeEncoding encoder = new UnicodeEncoding();

        public Form1()
        {
            RSACryptoServiceProvider myRSA = new RSACryptoServiceProvider();
            InitializeComponent();

            privateKey = myRSA.ToXmlString(true);
            publicKey = myRSA.ToXmlString(false);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPlainText.Text = "";
            txtPlainText.Refresh();
        }

        private void btnClear2_Click(object sender, EventArgs e)
        {
            txtCypherText.Text = "";
            txtCypherText.Refresh();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            var myRSA = new RSACryptoServiceProvider();
            myRSA.FromXmlString(publicKey);

            
            var sb = new StringBuilder();

            var dataToEncrypt = encoder.GetBytes(txtPlainText.Text);
            int data_length = dataToEncrypt.Length;

            for (int k = 0; k < (int)Math.Ceiling((Double)data_length / (Double)70); k++)
            {
                var temp_bytes = dataToEncrypt.Skip(k * 70).Take(70).ToArray();
                var encryptedByteArray = myRSA.Encrypt(temp_bytes, false).ToArray();

                var length = encryptedByteArray.Count();

                var item = 0;

                foreach (var x in encryptedByteArray)
                {
                    item++;
                    sb.Append(x);
                    if (item >= length)
                    {
                        sb.Append("-");
                        item = 0;
                    }
                    else
                    {
                        sb.Append(" ");
                    }
                }

            }

            txtCypherText.Text += sb.ToString();

        }


        private void btnDecrypt_Click(object sender, EventArgs e)
        {

            var myRSA = new RSACryptoServiceProvider();
            myRSA.FromXmlString(privateKey);

            var dataChunks = txtCypherText.Text.Split(new char[] { '-' });
            var numChunks = dataChunks.Length;

            StringBuilder sb = new StringBuilder();

            for (int chnk = 0; chnk < numChunks; chnk++)
            {
                

                var chunk = dataChunks[chnk].Split(' ');
                var chunk_length = chunk.Length;

                // Things break without this conditional. 
                if (chunk[0] != "")
                {
                    byte[] dataBytes = new Byte[chunk_length];

                    for (int i = 0; i < dataBytes.Length; i++)
                    {
                        dataBytes[i] = Convert.ToByte(chunk[i]);
                    }

                    var decryptedBytes = myRSA.Decrypt(dataBytes, false);
                    sb.Append(encoder.GetString(decryptedBytes));
                }

            }

            txtPlainText.Text = sb.ToString();

        }
    }
}

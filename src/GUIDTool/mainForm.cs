using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Security.Cryptography;
using System.Text;

using System.Windows.Forms;
using Microsoft.Win32;

namespace GUIDTool
{
    public partial class mainForm : Form
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string keyHexToKey(byte[] keyHex)
        {
            string arg = "";
            string text = "0123456789ABCDEFGHJKLMNPRSTVWXYZ";
            for (int i = 0; i < 3; i++)
            {
                ulong num = 0uL;
                for (int j = 0; j < 5; j++)
                {
                    num <<= 8;
                    num |= (ulong)keyHex[i * 5 + j];
                }
                for (int j = 0; j < 8; j++)
                {
                    ulong num2 = num >> j * 5 & 31uL;
                    char c = text[(int)num2];
                    arg += c;
                }
            }
            return arg;
        }
        private string MD5Hex(string password)
        {
            MD5 mD = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(password);
            byte[] array = mD.ComputeHash(bytes);
            string str = "";
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                str += string.Format("{0:x2}", b);
            }
            return str;
        }

        private void EnvironmentThread_Key()
        {
            try
            {
                byte[] keyHex = (byte[])Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Bohemia Interactive Studio\\ArmA 2 OA").GetValue("key");
                string text = keyHexToKey(keyHex);
                string password = string.Concat(new string[]
            {
                    text.Substring(0, 4),
                    "-",
                    text.Substring(4, 5),
                    "-",
                    text.Substring(9, 5),
                    "-",
                    text.Substring(14, 5),
                    "-",
                    text.Substring(19, 5)
            });

                outputBox.Text = MD5Hex("BE" + MD5Hex(password));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public mainForm()
        {
            InitializeComponent();

            EnvironmentThread_Key();




        }

        private void mainForm_Load(object sender, EventArgs e)
        {

        }
    }
}

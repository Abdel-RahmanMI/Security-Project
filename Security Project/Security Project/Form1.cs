using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Security_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Boolean SDes = false;
        public Boolean RC4 = false;
        public Boolean PlayFair = false;
        public Boolean SAES = false;
        public Boolean RSA = false;
        public Boolean flagDecryption = false;
        public Boolean PFstat = false;
        /// <summary>
        /// ENCRYPT
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (SDes)
            {
                if (textBox1.Text.Length == 8 && textBox2.Text.Length == 10)
                {
                    S_DESS_Encrypt();
                }
                else
                {
                    MessageBox.Show("The PlainText OR Key entered is not Valid !");
                }
            }
            
            if (RSA)
            {
                flagDecryption = false;
                RSA_();
            }
            if (PlayFair)
            {
                PFstat = true;
                PLAYFAIR();
            }
        }
        void S_DESS_Encrypt()
        {
            ////// STEP 0
            ///

            int[,] S0 = new int[4, 4] { { 1, 0, 3, 2 },
                                        { 3, 2, 1, 0 },
                                        { 0, 2, 1, 3 },
                                        { 3, 1, 3, 2 }
                                        };

            int[,] S1 = new int[4, 4] { { 0, 1, 2, 3 },
                                        { 2, 0, 1, 3 },
                                        { 3, 0, 1, 0 },
                                        { 2, 1, 0, 3 }
                                        };

            int[] P10 = { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };
            int[] P8 = { 6, 3, 7, 4, 8, 5, 10, 9 };
            int[] P4 = { 2, 4, 3, 1 };

            int[] IP = { 2, 6, 3, 1, 4, 8, 5, 7 };
            int[] IPInv = { 4, 1, 3, 5, 7, 2, 8, 6 };

            int[] EP = { 4, 1, 2, 3, 2, 3, 4, 1 };

            int[] K1 = new int[10];
            int[] K2 = new int[10];

            int[] Key1 = new int[8];
            int[] Key2 = new int[8];

            int[] IPPT = new int[8];

            int[] R1 = new int[8];
            int[] R2 = new int[8];

            int[] XOR = new int[8];
            int[] XOR4 = new int[4];

            int[] FinalResult = new int[8];


            Char[] PT = textBox1.Text.ToCharArray();
            int[] PlainText = new int[PT.Length];
            for (int i = 0; i < PT.Length; i++)
            {
                PlainText[i] = Convert.ToInt32(PT[i].ToString());
            }

            Char[] K = textBox2.Text.ToCharArray();
            int[] Key = new int[K.Length];
            for (int i = 0; i < K.Length; i++)
            {
                Key[i] = Convert.ToInt32(K[i].ToString());
            }

            ////// STEP 1
            ///Key Generation
            ///KEY ONE

            for (int i = 0; i < K1.Length; i++)
            {
                K1[i] = Key[P10[i] - 1];
            }

            int temp;
            temp = K1[0];
            for (int k = 0; k < 4; k++)
            {
                K1[k] = K1[k + 1];
            }
            K1[4] = temp;

            temp = K1[5];
            for (int k = 5; k < 9; k++)
            {
                K1[k] = K1[k + 1];
            }
            K1[9] = temp;

            for (int i = 0; i < P8.Length; i++)
            {
                Key1[i] = K1[P8[i] - 1];
            }

            ///KEY TWO

            for (int i = 0; i < K1.Length; i++)
                K2[i] = K1[i];

            for (int i = 0; i < 2; i++)
            {
                temp = K2[0];
                for (int k = 0; k < 4; k++)
                {
                    K2[k] = K2[k + 1];
                }
                K2[4] = temp;

                temp = K2[5];
                for (int k = 5; k < 9; k++)
                {
                    K2[k] = K2[k + 1];
                }
                K2[9] = temp;
            }

            for (int i = 0; i < P8.Length; i++)
            {
                Key2[i] = K2[P8[i] - 1];
            }


            ////// STEP 2
            ///IP(Plain text)
            ///

            for (int i = 0; i < PlainText.Length; i++)
            {
                IPPT[i] = PlainText[IP[i] - 1];
            }

            ////// STEP 3
            ///Round ONE
            ///

            int[] INPUT = new int[4];
            int[] OUTPUT = new int[4];
            for (int i = (IPPT.Length / 2); i < IPPT.Length; i++)
            {
                INPUT[i - 4] = IPPT[i];

            }

            for (int i = 0; i < R1.Length; i++)
            {
                R1[i] = INPUT[EP[i] - 1];
            }


            for (int i = 0; i < R1.Length; i++)
            {
                XOR[i] = R1[i];
            }

            for (int i = 0; i < R1.Length; i++)
            {
                if (XOR[i] != Key1[i])
                    R1[i] = 1;
                else
                    R1[i] = 0;
            }


            ////// S- BOXES
            ///s0
            string rr = "" + R1[0] + "" + R1[3];
            string cc = "" + R1[1] + "" + R1[2];
            int[] rc = new int[2];
            if (rr == "00")
                rc[0] = 0;
            if (rr == "01")
                rc[0] = 1;
            if (rr == "10")
                rc[0] = 2;
            if (rr == "11")
                rc[0] = 3;

            if (cc == "00")
                rc[1] = 0;
            if (cc == "11")
                rc[1] = 3;
            if (cc == "01")
                rc[1] = 1;
            if (cc == "10")
                rc[1] = 2;

            int res = S0[rc[0], rc[1]];

            if (res == 0)
            {
                INPUT[0] = 0;
                INPUT[1] = 0;
            }

            if (res == 1)
            {
                INPUT[0] = 0;
                INPUT[1] = 1;
            }

            if (res == 2)
            {
                INPUT[0] = 1;
                INPUT[1] = 0;
            }

            if (res == 3)
            {
                INPUT[0] = 1;
                INPUT[1] = 1;
            }

            ///s1
            rr = "" + R1[4] + "" + R1[7];
            cc = "" + R1[5] + "" + R1[6];


            if (rr == "00")
                rc[0] = 0;
            if (rr == "01")
                rc[0] = 1;
            if (rr == "10")
                rc[0] = 2;
            if (rr == "11")
                rc[0] = 3;

            if (cc == "00")
                rc[1] = 0;
            if (cc == "11")
                rc[1] = 3;
            if (cc == "01")
                rc[1] = 1;
            if (cc == "10")
                rc[1] = 2;



            res = S1[rc[0], rc[1]];


            if (res == 0)
            {
                INPUT[2] = 0;
                INPUT[3] = 0;
            }

            if (res == 1)
            {
                INPUT[2] = 0;
                INPUT[3] = 1;
            }

            if (res == 2)
            {
                INPUT[2] = 1;
                INPUT[3] = 0;
            }

            if (res == 3)
            {
                INPUT[2] = 1;
                INPUT[3] = 1;
            }

            for (int i = 0; i < P4.Length; i++)
            {
                XOR4[i] = INPUT[P4[i] - 1];
            }

            for (int i = 0; i < XOR4.Length; i++)
            {
                if (XOR4[i] != IPPT[i])
                    OUTPUT[i] = 1;
                else
                    OUTPUT[i] = 0;
            }

            for (int i = 0; i < R1.Length; i++)
            {
                if (i < 4)
                {
                    R1[i] = IPPT[i + 4];
                }
                else
                {
                    R1[i] = OUTPUT[i - 4];
                }
            }

            ///Round TWO
            ///
            for (int i = (R1.Length / 2); i < R1.Length; i++)
            {
                INPUT[i - 4] = R1[i];

            }
            for (int i = 0; i < R2.Length; i++)
            {
                R2[i] = INPUT[EP[i] - 1];
            }
            for (int i = 0; i < R2.Length; i++)
            {
                XOR[i] = R2[i];
            }
            for (int i = 0; i < R2.Length; i++)
            {
                if (XOR[i] != Key2[i])
                    R2[i] = 1;
                else
                    R2[i] = 0;
            }

            ////// S- BOXES
            ///s0
            ///
            rr = "" + R2[0] + "" + R2[3];
            cc = "" + R2[1] + "" + R2[2];
            rc = new int[2];
            if (rr == "00")
                rc[0] = 0;
            if (rr == "01")
                rc[0] = 1;
            if (rr == "10")
                rc[0] = 2;
            if (rr == "11")
                rc[0] = 3;

            if (cc == "00")
                rc[1] = 0;
            if (cc == "11")
                rc[1] = 3;
            if (cc == "01")
                rc[1] = 1;
            if (cc == "10")
                rc[1] = 2;

            res = S0[rc[0], rc[1]];

            if (res == 0)
            {
                INPUT[0] = 0;
                INPUT[1] = 0;
            }

            if (res == 1)
            {
                INPUT[0] = 0;
                INPUT[1] = 1;
            }

            if (res == 2)
            {
                INPUT[0] = 1;
                INPUT[1] = 0;
            }

            if (res == 3)
            {
                INPUT[0] = 1;
                INPUT[1] = 1;
            }
            ///s1
            rr = "" + R2[4] + "" + R2[7];
            cc = "" + R2[5] + "" + R2[6];


            if (rr == "00")
                rc[0] = 0;
            if (rr == "01")
                rc[0] = 1;
            if (rr == "10")
                rc[0] = 2;
            if (rr == "11")
                rc[0] = 3;

            if (cc == "00")
                rc[1] = 0;
            if (cc == "11")
                rc[1] = 3;
            if (cc == "01")
                rc[1] = 1;
            if (cc == "10")
                rc[1] = 2;



            res = S1[rc[0], rc[1]];


            if (res == 0)
            {
                INPUT[2] = 0;
                INPUT[3] = 0;
            }

            if (res == 1)
            {
                INPUT[2] = 0;
                INPUT[3] = 1;
            }

            if (res == 2)
            {
                INPUT[2] = 1;
                INPUT[3] = 0;
            }

            if (res == 3)
            {
                INPUT[2] = 1;
                INPUT[3] = 1;
            }
            for (int i = 0; i < P4.Length; i++)
            {
                XOR4[i] = INPUT[P4[i] - 1];
            }

            for (int i = 0; i < XOR4.Length; i++)
            {
                if (XOR4[i] != R1[i])
                    OUTPUT[i] = 1;
                else
                    OUTPUT[i] = 0;
            }
            for (int i = 0; i < R2.Length; i++)
            {
                if (i < 4)
                {
                    R2[i] = OUTPUT[i];
                }
                else
                {
                    R2[i] = R1[i];
                }
            }

            for (int i = 0; i < FinalResult.Length; i++)
            {
                FinalResult[i] = R2[IPInv[i] - 1];
            }

            string Finall = "";
            for (int i = 0; i < FinalResult.Length; i++)
                Finall += FinalResult[i];
            label4.Text = (Finall);
        }

        /// <summary>
        /// DYCRYPT
        /// </summary>
        /// 
        private void button2_Click(object sender, EventArgs e)
        {
            if (SDes)
            {
                //MessageBox.Show(textBox1.Text.Length.ToString());
                if (textBox1.Text.Length == 8 && textBox2.Text.Length == 10)
                {
                    S_DESS_Dycrypt();
                }
                else
                {
                    MessageBox.Show("The PlainText OR Key entered is not Valid !");
                }
            }
            
            if (RSA)
            {
                flagDecryption = true;
                RSA_();
            }

            if (PlayFair)
            {
                PFstat = false;
                PLAYFAIR();
            }
        }
        void S_DESS_Dycrypt()
        {
            ////// STEP 0
            ///

            int[,] S0 = new int[4, 4] { { 1, 0, 3, 2 },
                                        { 3, 2, 1, 0 },
                                        { 0, 2, 1, 3 },
                                        { 3, 1, 3, 2 }
                                        };

            int[,] S1 = new int[4, 4] { { 0, 1, 2, 3 },
                                        { 2, 0, 1, 3 },
                                        { 3, 0, 1, 0 },
                                        { 2, 1, 0, 3 }
                                        };

            int[] P10 = { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };
            int[] P8 = { 6, 3, 7, 4, 8, 5, 10, 9 };
            int[] P4 = { 2, 4, 3, 1 };

            int[] IP = { 2, 6, 3, 1, 4, 8, 5, 7 };
            int[] IPInv = { 4, 1, 3, 5, 7, 2, 8, 6 };

            int[] EP = { 4, 1, 2, 3, 2, 3, 4, 1 };

            int[] K1 = new int[10];
            int[] K2 = new int[10];

            int[] Key1 = new int[8];
            int[] Key2 = new int[8];

            int[] IPPT = new int[8];

            int[] R1 = new int[8];
            int[] R2 = new int[8];

            int[] XOR = new int[8];
            int[] XOR4 = new int[4];

            int[] FinalResult = new int[8];


            Char[] PT = textBox1.Text.ToCharArray();
            int[] PlainText = new int[PT.Length];
            for (int i = 0; i < PT.Length; i++)
            {
                PlainText[i] = Convert.ToInt32(PT[i].ToString());
            }

            Char[] K = textBox2.Text.ToCharArray();
            int[] Key = new int[K.Length];
            for (int i = 0; i < K.Length; i++)
            {
                Key[i] = Convert.ToInt32(K[i].ToString());
            }

            ////// STEP 1
            ///Key Generation
            ///KEY ONE

            for (int i = 0; i < K1.Length; i++)
            {
                K1[i] = Key[P10[i] - 1];
            }

            int temp;
            temp = K1[0];
            for (int k = 0; k < 4; k++)
            {
                K1[k] = K1[k + 1];
            }
            K1[4] = temp;

            temp = K1[5];
            for (int k = 5; k < 9; k++)
            {
                K1[k] = K1[k + 1];
            }
            K1[9] = temp;

            for (int i = 0; i < P8.Length; i++)
            {
                Key1[i] = K1[P8[i] - 1];
            }

            ///KEY TWO

            for (int i = 0; i < K1.Length; i++)
                K2[i] = K1[i];

            for (int i = 0; i < 2; i++)
            {
                temp = K2[0];
                for (int k = 0; k < 4; k++)
                {
                    K2[k] = K2[k + 1];
                }
                K2[4] = temp;

                temp = K2[5];
                for (int k = 5; k < 9; k++)
                {
                    K2[k] = K2[k + 1];
                }
                K2[9] = temp;
            }

            for (int i = 0; i < P8.Length; i++)
            {
                Key2[i] = K2[P8[i] - 1];
            }


            ////// STEP 2
            ///IP(Plain text)
            ///

            for (int i = 0; i < PlainText.Length; i++)
            {
                IPPT[i] = PlainText[IP[i] - 1];
            }

            ////// STEP 3
            ///Round ONE
            ///

            int[] INPUT = new int[4];
            int[] OUTPUT = new int[4];
            for (int i = (IPPT.Length / 2); i < IPPT.Length; i++)
            {
                INPUT[i - 4] = IPPT[i];

            }

            for (int i = 0; i < R1.Length; i++)
            {
                R1[i] = INPUT[EP[i] - 1];
            }


            for (int i = 0; i < R1.Length; i++)
            {
                XOR[i] = R1[i];
            }

            for (int i = 0; i < R1.Length; i++)
            {
                if (XOR[i] != Key2[i])
                    R1[i] = 1;
                else
                    R1[i] = 0;
            }

            ////// S- BOXES
            ///s0
            string rr = "" + R1[0] + "" + R1[3];
            string cc = "" + R1[1] + "" + R1[2];
            int[] rc = new int[2];
            if (rr == "00")
                rc[0] = 0;
            if (rr == "01")
                rc[0] = 1;
            if (rr == "10")
                rc[0] = 2;
            if (rr == "11")
                rc[0] = 3;

            if (cc == "00")
                rc[1] = 0;
            if (cc == "11")
                rc[1] = 3;
            if (cc == "01")
                rc[1] = 1;
            if (cc == "10")
                rc[1] = 2;

            int res = S0[rc[0], rc[1]];

            if (res == 0)
            {
                INPUT[0] = 0;
                INPUT[1] = 0;
            }

            if (res == 1)
            {
                INPUT[0] = 0;
                INPUT[1] = 1;
            }

            if (res == 2)
            {
                INPUT[0] = 1;
                INPUT[1] = 0;
            }

            if (res == 3)
            {
                INPUT[0] = 1;
                INPUT[1] = 1;
            }

            ///s1
            rr = "" + R1[4] + "" + R1[7];
            cc = "" + R1[5] + "" + R1[6];


            if (rr == "00")
                rc[0] = 0;
            if (rr == "01")
                rc[0] = 1;
            if (rr == "10")
                rc[0] = 2;
            if (rr == "11")
                rc[0] = 3;

            if (cc == "00")
                rc[1] = 0;
            if (cc == "11")
                rc[1] = 3;
            if (cc == "01")
                rc[1] = 1;
            if (cc == "10")
                rc[1] = 2;



            res = S1[rc[0], rc[1]];


            if (res == 0)
            {
                INPUT[2] = 0;
                INPUT[3] = 0;
            }

            if (res == 1)
            {
                INPUT[2] = 0;
                INPUT[3] = 1;
            }

            if (res == 2)
            {
                INPUT[2] = 1;
                INPUT[3] = 0;
            }

            if (res == 3)
            {
                INPUT[2] = 1;
                INPUT[3] = 1;
            }

            

            for (int i = 0; i < P4.Length; i++)
            {
                XOR4[i] = INPUT[P4[i] - 1];
            }

            

            for (int i = 0; i < XOR4.Length; i++)
            {
                if (XOR4[i] != IPPT[i])
                    OUTPUT[i] = 1;
                else
                    OUTPUT[i] = 0;
            }

            

            for (int i = 0; i < R1.Length; i++)
            {
                if (i < 4)
                {
                    R1[i] = IPPT[i + 4];
                }
                else
                {
                    R1[i] = OUTPUT[i - 4];
                }
            }
            ///Round TWO
            ///
            for (int i = (R1.Length / 2); i < R1.Length; i++)
            {
                INPUT[i - 4] = R1[i];

            }
            for (int i = 0; i < R2.Length; i++)
            {
                R2[i] = INPUT[EP[i] - 1];
            }
            for (int i = 0; i < R2.Length; i++)
            {
                XOR[i] = R2[i];
            }
            for (int i = 0; i < R2.Length; i++)
            {
                if (XOR[i] != Key1[i])
                    R2[i] = 1;
                else
                    R2[i] = 0;
            }

            
            ////// S- BOXES
            ///s0
            ///
            rr = "" + R2[0] + "" + R2[3];
            cc = "" + R2[1] + "" + R2[2];
            rc = new int[2];
            if (rr == "00")
                rc[0] = 0;
            if (rr == "01")
                rc[0] = 1;
            if (rr == "10")
                rc[0] = 2;
            if (rr == "11")
                rc[0] = 3;

            if (cc == "00")
                rc[1] = 0;
            if (cc == "11")
                rc[1] = 3;
            if (cc == "01")
                rc[1] = 1;
            if (cc == "10")
                rc[1] = 2;

            res = S0[rc[0], rc[1]];

            

            if (res == 0)
            {
                INPUT[0] = 0;
                INPUT[1] = 0;
            }

            if (res == 1)
            {
                INPUT[0] = 0;
                INPUT[1] = 1;
            }

            if (res == 2)
            {
                INPUT[0] = 1;
                INPUT[1] = 0;
            }

            if (res == 3)
            {
                INPUT[0] = 1;
                INPUT[1] = 1;
            }

            ///s1
            ///

            rr = "" + R2[4] + "" + R2[7];
            cc = "" + R2[5] + "" + R2[6];


            if (rr == "00")
                rc[0] = 0;
            if (rr == "01")
                rc[0] = 1;
            if (rr == "10")
                rc[0] = 2;
            if (rr == "11")
                rc[0] = 3;

            if (cc == "00")
                rc[1] = 0;
            if (cc == "11")
                rc[1] = 3;
            if (cc == "01")
                rc[1] = 1;
            if (cc == "10")
                rc[1] = 2;

            
            res = S1[rc[0], rc[1]];

            

            if (res == 0)
            {
                INPUT[2] = 0;
                INPUT[3] = 0;
            }

            if (res == 1)
            {
                INPUT[2] = 0;
                INPUT[3] = 1;
            }

            if (res == 2)
            {
                INPUT[2] = 1;
                INPUT[3] = 0;
            }

            if (res == 3)
            {
                INPUT[2] = 1;
                INPUT[3] = 1;
            }

            

            for (int i = 0; i < P4.Length; i++)
            {
                XOR4[i] = INPUT[P4[i] - 1];
            }

            for (int i = 0; i < XOR4.Length; i++)
            {
                if (XOR4[i] != R1[i])
                    OUTPUT[i] = 1;
                else
                    OUTPUT[i] = 0;
            }
            for (int i = 0; i < R2.Length; i++)
            {
                if (i < 4)
                {
                    R2[i] = OUTPUT[i];
                }
                else
                {
                    R2[i] = R1[i];
                }
            }

            for (int i = 0; i < FinalResult.Length; i++)
            {
                FinalResult[i] = R2[IPInv[i] - 1];
            }

            string Finall = "";
            for (int i = 0; i < FinalResult.Length; i++)
                Finall += FinalResult[i];
            label4.Text = (Finall);
        }

        private void SDES_Click(object sender, EventArgs e)
        {
            SDes = true;
            RC4 = false;
            SAES = false;
            PlayFair = false;
            RSA = false;
            label4.Text = (" ");
            label5.Text = (" ");
            label6.Text = (" ");
            label9.Text = (" ");
        }

       

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void button1_Click_1(object sender, EventArgs e)
        {
            RC4 = true;
            SDes = false;
            SAES = false;
            PlayFair = false;
            RSA = false;
            label4.Text = (" ");
            label5.Text = (" ");
            label6.Text = (" .");
            label9.Text = (" ");
            if (textBox1.Text.Length == 4 && textBox2.Text.Length == 4)
            {
                RC44();
            }
            else
            {
                MessageBox.Show("The PlainText OR Key entered is not Valid !");
            }
        }
        void RC44()
        {
            int[] S = { 0, 1, 2, 3, 4, 5, 6, 7 };
            int[] T = new int[8];

            Char[] PT = textBox1.Text.ToCharArray();
            int[] PlainText = new int[PT.Length];
            for (int i = 0; i < PT.Length; i++)
            {
                PlainText[i] = Convert.ToInt32(PT[i].ToString());
            }


            Char[] K1 = textBox2.Text.ToCharArray();
            int[] Key = new int[K1.Length];
            for (int i = 0; i < K1.Length; i++)
            {
                Key[i] = Convert.ToInt32(K1[i].ToString());
            }

            //string Final = "";
            //for (int i = 0; i < PlainText.Length; i++)
            //    Final += PlainText[i];
            //label4.Text = (Final);
            int k = 0;

            for (int i = 0; i < T.Length; i++)
            {
                T[i] = Key[k];
                k++;

                if (i == 3)
                    k = 0;
            }

            /// initial permutation ////

            int j = 0;
            int temp;

            for (int i = 0; i < 8; i++)
            {
                j = (j + S[i] + T[i]) % 8;
                temp = S[i];
                S[i] = S[j];
                S[j] = temp;

                //for ( k = 0; k < S.Length; k++)
                //    MessageBox.Show("i = "+ i + " , j = "+j+"S[" + k + "]" + "=" + " " + S[k]);
            }

            ///// Encryption ////

            j = 0;
            temp = 0;
            int t;
            k = 0;
            int[] C = new int[4];
            int[] K = new int[4];
            int[] kk = new int[3];
            int[] pp = new int[3];
            int[] cc = new int[3];

            for (int i = 0; i < 4;)
            {
                i = (i + 1) % 8;
                j = (j + S[i]) % 8;
                temp = S[i];
                S[i] = S[j];
                S[j] = temp;
                t = (S[i] + S[j]) % 8;
                k = S[t];

                //string fkme = "";
                //for (int l = 0; l < S.Length; l++)
                //{
                //    fkme += S[l] + " - ";
                //}
                //MessageBox.Show("i = " + i + " , j = " + j + " , S ="+ fkme+ " ,  k = "+ k +" , T = "+t);


                if (k == 0)
                {
                    kk[0] = 0;
                    kk[1] = 0;
                    kk[2] = 0;
                }
                if (k == 1)
                {
                    kk[0] = 0;
                    kk[1] = 0;
                    kk[2] = 1;
                }
                if (k == 2)
                {
                    kk[0] = 0;
                    kk[1] = 1;
                    kk[2] = 0;
                }
                if (k == 3)
                {
                    kk[0] = 0;
                    kk[1] = 1;
                    kk[2] = 1;
                }
                if (k == 4)
                {
                    kk[0] = 1;
                    kk[1] = 0;
                    kk[2] = 0;
                }
                if (k == 5)
                {
                    kk[0] = 1;
                    kk[1] = 0;
                    kk[2] = 1;
                }
                if (k == 6)
                {
                    kk[0] = 1;
                    kk[1] = 1;
                    kk[2] = 0;
                }
                if (k == 7)
                {
                    kk[0] = 1;
                    kk[1] = 1;
                    kk[2] = 1;
                }


                if (PlainText[i - 1] == 0)
                {
                    pp[0] = 0;
                    pp[1] = 0;
                    pp[2] = 0;
                }
                if (PlainText[i - 1] == 1)
                {
                    pp[0] = 0;
                    pp[1] = 0;
                    pp[2] = 1;
                }
                if (PlainText[i - 1] == 2)
                {
                    pp[0] = 0;
                    pp[1] = 1;
                    pp[2] = 0;
                }
                if (PlainText[i - 1] == 3)
                {
                    pp[0] = 0;
                    pp[1] = 1;
                    pp[2] = 1;
                }
                if (PlainText[i - 1] == 4)
                {
                    pp[0] = 1;
                    pp[1] = 0;
                    pp[2] = 0;
                }
                if (PlainText[i - 1] == 5)
                {
                    pp[0] = 1;
                    pp[1] = 0;
                    pp[2] = 1;
                }
                if (PlainText[i - 1] == 6)
                {
                    pp[0] = 1;
                    pp[1] = 1;
                    pp[2] = 0;
                }
                if (PlainText[i - 1] == 7)
                {
                    pp[0] = 1;
                    pp[1] = 1;
                    pp[2] = 1;
                }

                //for (int l = 0; l < kk.Length; l++)
                //{
                //    int lollll = i - 1;
                //    MessageBox.Show("P[" + lollll + "] = " + pp[l]);
                //}

                // XORR 
                string deccimal = "";
                for (int ff = 0; ff < kk.Length; ff++)
                {
                    if (kk[ff] == pp[ff])
                    {
                        deccimal += "0";
                        cc[ff] = 0;

                    }
                    else
                    {
                        cc[ff] = 1;
                        deccimal += "1";
                    }

                }
                //MessageBox.Show(decs);
                if (deccimal == "000")
                {
                    C[i - 1] = 0;
                }
                if (deccimal == "001")
                {
                    C[i - 1] = 1;
                }
                if (deccimal == "010")
                {
                    C[i - 1] = 2;
                }
                if (deccimal == "011")
                {
                    C[i - 1] = 3;
                }
                if (deccimal == "100")
                {
                    C[i - 1] = 4;
                }
                if (deccimal == "101")
                {
                    C[i - 1] = 5;
                }
                if (deccimal == "110")
                {
                    C[i - 1] = 6;
                }
                if (deccimal == "111")
                {
                    C[i - 1] = 7;
                }

                K[i - 1] = k;

            }

            string Finall = "[  ";
            for (int i = 0; i < C.Length; i++)
                Finall += C[i] + "  ";
            label4.Text = (Finall + "]");

            label5.Text = ("RC4 Key :");

            string Finall2 = "[  ";
            for (int i = 0; i < K.Length; i++)
                Finall2 += K[i] + "  ";
            label6.Text = (Finall2 + "]");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RC4 = false;
            SDes = false;
            SAES = false;
            PlayFair = true;
            RSA = false;
            label4.Text = (" ");
            label5.Text = (" ");
            label6.Text = (" ");
            label9.Text = (" ");

        }


        void PLAYFAIR()
        {

            ///  MessageBox.Show((41 ^ 29) % 91+"");
            char[,] mat = new Char[5, 5];
            string pt = textBox1.Text.ToUpper();
            string key = textBox2.Text.ToUpper();
            //MessageBox.Show(pt);
            char[] alpha = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] temp = key.ToCharArray();
            int count = 0;
            int m = 0;
            for (int i = 0; i < key.Length; i++)
            {
                if (temp[i] == ' ')
                {
                    count++;
                }
            }
            for (int i = 0; i < key.Length; i++)
            {
                for (int j = i + 1; j < key.Length; j++)
                {
                    if (temp[i] == temp[j] && temp[i] != ' ')
                    {
                        Console.WriteLine(temp[j] + " ");
                        temp[j] = ' ';
                        count++;
                    }


                }
            }
            char[] tempp = new char[key.Length - count];
            for (int i = 0; i < key.Length; i++)
            {
                if (temp[i] != ' ')
                {
                    tempp[m] = temp[i];

                    //Console.WriteLine(""+tempp[m]);
                    m++;
                    //MessageBox.Show(""+temp[i]);
                }
            }
            string display = string.Join(Environment.NewLine, tempp);
            //MessageBox.Show(display);
            char ab = 'A';
            int ct = 0;
            int flag = 0;
            int posr = 0, posc = 0;


            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    if (ct < tempp.Length)
                    {
                        mat[r, c] = tempp[ct];
                        ct++;
                        posc = c;
                        posr = r;
                        // MessageBox.Show("("+r+","+c+")"+mat[r, c] + "");
                    }
                    else
                    {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1)
                {
                    break;
                }
            }

            int flag2 = 0;
            int cc = 0;
            int l = 0;
            int s = 0;
            //Console.WriteLine(mat[posr, posc] + " /" + posr + " /" + posc +"//"+ ab.ToString());
            int flag1stround = 1;
            for (int r = posr; r < 5; r++)
            {
                if (flag1stround == 1)
                {
                    cc = posc + 1;
                }
                else
                {
                    cc = 0;
                }
                while (cc < 5)
                {
                    for (l = 0; l < tempp.Length; l++)
                    {
                        if (alpha[s] != 'J')
                        {
                            if (alpha[s] == tempp[l])
                            {
                                s++;
                                l = -1;
                                //break;
                                flag2 = 0;
                            }
                            else
                            {
                                flag2 = 1;
                            }
                        }
                        else
                        {
                            s++;
                            l = -1;
                        }
                    }
                    if (flag2 == 1)
                    {
                        mat[r, cc] = alpha[s];
                        // MessageBox.Show(" " + mat[r, cc]);
                        //Console.WriteLine("" + mat[r, cc]);
                        s++;
                        l = -1;
                        flag2 = 0;
                    }
                    cc++;
                }
                flag1stround = 0;
            }

            ////// sorting in the matrix ^ ////////////
            MessageBox.Show("|  " + mat[0, 0] + "  ||  " + mat[0, 1] + "  ||  " + mat[0, 2] + "  ||  " + mat[0, 3] + "  ||  " + mat[0, 4] + "  |\n" +
                            "|  " + mat[1, 0] + "  ||  " + mat[1, 1] + "  ||  " + mat[1, 2] + "  ||  " + mat[1, 3] + "  ||  " + mat[1, 4] + "  |\n" +
                            "|  " + mat[2, 0] + "  ||  " + mat[2, 1] + "  ||  " + mat[2, 2] + "  ||  " + mat[2, 3] + "  ||  " + mat[2, 4] + "  |\n" +
                            "|  " + mat[3, 0] + "  ||  " + mat[3, 1] + "  ||  " + mat[3, 2] + "  ||  " + mat[3, 3] + "  ||  " + mat[3, 4] + "  |\n" +
                            "|  " + mat[4, 0] + "  ||  " + mat[4, 1] + "  ||  " + mat[4, 2] + "  ||  " + mat[4, 3] + "  ||  " + mat[4, 4] + "  |"
                            );

            //string display2 = strin+g.Join(Environment.NewLine, mat);
            //MessageBox.Show(" "+mat[4,4]);
            pt = pt.Trim();
            string pt2 = pt.Replace(" ", string.Empty);

            int counter = 0;
            int marra = 0;

            Boolean odd = true;

            for (int k = 0; k < pt2.Length - 1; k++)
            {
                if (pt2[k] == pt2[k + 1] && k % 2 == 0)
                {
                    if (marra % 2 == 0)
                    {
                        counter++;
                        k++;
                        marra++;
                    }
                }
            }
            if ((counter + pt2.Length) % 2 == 1)
            {
                counter++;
                odd = false;
            }

            char[] pairs = new char[pt2.Length + counter];
            int h = 0;
            int even = 0;
            string xx;
            int mm = 0;
            //MessageBox.Show(pairs.Length + "/counter = " + counter);
            if (odd)
            {

                for (int i = 0; i < pt2.Length; i++)
                {
                    if (i < pt2.Length - 1)
                    {
                        if (pt2[i] == pt2[i + 1] && i % 2 == 0 && mm % 2 == 0)
                        {
                            pairs[h] = pt2[i];
                            pairs[h + 1] = 'X';
                            mm++;
                            h += 2;
                        }
                        else
                        {
                            pairs[h] = pt2[i];
                            h++;
                        }
                    }
                    if (h == pairs.Length - 1 && pt2.Length % 2 == 1)
                    {
                        pairs[h] = pt2[pt2.Length - 1];

                    }
                    if (h == pairs.Length - 1 && pt2.Length % 2 == 0)
                    {
                        pairs[h] = pt2[pt2.Length - 1];
                        //even = 1;
                        break;
                    }
                    mm++;

                }
                if (even == 1)
                {
                    pairs[pairs.Length - 1] = 'X';
                }
            }
            else
            {
                {

                    for (int i = 0; i < pt2.Length; i++)
                    {
                        if (i < pt2.Length - 1)
                        {
                            if (pt2[i] == pt2[i + 1] && i % 2 == 0 && mm % 2 == 0)
                            {
                                pairs[h] = pt2[i];
                                pairs[h + 1] = 'X';
                                mm++;
                                h += 2;
                            }
                            else
                            {
                                pairs[h] = pt2[i];
                                h++;
                            }
                        }
                        if (h == pairs.Length - 1 && pairs.Length % 2 == 1)
                        {
                            pairs[h] = pt2[pt2.Length - 1];

                        }
                        if (h == pairs.Length - 2 && pairs.Length % 2 == 0)
                        {
                            pairs[h] = pt2[pt2.Length - 1];
                            even = 1;
                        }
                        mm++;
                    }
                }
                if (even == 1)
                {
                    pairs[pairs.Length - 1] = 'X';
                }
            }
            //MessageBox.Show("h=" + h);
            string display3 = string.Join(Environment.NewLine, pairs);
            //MessageBox.Show(display3);
            ///////////////////
            int pr = 0, pr2 = 0, pc = 0, pc2 = 0;
            int found = 0;
            int found2 = 0;
            //MessageBox.Show(pairs[pairs.Length - 1] + "");
            char[] cipher = new char[pairs.Length];
            int f = 0;
            if (PFstat)
            {
                for (int i = 0; i < pairs.Length; i += 2)
                {
                    for (int r = 0; r < 5; r++)
                    {
                        for (int c = 0; c < 5; c++)
                        {
                            if (pairs[i] == mat[r, c])
                            {
                                pr = r;
                                pc = c;
                                found = 1;
                                for (int r2 = 0; r2 < 5; r2++)
                                {
                                    for (int c2 = 0; c2 < 5; c2++)
                                    {
                                        if (pairs[i + 1] == mat[r2, c2])
                                        {
                                            pr2 = r2;
                                            pc2 = c2;
                                            found2 = 1;
                                            break;
                                        }

                                    }
                                    if (found2 == 1)
                                    {
                                        break;
                                    }
                                }
                                if (found == found2)
                                {
                                    /////same row /////
                                    if (pr == pr2)
                                    {
                                        if (pc == 4)
                                        {
                                            pc = -1;
                                        }
                                        if (pc2 == 4)
                                        {
                                            pc2 = -1;
                                        }
                                        cipher[f] = mat[pr, pc + 1];
                                        cipher[f + 1] = mat[pr2, pc2 + 1];
                                        found = 0;
                                        found2 = 0;
                                        f += 2;
                                    }
                                    //////same col///////
                                    else if (pc == pc2)
                                    {
                                        if (pr == 4)
                                        {
                                            pr = -1;

                                        }
                                        if (pr2 == 4)
                                        {
                                            pr2 = -1;

                                        }
                                        cipher[f] = mat[pr + 1, pc];
                                        cipher[f + 1] = mat[pr2 + 1, pc2];
                                        found = 0;
                                        found2 = 0;
                                        f += 2;
                                    }
                                    ////////diff row and col//////
                                    else if (pr != pr2 && pc != pc2)
                                    {
                                        cipher[f] = mat[pr, pc2];
                                        cipher[f + 1] = mat[pr2, pc];
                                        found = 0;
                                        found2 = 0;
                                        f += 2;
                                    }
                                }
                            }


                        }
                    }
                }
            }
            ////////////////////////decryption///////////////
            else
            {
                for (int i = 0; i < pairs.Length; i += 2)
                {
                    for (int r = 0; r < 5; r++)
                    {
                        for (int c = 0; c < 5; c++)
                        {
                            if (pairs[i] == mat[r, c])
                            {
                                pr = r;
                                pc = c;
                                found = 1;
                                for (int r2 = 0; r2 < 5; r2++)
                                {
                                    for (int c2 = 0; c2 < 5; c2++)
                                    {
                                        if (pairs[i + 1] == mat[r2, c2])
                                        {
                                            pr2 = r2;
                                            pc2 = c2;
                                            found2 = 1;
                                            break;
                                        }

                                    }
                                    if (found2 == 1)
                                    {
                                        break;
                                    }
                                }
                                if (found == found2)
                                {
                                    /////same row /////
                                    if (pr == pr2)
                                    {
                                        if (pc == 0)
                                        {
                                            pc = 5;
                                        }
                                        if (pc2 == 0)
                                        {
                                            pc2 = 5;
                                        }
                                        cipher[f] = mat[pr, pc - 1];
                                        cipher[f + 1] = mat[pr2, pc2 - 1];
                                        found = 0;
                                        found2 = 0;
                                        f += 2;
                                    }
                                    //////same col///////
                                    else if (pc == pc2)
                                    {
                                        if (pr == 0)
                                        {
                                            pr = 5;

                                        }
                                        if (pr2 == 0)
                                        {
                                            pr2 = 5;

                                        }
                                        cipher[f] = mat[pr - 1, pc];
                                        cipher[f + 1] = mat[pr2 - 1, pc2];
                                        found = 0;
                                        found2 = 0;
                                        f += 2;
                                    }
                                    ////////diff row and col//////
                                    else if (pr != pr2 && pc != pc2)
                                    {
                                        cipher[f] = mat[pr, pc2];
                                        cipher[f + 1] = mat[pr2, pc];
                                        found = 0;
                                        found2 = 0;
                                        f += 2;
                                    }
                                }
                            }


                        }
                    }
                }
            }
            string ciphertext = "";
            for (int i = 0; i < cipher.Length; i++)
            {
                if (i % 2 == 0)
                    ciphertext += "  ";
                ciphertext += cipher[i];
            }
            string ciphertext2 = "";
            for (int i = 0; i < cipher.Length; i++)
            {
                if (cipher[i] == 'I')
                {
                    cipher[i] = 'J';
                }
                if (i % 2 == 0)
                    ciphertext2 += "  ";
                ciphertext2 += cipher[i];
            }
            label4.Text = "" + ciphertext;
            label5.Text = "OR";
            label6.Text = "" + ciphertext2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RC4 = false;
            SDes = false;
            SAES = true;
            PlayFair = false;
            RSA = false;
            label4.Text = (" ");
            label5.Text = (" ");
            label6.Text = (" ");
            label9.Text = (" ");
            S_AES();
        }
        void S_AES()
        {
            string plain = textBox1.Text.ToString();
            string key = textBox2.Text.ToString();
            if (plain.Length < 8)
            {
                int edit = 8 - plain.Length;
                for (int i = 0; i < edit; i++)
                {
                    plain = 0 + "" + plain;
                }

            }
            if (key.Length < 8)
            {
                int edit = 8 - key.Length;
                for (int i = 0; i < edit; i++)
                {
                    key = 0 + "" + key;
                }

            }
            string w0 = "";
            string w1 = "";
            string xorres;
            string w2const = "" + 10000000;
            string w4const = "00110000";
            string nib1 = "";
            string nib2 = "";
            string w2 = "";
            string w3 = "";
            string w4 = "";
            string w5 = "";
            for (int i = 0; i < 16; i++)
            {
                if (i < 8)
                    w0 = w0 + "" + key[i];
                if (i >= 8)
                    w1 = w1 + "" + key[i];
            }
            xorres = XORaes(w0, w2const);
            string[] nibble = { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };
            string[] sbox = { "1001", "0100", "1010", "1011", "1101", "0001", "1000", "0101", "0110", "0010", "0000", "0011", "1100", "1110", "1111", "0111" };
            string subnib = subnibrotnib(nib1, nib2, nibble, sbox, w1);
            w2 = XORaes(subnib, xorres);
            w3 = XORaes(w1, w2);
            xorres = XORaes(w2, w4const);
            subnib = subnibrotnib(nib1, nib2, nibble, sbox, w3);
            w4 = XORaes(xorres, subnib);
            w5 = XORaes(w3, w4);
            string k0 = w0 + "" + w1;
            string k1 = w2 + "" + w3;
            string k2 = w4 + "" + w5;
            string step2 = XORaes(plain, k0);
            string shift = shiftsubnib16(nibble, sbox, step2);
            string nib3 = "", nib4 = "";
            nib1 = ""; nib2 = "";
            for (int i = 0; i < 16; i++)
            {
                if (i < 4)
                {
                    nib1 = nib1 + "" + shift[i];
                }
                if (i >= 4 && i < 8)
                {
                    nib2 = nib2 + "" + shift[i];
                }
                if (i >= 8 && i < 12)
                {
                    nib3 = nib3 + "" + shift[i];
                }
                if (i >= 12 && i < 16)
                {
                    nib4 = nib4 + "" + shift[i];
                }
            }
            string[,] mat = new string[2, 2];
            mat[0, 0] = nib1;
            mat[0, 1] = nib3;
            mat[1, 0] = nib2;
            mat[1, 1] = nib4;
            string[,] me = new string[2, 2];
            me[0, 0] = "1";
            me[0, 1] = "4";
            me[1, 0] = "4";
            me[1, 1] = "1";
            int mix1 = Convert.ToInt32(mat[0, 0], 2);
            int mix2 = Convert.ToInt32(mat[1, 0], 2);
            int mix3 = Convert.ToInt32(mat[0, 1], 2);
            int mix4 = Convert.ToInt32(mat[1, 1], 2);
            string[] multiblicationarray =
            {
            "1","2","3","4","5","6","7","8","9","A","B","C","D","E","F",
            "2","4","6","8","A","C","E","3","1","7","5","B","9","F","D",
            "3","6","5","C","F","A","9","B","8","D","E","7","4","1","2",
            "4","8","C","3","7","B","F","6","2","E","A","5","1","D","9",
            "5","A","F","7","2","D","8","E","B","4","1","9","C","3","6",
            "6","C","A","B","D","7","1","5","3","9","F","E","8","2","4",
            "7","E","9","F","8","1","6","D","A","3","4","2","5","C","B",
            "8","3","B","6","E","5","D","C","4","F","7","A","2","9","1",
            "9","1","8","2","B","3","A","4","D","5","C","6","F","7","E",
            "A","7","D","E","4","9","3","F","5","8","2","1","B","6","C",
            "B","5","E","A","1","F","4","7","C","2","9","D","6","8","3",
            "C","B","7","5","9","E","2","A","6","1","D","F","3","4","8",
            "D","9","4","1","C","8","5","2","F","B","6","3","E","A","7",
            "E","F","1","D","3","2","C","9","7","6","8","4","A","B","5",
            "F","D","2","9","6","4","B","1","E","C","3","8","7","5","A"
            };
            string[,] multiblicationtable = new string[15, 15];
            int ct = 0;
            for (int r = 0; r < 15; r++)
            {

                for (int c = 0; c < 15; c++)
                {
                    multiblicationtable[r, c] = multiblicationarray[ct];
                    if (ct < 225)
                    {
                        ct++;
                    }
                }
            }
            string binaryval1 = getmix(multiblicationtable, me, mix1, mix2, 0, 1, 0, 0);
            string binaryval2 = getmix(multiblicationtable, me, mix3, mix4, 0, 1, 0, 0);
            string binaryval3 = getmix(multiblicationtable, me, mix1, mix2, 0, 1, 1, 1);
            string binaryval4 = getmix(multiblicationtable, me, mix3, mix4, 0, 1, 1, 1);
            string mixcolumn = (binaryval1 + "" + binaryval3 + "" + binaryval2 + "" + binaryval4);
            string k1xormix = XORaes(mixcolumn, k1);
            shift = shiftsubnib16(nibble, sbox, k1xormix);
            string final = XORaes(shift, k2);
            label4.Text = (final);
        }
        string getmix(string[,] multiblicationtable, string[,] me, int mix1, int mix2, int r1, int r2, int c1, int c2)
        {
            string mixcolumn1 = multiblicationtable[Convert.ToInt32(me[r1, c1]) - 1, mix1 - 1];
            string mixcolumn2 = multiblicationtable[Convert.ToInt32(me[r2, c2]) - 1, mix2 - 1];

            if (mixcolumn1 == "0")
            {
                mixcolumn1 = "0000";
            }
            if (mixcolumn1 == "1")
            {
                mixcolumn1 = "0001";
            }
            if (mixcolumn1 == "2")
            {
                mixcolumn1 = "0010";
            }
            if (mixcolumn1 == "3")
            {
                mixcolumn1 = "0011";
            }
            if (mixcolumn1 == "4")
            {
                mixcolumn1 = "0100";
            }
            if (mixcolumn1 == "5")
            {
                mixcolumn1 = "0101";
            }
            if (mixcolumn1 == "6")
            {
                mixcolumn1 = "0110";
            }
            if (mixcolumn1 == "7")
            {
                mixcolumn1 = "0111";
            }
            if (mixcolumn1 == "8")
            {
                mixcolumn1 = "1000";
            }
            if (mixcolumn1 == "9")
            {
                mixcolumn1 = "1001";
            }
            if (mixcolumn1 == "A")
            {
                mixcolumn1 = "1010";
            }
            if (mixcolumn1 == "B")
            {
                mixcolumn1 = "1011";
            }
            if (mixcolumn1 == "C")
            {
                mixcolumn1 = "1100";
            }
            if (mixcolumn1 == "D")
            {
                mixcolumn1 = "1101";
            }
            if (mixcolumn1 == "E")
            {
                mixcolumn1 = "1110";
            }
            if (mixcolumn1 == "F")
            {
                mixcolumn1 = "1111";
            }
            if (mixcolumn2 == "0")
            {
                mixcolumn2 = "0000";
            }
            if (mixcolumn2 == "1")
            {
                mixcolumn2 = "0001";
            }
            if (mixcolumn2 == "2")
            {
                mixcolumn2 = "0010";
            }
            if (mixcolumn2 == "3")
            {
                mixcolumn2 = "0011";
            }
            if (mixcolumn2 == "4")
            {
                mixcolumn2 = "0100";
            }
            if (mixcolumn2 == "5")
            {
                mixcolumn2 = "0101";
            }
            if (mixcolumn2 == "6")
            {
                mixcolumn2 = "0110";
            }
            if (mixcolumn2 == "7")
            {
                mixcolumn2 = "0111";
            }
            if (mixcolumn2 == "8")
            {
                mixcolumn2 = "1000";
            }
            if (mixcolumn2 == "9")
            {
                mixcolumn2 = "1001";
            }
            if (mixcolumn2 == "A")
            {
                mixcolumn2 = "1010";
            }
            if (mixcolumn2 == "B")
            {
                mixcolumn2 = "1011";
            }
            if (mixcolumn2 == "C")
            {
                mixcolumn2 = "1100";
            }
            if (mixcolumn2 == "D")
            {
                mixcolumn2 = "1101";
            }
            if (mixcolumn2 == "E")
            {
                mixcolumn2 = "1110";
            }
            if (mixcolumn2 == "F")
            {
                mixcolumn2 = "1111";
            }
            string mix = XORaes(mixcolumn1, mixcolumn2);
            return mix;
        }
        string XORaes(string s1, string s2)
        {
            string res = "";
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] == s2[i])
                {
                    res = res + "" + 0;
                }
                else
                {
                    res = res + "" + 1;
                }
            }
            return (res);
        }
        string subnibrotnib(string nib11, string nib22, string[] nibble2, string[] sboxs, string w)
        {
            string rotnib = "";
            nib11 = "";
            nib22 = "";
            for (int i = 0; i < 8; i++)
            {
                if (i < 4)
                {
                    nib11 = nib11 + "" + w[i];
                }
                if (i >= 4)
                {
                    nib22 = nib22 + "" + w[i];

                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (nib11 == nibble2[i])
                {
                    nib11 = sboxs[i]; break;
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (nib22 == nibble2[i])
                {
                    nib22 = sboxs[i]; break;
                }
            }
            rotnib = nib22 + "" + nib11;
            return rotnib;
        }
        string shiftsubnib16(string[] nibble2, string[] sboxs, string w)
        {
            string subnib = "";
            string nib1 = "", nib2 = "", nib3 = "", nib4 = "";

            for (int i = 0; i < 16; i++)
            {
                if (i < 4)
                {
                    nib1 = nib1 + "" + w[i];
                }
                if (i >= 4 && i < 8)
                {
                    nib2 = nib2 + "" + w[i];

                }
                if (i >= 8 && i < 12)
                {
                    nib3 = nib3 + "" + w[i];

                }
                if (i >= 12 && i < 16)
                {
                    nib4 = nib4 + "" + w[i];

                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (nib1 == nibble2[i])
                {
                    nib1 = sboxs[i]; break;
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (nib2 == nibble2[i])
                {
                    nib2 = sboxs[i]; break;
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (nib3 == nibble2[i])
                {
                    nib3 = sboxs[i]; break;
                }
            }
            for (int i = 0; i < 16; i++)
            {
                if (nib4 == nibble2[i])
                {
                    nib4 = sboxs[i]; break;
                }
            }
            subnib = nib1 + "" + nib4 + "" + nib3 + "" + nib2;
            return subnib;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            RC4 = false;
            SDes = false;
            SAES = false;
            PlayFair = false;
            flagDecryption = false;
            RSA = true;
            label4.Text = (" ");
            label5.Text = (" ");
            label6.Text = (" ");
            label9.Text = (" .");
        }
        void RSA_()
        {
            string[] given = textBox2.Text.Split(',');
            double Message = Double.Parse(textBox1.Text);
            double EncryptedKey = Double.Parse(given[0]);
            double P = Double.Parse(given[1]);
            double q = Double.Parse(given[2]);
            double n = P * q;
            double phyn = (P - 1) * (q - 1);
            double cyphertxt = Math.Pow(Message, EncryptedKey);
            double resultEncyption = 0;
            if (flagDecryption == false) resultEncyption = cyphertxt % n;
            else resultEncyption = Double.Parse(textBox1.Text);
            label4.Text = ("" + resultEncyption.ToString());
            if (flagDecryption)
            {
                List<Class1> roww = new List<Class1>();
                Class1 pnn = new Class1();
                pnn.row[0] = 1;
                pnn.row[1] = 0;
                pnn.row[2] = Convert.ToInt32(phyn);
                pnn.row[3] = 0;
                roww.Add(pnn);
                pnn = new Class1();
                pnn.row[0] = 0;
                pnn.row[1] = 1;
                pnn.row[2] = Convert.ToInt32(EncryptedKey);
                pnn.row[3] = Convert.ToInt32(phyn) / Convert.ToInt32(EncryptedKey);
                roww.Add(pnn);
                int itteration = 2;
                while (true)
                {
                    int c0 = Convert.ToInt32(roww[itteration - 2].row[0]) - (Convert.ToInt32((roww[itteration - 1].row[0]) * Convert.ToInt32(roww[itteration - 1].row[3])));
                    int c1 = Convert.ToInt32(roww[itteration - 2].row[1]) - (Convert.ToInt32((roww[itteration - 1].row[1]) * Convert.ToInt32(roww[itteration - 1].row[3])));
                    int c2 = Convert.ToInt32(roww[itteration - 2].row[2]) - (Convert.ToInt32((roww[itteration - 1].row[2]) * Convert.ToInt32(roww[itteration - 1].row[3])));
                    pnn = new Class1();
                    pnn.row[0] = c0;
                    pnn.row[1] = c1;
                    pnn.row[2] = c2;
                    if (c2 != 1)
                    {
                        int c3 = Convert.ToInt32(roww[itteration - 1].row[2]) / c2;
                        pnn.row[3] = c3;
                        roww.Add(pnn);
                    }
                    else
                    {
                        pnn.row[3] = 0;
                        roww.Add(pnn);
                        break;
                    }
                    itteration++;
                }
                Console.WriteLine("DECryption KEY: " + roww[roww.Count - 1].row[1]);
                //Console.WriteLine(n);
                //Console.WriteLine(resultEncyption);

                if (roww[roww.Count - 1].row[1] < 0)
                {
                    int temp = 0;
                    while (true)
                    {
                        if (roww[roww.Count - 1].row[1] < 0)
                        {
                            roww[roww.Count - 1].row[1] = roww[roww.Count - 1].row[1] - (Convert.ToInt32(phyn) * temp);
                        }
                        else
                        {
                            break;
                        }
                        temp--;
                    }
                }
                //Console.WriteLine("DECryption KEY: " + roww[roww.Count - 1].row[1]);

                if (roww[roww.Count - 1].row[1] > 10 && roww[roww.Count - 1].row[1] < 30)
                {
                    if (roww[roww.Count - 1].row[1] % 2 == 0)
                    {
                        int power = (roww[roww.Count - 1].row[1] / 2);
                        double A = Math.Pow(resultEncyption, power);
                        A = A % n;

                        double B = Math.Pow(resultEncyption, power);
                        B = B % n;
                        A *= B;
                        A = A % n;
                        //Console.WriteLine(A);
                        label4.Text = "" + A.ToString();
                    }
                    else
                    {
                        //Console.WriteLine("-----------------");
                        int power = (roww[roww.Count - 1].row[1] / 2);
                        int power2 = power / 2;
                        double A = Math.Pow(resultEncyption, power2);
                        //Console.WriteLine("A BEF % : " + A);
                        A = A % n;
                        A = A * A;
                        A = A % n;

                        //Console.WriteLine("A : " + A);
                        double B = Math.Pow(resultEncyption, power2);
                        B = B % n;
                        B = B * B;
                        B = B % n;
                        B = B * resultEncyption;
                        B = B % n;
                        //Console.WriteLine("B : " + B);
                        A *= B;
                        A = A % n;
                        //Console.WriteLine("A:"+ A);
                        label4.Text = "" + A.ToString();

                    }
                }
                else
                {
                    if (roww[roww.Count - 1].row[1] < 10)
                    {
                        double Decryotion = Math.Pow(resultEncyption, roww[roww.Count - 1].row[1]) % n;
                        label4.Text = "" + Decryotion.ToString();
                    }
                    else
                    {
                        label9.Text = "Decryption Equation value exceed number of bits of the ProgrammingLanguage !";
                        label4.Text = "" + resultEncyption + "^" + roww[roww.Count - 1].row[1] + " MOD " + n;
                    }
                }
            }
            
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}

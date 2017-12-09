using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Backprop;

namespace Vowel_Consonant
{
    public partial class Form1 : Form
    {
        const int numInput = 35; //35 checkboxes
        int num_patterns; // how many patterns
        int length_pattern; // how many digits per pattern + 6 output6
        int numOutput = 6; //output patterns


        List<string> unformatted_input;
        double[,] w;
        double[] bias;
        int[] x; //input
        double learning_rate;
        int[] output;
        

        //input patterns
        int[,] patterns;

        //desired patterns
        int[,] desired;

        Random rnd;
        bool loadFile1;
        bool loadFile2;
        String loadedFile1;
        String loadedFile2;

       // const int numInput = 280;                       //no. of pixels
        //const int numOutput = 3;                        //no. of output, A, I, U, E, O
        const int numHidden = 20;
        List<List<int>> inputData;
        List<int[]> outputData;
        NeuralNet bp;


        //test button
        private void button2_Click(object sender, EventArgs e)
        {
            /*
            output = calculateOutput();
            string s = "";
            for (int i = 0; i < output.Length; i++ )
            {
                s += output[i] + "";
            }*/

            for (int j = 0; j < numInput; j++)
            {
                bp.setInputs(j, x[j]);
            }
            bp.run();

            double[] o = new double[numOutput];

            string s = "";
            for (int i = 0; i < numOutput; i++)
            {
                o[i] = bp.getOuputData(i);
                if (o[i] > 0.5)
                    o[i] = 1;
                else
                    o[i] = 0;
                s += o[i].ToString() + "";
            }


            if (s=="000000"){
                MessageBox.Show("A");
            }
            else if (s == "000001")
            {
                MessageBox.Show("B");
            }
            else if (s == "000010")
            {
                MessageBox.Show("C");
            }
            else if (s == "000011")
            {
                MessageBox.Show("D");
            }
            else if (s == "000100")
            {
                MessageBox.Show("E");
            }
            else if (s == "000101")
            {
                MessageBox.Show("F");
            }
            else if (s == "000110")
            {
                MessageBox.Show("G");
            }
            else if (s == "000111")
            {
                MessageBox.Show("H");
            }
            else if (s == "001000")
            {
                MessageBox.Show("I");
            }
            else if (s == "001001")
            {
                MessageBox.Show("J");
            }
            else if (s == "001010")
            {
                MessageBox.Show("K");
            }
            else if (s == "001011")
            {
                MessageBox.Show("L");
            }
            else if (s == "001100")
            {
                MessageBox.Show("M");
            }
            else if (s == "001101")
            {
                MessageBox.Show("N");
            }
            else if (s == "001110")
            {
                MessageBox.Show("O");
            }
            else if (s == "001111")
            {
                MessageBox.Show("P");
            }
            else if (s == "010000")
            {
                MessageBox.Show("Q");
            }
            else if (s == "010001")
            {
                MessageBox.Show("R");
            }
            else if (s == "010010")
            {
                MessageBox.Show("S");
            }
            else if (s == "010011")
            {
                MessageBox.Show("T");
            }
            else if (s == "010100")
            {
                MessageBox.Show("U");
            }
            else if (s == "010101")
            {
                MessageBox.Show("V");
            }
            else if (s == "010110")
            {
                MessageBox.Show("W");
            }
            else if (s == "010111")
            {
                MessageBox.Show("X");
            }
            else if (s == "011000")
            {
                MessageBox.Show("Y");
            }
            else if (s == "011001")
            {
                MessageBox.Show("Z");
            }
            else if (s == "011010")
            {
                MessageBox.Show("0");
            }
            else if (s == "011011")
            {
                MessageBox.Show("1");
            }
            else if (s == "011100")
            {
                MessageBox.Show("2");
            }
            else if (s == "011101")
            {
                MessageBox.Show("3");
            }
            else if (s == "011110")
            {
                MessageBox.Show("4");
            }
            else if (s == "011111")
            {
                MessageBox.Show("5");
            }
            else if (s == "100000")
            {
                MessageBox.Show("6");
            }
            else if (s == "100001")
            {
                MessageBox.Show("7");
            }
            else if (s == "100010")
            {
                MessageBox.Show("8");
            }
            else if (s == "100011")
            {
                MessageBox.Show("9");
            }
            else
                MessageBox.Show(s);


        }

        private int[] calculateOutput() {
            

            //summation of input, weight and bias
            for (int i = 0; i < numOutput; i++ )
            {
                double v = 0;
                for (int j = 0; j < numInput; j++)
                {
                    v += (double)x[j] * w[j,i];
                }
                v += bias[i];
               // Console.WriteLine("v::: " + v);
                if (v > 0)
                    output[i] = 1;
                else
                    output[i] = 0;
            }
      
           
            return output;
        }


        //this is train button
        private void button1_Click(object sender, EventArgs e)
        {
            //started training for alphabet/number

            //read from file
            readFromFile();

            formatInputs();
            
            bp = new NeuralNet(numInput, numHidden, numOutput);
            for (int y = 0; y < 2000; y++) //20000 epochs
            {
                for (int i = 0; i < patterns.Length/numInput; i++)
                {
                    for (int j = 0; j < numInput; j++)
                    {
                        bp.setInputs(j, patterns[i,j]);
                    }
                    for (int j = 0; j < numOutput; j++)
                    {
                        bp.setDesiredOutput(j, desired[i,j]);
                    }
                    bp.learn();

                }

            }
            MessageBox.Show("training done");

            x = new int[numInput];
            this.btnTest.Enabled = true;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            outputData = new List<int[]>();
            inputData = new List<List<int>>();


            //initialize files
            loadFile1 = false;
            loadFile2 = false;

            //for inputs (unformmated)
            unformatted_input = new List<string>();

            //assign weights for each input
            w = new double[numInput, numOutput];
            length_pattern = numInput + numOutput;
            num_patterns = 36; //number of characters in the alphabet + 10 numbers

            x = new int[numInput];
            output = new int[numOutput];
            bias = new double[numOutput];

           

            rnd = new Random();

            for (int i = 0; i < numInput; i++ )
            {
                for (int j = 0; j < numOutput; j++) {
                    w[i, j] = rnd.NextDouble();
                }
            }

            for (int i = 0; i < numOutput; i++)
            {
                bias[i] = rnd.NextDouble();
            }

            learning_rate = 0.13;
        }

        private void readFromFile() {
            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(loadedFile1);
            while ((line = file.ReadLine()) != null)
            {
                unformatted_input.Add(line);
                counter++;
            }

            file.Close();
        }

        private void formatInputs(){
            
                patterns = new int[num_patterns, numInput];
                desired = new int[num_patterns, numOutput];
                
                
                for (int i = 0; i < unformatted_input.Count; i++ )
                {
                    int count = 0;

                    for (int j = 0; j < length_pattern; j++ )
                    {

                        if (j >= numInput)
                        {
                            desired[i, count++] = Convert.ToInt32(unformatted_input.ElementAt(i).Substring(j, 1));
                            //Console.Write(unformatted_input.ElementAt(i).Substring(j, 1));
                        }
                        else
                        {
                            patterns[i, j] = Convert.ToInt32(unformatted_input.ElementAt(i).Substring(j, 1));
                            //Console.WriteLine(unformatted_input.ElementAt(i).Substring(j, 1));
                        }
                    }
                    //Console.WriteLine();
                    
                }
        
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(openFileDialog1.FileName);
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine(line);
                counter++;
            }

            file.Close();

            // Suspend the screen.
            Console.ReadLine();
        }

        private void buttonGetData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string strDir = Directory.GetCurrentDirectory();
            ofd.Filter = "Text Files(*.txt)|*.txt";
            ofd.InitialDirectory = strDir;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Contains("_data.txt"))
                {
                    loadedFile1 = ofd.FileName;
                    loadedFile1 = loadedFile1.Replace(strDir + "\\", "");
                    labelLoadedFileName.Text = loadedFile1;
                    loadFile1 = true;
                }
                else
                {
                    MessageBox.Show("Please select a file which ends with _data.txt");
                }
            }

            if (loadFile1 == true)
            {
                button1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Load data first");
            }
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.BackColor = Color.Green;
                x[0] = 1;
            }
            else
            {
                checkBox1.BackColor = Color.Pink;
                x[0] = 0;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox2.BackColor = Color.Green;
                x[1] = 1;
            }
            else
            {
                checkBox2.BackColor = Color.Pink;
                x[1] = 0;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox3.BackColor = Color.Green;
                x[2] = 1;
            }
            else
            {
                checkBox3.BackColor = Color.Pink;
                x[2] = 0;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox4.BackColor = Color.Green;
                x[3] = 1;
            }
            else
            {
                checkBox4.BackColor = Color.Pink;
                x[3] = 0;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                checkBox5.BackColor = Color.Green;
                x[4] = 1;
            }
            else
            {
                checkBox5.BackColor = Color.Pink;
                x[4] = 0;
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                checkBox10.BackColor = Color.Green;
                x[9] = 1;
            }
            else
            {
                checkBox10.BackColor = Color.Pink;
                x[9] = 0;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
            {
                checkBox9.BackColor = Color.Green;
                x[8] = 1;
            }
            else
            {
                checkBox9.BackColor = Color.Pink;
                x[8] = 0;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                checkBox8.BackColor = Color.Green;
                x[7] = 1;
            }
            else
            {
                checkBox8.BackColor = Color.Pink;
                x[7] = 0;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                checkBox7.BackColor = Color.Green;
                x[6] = 1;
            }
            else
            {
                checkBox7.BackColor = Color.Pink;
                x[6] = 0;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                checkBox6.BackColor = Color.Green;
                x[5] = 1;
            }
            else
            {
                checkBox6.BackColor = Color.Pink;
                x[5] = 0;
            }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox15.Checked)
            {
                checkBox15.BackColor = Color.Green;
                x[14] = 1;
            }
            else
            {
                checkBox15.BackColor = Color.Pink;
                x[14] = 0;
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
            {
                checkBox11.BackColor = Color.Green;
                x[10] = 1;
            }
            else
            {
                checkBox11.BackColor = Color.Pink;
                x[10] = 0;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked)
            {
                checkBox12.BackColor = Color.Green;
                x[11] = 1;
            }
            else
            {
                checkBox12.BackColor = Color.Pink;
                x[11] = 0;
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked)
            {
                checkBox13.BackColor = Color.Green;
                x[12] = 1;
            }
            else
            {
                checkBox13.BackColor = Color.Pink;
                x[12] = 0;
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.Checked)
            {
                checkBox14.BackColor = Color.Green;
                x[13] = 1;
            }
            else
            {
                checkBox14.BackColor = Color.Pink;
                x[13] = 0;
            }
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox16.Checked)
            {
                checkBox16.BackColor = Color.Green;
                x[15] = 1;
            }
            else
            {
                checkBox16.BackColor = Color.Pink;
                x[15] = 0;
            }

        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox17.Checked)
            {
                checkBox17.BackColor = Color.Green;
                x[16] = 1;
            }
            else
            {
                checkBox17.BackColor = Color.Pink;
                x[16] = 0;
            }

        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox18.Checked)
            {
                checkBox18.BackColor = Color.Green;
                x[17] = 1;
            }
            else
            {
                checkBox18.BackColor = Color.Pink;
                x[17] = 0;
            }
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox19.Checked)
            {
                checkBox19.BackColor = Color.Green;
                x[18] = 1;
            }
            else
            {
                checkBox19.BackColor = Color.Pink;
                x[18] = 0;
            }
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox20.Checked)
            {
                checkBox20.BackColor = Color.Green;
                x[19] = 1;
            }
            else
            {
                checkBox20.BackColor = Color.Pink;
                x[19] = 0;
            }
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox21.Checked)
            {
                checkBox21.BackColor = Color.Green;
                x[20] = 1;
            }
            else
            {
                checkBox21.BackColor = Color.Pink;
                x[20] = 0;
            }

        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox22.Checked)
            {
                checkBox22.BackColor = Color.Green;
                x[21] = 1;
            }
            else
            {
                checkBox22.BackColor = Color.Pink;
                x[21] = 0;
            }
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox23.Checked)
            {
                checkBox23.BackColor = Color.Green;
                x[22] = 1;
            }
            else
            {
                checkBox23.BackColor = Color.Pink;
                x[22] = 0;
            }
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox24.Checked)
            {
                checkBox24.BackColor = Color.Green;
                x[23] = 1;
            }
            else
            {
                checkBox24.BackColor = Color.Pink;
                x[23] = 0;
            }
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox25.Checked)
            {
                checkBox25.BackColor = Color.Green;
                x[24] = 1;
            }
            else
            {
                checkBox25.BackColor = Color.Pink;
                x[24] = 0;
            }
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox26.Checked)
            {
                checkBox26.BackColor = Color.Green;
                x[25] = 1;
            }
            else
            {
                checkBox26.BackColor = Color.Pink;
                x[25] = 0;
            }
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox27.Checked)
            {
                checkBox27.BackColor = Color.Green;
                x[26] = 1;
            }
            else
            {
                checkBox27.BackColor = Color.Pink;
                x[26] = 0;
            }
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox28.Checked)
            {
                checkBox28.BackColor = Color.Green;
                x[27] = 1;
            }
            else
            {
                checkBox28.BackColor = Color.Pink;
                x[27] = 0;
            }
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox29.Checked)
            {
                checkBox29.BackColor = Color.Green;
                x[28] = 1;
            }
            else
            {
                checkBox29.BackColor = Color.Pink;
                x[28] = 0;
            }
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox30.Checked)
            {
                checkBox30.BackColor = Color.Green;
                x[29] = 1;
            }
            else
            {
                checkBox30.BackColor = Color.Pink;
                x[29] = 0;
            }
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox31.Checked)
            {
                checkBox31.BackColor = Color.Green;
                x[30] = 1;
            }
            else
            {
                checkBox31.BackColor = Color.Pink;
                x[30] = 0;
            }
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox32.Checked)
            {
                checkBox32.BackColor = Color.Green;
                x[31] = 1;
            }
            else
            {
                checkBox32.BackColor = Color.Pink;
                x[31] = 0;
            }
        }

        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox33.Checked)
            {
                checkBox33.BackColor = Color.Green;
                x[32] = 1;
            }
            else
            {
                checkBox33.BackColor = Color.Pink;
                x[32] = 0;
            }
        }

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox34.Checked)
            {
                checkBox34.BackColor = Color.Green;
                x[33] = 1;
            }
            else
            {
                checkBox34.BackColor = Color.Pink;
                x[33] = 0;
            }
        }

        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox35.Checked)
            {
                checkBox35.BackColor = Color.Green;
                x[34] = 1;
            }
            else
            {
                checkBox35.BackColor = Color.Pink;
                x[34] = 0;
            }
        }

        private void buttonSaveNet_Click(object sender, EventArgs e)
        {
                SaveFileDialog sfd = new SaveFileDialog();
                string strDir = Directory.GetCurrentDirectory();

                sfd.InitialDirectory = strDir;
                sfd.Filter = "Text Files(*.txt)|*.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {                                              
                        string str = sfd.FileName;
                        if (File.Exists(str))
                        {
                            File.Delete(str);
                        }

                        StreamWriter sw = File.AppendText(str);

                        for (int i = 0; i < numInput; i++)
                        {
                            for (int j = 0; j < numOutput; j++ )
                            {
                                sw.Write(w[i, j]);
                                sw.WriteLine();
                            }
                        }

                        for (int j = 0; j < numOutput; j++)
                        {
                            sw.Write(bias[j]);
                            sw.WriteLine();
                        }

                        MessageBox.Show("Network Saved");
                        sw.Close();                        
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("File not saved: " + exc.Message);
                    }
                    
                }
           
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string strDir = Directory.GetCurrentDirectory();
            ofd.Filter = "Text Files(*.txt)|*.txt";
            ofd.InitialDirectory = strDir;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Contains("_net.txt"))
                {
                    loadedFile2 = ofd.FileName;
                    int c1 = 0;
                    int c2 = 0;
                    int b = 0;
                    string line;
                    

                    // Read the file and display it line by line.
                    System.IO.StreamReader file =
                       new System.IO.StreamReader(loadedFile2);
                    while ((line = file.ReadLine()) != null)
                    {
                        if (c1 == numInput)
                            bias[b++] = Convert.ToDouble(line);
                        else
                            w[c1, c2++] = Convert.ToDouble(line);
                        if (c2 == 6)
                        {
                            c1++;
                            c2 = 0;
                        }
                        
                            
                    }

                    file.Close();
                    btnTest.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Please select a file which ends with _net.txt");
                }
            }

           
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls) {
                if (c is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)c;
                    checkBox.Checked = false;
                }

            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void labelLoadedFileName_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        
    }
}

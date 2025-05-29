using System.Text;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        StringToHandle currentString;
        int factualKey;

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            var tmp = "йцукенгшщзхъфывапролджэ€чсмитьбю".Order().Select(z => "\'" + z + "\', ").ToArray();
            var Text = CaeserCipher.Cipher(StringToHandle.CleanString("abcdef"), -2);
            //label1.Text = (-1%26).ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cleaned = StringToHandle.CleanString(textBox1.Text);

            if (cleaned.Language == Language.Mixed || cleaned.Language == Language.Invalid)
            {
                MessageBox.Show("“екст в неправильном формате", "ќшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            currentString = cleaned;
            textBox1.Text = cleaned.StringOriginal;
            UpdateFactualKey();
            UnlockButtons();
        }

        void LockButtons()
        {
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        void UnlockButtons()
        {
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            currentString = null;
            UpdateFactualKey();
            LockButtons();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = DivideString(CaeserCipher.Cipher(currentString, factualKey), 5);
        }


        string DivideString(string str, int chunkSize)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                if (i > 0 && i % chunkSize == 0)
                {
                    sb.Append(' ');
                }
                sb.Append(str[i]);
            }

            return sb.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = DivideString(CaeserCipher.Decipher(currentString, factualKey), 5);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateFactualKey();
        }

        void UpdateFactualKey()
        {
            factualKey = 0;
            if (currentString != null)
            {
                var tmp = (int)numericUpDown1.Value;

                char[] alphabet;

                if (currentString.Language == Language.English)
                    alphabet = CaeserCipher.EnglishAlphabet;
                else
                    alphabet = CaeserCipher.RussianAlphabet;

                tmp = tmp % alphabet.Length;
                if(tmp < 0)
                    tmp = tmp + alphabet.Length;
                factualKey = tmp;
            }
            label3.Text = factualKey.ToString();
        }

    }
}

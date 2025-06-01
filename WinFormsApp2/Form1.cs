using System.Text;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        StringToHandle currentString;
        int factualKey;
        string handledtext;

        public Form1()
        {
            InitializeComponent();
        }



        (int, bool) tryGetKey()
        {
            bool suc = true;
            int key = 0;

            var tmp = textBox3.Text;

            try
            {
                key = int.Parse(tmp);
            }
            catch
            {
                suc = false;
                MessageBox.Show("Неправильный формат ключа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (key,  suc);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        void handleinput()
        {
            if (currentString != null)
                return;
            var cleaned = StringToHandle.CleanString(textBox1.Text);

            if (cleaned.Language == Language.Invalid)
            {
                MessageBox.Show("Текст в неправильном формате", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            currentString = cleaned;
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
            //LockButtons();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            handleinput();
            var t = tryGetKey();
            if (t.Item2 == false)
                return;
            handledtext = CaeserCipher.Cipher(currentString, t.Item1);
            textBox2.Text = DivideString(handledtext, (int)numericUpDown2.Value);
        }


        string DivideString(string str, int chunkSize)
        {
            if (chunkSize == 0)
                return str;
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
            handleinput();
            var t = tryGetKey();
            if (t.Item2 == false)
                return;
            handledtext = CaeserCipher.Cipher(currentString, CaeserCipher.DecipherKey(currentString, t.Item1));
            textBox2.Text = DivideString(handledtext, (int)numericUpDown2.Value);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //UpdateFactualKey();
        }

        void UpdateFactualKey()
        {
            /*
            factualKey = 0;
            if (currentString != null)
            {
                var tmp = CaeserCipher.CipherKey(currentString, (int)numericUpDown1.Value);

                var tmp = (int)numericUpDown1.Value;

                char[] alphabet;

                if (currentString.Language == Language.English)
                    alphabet = CaeserCipher.EnglishAlphabet;
                else
                    alphabet = CaeserCipher.RussianAlphabet;

                tmp = tmp % alphabet.Length;
                if (tmp < 0)
                    tmp = tmp + alphabet.Length;

                factualKey = tmp;
            }
            //label3.Text = factualKey.ToString();
            */
        }

        private void button4_Click(object sender, EventArgs e)
        {
            handleinput();

            if (currentString.Language == Language.Mixed)
            {
                MessageBox.Show("Взлом возможен только русского или только английского текста", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var tmp = CaeserCipher.BreakCipher(currentString);
            handledtext = tmp.Item2;
            textBox2.Text = DivideString(handledtext, (int)numericUpDown2.Value);
            textBox3.Text = tmp.Item1.ToString();
            factualKey = tmp.Item1;
            UpdateFactualKey();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if(handledtext != null)
                textBox2.Text = DivideString(handledtext, (int)numericUpDown2.Value);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            currentString = null;
            factualKey = 0;
            textBox3.Clear();
            handledtext = null;
            textBox1.Clear();
            textBox2.Clear();
        }
    }
}

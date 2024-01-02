using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        private Calculus calc = new Calculus();

        public char lastKey;

        private void textBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8 && (e.KeyChar <= 39 || e.KeyChar >= 46) && e.KeyChar != 47 && e.KeyChar != 61) //калькулятор
            {
                e.Handled = true;
            }
            else if ((e.KeyChar == 61 || e.KeyChar == 47 || e.KeyChar == 58 || (e.KeyChar >= 41&&e.KeyChar<=43)) && textBoxInput.Text.Length == 0)
            {
                e.Handled = true;
            }
            else if (e.KeyChar==41&&lastKey==40)
            {
                e.Handled = true;
            }
            if (((e.KeyChar >= 42 && e.KeyChar <= 45) || e.KeyChar == 47 || e.KeyChar == 58) && ((lastKey >= 42 && lastKey <= 45) || lastKey == 47 || lastKey == 58))
            {
                e.Handled = true;
            }

            if (e.KeyChar == 42)
            {
                e.KeyChar = '×';
            }
            if (e.KeyChar == 47 || e.KeyChar == 58)
            {
                e.KeyChar = '÷';
            }

            lastKey = e.KeyChar;
        }
        private int i = 0;
        private void buttonNumeric_Click(object sender, EventArgs e)
        {
            textBoxInput.Text += ((Button)sender).Text;
        }
        private static readonly char[] charSeparators = { '÷', '×', '-', '+', '(', ')' };
        private static char[] actions;
        private static int[] numbers;
        private void buttonAction_Click(object sender, EventArgs e)
        {
            if (textBoxInput.Text.Length == 0)
                return;
            textBoxInput.Text += ((Button)sender).Text;
            try
            {
                numbers = textBoxInput.Text.Split(charSeparators).Select(n => Convert.ToInt32(n)).ToArray();
                actions = (textBoxInput.Text.Where(t => !char.IsDigit(t)).ToArray());


            }
            catch (Exception)
            {

            }
           

            switch (((Button)sender).Name)
            {
                case "buttonMultiplication":
                    try
                    {
                        labelPreResult.Text = calc.Multiplication(numbers[i], numbers[i + 1]).ToString();
                        i++;
                    }
                    catch (Exception)
                    {

                    }
                   
                    break;
                case "buttonDivision":
                    labelPreResult.Text = calc.Division(numbers[i], numbers[i + 1]).ToString();
                    i++;
                    break;
                case "buttonAddition":
                    labelPreResult.Text = calc.Addition(numbers[i], numbers[i + 1]).ToString();
                    i++;
                    break;
                case "buttonSubstraction":
                    labelPreResult.Text = calc.Substraction(numbers[i], numbers[i + 1]).ToString();
                    i++;
                    break;
                
            }
            labelPreResult.Text = calc.Multiplication(2, 2).ToString();

        }

        private void buttonBracket_Click(object sender, EventArgs e)
        {
            textBoxInput.Text += ((Button)sender).Text;
        }

        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxInput.Text = textBoxInput.Text.Remove(textBoxInput.Text.Length - 1);
            }
            catch (Exception)
            {

               
            }
           
        }

        private void buttonEqual_Click(object sender, EventArgs e)
        {
            if (textBoxInput.Text.Length == 0)
                return;
            textBoxInput.Text = "0";

        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            try
            {
                numbers = textBoxInput.Text.Split(charSeparators).Select(n => Convert.ToInt32(n)).ToArray();
                actions = (textBoxInput.Text.Where(t => !char.IsDigit(t)).ToArray());
            }
            catch (Exception)
            {
               
            }
        }
    }
}

class Calculus
{
    internal double Multiplication(double x1, double x2)
    {
        return x1*x2;
    }
    internal double Division(double x1, double x2)
    {
        return x1 / x2;
    }
    internal double Addition(double x1, double x2)
    {
        return x1 + x2;
    }
    internal double Substraction(double x1, double x2)
    {
        return x1 - x2;
    }
    
}

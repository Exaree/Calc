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
       

        public char lastKey;
        private bool lastKeyC = true;

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
            if (((e.KeyChar >= 42 && e.KeyChar <= 45) || e.KeyChar == 47 || e.KeyChar == 58|| e.KeyChar==61) && ((lastKey >= 42 && lastKey <= 45) || lastKey == 47 || lastKey == 58||lastKey==61))
            {
                e.Handled = true;
            }

            if (e.KeyChar == 61)
            {
                if (textBoxInput.Text.Length == 0)
                    return;
                try
                {
                    e.Handled = true;
                    textBoxInput.Text = (RPN.Calculate(textBoxInput.Text).ToString());
                    labelPreResult.Text = "";
                    lastKeyC = false;
                }
                catch (Exception)
                {
                    e.Handled = true;

                }
               
            }
            if (lastKeyC)
            {
                lastKey = e.KeyChar;
            }
            
        }
       
        private void buttonNumeric_Click(object sender, EventArgs e)
        {
            textBoxInput.Text += ((Button)sender).Text;
        }
     
        private void buttonAction_Click(object sender, EventArgs e)
        {
            textBoxInput.Text += ((Button)sender).Text;
           
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
            try
            {
                textBoxInput.Text = (RPN.Calculate(textBoxInput.Text).ToString());
                labelPreResult.Text = "";
            }
            catch (Exception)
            {

               
            }

        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            try
            {
                labelPreResult.Text = (RPN.Calculate(textBoxInput.Text).ToString());
            }
            catch (Exception)
            {
                labelPreResult.Text = "";
            }
        }
    }
}

class RPN
{
    //Метод Calculate принимает выражение в виде строки и возвращает результат, в своей работе использует другие методы класса
    static public double Calculate(string input)
    {
        string output = GetExpression(input); //Преобразовываем выражение в постфиксную запись
        double result = Counting(output); //Решаем полученное выражение
        return result; //Возвращаем результат
    }

    //Метод, преобразующий входную строку с выражением в постфиксную запись
    static private string GetExpression(string input)
    {
        string output = string.Empty; //Строка для хранения выражения
        Stack<char> operStack = new Stack<char>(); //Стек для хранения операторов

        for (int i = 0; i < input.Length; i++) //Для каждого символа в входной строке
        {
            //Разделители пропускаем
            if (IsDelimeter(input[i]))
                continue; //Переходим к следующему символу

            //Если символ - цифра, то считываем все число
            if (Char.IsDigit(input[i])) //Если цифра
            {
                //Читаем до разделителя или оператора, что бы получить число
                while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                {
                    output += input[i]; //Добавляем каждую цифру числа к нашей строке
                    i++; //Переходим к следующему символу

                    if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                }

                output += " "; //Дописываем после числа пробел в строку с выражением
                i--; //Возвращаемся на один символ назад, к символу перед разделителем
            }

            //Если символ - оператор
            if (IsOperator(input[i])) //Если оператор
            {
                if (input[i] == '(') //Если символ - открывающая скобка
                    operStack.Push(input[i]); //Записываем её в стек
                else if (input[i] == ')') //Если символ - закрывающая скобка
                {
                    //Выписываем все операторы до открывающей скобки в строку
                    char s = operStack.Pop();

                    while (s != '(')
                    {
                        output += s.ToString() + ' ';
                        s = operStack.Pop();
                    }
                }
                else //Если любой другой оператор
                {
                    if (operStack.Count > 0) //Если в стеке есть элементы
                        if (GetPriority(input[i]) <= GetPriority(operStack.Peek())) //И если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                            output += operStack.Pop().ToString() + " "; //То добавляем последний оператор из стека в строку с выражением

                    operStack.Push(char.Parse(input[i].ToString())); //Если стек пуст, или же приоритет оператора выше - добавляем операторов на вершину стека

                }
            }
        }

        //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
        while (operStack.Count > 0)
            output += operStack.Pop() + " ";

        return output; //Возвращаем выражение в постфиксной записи
    }

    //Метод, вычисляющий значение выражения, уже преобразованного в постфиксную запись
    static private double Counting(string input)
    {
        double result = 0; //Результат
        Stack<double> temp = new Stack<double>(); //Dhtvtyysq стек для решения

        for (int i = 0; i < input.Length; i++) //Для каждого символа в строке
        {
            //Если символ - цифра, то читаем все число и записываем на вершину стека
            if (Char.IsDigit(input[i]))
            {
                string a = string.Empty;

                while (!IsDelimeter(input[i]) && !IsOperator(input[i])) //Пока не разделитель
                {
                    a += input[i]; //Добавляем
                    i++;
                    if (i == input.Length) break;
                }
                temp.Push(double.Parse(a)); //Записываем в стек
                i--;
            }
            else if (IsOperator(input[i])) //Если символ - оператор
            {
                //Берем два последних значения из стека
                double a = temp.Pop();
                double b = temp.Pop();

                switch (input[i]) //И производим над ними действие, согласно оператору
                {
                    case '+': result = b + a; break;
                    case '-': result = b - a; break;
                    case '*': result = b * a; break;
                    case '/': result = b / a; break;
                    
                }
                temp.Push(result); //Результат вычисления записываем обратно в стек
            }
        }
        return temp.Peek(); //Забираем результат всех вычислений из стека и возвращаем его
    }

    static private bool IsDelimeter(char c)
    {
        if ((" =".IndexOf(c) != -1))
            return true;
        return false;
    }
    static private bool IsOperator(char с)
    {
        if (("+-/*()".IndexOf(с) != -1))
            return true;
        return false;
    }
    static private byte GetPriority(char s)
    {
        switch (s)
        {
            case '(': return 0;
            case ')': return 1;
            case '+': return 2;
            case '-': return 3;
            case '*': return 4;
            case '/': return 4;
           
            default: return 6;
        }
    }
}

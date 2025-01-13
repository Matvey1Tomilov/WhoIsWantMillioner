using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WhoIsWantMillioner
{
    public partial class Form1 : Form
    {
        ///SortedList<int, int> Money = new SortedList<int, int>();
        List<Question> questions = new List<Question>();
        private Random rnd = new Random();
        int level = 0;
        int Money;
        int MoneyLevel;
        Question currentQuestion;
        bool FiftyFifty = true;
        bool MaybeError = true;
        bool HelpX2 = false;
        bool PhoneFriend = true;
        bool HelpPeople = true;
        public Form1()
        {
            InitializeComponent();
           /*ReadFile();*/
            startGame();
        }
        private void ShowQuestion(Question q)
        {
            lblQuestion.Text = q.Text;
            btnAnswerA.Text = q.Answers[0];
            btnAnswerB.Text = q.Answers[1];
            btnAnswerC.Text = q.Answers[2];
            btnAnswerD.Text = q.Answers[3];
        }
        /*private SortedList<int, int> AddsMoney(SortedList<int,int> Money)
        {
            for (int i=0; i < 15 ;i++)
            {
                Money.Add(i + 1, lstLevel.);
            }
        }*/
        private void ShowQuestion(Question q,int help)//50/50
        {
            lblQuestion.Text = q.Text;
            if (help == 4)
            {
                if (q.RightAnswer == 1)//a
                {
                    btnAnswerB.Text = "";
                    btnAnswerC.Text = "";
                }
                else if (q.RightAnswer == 3)//c
                {
                    btnAnswerD.Text = "";
                    btnAnswerA.Text = "";
                }
                else if (q.RightAnswer == 2 )//b
                {
                    btnAnswerC.Text = "";
                    btnAnswerD.Text = "";
                }
                else if (q.RightAnswer == 4 )//d
                {
                    btnAnswerA.Text = "";
                    btnAnswerB.Text = "";
                }
                FiftyFifty = false;
            }
        }
        private Question GetQuestion(int level)//бд
        {
            SQLiteConnection cn = new SQLiteConnection();
            cn.ConnectionString = @"Data Source=WhoWantsToBeAMillionaire.db;Version=3";

            cn.Open();

            var cmd = new SQLiteCommand($@"select * from Questions WHERE Level={level} 
                                            order by Random() LIMIT 1", cn);

            var dr = cmd.ExecuteReader();
            dr.Read();
            Question q = new Question(dr);

            return q;
        }

        private void NextStep()
        {
            Button[] btns = new Button[] { btnAnswerA, btnAnswerB,btnAnswerC, btnAnswerC };
            foreach (Button btn in btns)
                btn.Enabled = true;

            level++;
            currentQuestion = GetQuestion(level);
            ShowQuestion(currentQuestion);
            lstLevel.SelectedIndex = lstLevel.Items.Count - level;
        }
        private void startGame()
        {
            level = 0;
            MessageBox.Show("Выберите несгораемую сумму:");
            NextStep();
        }
        private void btnAnswer_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (currentQuestion.RightAnswer == int.Parse(button.Tag.ToString()))
            {
                if (level == 15)
                {
                    MessageBox.Show("Поздравляем миллионера! Вы выиграли " + Money + " рублей! ");
                    Application.Exit();
                }
                if (HelpX2)
                {
                    button.Text = "";
                    HelpX2 = false;
                    MaybeError = false;
                    NextStep(); 
                }
                NextStep();
            }
            else if (HelpX2)
            {

                button.Text = "";
                HelpX2 = false;
                MaybeError = false;
            }
            else
            {
                MessageBox.Show("Неверный ответ!Верный ответ: "+ currentQuestion.RightAnswer.ToString());
                if (MoneyLevel > Convert.ToInt32(lstLevel.SelectedIndex.ToString().Replace(" ","")))
                {
                    MessageBox.Show("Поздравляем! Вы выиграли "+Money+" рублей!");
                }

                Application.Exit();
               // startGame();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnHelps_Click(object sender, EventArgs e)//подсказки
        {
            Button button = (Button)sender;
            if (int.Parse(button.Tag.ToString()) == 4 && FiftyFifty)
            {
                ShowQuestion(currentQuestion, 4);
                button.Text = "Потрачено";
            }
            if (int.Parse(button.Tag.ToString()) == 6 && MaybeError)
            {
                HelpX2 = true;
                MaybeError = false;
                button.Text = "Потрачено";
            }
            if (int.Parse(button.Tag.ToString()) == 5 && HelpPeople)
            {
                Random rnd = new Random();
                int first,second,third,fourth;
                string Answer="";
       
                first = rnd.Next(53,90) - level * 2;//34
                second = rnd.Next(0, 100 - first) / 2;//33
                third = (100 - first - second )/ 2;//14
                fourth = 100 - first - second - third;
                if(currentQuestion.RightAnswer==1)
                {
                    Answer = "а - "+first+"%"+" b - "+second+"%"+ "\nc - " + third + "%" + " d - " + fourth + "%";
                }
                else if (currentQuestion.RightAnswer == 3)
                {
                    Answer = "а - " + second + "%" + " b - " + first + "%" + "\nc - " + third + "%" + " d - " + fourth + "%";
                }
                else if (currentQuestion.RightAnswer == 2)
                {
                    Answer = "а - " + third + "%" + " b - " + second + "%" + "\nc - " + first + "%" + " d - " + fourth + "%";
                }
                else if (currentQuestion.RightAnswer == 4)
                {
                    Answer = "а - " + fourth + "%" + " b - " + second + "%" + "\nc - " + third + "%" + " d - " + first + "%";
                }
                MessageBox.Show(Answer);
                button.Text = "Потрачено";
                HelpPeople = false;
            }
            if (int.Parse(button.Tag.ToString()) == 7 && PhoneFriend)
            {
                Random rnd = new Random();
                int first;
                string Answer = "";
                first = rnd.Next(64, 95) - level * 2;
                if (currentQuestion.RightAnswer == 1)
                {
                    if (first > 50) { Answer = "Думаю, ответ - a"; }
                    else { Answer = "Думаю, ответ - b"; }
                }
                else if (currentQuestion.RightAnswer == 3)
                {
                    if (first > 50) { Answer = "Думаю, ответ - b"; }
                    else { Answer = "Думаю, ответ - c"; }
                }
                else if (currentQuestion.RightAnswer == 2)
                {
                    if (first > 50) { Answer = "Думаю, ответ - c"; }
                    else { Answer = "Думаю, ответ - d"; }
                }
                else if (currentQuestion.RightAnswer == 4)
                {
                    if (first > 50) { Answer = "Думаю, ответ - d"; }
                    else { Answer = "Думаю, ответ - a"; }
                }
                MessageBox.Show(Answer);
                PhoneFriend = false;
                button.Text = "Потрачено";
            }
        }

        private void btnMoney(object sender, EventArgs e)
        {
            ListBox listbox = (ListBox)sender;
            string money = listbox.SelectedItem.ToString();
            string moneylevel = listbox.SelectedIndex.ToString();
            money= money.Replace(" ","");
            Money = Convert.ToInt32(money);
            MoneyLevel = Convert.ToInt32(moneylevel);
            MessageBox.Show("Вы выбрали "+Money+" рублей ");
        }
    }
}

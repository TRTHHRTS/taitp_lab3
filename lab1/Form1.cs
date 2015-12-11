using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private int begin_value;
        private int end_value;
        private int pixels_count;
        private int measurement_count;
        private int[] my_array;
        private Random rnd;
        private long[,] timeArray1;
        private long[,] timeArray2;
        private long[,] timeArray3;
        private Task task1;
        CancellationTokenSource tokenSource2;
        private CancellationToken ct;

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.ForeColor = Color.Blue;
            toolStripStatusLabel1.Text = "Выполняется";

            if ((Int32.TryParse(textBox1.Text, out begin_value)) &&
                (Int32.TryParse(textBox2.Text, out end_value)) &&
                (Int32.TryParse(textBox3.Text, out pixels_count)) &&
                (Int32.TryParse(textBox4.Text, out measurement_count))&&
                (end_value > begin_value)
                )
            {
                if (begin_value > 0 && end_value > 0 && pixels_count > 0 && measurement_count > 0)
                {
                    if (measurement_count < 3)
                    {
                        measurement_count = 3;
                        textBox4.Text = "3";
                    }
                    button1.Enabled = false;
                    chart1.Series[0].Points.Clear();
                    chart1.Series[1].Points.Clear();
                    chart1.Series[2].Points.Clear();

                    tokenSource2 = new CancellationTokenSource();
                    ct = tokenSource2.Token;
                    var context = TaskScheduler.FromCurrentSynchronizationContext();
                    task1 = Task.Factory.StartNew(() => count_time())
                        .ContinueWith(t =>
                        {
                            toolStripStatusLabel1.ForeColor = Color.Green;
                            toolStripStatusLabel1.Text = "Выполнено";
                            draw_graphic();
                        }, tokenSource2.Token, TaskContinuationOptions.OnlyOnRanToCompletion, context);
                }
                else
                {
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    toolStripStatusLabel1.Text = "Неверно введены данные";
                    MessageBox.Show("Введены неверные данные", "Внимание");
                }
            }
            else
            {
                toolStripStatusLabel1.ForeColor = Color.Red;
                toolStripStatusLabel1.Text = "Неверно введены данные";
                MessageBox.Show("Введены неверные данные","Внимание");
            }

        }

        private int[] randomArray(int len, int beg, int end)
        {
            int[] ran = new int[len];
            int num = 1;
            for (int i = 0; i < len; i++)
            {
                bool exists = true;
                while (exists)
                {
                    num = rnd.Next(beg, end);
                    exists = Array.Exists(ran, element => element.Equals(num));
                }
                ran[i] = num;
            }

            return ran;
        }

        private void count_time()
        {
            try
            {
                rnd = new Random(DateTime.Now.Millisecond);
                timeArray1 = new long[pixels_count, measurement_count + 1];
                timeArray2 = new long[pixels_count, measurement_count + 1];
                timeArray3 = new long[pixels_count, measurement_count + 1];

                Stopwatch timer;

                int step = (end_value - begin_value)/pixels_count;

                for (int b = begin_value, col = 0; b < end_value; b += step, col++)
                {
                    if (checkBox3.Checked)
                    { 

                        for (int j = 0; j < measurement_count; j++)
                        {
                            ct.ThrowIfCancellationRequested();
                            timer = new Stopwatch();

                            my_array = randomArray(b, 1, b * 2);
                            SortedSet<int> dict = new SortedSet<int>();
                            for (int i = 0; i < b; i++)
                            {
                               dict.Add(my_array[i]);
                            } 
                            timer.Start();
                            dict.Contains(rnd.Next(1, b*2));
                            timer.Stop();
                            if (j == 0) timeArray1[col, 0] = b;
                            timeArray1[col, j + 1] = timer.ElapsedTicks;
                        }
                        middle_count(timeArray1, pixels_count, measurement_count);
                    }
                    if (checkBox2.Checked)
                    {

                        for (int j = 0; j < measurement_count; j++)
                        {
                            ct.ThrowIfCancellationRequested();
                            timer = new Stopwatch();
                            my_array = randomArray(b, 1, b * 2);
                            RB_Tree tree = new RB_Tree(my_array[0], this);
                            for (int i = 1; i < b; i++)
                            {
                                tree.RB_Insert(my_array[i]);
                            }  
                            timer.Start();
                            tree.Search(rnd.Next(1, b*2));
                            timer.Stop();
                            if (j == 0) timeArray2[col, 0] = b;
                            timeArray2[col, j + 1] = timer.ElapsedTicks;
                        }
                        middle_count(timeArray2, pixels_count, measurement_count);
                    }
                    if (checkBox1.Checked)
                    {
                        for (int j = 0; j < measurement_count; j++)
                        {
                            ct.ThrowIfCancellationRequested();
                            timer = new Stopwatch();

                            //  
                            timer.Start();
                            //comb_sort(comb_array, b);
                            timer.Stop();
                            if (j == 0) timeArray3[col, 0] = b;
                            timeArray3[col, j + 1] = timer.ElapsedMilliseconds;
                        }
                        middle_count(timeArray3, pixels_count, measurement_count);
                    }

                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Отмена");
                tokenSource2.Dispose();
            }
        }

        private void draw_graphic()
        {
            if (checkBox3.Checked)
            {
                for (int i = 0; i < pixels_count; i++)
                {
                    chart1.Series[0].Points.AddXY(i, timeArray1[i, 1]);
                }
            }

            if (checkBox2.Checked)
            {
                for (int i = 0; i < pixels_count; i++)
                {
                    chart1.Series[1].Points.AddXY(i, timeArray2[i, 1]);
                }
            }
            if (checkBox1.Checked)
            {
                for (int i = 0; i < pixels_count; i++)
                {
                    chart1.Series[2].Points.AddXY(i, timeArray3[i, 1]);
                }
            }
            button1.Enabled = true;
        }

        private static void middle_count(long[,] timeArray1, int pixels_count, int measurement_count)
        {
            for (int i = 0; i < pixels_count; i++)
            {

                long max1;
                long max2;
                int imax1;
                int imax2;
                if (timeArray1[i, 1] >= timeArray1[i, 2])
                {
                    imax1 = 1;
                    imax2 = 2;
                    max1 = timeArray1[i, 1];
                    max2 = timeArray1[i, 2];
                }
                else
                {
                    imax1 = 2;
                    imax2 = 1;
                    max1 = timeArray1[i, 2];
                    max2 = timeArray1[i, 1];
                }

                for (int n = i + 2; n < measurement_count - 1; n++)
                {
                    if (timeArray1[i, n] > max2)
                        if (timeArray1[i, n] > max1)
                        {
                            max2 = max1;
                            imax2 = imax1;
                            max1 = timeArray1[i, n];
                            imax1 = n;
                        }
                        else
                        {
                            max2 = timeArray1[i, n];
                            imax2 = n;
                        }
                }
                long s = 0;
                int count = 0;
                for (int n = 1; n <= measurement_count; n++)
                {
                    if (n != imax1 && n != imax2)
                    {
                        s += timeArray1[i, n];
                        count++;
                    }
                }
                s = (long)s / count;
                timeArray1[i, 1] = s;
            }
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            toolStripStatusLabel1.ForeColor = Color.Red;
            toolStripStatusLabel1.Text = "Отмена";
            tokenSource2.Cancel();

        }

        static private RB_Tree tree;
        private void button3_Click(object sender, EventArgs e)
        {
            rnd = new Random(DateTime.Now.Millisecond);
            toolStripStatusLabel1.ForeColor = Color.Blue;
            toolStripStatusLabel1.Text = "Выполняется";
            int countValue;
            if (int.TryParse(textBox5.Text, out countValue))
                if (countValue > 0)
                {
                    //my_array = randomArray(countValue, 1, countValue * 2);
                    my_array = new[]{3,2,4,9,6,8,7,5};
                    tree = new RB_Tree(my_array[0], this);
                    for (var i = 1; i < countValue; i++)
                    {
                        tree.RB_Insert(rnd.Next(0,40));
                    }
                    tree.printTree();
                }
                else
                {
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    toolStripStatusLabel1.Text = "Неверно введены данные";
                    MessageBox.Show("Введены неверные данные", "Внимание");
                }
      
            else
            {
                toolStripStatusLabel1.ForeColor = Color.Red;
                toolStripStatusLabel1.Text = "Неверно введены данные";
                MessageBox.Show("Количество узлов в дереве должно быть больше 0","Внимание");
            }
            toolStripStatusLabel1.ForeColor = Color.Green;
            toolStripStatusLabel1.Text = "Выполнено";
        }

        private static readonly ManualResetEvent mre = new ManualResetEvent(false);

        private void button4_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.ForeColor = Color.Blue;
            toolStripStatusLabel1.Text = "Выполняется";
            MessageBox.Show("next");
            int addingValue;
            if (int.TryParse(textBox6.Text, out addingValue))
            {
                if (addingValue > 0)
                {
                    if (!tree.Search(addingValue))
                    {
                        Thread myThread = StartTheThread(addingValue); //Создаем новый объект потока (Thread)
                        //tree.RB_Insert(addingValue);
                        //tree.printTree();

                    }
                    else
                    {
                        toolStripStatusLabel1.ForeColor = Color.Red;
                        toolStripStatusLabel1.Text = "Неверно введены данные";
                        MessageBox.Show("В дереве уже есть узел с таким значением", "Внимание");
                    }

                }
                else
                {
                    toolStripStatusLabel1.ForeColor = Color.Red;
                    toolStripStatusLabel1.Text = "Неверно введены данные";
                    MessageBox.Show("Значение узла в дереве должно быть больше 0", "Внимание");
                }
            }
            else
            {
                toolStripStatusLabel1.ForeColor = Color.Red;
                toolStripStatusLabel1.Text = "Неверно введены данные";
                MessageBox.Show("Введены неверные данные", "Внимание");
            }

            toolStripStatusLabel1.ForeColor = Color.Green;
            toolStripStatusLabel1.Text = "Выполнено";
        }

        public Thread StartTheThread(int addingValue)
        {
            var t = new Thread(() => RealStart(addingValue));
            t.Start();
            return t;
        }

        private static void RealStart(int addingValue)
        {
            mre.WaitOne();
            MessageBox.Show("next2");
            tree.RB_Insert(addingValue);
            tree.printTree();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mre.Set();
        }
    }
}

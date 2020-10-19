// Made by Siddhant Tandon 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenHardwareMonitor;
using OpenHardwareMonitor.Hardware;
using System.Windows.Forms.DataVisualization.Charting;
using System.Security.Cryptography;
using System.Windows.Forms.VisualStyles;

namespace CPUTemperatureMonitor
{
    public partial class Form1 : Form
    {

        private float[] sampleArray = new float[100];

        private string[] cpuName = new string[7];
        private float[] cpuTemp = new float[7];
        private float[] cpuClock = new float[8];
        private float[] cpuLoad = new float[7];
        
        private int itr = 0, itr2 = 0, itr3 = 0, histIt = 0;
        private Color[] cellBackCols = new Color[7];

        private int[] timeVals = new int[historyLength];

        private static int historyLength = 100;
        private int gpitr = 0, gpuItr2 = 0;

        private int gpuPower, gpuClock, gpuLoad, gpuTemp;
        private int RAMUsage;

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            int redVal = 0;
            int greenVal = 150;


            redVal = (int)((RAMUsage) * (150f / 100f));
            greenVal = 255 - (int)((RAMUsage) * (170 / 100f));

            Color back = Color.FromArgb(redVal, greenVal, 0);

            panel1.BackColor = back;

            e.Graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, panel1.Width, (int)(panel1.Height * (1 - (0.01 * RAMUsage)))));
        }

        private List<int> cpuHist = new List<int>();
        private List<int> gpuTempHist = new List<int>();

        private Computer thisComputer;

        public Form1()
        {

            InitializeComponent();
            
            thisComputer = new Computer() { CPUEnabled = true, GPUEnabled = true, RAMEnabled = true };

            thisComputer.Open();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Hello World");

            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox3.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox4.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox5.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox6.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox7.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox12.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox8.SelectionAlignment = HorizontalAlignment.Center;

            richTextBox1.ForeColor = Color.White;
            richTextBox2.ForeColor = Color.White;
            richTextBox3.ForeColor = Color.White;
            richTextBox4.ForeColor = Color.White;
            richTextBox5.ForeColor = Color.White;
            richTextBox6.ForeColor = Color.White;
            richTextBox7.ForeColor = Color.White;

            richTextBox1.BackColor = Color.Green;
            richTextBox2.BackColor = Color.Green;
            richTextBox3.BackColor = Color.Green;
            richTextBox4.BackColor = Color.Green;
            richTextBox5.BackColor = Color.Green;
            richTextBox6.BackColor = Color.Green;
            richTextBox7.BackColor = Color.Green;

            textBox16.TextAlign = HorizontalAlignment.Center;
            textBox17.TextAlign = HorizontalAlignment.Center;
            textBox18.TextAlign = HorizontalAlignment.Center;
            textBox19.TextAlign = HorizontalAlignment.Center;
            textBox20.TextAlign = HorizontalAlignment.Center;
            textBox21.TextAlign = HorizontalAlignment.Center;
            textBox12.TextAlign = HorizontalAlignment.Center;

            Color TitleColours = Color.FromArgb(180, 70, 0);

            //chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 6;
            //chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            //chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            
            chart1.ChartAreas["ChartArea1"].AxisX.TitleForeColor = TitleColours;
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "Cores";
            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.ForeColor = Color.FromArgb(180, 20, 0);
            chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.ForeColor = Color.FromArgb(180, 20, 0);
            chart1.ChartAreas["ChartArea1"].AxisY.TitleForeColor = TitleColours;
            chart1.ChartAreas["ChartArea1"].AxisY.Title = "Load (%)";

            chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
            chart1.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            chart1.ChartAreas["ChartArea1"].AxisY.Interval = 20;

            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineWidth =1;


            chart1.Series["CPULoad"].Color = Color.FromArgb(150, 90, 0);


            chart2.ChartAreas["ChartArea1"].AxisX.Maximum = historyLength;
            chart2.ChartAreas["ChartArea1"].AxisY.Maximum = 105;
            chart2.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth = 0;
            chart2.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineWidth = 1;
            chart2.Series["CPUPast"].Color = Color.FromArgb(150, 90, 0);

            chart2.ChartAreas["ChartArea1"].AxisX.TitleForeColor = TitleColours;
            //chart2.ChartAreas["ChartArea1"].AxisX.Title = "Time (s)";
            chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.ForeColor = Color.FromArgb(180, 20, 0);
            chart2.ChartAreas["ChartArea1"].AxisX.Interval = 0;
            chart2.ChartAreas["ChartArea1"].AxisY.LabelStyle.ForeColor = Color.FromArgb(180, 20, 0);
            chart2.ChartAreas["ChartArea1"].AxisY.TitleForeColor = TitleColours;
            chart2.ChartAreas["ChartArea1"].AxisY.Title = "Temperature (Celsius)";


            chart2.ChartAreas["ChartArea2"].AxisX.Maximum = historyLength;
            chart2.ChartAreas["ChartArea2"].AxisY.Maximum = 105;
            chart2.ChartAreas["ChartArea2"].AxisX.MajorGrid.LineWidth = 0;
            chart2.ChartAreas["ChartArea2"].AxisY.MajorGrid.LineWidth = 1;
            chart2.Series["GPUPast"].Color = Color.FromArgb(150, 90, 0);

            chart2.ChartAreas["ChartArea2"].AxisX.TitleForeColor = TitleColours;
            //chart2.ChartAreas["ChartArea2"].AxisX.Title = "Time (s)";
            chart2.ChartAreas["ChartArea2"].AxisX.LabelStyle.ForeColor = Color.FromArgb(180, 20, 0);
            chart2.ChartAreas["ChartArea2"].AxisX.Interval = 0;
            chart2.ChartAreas["ChartArea2"].AxisY.LabelStyle.ForeColor = Color.FromArgb(180, 20, 0);
            chart2.ChartAreas["ChartArea2"].AxisY.TitleForeColor = TitleColours;
            chart2.ChartAreas["ChartArea2"].AxisY.Title = "Temperature (Celsius)";

            for (int r = 0; r < timeVals.Length; r++)
            {
                timeVals[r] = timer1.Interval * (historyLength - r);
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {         
            for (int y = 0; y < sampleArray.Length; y++)
            {
                sampleArray[y] = 7;
            }

            //var hardwareItem = thisComputer.Hardware;
            //hardwareItem.Update();
            foreach (var hardwareItem in thisComputer.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.CPU)
                {
                    hardwareItem.Update();
                    foreach (IHardware subHardware in hardwareItem.SubHardware)
                        subHardware.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            //temp += String.Format("{0} Temperature = {1}\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value");
                            //avgCoreValue += sensor.Value.Value;
                            //cpuName[itr] = sensor.Name;
                            cpuTemp[itr] = sensor.Value.Value;
                            itr++;

                            if (itr == 6)
                            {
                                cpuHist.Add((int)sensor.Value.Value);
                                if (cpuHist.Count > historyLength)
                                {
                                    cpuHist.RemoveAt(0);
                                }
                                //Console.WriteLine(sensor.Value.Value);
                                histIt++;
                            }
                        }

                        if (sensor.SensorType == SensorType.Clock)
                        {
                            //temp += String.Format("{0} Temperature = {1}\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value");
                            //avgCoreValue += sensor.Value.Value;
                            //cpuName[itr] = sensor.Name;
                            cpuClock[itr2] = (int)sensor.Value.Value;
                           // Console.WriteLine(sensor.Value.Value);
                            itr2++;
                        }

                        if (sensor.SensorType == SensorType.Load)
                        {
                            //temp += String.Format("{0} Temperature = {1}\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value");
                            //avgCoreValue += sensor.Value.Value;
                            //cpuName[itr] = sensor.Name;
                            cpuLoad[itr3] = (int)sensor.Value.Value;
                           // Console.WriteLine(sensor.Value.Value);
                            itr3++;
                        }
                    }
                }
                


                if (hardwareItem.HardwareType == HardwareType.GpuNvidia)
                {
                    //Console.WriteLine("GPU HERE");
                    hardwareItem.Update();
                    foreach (IHardware subHardware in hardwareItem.SubHardware)
                        subHardware.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            //gpuTemp += String.Format("{0} Temperature = {1}\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "no value");
                            //Console.WriteLine(sensor.Name);
                            gpuTempHist.Add((int)sensor.Value.Value);
                            gpuTemp = (int)sensor.Value.Value;
                            if (gpuTempHist.Count > historyLength)
                            {
                                gpuTempHist.RemoveAt(0);
                            }

                            int redVal = 0;
                            int greenVal = 150;
                            if (gpuTemp < 45)
                            {
                                redVal = 0;
                            }
                            else
                            {
                                redVal = (int)((gpuTemp - 45f) * (150f / 40f));
                                greenVal = 170 - (int)((gpuTemp - 45f) * (170 / 40f));
                            }
                            richTextBox12.BackColor = Color.FromArgb(redVal, greenVal, 0);
                        }

                        if (sensor.SensorType == SensorType.Power)
                        {
                            gpuPower = (int)sensor.Value.Value;
                            
                        }

                        if (sensor.SensorType == SensorType.Clock)
                        {
                            if (gpitr == 0)
                            {
                                gpuClock = (int)sensor.Value.Value;

                                textBox12.Text = gpuClock.ToString() + " MHz";

                                //Console.WriteLine(gpuClock);
                                
                            }
                            gpitr++;
                        }

                        if (sensor.SensorType == SensorType.Load)
                        {
                            //Console.WriteLine("LOAD  " + sensor.Value.Value.ToString());
                            if (gpuItr2 == 3)
                            {
                                gpuLoad = (int)sensor.Value.Value;
                                richTextBox8.Text = gpuLoad.ToString() + "%";

                                int redVal = 0;
                                int greenVal = 150;


                                redVal = (int)((gpuLoad) * (150f / 100f));
                                greenVal = 170 - (int)((gpuLoad) * (170 / 100f));

                                richTextBox8.BackColor = Color.FromArgb(redVal, greenVal, 0);
                            }
                            gpuItr2++;
                        }

                        

                        //Console.WriteLine(gpuLoad.ToString() + "  " + gpuClock.ToString());
                    }
                }




                if (hardwareItem.HardwareType == HardwareType.RAM)
                {
                    hardwareItem.Update();
                    foreach (IHardware subHardware in hardwareItem.SubHardware)
                        subHardware.Update();

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load)
                        {
                            RAMUsage = (int)sensor.Value.Value;
                        }

                        //Console.WriteLine(gpuLoad.ToString() + "  " + gpuClock.ToString());
                    }
                }

                itr = 0;
                itr2 = 0;
                itr3 = 0;
                gpitr = 0;
                gpuItr2 = 0;

                int[] points = cpuHist.ToArray();
                int[] GPUPoints = gpuTempHist.ToArray();

                //chart2.Series["CPUPast"].Points.Clear();
                chart2.Series["CPUPast"].Points.DataBindY(points);

                chart2.Series["GPUPast"].Points.DataBindY(GPUPoints);
                // Console.WriteLine("cpuHist[histIt]  " + cpuHist[histIt].ToString());
                //if (histIt == historyLength) histIt = 0;

                for (int o = 0; o < cellBackCols.Length; o++)
                {
                    int redVal = 0;
                    int greenVal = 150;
                    if (cpuTemp[o] < 50)
                    {
                        redVal = 0;
                    }
                    else
                    {
                        redVal = (int)((cpuTemp[o] - 50f) * (150f/50f));
                        greenVal = 170 - (int)((cpuTemp[o] - 50f) * (170 / 50f));
                    }
                    cellBackCols[o] = Color.FromArgb(redVal, greenVal, 0);
                }

                richTextBox1.BackColor = cellBackCols[0];
                richTextBox2.BackColor = cellBackCols[1];
                richTextBox3.BackColor = cellBackCols[2];
                richTextBox4.BackColor = cellBackCols[3];
                richTextBox5.BackColor = cellBackCols[4];
                richTextBox6.BackColor = cellBackCols[5];
                richTextBox7.BackColor = cellBackCols[6];

                richTextBox1.Text = cpuTemp[0].ToString();
                richTextBox1.SelectionCharOffset = 10;
                //richTextBox1.Text += "o";
                richTextBox1.SelectionCharOffset = 0;
                richTextBox1.Text += "C";

                richTextBox2.Text = cpuTemp[1].ToString();
                richTextBox2.SelectionCharOffset = 10;
                //richTextBox2.Text += "o";
                richTextBox2.SelectionCharOffset = 0;
                richTextBox2.Text += "C";

                richTextBox3.Text = cpuTemp[2].ToString();
                richTextBox3.SelectionCharOffset = 10;
                //richTextBox3.Text += "o";
                richTextBox3.SelectionCharOffset = 0;
                richTextBox3.Text += "C";

                richTextBox4.Text = cpuTemp[3].ToString();
                richTextBox4.SelectionCharOffset = 10;
                //richTextBox4.Text += "o";
                richTextBox4.SelectionCharOffset = 0;
                richTextBox4.Text += "C";

                richTextBox5.Text = cpuTemp[4].ToString();
                richTextBox5.SelectionCharOffset = 10;
                //richTextBox5.Text += "o";
                richTextBox5.SelectionCharOffset = 0;
                richTextBox5.Text += "C";

                richTextBox6.Text = cpuTemp[5].ToString();
                richTextBox6.SelectionCharOffset = 10;
                //richTextBox6.Text += "o";
                richTextBox6.SelectionCharOffset = 0;
                richTextBox6.Text += "C";

                richTextBox7.Text = cpuTemp[6].ToString();
                richTextBox7.SelectionCharOffset = 10;
                //richTextBox7.Text += "o";
                richTextBox7.SelectionCharOffset = 0;
                richTextBox7.Text += "C";

                textBox16.Text = cpuClock[0].ToString() + " MHz";
                textBox17.Text = cpuClock[1].ToString() + " MHz";
                textBox18.Text = cpuClock[2].ToString() + " MHz";
                textBox19.Text = cpuClock[3].ToString() + " MHz";
                textBox20.Text = cpuClock[4].ToString() + " MHz";
                textBox21.Text = cpuClock[5].ToString() + " MHz";

                chart1.Series["CPULoad"].Points.Clear();
                for (int r = 0; r < cpuLoad.Length-1; r++)
                {
                    chart1.Series["CPULoad"].Points.AddXY(r.ToString(), cpuLoad[r].ToString());
                }

                //12 8
                richTextBox12.Text = gpuTemp.ToString() + "C";

                // Console.WriteLine(gpuClock.ToString() + "////////");

                panel1.Invalidate();
            }

            //textBox1.Text = temp;
            //Console.WriteLine(temp);

            //textBox2.Text = gpuTemp;
            //Console.WriteLine(gpuTemp);
            //avgCoreValue /= 9;
        }
    }
}
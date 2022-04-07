using System;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using MissionPlanner.Utilities;
using MissionPlanner.Controls;
using System.IO;
using System.Threading;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    public partial class MyApplication : UserControl
    {
        string CurrentDirectory;
        string FileDirectory;
        string ComportDirectory;
        bool bRunning;

        /// <summary>
        /// initialize all components here.
        /// </summary>
        public MyApplication()
        {
            InitializeComponent();
            getavailableports();
            InitializeBaudrate();
            InitializeParity();
            InitializeDatabit();
            InitializeStopbit();
            //checkConnection();
            InitializeDirectories();
        }

        /// <summary>
        /// initialize directories and file contents here.
        /// </summary>
        private void InitializeDirectories()
        {
            CurrentDirectory = Directory.GetCurrentDirectory();
            FileDirectory = CurrentDirectory + "\\MyFile.txt";
            ComportDirectory = CurrentDirectory + "\\MyComport.txt";

            if (checkDirectory(ComportDirectory) == false){                                     // if MyComport.txt does not exist, create one
                using (FileStream fs = File.Create(ComportDirectory)){                          // create a new file for MyComport.txt
                    Byte[] title = new UTF8Encoding(true).GetBytes("COM1,115200,None,8,1");     // add some text to file
                    fs.Write(title, 0, title.Length);
                }
            }

            if (checkDirectory(FileDirectory) == false){                                        // if MyFile.txt does not exist, create one                    
                using (FileStream fs = File.Create(FileDirectory)){                             // create a new file 
                    Byte[] title = new UTF8Encoding(true).GetBytes("0,0,0,0,0,false");          // Add some text to file
                    fs.Write(title, 0, title.Length);
                }
            }

            string AllComportText = System.IO.File.ReadAllText(ComportDirectory);               // if MyFile.txt is empty, add one
            string[] comportWords = AllComportText.Split(',');
            if (comportWords.Length < 5)
                System.IO.File.WriteAllText(FileDirectory, 
                    g + "," + g1 + "," + g2 + "," + g3 + "," + g4 + "," + "false");
        }

        /// <summary>
        /// check directory either it exists or not. return true = exist, false = not exist.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool checkDirectory(string path)
        {
            bool exist = false;

            if(File.Exists(path) == true)
                exist = true;
            else
                exist = false;

            return exist;
        }

        /// <summary>
        /// destroy components here.
        /// </summary>
        ~MyApplication()
        {
            
        }

        /// <summary>
        /// load components here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyApplication_Load(object sender, EventArgs e)
        {
            string AllComportText = System.IO.File.ReadAllText(ComportDirectory);
            string AllFileText = System.IO.File.ReadAllText(FileDirectory);

            string[] comportWords = AllComportText.Split(',');
            string[] fileWords = AllFileText.Split(',');

            if (fileWords.ElementAt(0) == "true")
            {
                bRunning = true;
                comboComport.Text = comportWords[0];
                comboBaudrate.Text = comportWords[1];
                comboDatabit.Text = comportWords[2];
                comboParity.Text = comportWords[3];
                comboStopbit.Text = comportWords[4];
                System.IO.File.WriteAllText(FileDirectory,
                    g + "," + g1 + "," + g2 + "," + g3 + "," + g4 + "," + "true");
            }
            else
            {
                bRunning = false;
                System.IO.File.WriteAllText(FileDirectory,
                    g + "," + g1 + "," + g2 + "," + g3 + "," + g4 + "," + "false");
            }

            if (comportWords.Length > 4)
            {
                comboComport.Text = comportWords[0];
                comboBaudrate.Text = comportWords[1];
                comboDatabit.Text = comportWords[2];
                comboParity.Text = comportWords[3];
                comboStopbit.Text = comportWords[4];
            }
            else
            {
                bRunning = false;
            }
        }

        private string ReceivedData()
        {
            g = com.RPMData();

            g1 = com.TPData();

            g2 = com.TemperatureData();

            g3 = com.BattData();

            g4 = com.RawData();

            // para.Inlines.Add(g);
            // mcFlowDoc.Blocks.Add(para);
            //RecievedData.Document = mcFlowDoc;

            // ReceivedData.Text = "Result: "+g;


            // **** Engine RPM **** //
            textboxEngineSpeed.Text = g;                    // show on textbox

            // **** Throtle Position **** //
            textboxThrottlePosition.Text = g1;

            // **** Engine Temperature **** //
            textboxEngineTemperature.Text = g2;

            // **** Battery Voltage **** //
            textboxBatteryVoltage.Text = g3; ;

            // **** Raw Data **** //
            textboxReceivedData.Text = g4;

            // write to MyFile.txt
            System.IO.File.WriteAllText(FileDirectory,
                    g + "," + g1 + "," + g2 + "," + g3 + "," + g4 + "," + "true");

            return g;
        }

        private void Check()
        {
            bool chk;
            chk = com.CheckPort();
            if (chk == false)
            {
                MessageBox.Show("Disconnected!!!");
                Application.Exit();
                bRunning = false;
                System.IO.File.WriteAllText(FileDirectory, "0,0,0,0,0,false");
            }
            else
            {
                
                //? Process.Start(Application.ResourceAssembly.Location);
                //? Application.Current.Shutdown();
            }
        }

        public void dtcker(object sender, EventArgs e)
        {
            ReceivedData();
            Check();
        }

        private void setComport(string comport, string baudrate, 
            string paritybit, string databit, string stopbit)
        {
            switch (baudrate)
            {
                case "9600":
                    com.BaudRateA();
                    break;

                case "11200":
                    com.BaudRateB();
                    break;

                case "19200":
                    com.BaudRateC();
                    break;

                case "115200":
                    com.BaudRateD();
                    break;
            }

            switch (paritybit)
            {
                case "Even":
                    com.ParityA();
                    break;

                case "Odd":
                    com.ParityB();
                    break;

                case "None":
                    com.ParityC();
                    break;

                case "Space":
                    com.ParityD();
                    break;
            }

            switch (databit)
            {
                case "4":
                    com.DataBitsA();
                    break;

                case "5":
                    com.DataBitsB();
                    break;

                case "6":
                    com.DataBitsC();
                    break;

                case "7":
                    com.DataBitsD();
                    break;

                case "8":
                    com.DataBitsE();
                    break;
            }

            switch (stopbit)
            {
                case "0":
                    com.StopBitsA();
                    break;

                case "1":
                    com.StopBitsB();
                    break;

                case "2":
                    com.StopBitsC();
                    break;
            }
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            bool bopen = false;
            bool bclose = false;
            string Comport;
            string Baudrate;
            string Databit;
            string Paritybit;
            string Stopbit;

            if ((comboComport.Text != "") && (comboBaudrate.Text != "") && (comboParity.Text != "") &&
                (comboDatabit.Text != "") && (comboStopbit.Text != ""))
            {
                Comport = comboComport.Text;
                Baudrate = comboBaudrate.Text;
                Paritybit = comboParity.Text;
                Databit = comboDatabit.Text;
                Stopbit = comboStopbit.Text;

                com.portname(Comport);

                bopen = com.OpenPort();
                //bclose = com.ClosePort();
                if (bopen == true)
                //if(bclose == false)
                {
                    dispatcherTimer.Interval = TimeSpan.FromTicks(1000);//TimeSpan(0, 0, 1);//;
                    //dispatcherTimer.Tick -= dtcker;
                    dispatcherTimer.Tick += dtcker;
                    dispatcherTimer.Start();
                    buttonSet.Enabled = false;
                    buttonClose.Enabled = true;

                    //asiah
                    CustomMessageBox.Show("Com port is set!");
                    comboComport.Enabled = false;
                    comboBaudrate.Enabled = false;
                    comboParity.Enabled = false;
                    comboDatabit.Enabled = false;
                    comboStopbit.Enabled = false;
                }
                else if(bopen == false)
                {
                    string AllComportText = System.IO.File.ReadAllText(ComportDirectory);
                    string[] words = AllComportText.Split(',');

                    if (words[0] == comboComport.Text)
                    {
                        comboComport.Enabled = false;
                        comboBaudrate.Enabled = false;
                        comboDatabit.Enabled = false;
                        comboParity.Enabled = false;
                        comboStopbit.Enabled = false;
                        buttonClose.Enabled = true;
                        buttonSet.Enabled = false;
                        CustomMessageBox.Show("Comport is already open!");
                    }
                    else
                    {
                        buttonSet.Enabled = true;
                        buttonClose.Enabled = false;
                        comboComport.Enabled = true;
                        comboBaudrate.Enabled = true;
                        comboParity.Enabled = true;
                        comboDatabit.Enabled = true;
                        comboStopbit.Enabled = true;
                        CustomMessageBox.Show("Please check serial com port connection!");
                    }
                }

                setComport(Comport, Baudrate, Paritybit, Databit, Stopbit);
                System.IO.File.WriteAllText(ComportDirectory,
                    Comport + "," + Baudrate + "," + Paritybit + "," + Databit + "," + Stopbit);
            }
            else
                CustomMessageBox.Show("Please select all the COM Port Settings");
        }

        // asiah
        private void buttonClose_Click(object sender, EventArgs e)
        {
            bool close = false;
            dispatcherTimer.Tick += dtcker;
            dispatcherTimer.Tick -= dtcker;
            dispatcherTimer.Stop();

            //Process.
            //Application.ExitThread();
            //dispatcherTimer.Stop();
            //dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            close = com.ClosePort();

            if (close == true)
            {
                bRunning = false;
                this.buttonSet.Enabled = true;
                this.buttonClose.Enabled = false;
                CustomMessageBox.Show("Com port is close!");

                comboComport.Enabled = true;
                comboBaudrate.Enabled = true;
                comboParity.Enabled = true;
                comboDatabit.Enabled = true;
                comboStopbit.Enabled = true;

                System.IO.File.WriteAllText(FileDirectory, "0,0,0,0,0,false");
            }
            else
            {
                bRunning = true;
                this.buttonSet.Enabled = false;
                this.buttonClose.Enabled = true;
                CustomMessageBox.Show("Com port is still open!");

                comboComport.Enabled = false;
                comboBaudrate.Enabled = false;
                comboParity.Enabled = false;
                comboDatabit.Enabled = false;
                comboStopbit.Enabled = false;

                System.IO.File.WriteAllText(FileDirectory, "0,0,0,0,0,true");
            }

        }
    }
}

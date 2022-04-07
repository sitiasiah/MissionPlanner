using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows;
using System.Collections;

namespace RS232._1
{  
    public class comm1
    {
        [Serializable] // BinaryFormatter needs this attribute
        public struct DATARESULT1
        {
            public ushort reserved1;
            public ushort save_in_flash;
            public ushort reserved2;
            public ushort engine_status;
          //  public ushort RPM;
            public ushort reserved3;
            public ushort time_after_start_till_end_warmup;
            public ushort reserved4;
            public ushort idling_actuator;
            public ushort num_interference_pulses;
            public ushort air_mass;
            public ushort boost_pressure_output;
            public ushort number_rpm_errors;
            public ushort injection_time;
            public ushort ignition_angle;
            public ushort reserved5;
            public ushort voltage_throttle_valve;
            public ushort battery_voltage_1;
            public ushort voltage_lambda_sensor;
            public ushort voltage_engine_temp;
            public ushort voltage_air_temp;
            public ushort voltage_ext_air_pressure_sensor;
            public ushort voltage_int_air_pressure_sensor;
            public ushort voltage_air_mass_sensor;
            public int ilambdaVal;
            public ushort reserved6;
            public int ithrottle_valve;
            public int iengine_temp;
            public int ibattery_voltage_2;
            public int iair_temp;
            public int iair_pressure;
            public ushort sensor_OK;
            public double rpm;
            public float throttle_pos;
            public double EngineTemp;
            public double battVoltage;

        }

        [Serializable] // BinaryFormatter needs this attribute
        public struct DATARESULT2
        {
            public ushort reserved1;
            public int qty_air_mass;
            public int qty_air_mass_correction_map;
            public int qty_basic_map;
            public int intake_manifold_vacuum_map;
            public int base_qty;
            public int qty_air_correction;
            public int qty_idle_actuator;
            public int qty_warmup_curve;
            public int qty_acceleration_enrichment;
            public int switch_on_time_electronic;
            public int qty_race_switch;
            public int qty_lambda_control;
            public int angle_ignition_map;
            public int angle_air_temp_map;
            public int angle_air_pressure_map;
            public int angle_eng_temp_curve;
            public int angle_acceleration;
            public int angle_race_switch;
            public long total_time_26ms;
            public long total_num_revolutions;
            public ushort fuel_consumption;
            public ushort num_error_memory;
            public ushort reserved2;
        }

       

        SerialPort serial = new SerialPort();
        int received_data = 0;
        string strTemp = "";
        byte[] bytesSend;
        string tempValue; //serial rawdata

        DATARESULT1 dataResult = new DATARESULT1();
        DATARESULT2 dataResult2 = new DATARESULT2();

        public List<string> GetAvailablePorts()
        {
            List<string> w = new List<string>();
            try
            {  
                foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                {
                    try
                    {
                        //serial.PortName = s;
                        //serial.Open();
                        w.Add(s);
                    }
                    catch { }

                    //if (serial.IsOpen)
                    //{
                    //    serial.Close();
                    //    w.Add(s);
                    //}

                    //else
                    //{                   
                    //}
                }
            }
            catch { }           
            return w;
        }

        /// <summary>
        /// 
        /// </summary>
        public void portname(string h)
        {
            if (h != "")
                serial.PortName = h;
            else
                serial.PortName = "";
        }

        public bool OpenPort()
        {        
            bool ret = false;
            try
            {
                serial.Open();
            }
            catch (UnauthorizedAccessException) { ret = false; }
            catch (System.IO.IOException) { ret = false; }
            catch (ArgumentException) { ret = false; }

            if (ret)
            {
            }

            if (serial.IsOpen)
            {
                serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);
                ret = true;
            }
            else
            {
            }
            return ret;
        }

        public bool CheckPort()
        {
            bool rot = false;
            if (serial.IsOpen == true)
            {
                rot = true;
            }
            else if(serial.IsOpen == false)
            {
                rot = false;
            }
            return rot;
        }

        //asiah
        public bool ClosePort()
        {
            serial.Close();
            //serial.Dispose();

            if (serial.IsOpen == false)
                return true;    // comport is 'true'ly closed
            else
                return false;   // comport is still open
        }

        public void BaudRateA()
        {
            serial.BaudRate = 9600;
        }

        public void BaudRateB()
        {
            serial.BaudRate = 11200;
        }

        public void BaudRateC()
        {
            serial.BaudRate = 19200;
        }

        public void BaudRateD()
        {
            serial.BaudRate = 115200;
        }

        public void ParityA()
        {
            serial.Parity = Parity.Even;
        }

        public void ParityB()
        {
            serial.Parity = Parity.Odd;
        }

        public void ParityC()
        {
            serial.Parity = Parity.None;
        }

        public void ParityD()
        {
            serial.Parity = Parity.Space;
        }

        public void DataBitsA()
        {
            serial.DataBits = 4;
        }

        public void DataBitsB()
        {
            serial.DataBits = 5;
        }

        public void DataBitsC()
        {
            serial.DataBits = 6;
        }

        public void DataBitsD()
        {
            serial.DataBits = 7;
        }
      
        public void DataBitsE()
        {
            serial.DataBits = 8;
        }

        public void StopBitsA()
        {
            serial.StopBits = StopBits.None;
        }

        public void StopBitsB()
        {
            serial.StopBits = StopBits.One;
        }

        public void StopBitsC()
        {
            serial.StopBits = StopBits.Two;
        }

        #region Sending
        //public void SerialCmdSend(string data)
        //{
        //    if (serial.IsOpen)
        //    {
        //        try
        //        {

        //            //UserDataStruct userData = new UserDataStruct();



        //           // userData.code2 = data;
                 

        //            // Send the binary data out the port
        //            byte[] hexstring = Encoding.ASCII.GetBytes(data);
        //            //There is a intermitant problem that I came across
        //            //If I write more than one byte in succesion without a 
        //            //delay the PIC i'm communicating with will Crash
        //            //I expect this id due to PC timing issues ad they are
        //            //not directley connected to the COM port the solution
        //            //Is a ver small 1 millisecound delay between chracters
        //            foreach (byte hexval in hexstring)
        //            {
        //                byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
        //                serial.Write(_hexval, 0, 1);
        //                Thread.Sleep(1);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }  
        //    else
        //    {
        //    }
        //}

        public void SerialCmdByteSend()
        {
            if (serial.IsOpen)
            {

                byte[] byteMsg = new byte[3];

                byteMsg[0] = 3;
                byteMsg[1] = 4;
                byteMsg[2] = 249;

                try
                {
          
                 serial.Write(byteMsg, 0, 3);
                      
                    
                }
                catch (Exception)
                {
                }
            }
            else
            {
            }
        }
        #endregion

        #region Recieving


     
        public void Recieve(object sender,SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(100);

            tempValue = serial.ReadExisting();

            string[] strArr = tempValue.Split('$');

            for (int i = 0; i < strArr.Length; i++)
            {
                string strTemp = strArr[i];
                string[] lineArr = strTemp.Split(',');

                float newValue;
                float newValue2;
                float newValue3;

                float num;

                //switch(lineArr[0])
                //{
                //    case "770":
                //        dataResult.rpm = Convert.ToDouble(lineArr[1]);
                //        break;

                //    case "771":
                //        for (int t = 1; t < 3; i++)
                //        {
                //            float newValue;
                //            float newValue2;
                //            float num;

                //            if(i == 1)
                //            {

                //                num = Int32.Parse(lineArr[i], System.Globalization.NumberStyles.HexNumber);

                //                newValue = (num * 5) / 1024;

                //                dataResult.battVoltage = newValue;

                //            }
                //            else if (i == 2)
                //            {
                //                newValue2 = Int32.Parse(lineArr[i], System.Globalization.NumberStyles.HexNumber);

                //                dataResult.throttle_pos = newValue2;

                //            }



                //        }
                //        break;

                //    case "T":
                //        //extract Temp
                //        dataResult.EngineTemp = Convert.ToDouble(lineArr[1]);
                //        break;




                //        //if(lineArr[])
                //        //{
                //        //    int num = Int32.Parse(lineArr[1], System.Globalization.NumberStyles.HexNumber);
                //        //    float newValue;

                //        //    newValue = (num * 5) / 1024;

                //        //    dataResult.throttle_pos = newValue;

                //        //    //extract TP

                //        //}
                //        //catch (Exception ex)
                //        //{
                //        //    Console.WriteLine(ex.Message.ToString());
                //        //}



                //}

                //extract RPM
                if (lineArr[0] == "770")
                {
                    try
                    {
                        //if(lineArr[1] == ",")
                        //{

                        //}
                        newValue3 = Int32.Parse(lineArr[1], System.Globalization.NumberStyles.HexNumber);
                        dataResult.rpm = Convert.ToDouble(newValue3);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                }

                ////extract Throttle pos/BattVolt
                if (lineArr[0] == "771")
                {
                    //try
                    //{
                    //if (i == 1)
                    //{ //extract Batt Volt
                    try
                    {

                        num = Int32.Parse(lineArr[1], System.Globalization.NumberStyles.HexNumber);

                        newValue = (num * 5) / 1024;

                        dataResult.battVoltage = Math.Round(newValue, 2);

                        newValue2 = Int32.Parse(lineArr[2], System.Globalization.NumberStyles.HexNumber);

                        dataResult.throttle_pos = (float)(newValue2 * 0.1);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                        //}

                        //else (i == 2)
                        //{
                            //extract TP
                            //newValue2 = Int32.Parse(lineArr[2], System.Globalization.NumberStyles.HexNumber);

                            //dataResult.throttle_pos = (float)(newValue2 * 0.1);
                       // }


                        //extract Batt Volt
                        //num = Int32.Parse(lineArr[1], System.Globalization.NumberStyles.HexNumber);

                        //newValue = (num * 5) / 1024;


                        //new Decimal(1230000000, 0, 0, true, 7);
                        //dataResult.battVoltage = Math.Round(newValue, 2);



                        ////extract TP
                        //newValue2 = Int32.Parse(lineArr[2], System.Globalization.NumberStyles.HexNumber);

                        //dataResult.throttle_pos = (float)(newValue2 * 0.1);


                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message.ToString());
                    //}                   
                }
                //extract Engine Temp
                if (lineArr[0] == "T")
                {
                    try
                    {
                        //extract Temp
                        dataResult.EngineTemp = Convert.ToDouble(lineArr[1]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                }

            }


            Console.WriteLine("");


      
            
        }

        public string RawData()
        {
            string data;
            data = tempValue;
            return data;
        }
             
        public string RPMData()
        {
            string data;
            data = dataResult.rpm.ToString();
            return data;
        }

        public string TPData()
        {
            string data;
            data = dataResult.throttle_pos.ToString();
            return data;
        }

        public string TemperatureData()
        {
            string data;
            data = dataResult.EngineTemp.ToString();
            return data;
        }

        public string BattData()
        {
            string data;
            data = dataResult.battVoltage.ToString();
            return data;
        }
    }
    #endregion

   
}



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO.Ports;
namespace SerialCommunicate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }
//打开端口
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text,10);//十进制数据转换
                serialPort1.Open();
                button1.Enabled = false;//打开串口按钮不可用
                button2.Enabled = true;//关闭串口
            }
            catch {
                MessageBox.Show("端口错误,请检查串口", "错误");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*for (int i = 1; i < 20; i++)
            {
                comboBox1.Items.Add("COM" + i.ToString());
            }
            comboBox1.Text = "COM1";//串口号默认值*/
            comboBox2.Text = "9600";//波特率默认值
            comboBox3.Text = "COM11";//串口号默认值*/
            comboBox4.Text = "9600";//波特率默认值

            /*****************非常重要************************/

            serialPort1.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);//必须手动添加事件处理程序

        }
//接收数据
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)//串口数据接收事件
        {
            if (!radioButton3.Checked)//如果接收模式为字符模式
            {
                string str = serialPort1.ReadExisting();//字符串方式读
                textBox1.AppendText(str);//添加内容
            }
            else
            { //如果接收模式为数值接收
              // byte data;  
              //data = (byte)serialPort1.Read();//此处需要强制类型转换，将(int)类型数据转换为(byte类型数据，不必考虑是否会丢失数据
              //一次显示全部数据，显示一次换行
                int bytesToRead = serialPort1.BytesToRead;//获取缓冲区中的字节数
                byte[] BRecieve = new byte[bytesToRead];
               int[] intrecieve = new int[bytesToRead];
                int bytesRead = 0;
                string intstr;
                string temp25 = null;//临时变量
                string temp10 = null;
                serialPort1.Read(BRecieve, 0, bytesToRead);//从第0位开始，读bytesToRead个字节至BRecieve
                for (int i = 0; i <bytesToRead; i++)
                {
                    string str = Convert.ToString(BRecieve[i], 16).ToUpper();//转换为大写十六进制字符串  
                    textBox1.AppendText((str.Length == 1 ? "0" + str : str) + " ");//空位补“0” 显示原16进制数据
                    intrecieve[i] = Convert.ToInt32(BRecieve[i]);
                    intstr = Convert.ToString(intrecieve[i]);//存储转换之后的十进制值
                    temp25 = Convert.ToString((double)(intrecieve[3]*256+ intrecieve[2])/10);
                   temp10= Convert.ToString((double)(intrecieve[5] * 256 + intrecieve[4]) / 10);
                    //textBox2.AppendText((intstr.Length == 1 ? "0" + intstr : intstr) + " ");

                }
               
                textBox1.AppendText("\n");//换行
                textBox1.AppendText("The Concentration of PM2.5 is:" + temp25+"ug/m3");
                textBox1.AppendText("The Concentration of PM10 is:" + temp10 + "ug/m3");
                textBox1.AppendText("\n");
                textBox2.Clear();
                textBox2.AppendText( "PM2.5:"+temp25 + "ug/m3");
                textBox2.AppendText("\n");
                textBox2.AppendText( "PM10:"+temp10 + "ug/m3");
                if (serialPort2.IsOpen)
                {
                    serialPort2.WriteLine(textBox2.Text);//写数据
                    serialPort2.WriteLine("\r\n");
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Close();//关闭串口
                button1.Enabled = true;//打开串口按钮可用
                button2.Enabled = false;//关闭串口按钮不可用
            }
            catch (Exception err)//一般情况下关闭串口不会出错，所以不需要加处理程序
            {

            }
        }
//发送数据
        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            //byte[] Data = new byte[1];//作用同上集
            //if (serialPort2.IsOpen)//判断串口是否打开，如果打开执行下一步操作
            //{
            //    if (textBox2.Text != "")
            //    {
            //        if (!radioButton1.Checked)//如果发送模式是字符模式
            //        {
            //            try
            //            {
            //                serialPort2.WriteLine(textBox2.Text);//写数据
            //                serialPort2.WriteLine("\r\n");
            //            }
            //            catch (Exception err)
            //            {
            //                MessageBox.Show("串口数据写入错误", "错误");//出错提示
            //                serialPort2.Close();
            //                button7.Enabled = true;//打开串口按钮可用
            //                button6.Enabled = false;//关闭串口按钮不可用
            //            }
            //        }
            //        else
            //        {
            //            for (int i = 0; i < (textBox2.Text.Length - textBox2.Text.Length % 2) / 2; i++)//取余3运算作用是防止用户输入的字符为奇数个
            //            {
            //                Data[0] = Convert.ToByte(textBox2.Text.Substring(i * 2, 2), 16);
            //                serialPort2.Write(Data, 0, 1);//循环发送（如果输入字符为0A0BB,则只发送0A,0B）
            //            }
            //            if (textBox2.Text.Length % 2 != 0)//剩下一位单独处理
            //            {
            //                Data[0] = Convert.ToByte(textBox2.Text.Substring(textBox2.Text.Length-1, 1), 16);//单独发送B（0B）
            //                serialPort2.Write(Data, 0, 1);//发送
            //            }
            //       }
              //  }
          //  }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

//传感器扫描端口
        private void 扫描端口_Click(object sender, EventArgs e)
        {
            SearchAndAddSerialToComboBox(serialPort1, comboBox1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void SearchAndAddSerialToComboBox(SerialPort MyPort, ComboBox MyBox)
        {                                                               //将可用端口号添加到ComboBox
            //string[] MyString = new string[20];                         //最多容纳20个，太多会影响调试效率
            string Buffer;                                              //缓存
            MyBox.Items.Clear();                                        //清空ComboBox内容
            //int count = 0;
            for (int i = 1; i < 20; i++)                                //循环
            {
                try                                                     //核心原理是依靠try和catch完成遍历
                {
                    Buffer = "COM" + i.ToString();
                    MyPort.PortName = Buffer;
                    MyPort.Open();                                      //如果失败，后面的代码不会执行
                                                                        // MyString[count] = Buffer;
                    MyBox.Items.Add(Buffer);                            //打开成功，添加至下俩列表
                    MyPort.Close();                                     //关闭
                    //count++;
                }
                catch
                {
                    //count--;
                }
            }
            //MyBox.Text = MyString[0];                                   //初始化
        }
//下位机扫描端口
        private void button4_Click(object sender, EventArgs e)
        {
            SearchAndAddSerialToComboBox(serialPort2, comboBox3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
//下位机打开关闭串口
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort2.PortName = comboBox3.Text;
                serialPort2.BaudRate = Convert.ToInt32(comboBox4.Text, 10);//十进制数据转换
                serialPort2.Open();
                button7.Enabled = false;//打开串口按钮不可用
                button6.Enabled = true;//关闭串口
            }
            catch
            {
                MessageBox.Show("端口错误,请检查串口", "错误");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort2.Close();//关闭串口
                button7.Enabled = true;//打开串口按钮可用
                button6.Enabled = false;//关闭串口按钮不可用
            }
            catch (Exception err)//一般情况下关闭串口不会出错，所以不需要加处理程序
            {

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

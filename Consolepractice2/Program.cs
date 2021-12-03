using System;
using System.Text;

namespace Consolepractice2
{
    internal class Program
    {
        private static byte[] m_PacketData;
        private static uint m_Pos;

        public static void Main(string[] args)
        {
            m_PacketData = new byte[1024];
            m_Pos = 0;
            int iiii = 109;
            Write(iiii);
            float ffff = 109.99f;
            Write(ffff);
            string ssss = "Hello";
            Write(ssss);

            Console.Write($"Output Byte array(length:{m_Pos}): ");
            for (var i = 0; i < m_Pos; i++)
            {
                Console.Write(m_PacketData[i] + ", ");
                
                if (i == m_Pos-1)
                {
                    Console.WriteLine("\n");
                    Console.WriteLine(iiii);
                    Console.WriteLine(ffff);
                    Console.WriteLine(ssss);
                }
            }
            if(m_Pos<=4)
            {
                var buffer = new byte[m_Pos];
                var request = Encoding.ASCII.GetString(buffer).Substring(0, buffer.Length);
                Console.WriteLine(request);
            }
            

        }

        // write an integer into a byte array
        
        
        public static void Write(int i)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(i);
            _Write(bytes);
            Console.WriteLine(i);
            _Read(bytes);

        }

        // write a float into a byte array
        public static void Write(float f)
        {
            // convert int to byte array
            var bytes = BitConverter.GetBytes(f);
            _Write(bytes);
            Console.WriteLine(f);
            _Read(bytes);
        }

        // write a string into a byte array
        public static void Write(string s)
        {
            // convert string to byte array
            var bytes = Encoding.Unicode.GetBytes(s);

            // write byte array length to packet's byte array
           

            _Write(bytes);
            Console.WriteLine(s);
            _Read(bytes);
        }

        // write a byte array into packet's byte array
        private static void _Write(byte[] byteData)
        {
            //converter little-endian to network's big-endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteData);
            }

            byteData.CopyTo(m_PacketData, m_Pos);
            m_Pos += (uint)byteData.Length;
        }

        public static void _Read(byte[] byteData)
        {
            byteData.CopyTo(m_PacketData, m_Pos);
            Console.WriteLine(m_Pos);
            //while (true)
            //{
            //    if (m_Pos != 0 && m_Pos < 4)
            //    {
            //        Console.WriteLine(byteData);
            //    }
            //    if (m_Pos != 0 && m_Pos < 8)
            //    {
            //        Console.WriteLine(byteData.ToString());
            //    }

            //}
        }

    }
}

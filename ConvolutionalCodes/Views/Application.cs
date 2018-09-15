using ConvolutionalCodes.Entities;
using System;
using System.Text;
using System.Windows.Forms;

namespace ConvolutionalCodes
{
    public partial class Application : Form
    {
        public Application()
        {
            InitializeComponent();
            IBitStream bitsStream = new BitStream(Encoding.UTF8.GetBytes("YZa"));
            int count = 0;
            foreach (var bit in bitsStream.ReadAllBits())
            {
                if (count++ % 8 == 0)
                {
                    Console.Write(" ");
                }
                Console.Write(bit);
            }
        }
    }
}

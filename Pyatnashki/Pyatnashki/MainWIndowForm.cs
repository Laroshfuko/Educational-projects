using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pyatnashki
{
    public partial class mainWindow : Form
    {
        Game game;
        int count = 0;

        public mainWindow()
        {
            InitializeComponent();
            game = new Game(4);    
        }

        private void button16_Click(object sender, EventArgs e)
        {
            int position = Convert.ToInt32(((Button)sender).Tag);
            if (game.Shift(position))
            {
                count++;
                clickCounter.Text = count.ToString();
            }
            Refresh();
            if (game.EndGame())
                MessageBox.Show("Вы победили!!!", "Конец игры");
        }

        private Button ButtonPlace(int position)
        { 
        switch (position)
        {
            case 0: return button0;
            case 1: return button1;
            case 2: return button2;
            case 3: return button3;
            case 4: return button4;
            case 5: return button5;
            case 6: return button6;
            case 7: return button7;
            case 8: return button8;
            case 9: return button9;
            case 10: return button10;
            case 11: return button11;
            case 12: return button12;
            case 13: return button13;
            case 14: return button14;
            case 15: return button15;
            default: return null;
        }

        }

        private void menuStart_Click(object sender, EventArgs e)
        {
            game.Start();
            clickCounter.Text = "";
            count = 0;
            for (int i = 0; i < 100; i++)
                game.RandomShift();
                Refresh();
        }

        private void Refresh()
        {

            for (int position = 0; position < 16; position++)
            {
                int number = game.GetNumber(position);
                ButtonPlace(position).Text = number.ToString();
                ButtonPlace(position).Visible = (number > 0);
            }
        }

        private void mainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}

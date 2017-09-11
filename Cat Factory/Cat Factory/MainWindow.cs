using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable IDE1005 // L'appel de délégué peut être simplifié.

namespace Cat_Factory
{
    public partial class MainWindow : Form
    {
        #region Variables
        private double _totalMilk;
        private const double FACTOR = 1.10;
        SoundPlayer MainMusic;
        List<Kitty> _listKitties;
        private double MPS;
        Thread UpdateMPS;
        Thread EraseError;
        delegate void StringArgReturningVoidDelegate(string text);

        //SEMAPHORES
        private Semaphore SemTotalMilk;
        private Semaphore SemMilkCount;

        //TOOLTIPS
        ToolTip TTKitty;
        ToolTip TTFKitty;
        ToolTip TTBKitty;
        ToolTip TTAKitty;
        ToolTip TTALKitty;
        ToolTip TTAGKitty;
        #endregion

        #region Properties
        public double TotalMilk
        {
            get { return _totalMilk; }
            set
            {
                _totalMilk = value;

                SemMilkCount.WaitOne();
                SetMilkCount(FormatNumber(_totalMilk));
                SemMilkCount.Release();
            }
        }
        #endregion

        #region Initialisations
        public MainWindow()
        {
            InitializeComponent();

            SemTotalMilk = new Semaphore(1, 1);
            SemMilkCount = new Semaphore(1, 1);

            UpdateMPS = new Thread(ThreadTotalMilk);
            UpdateMPS.Name = "Thread Total Milk";
            UpdateMPS.Start();

            _listKitties = new List<Kitty>();
            _listKitties.Add(new Kitty("Kitty", 10, 1)); // 0
            _listKitties.Add(new Kitty("Farmer Kitty", 90, 9)); // 1
            _listKitties.Add(new Kitty("Business Kitty", 300, 20)); // 2
            _listKitties.Add(new Kitty("Astronaut Kitty", 1200, 150)); // 3
            _listKitties.Add(new Kitty("Alien Kitty", 40000, 3000)); // 4
            _listKitties.Add(new Kitty("Angel Kitty", 9999999, 99999)); // 5

            _totalMilk = new double();
            _totalMilk = 0;
            MPS = new double();
            MPS = 0;

            Background.SendToBack();
            Background.Controls.Add(MilkBowl);
            Background.Controls.Add(MilkLabel);
            Background.Controls.Add(MilkCount);
            Background.Controls.Add(MilkLabelPB);
            Background.Controls.Add(Help);
            Background.Controls.Add(Kitty);
            Background.Controls.Add(FarmKitty);
            Background.Controls.Add(BusiKitty);
            Background.Controls.Add(AstroKitty);
            Background.Controls.Add(AlienKitty);
            Background.Controls.Add(AngelKitty);
            Background.Controls.Add(KittyNumber);
            Background.Controls.Add(FKittyNumber);
            Background.Controls.Add(BKittyNumber);
            Background.Controls.Add(AKittyNumber);
            Background.Controls.Add(ALKittyNumber);
            Background.Controls.Add(AGKittyNumber);
            Background.Controls.Add(KittyPrice);
            Background.Controls.Add(FKittyPrice);
            Background.Controls.Add(BKittyPrice);
            Background.Controls.Add(AKittyPrice);
            Background.Controls.Add(ALKittyPrice);
            Background.Controls.Add(AGKittyPrice);
            Background.Controls.Add(label1);
            Background.Controls.Add(label2);
            Background.Controls.Add(label3);
            Background.Controls.Add(label4);
            Background.Controls.Add(label5);
            Background.Controls.Add(label6);
            Background.Controls.Add(ErrorLabel);

            MilkLabelPB.BackColor = Color.Transparent;
            MilkBowl.BackColor = Color.Transparent;
            MilkLabel.BackColor = Color.Transparent;
            MilkCount.BackColor = Color.Transparent;
            Help.BackColor = Color.Transparent;
            Kitty.BackColor = Color.Transparent;
            FarmKitty.BackColor = Color.Transparent;
            BusiKitty.BackColor = Color.Transparent;
            AstroKitty.BackColor = Color.Transparent;
            AlienKitty.BackColor = Color.Transparent;
            AngelKitty.BackColor = Color.Transparent;
            KittyNumber.BackColor = Color.Transparent;
            FKittyNumber.BackColor = Color.Transparent;
            BKittyNumber.BackColor = Color.Transparent;
            AKittyNumber.BackColor = Color.Transparent;
            ALKittyNumber.BackColor = Color.Transparent;
            AGKittyNumber.BackColor = Color.Transparent;
            KittyPrice.BackColor = Color.Transparent;
            FKittyPrice.BackColor = Color.Transparent;
            BKittyPrice.BackColor = Color.Transparent;
            AKittyPrice.BackColor = Color.Transparent;
            ALKittyPrice.BackColor = Color.Transparent;
            AGKittyPrice.BackColor = Color.Transparent;
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            ErrorLabel.BackColor = Color.Transparent;

            KittyPrice.Text = _listKitties[0].CurrentPrice.ToString();
            KittyNumber.Text = _listKitties[0].NumberOfKitties.ToString();
            FKittyPrice.Text = _listKitties[1].CurrentPrice.ToString();
            FKittyNumber.Text = _listKitties[1].NumberOfKitties.ToString();
            BKittyPrice.Text = _listKitties[2].CurrentPrice.ToString();
            BKittyNumber.Text = _listKitties[2].NumberOfKitties.ToString();
            AKittyPrice.Text = _listKitties[3].CurrentPrice.ToString();
            AKittyNumber.Text = _listKitties[3].NumberOfKitties.ToString();
            ALKittyPrice.Text = _listKitties[4].CurrentPrice.ToString();
            ALKittyNumber.Text = _listKitties[4].NumberOfKitties.ToString();
            AGKittyPrice.Text = _listKitties[5].CurrentPrice.ToString();
            AGKittyNumber.Text = _listKitties[5].NumberOfKitties.ToString();

            TTKitty = new ToolTip(); TTKitty.SetToolTip(Kitty, "+1 Milk per second");
            TTFKitty = new ToolTip(); TTFKitty.SetToolTip(FarmKitty, "+9 Milk per second");
            TTBKitty = new ToolTip(); TTBKitty.SetToolTip(BusiKitty, "+20 Milk per second");
            TTAKitty = new ToolTip(); TTAKitty.SetToolTip(AstroKitty, "+150 Milk per second");
            TTALKitty = new ToolTip(); TTALKitty.SetToolTip(AlienKitty, "+3000 Milk per second");
            TTAGKitty = new ToolTip(); TTAGKitty.SetToolTip(AngelKitty, "+99999 Milk per second");

            ErrorLabel.Text = "";

            //InitMusic();
        }

        private void InitMusic()
        {
            MainMusic = new SoundPlayer(Properties.Resources.main_music);
            MainMusic.PlayLooping();
        }
        #endregion

        #region Milk_Bowl
        private void MilkBowl_MouseDown(object sender, MouseEventArgs e)
        {
            MilkBowl.Image = Properties.Resources.downImage;
            MilkBowl.SizeMode = PictureBoxSizeMode.Zoom;
            MilkBowl.Invalidate();
            MilkBowl.Refresh();
        }

        private void MilkBowl_MouseUp(object sender, MouseEventArgs e)
        {
            MilkBowl.Image = Properties.Resources.MilkBowl;
            MilkBowl.SizeMode = PictureBoxSizeMode.Normal;
            MilkBowl.Invalidate();
            MilkBowl.Refresh();

            TotalMilk++;
        }
        #endregion

        #region Buy_Buttons
        private void Kitty1_Click(object sender, EventArgs e)
        {
            int position = 0;

            if (TotalMilk >= _listKitties[position].CurrentPrice)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= _listKitties[position].CurrentPrice;
                MPS += _listKitties[position].MilkPerSecond;
                SemTotalMilk.Release();
                _listKitties[position].NumberOfKitties++;

                double var = _listKitties[position].CurrentPrice;
                KittyPrice.Text = FormatNumber(var);
                KittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
        }

        private void Kitty10_Click(object sender, EventArgs e)
        {
            int position = 0;

            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 10) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 10;
                SemTotalMilk.Release();
                _listKitties[position].NumberOfKitties += 10;

                double var = _listKitties[position].CurrentPrice;
                KittyPrice.Text = FormatNumber(var);
                KittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void Kitty100_Click(object sender, EventArgs e)
        {
            int position = 0;
            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 100) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 100;
                SemTotalMilk.Release();
                _listKitties[position].NumberOfKitties += 100;

                double var = _listKitties[position].CurrentPrice;
                KittyPrice.Text = FormatNumber(var);
                KittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void FKitty1_Click(object sender, EventArgs e)
        {
            int position = 1;
            if (TotalMilk >= _listKitties[position].CurrentPrice)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= _listKitties[position].CurrentPrice;
                MPS += _listKitties[position].MilkPerSecond;
                SemTotalMilk.Release();
                _listKitties[position].NumberOfKitties++;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
        }

        private void FKitty10_Click(object sender, EventArgs e)
        {
            int position = 1;
            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 10) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 10;
                SemTotalMilk.Release();
                _listKitties[1].NumberOfKitties += 10;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void FKitty100_Click(object sender, EventArgs e)
        {
            int position = 1;
            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 100) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 100;
                SemTotalMilk.Release();
                _listKitties[1].NumberOfKitties += 100;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void BKitty1_Click(object sender, EventArgs e)
        {
            int position = 2;
            if (TotalMilk >= _listKitties[position].CurrentPrice)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= _listKitties[position].CurrentPrice;
                MPS += _listKitties[position].MilkPerSecond;
                SemTotalMilk.Release();
                _listKitties[2].NumberOfKitties++;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
        }

        private void BKitty10_Click(object sender, EventArgs e)
        {
            int position = 2;

            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 10) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 10;
                SemTotalMilk.Release();
                _listKitties[position].NumberOfKitties += 10;

                double var = _listKitties[position].CurrentPrice;
                KittyPrice.Text = FormatNumber(var);
                KittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void BKitty100_Click(object sender, EventArgs e)
        {
            int position = 2;
            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 100) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 100;
                SemTotalMilk.Release();
                _listKitties[1].NumberOfKitties += 100;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void AKitty1_Click(object sender, EventArgs e)
        {
            int position = 3;
            if (TotalMilk >= _listKitties[position].CurrentPrice)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= _listKitties[position].CurrentPrice;
                MPS += _listKitties[position].MilkPerSecond;
                SemTotalMilk.Release();
                _listKitties[2].NumberOfKitties++;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
        }

        private void AKitty10_Click(object sender, EventArgs e)
        {
            int position = 3;
            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 100) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 10;
                SemTotalMilk.Release();
                _listKitties[1].NumberOfKitties += 10;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void AKitty100_Click(object sender, EventArgs e)
        {
            int position = 3;
            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 100) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 100;
                SemTotalMilk.Release();
                _listKitties[1].NumberOfKitties += 100;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void ALKitty1_Click(object sender, EventArgs e)
        {
            int position = 4;
            if (TotalMilk >= _listKitties[position].CurrentPrice)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= _listKitties[position].CurrentPrice;
                MPS += _listKitties[position].MilkPerSecond;
                SemTotalMilk.Release();
                _listKitties[2].NumberOfKitties++;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
        }

        private void ALKitty10_Click(object sender, EventArgs e)
        {
            int position = 4;
            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 100) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 10;
                SemTotalMilk.Release();
                _listKitties[1].NumberOfKitties += 10;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void ALKitty100_Click(object sender, EventArgs e)
        {
            int position = 4;
            double c = (double)_listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 100) - 1) / (FACTOR - 1));
            double Cumul = c;

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 100;
                SemTotalMilk.Release();
                _listKitties[1].NumberOfKitties += 100;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void AGKitty1_Click(object sender, EventArgs e)
        {
            int position = 5;
            if (TotalMilk >= _listKitties[position].CurrentPrice)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= _listKitties[position].CurrentPrice;
                MPS += _listKitties[position].MilkPerSecond;
                SemTotalMilk.Release();
                _listKitties[2].NumberOfKitties++;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
        }

        private void AGKitty10_Click(object sender, EventArgs e)
        {
            int position = 5;
            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 100) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 10;
                SemTotalMilk.Release();
                _listKitties[1].NumberOfKitties += 10;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }

        private void AGKitty100_Click(object sender, EventArgs e)
        {
            int position = 5;
            double Cumul = _listKitties[position].CurrentPrice * ((Math.Pow(FACTOR, 100) - 1) / (FACTOR - 1));

            if (TotalMilk >= Cumul)
            {
                SemTotalMilk.WaitOne();
                TotalMilk -= Cumul;
                MPS += _listKitties[position].MilkPerSecond * 100;
                SemTotalMilk.Release();
                _listKitties[1].NumberOfKitties += 100;

                double var = _listKitties[position].CurrentPrice;
                FKittyPrice.Text = FormatNumber(var);
                FKittyNumber.Text = _listKitties[position].NumberOfKitties.ToString();
            }
            else
            {
                SetErrorLabel("Not enough milk, you need " + FormatNumber(Cumul) + " milk :(");
                EraseError = new Thread(ThreadEraseMessage);
                EraseError.Name = "Thread Erase Error";
                EraseError.Start();
            }
        }
        #endregion

        #region Miscellanous_Methods
        private void Help_Click(object sender, EventArgs e)
        {
            HelpWindow UDumbFuckItsAFuckingClicker = new HelpWindow();
            UDumbFuckItsAFuckingClicker.Show();
        }

        private String FormatNumber(double num)
        {
            #region OLD_CODE
            /*if (Nb < 0)
                return "";
            else
            {
                if (Nb >= 1000 && Nb <= 999999)
                    return (Nb / 1000 + "." + (Nb % 1000) / 100).ToString() + "K"; //Thousands = 10^3
                //----------------------------------------------------------------------------
                else if (Nb >= 1000000 && Nb <= 9999999)
                    return (Nb / 1000000 + "." + (Nb % 1000000) / 100000).ToString() + "M"; //Millions = 10^6
                //----------------------------------------------------------------------------
                else if (Nb >= Math.Pow(10, 9) && Nb <= Math.Pow(10, 12) - 1)
                    return (Nb / Math.Pow(10, 9) + "." + (Nb % Math.Pow(10, 9) / Math.Pow(10, 8))).ToString() + "B"; //Billions = 10^9
                //----------------------------------------------------------------------------
                else if (Nb >= Math.Pow(10, 12) && Nb <= Math.Pow(10, 15) - 1)
                    return Nb / Math.Pow(10, 12) + "." + (Nb % Math.Pow(10, 12) / Math.Pow(10, 11)) + "t"; //Trillions = 10^12
                //----------------------------------------------------------------------------
                else if (Nb >= Math.Pow(10, 15) && Nb <= Math.Pow(10, 18) - 1)
                    return Nb / Math.Pow(10, 15) + "." + (Nb % Math.Pow(10, 15)) / Math.Pow(10, 14) + "q"; //Quadrillions = 10^15
                //----------------------------------------------------------------------------
                else if (Nb >= Math.Pow(10, 18) && Nb <= Math.Pow(10, 21) - 1)
                    return Nb / Math.Pow(10, 18) + "." + ((Nb % Math.Pow(10, 18)) / Math.Pow(10, 17)) + "Q"; //Quintillions = 10^18
                //----------------------------------------------------------------------------
                else if (Nb >= Math.Pow(10, 21) && Nb <= Math.Pow(10, 24) - 1)
                    return Nb / Math.Pow(10, 21) + "." + ((Nb % Math.Pow(10, 21)) / Math.Pow(10, 20)) + "s"; //Sextillions = 10^21
                //----------------------------------------------------------------------------
                else if (Nb >= Math.Pow(10, 24) && Nb <= Math.Pow(10, 27) - 1)
                    return Nb / Math.Pow(10, 24) + "." + ((Nb % Math.Pow(10, 24)) / Math.Pow(10, 23)) + "S"; //Septillions = 10^24
                //----------------------------------------------------------------------------
                else if (Nb >= Math.Pow(10, 27) && Nb <= Math.Pow(10, 30) - 1)
                    return Nb / Math.Pow(10, 27) + "." + ((Nb % Math.Pow(10, 27)) / Math.Pow(10, 26)) + "O"; //Octillions = 10^27
                //----------------------------------------------------------------------------
                else if (Nb >= Math.Pow(10, 30) && Nb <= Math.Pow(10, 33) - 1)
                    return Nb / Math.Pow(10, 30) + "." + ((Nb % Math.Pow(10, 30)) / Math.Pow(10, 29)) + "N"; //Nonillions = 10^30
                //----------------------------------------------------------------------------
                else if(Nb >= Math.Pow(10, 33) && Nb <= Math.Pow(10, 36) - 1)
                    return Nb / Math.Pow(10, 33) + "." + ((Nb % Math.Pow(10, 33)) / Math.Pow(10, 32)) + "d"; //Decillions = 10^33
                //----------------------------------------------------------------------------
                else if(Nb >= Math.Pow(10, 36) && Nb <= Math.Pow(10, 39) - 1)
                    return Nb / Math.Pow(10, 36) + "." + ((Nb % Math.Pow(10, 36)) / Math.Pow(10, 35)) + "U"; //Undecillions = 10^36
                //----------------------------------------------------------------------------
                /*else if(Nb >= Math.Pow(10, 39) && Nb <= Math.Pow(10, 42) - 1)
                    return Nb / Math.Pow(10, 39) + "." + ((Nb % Math.Pow(10, 39)) / Math.Pow(10, 38)) + "D"; //Duodecillions = 10^39
                //----------------------------------------------------------------------------
                else if(Nb >= Math.Pow(10, 42) && Nb <= Math.Pow(10, 45) - 1)
                    return Nb / Math.Pow(10, 42) + "." + ((Nb % Math.Pow(10, 42)) / Math.Pow(10, 41)) + "T"; //Tredecillions = 10^42*/
            //----------------------------------------------------------------------------

            /*
             --LEFT--

                Quattuordecillions = 10^45
                Quindecillions = 10^48
                Sexdecillions = 10^51
                Septendecillions = 10^54
                Octodecillions = 10^57
                Novemdecillions = 10^60
                Vigintillion = 10^63

                -> No other than Centillions exist (10^303) -> Invent some new units for each multiple of 3 from 66 -> 300

             */
            #endregion
            #region NEW_CODE
            if (num < 1000)
                return Math.Round(num, 1).ToString();
            if (num >= 1000 && num < 1000000)
                return (Math.Round(num / 1000, 1)).ToString() + "K";
            if (num >= 1000000 && num < 1000000000)
                return (Math.Round(num / 1000000, 1)).ToString() + "M";
            if (num >= 1000000000 && num < 1000000000000)
                return (Math.Round(num / 1000000000, 1)).ToString() + "B";
            if (num >= 1000000000000 && num < 1000000000000000)
                return (Math.Round(num / 1000000000000, 1)).ToString() + "T";
            if (num >= 1000000000000000 && num < 1000000000000000000)
                return (Math.Round(num / 1000000000000000, 1)).ToString() + "t";
            if (num >= 1000000000000000000)
                return (Math.Round(num / 1000000000000000000, 1)).ToString() + "q";
            else
                return "ERR";
            #endregion
        }
        #endregion

        #region ThreadRelatedStuff
        private void SetMilkCount(String text)
        {
            if (MilkCount.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetMilkCount);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                MilkCount.Text = text;
            }
        }

        private void ThreadTotalMilk()
        {
            while (true)
            {
                SemTotalMilk.WaitOne();
                TotalMilk += MPS;
                SemTotalMilk.Release();
                Thread.Sleep(1000);
            }
        }

        private void ThreadEraseMessage()
        {
            Thread.Sleep(3000);
            SetErrorLabel("");

        }

        private void SetErrorLabel(String text)
        {
            if (ErrorLabel.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetErrorLabel);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                ErrorLabel.Text = text;
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                UpdateMPS.Abort();
                EraseError.Abort();
            }
            catch(Exception ex)
            {
                //actually I don't give a fuck
            }
        }

        #endregion


    }
}

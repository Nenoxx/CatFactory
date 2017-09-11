using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat_Factory
{
    public class Kitty
    {
        #region Variables
        private int _numberOfKitties;
        private String _name;
        private double _basePrice;
        private double _currentPrice;
        private int _milkPerSecond;

        private const double FACTOR = 1.10;
        #endregion

        #region Properties
        public int NumberOfKitties
        {
            set
            {
                _numberOfKitties = value;
                UpdatePrice();
            }
            get { return _numberOfKitties; }
        }

        public String Name
        {
            set { if (value != null) _name = value; }
            get { return _name; }
        }

        public double BasePrice
        {
            set { _basePrice = value; }
            get { return _basePrice; }
        }

        public double CurrentPrice
        {
            set { _currentPrice = value; }
            get { return _currentPrice; }
        }

        public int MilkPerSecond
        {
            set { _milkPerSecond = value; }
            get { return _milkPerSecond; }
        }
        #endregion

        #region Methods
        public void UpdatePrice()
        {
            CurrentPrice = BasePrice * Math.Pow(FACTOR, NumberOfKitties);
        }

        public Kitty(String Name, double BasePrice, int MPS)
        {
            this.Name = Name;
            this.BasePrice = BasePrice;
            CurrentPrice = BasePrice;
            this.MilkPerSecond = MPS;
        }
        #endregion
    }
}

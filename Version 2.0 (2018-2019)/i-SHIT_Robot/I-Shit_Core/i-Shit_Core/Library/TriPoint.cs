using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i_Shit_Core.Library {
    public class TriPoint {
        public TriPoint (double x, double y, double z) {
            _x = x;
            _y = y;
            _z = z;
        }

        private double _x;
        public double X {
            get { return _x; }
            set { _x = value; }
        }

        private double _y;
        public double Y {
            get { return _y; }
            set { _y = value; }
        }

        private double _z;
        public double Z {
            get { return _z; }
            set { _z = value; }
        }

        public TriPoint () {
            X = 0;
            Y = 0;
            Z = 0;
        }
    }
}
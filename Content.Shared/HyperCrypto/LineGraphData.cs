using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.HyperCrypto
{
    public class LineGraphData
    {
        
        public double HighestValue { get; private set; }
        public double LowestValue { get; private set; }
        public double XInterval { get; private set; }

        private List<double> _data = new List<double>();


        public LineGraphData(){
            HighestValue = double.MinValue;
            LowestValue = double.MaxValue;
        }

        public void AddPoint(double point)
        {
            if (point > HighestValue)
                HighestValue = point;
            if (point < LowestValue)
                LowestValue = point;
            _data.Add(point);
        }

        public void RemoveIndex()
        {

        }



        public List<double> GetData()
        {
            return _data;
        }

        
    }
}

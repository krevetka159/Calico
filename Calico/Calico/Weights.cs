using Calico.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calico
{
    public class Weights
    {
        public double ButtonW;
        public (double, double, double) CatsW;
        public double[] TaskW;

        public Weights(double b, (double, double, double) c, double[] t)
        {
            ButtonW = b;
            CatsW = c;
            TaskW = new double[] { t[0], t[1], t[2], t[3], t[4], t[5] };
        }
    }

    public class WeightsDict
    {
        private Dictionary<(int, int, int), Weights> WDict;

        public WeightsDict()
        {
            WDict = new Dictionary<(int, int, int), Weights>();

            var res = Resources.weights.Split();

            using (var reader = new StringReader(Resources.weights))
            {
                var line = reader.ReadLine();
                var values = line.Split(';'); // columns

                while (true)
                {
                    line = reader.ReadLine();

                    if (line == null)
                    {
                        break;
                    }
                    values = line.Split(';');

                    var tasks = values[0].Split(",");

                    double weightB = Convert.ToDouble(values[2]);
                    (double, double, double) weightC = (Convert.ToDouble(values[3]), Convert.ToDouble(values[4]), Convert.ToDouble(values[5]));
                    double[] weightT = new double[]
                            {

                                                Convert.ToDouble(values[6]),
                                                Convert.ToDouble(values[7]),
                                                Convert.ToDouble(values[8]),
                                                Convert.ToDouble(values[9]),
                                                Convert.ToDouble(values[10]),
                                                Convert.ToDouble(values[11]),
                            };

                    WDict[(Convert.ToInt32(tasks[0]), Convert.ToInt32(tasks[1]), Convert.ToInt32(tasks[2]))] = new Weights(weightB, weightC, weightT);
                }
            }
        }

        public Weights GetWeights((int, int, int) tasks)
        {
            return WDict[tasks];
        }
    }
}

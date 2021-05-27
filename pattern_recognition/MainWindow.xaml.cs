using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ILNumerics;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.ILMath;
using static ILNumerics.Globals;
using ILNumerics.Drawing;

namespace pattern_recognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            textBoxResult.Text = "";
            double[] weights = new double[3];
            int t = Convert.ToInt32(threshold.Value); // пороговое значение
            int c = Convert.ToInt32(clusterAmount.Value); // число кластеров
            bool? normalize = false;
            if ((bool)checkBoxNormalize.IsChecked)
                normalize = true;
            weights[0] = Convert.ToDouble(xWeightBox.Text);
            weights[1] = Convert.ToDouble(yWeightBox.Text);
            weights[2] = Convert.ToDouble(zWeightBox.Text);
            
            // метод подсчет расстояния между точками; 
            string distMethod = comboBox.Text;

            List<Cluster> data = Initialization.ReturnClusterList(normalize);

            ClusteringAlgorithm alg = new ClusteringAlgorithm { };
            List<Cluster> finalClusters = alg.ClusterData(data, c, t, distMethod, weights);
            string str = "";

            // обработка данных для построения графика
            var panel = new ILNumerics.Drawing.Panel();
            Panel1.Child = panel;
            // создаем трёхмерное пространство координат
            PlotCube plotCube = new PlotCube(twoDMode: false);
            foreach(Cluster cluster in finalClusters)
            {
                double[,] clustArray = new double[cluster.Points.Count,3];

                for(int i = 0; i < cluster.Points.Count; i++) 
                {
                    clustArray[i, 0] = cluster.Points[i].x;
                    clustArray[i, 1] = cluster.Points[i].y;
                    clustArray[i, 2] = cluster.Points[i].z;
                    str += " (" + cluster.Points[i].x.ToString() + ", " + cluster.Points[i].y.ToString() + ", " + cluster.Points[i].z.ToString() + ") ";
                }
                str += "cluster";
                Array<float> positions = clustArray;
                positions = tosingle(positions);
                Random rand = new Random();
                System.Drawing.Color? color = System.Drawing.Color.FromArgb(190, rand.Next(0, 256), rand.Next(0,256), rand.Next(0, 256));
                
                plotCube.Children.Add(new Points() { Positions = positions, Color = color });
            }
            panel.Scene.Add(plotCube);

            textBoxResult.Text = str;
        }
    }
}

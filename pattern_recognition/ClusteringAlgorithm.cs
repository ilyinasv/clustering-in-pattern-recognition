using System;
using System.Collections.Generic;
using System.Text;

namespace pattern_recognition
{
    class ClusteringAlgorithm
    {
        private List<Cluster> CurrentClusters { get; set; }

        private readonly int lambda;
        private bool flagThreshold { get; set; }

        private Dictionary<Cluster, Dictionary<Cluster, double>> distanceMatrix; // матрица расстояний между кластерами

        public ClusteringAlgorithm()
        {
            // задаем произвольную лямбду для формулы подсчёта расстояний между кластерами
            Random x = new Random();
            this.lambda = x.Next(-100, 100);
        }

        public List<Cluster> ClusterData(List<Cluster> data, int clusterAmount, int threshold, string distanceMethod, double[] weights)
        {
            flagThreshold = true;
            distanceMatrix = new Dictionary<Cluster, Dictionary<Cluster, double>>();
            CurrentClusters = data;

            while (CurrentClusters.Count != clusterAmount && flagThreshold)
                    MergeClusters(threshold, distanceMethod, weights);
            return CurrentClusters;
        }

        public void MergeClusters(int threshold, string distanceMethod, double[] weights)
        {
            double minDist = double.MaxValue;
            Cluster minClust1 = null, minClust2 = null;

            for (int i = 0; i < CurrentClusters.Count; ++i)
            {
                Cluster c1 = CurrentClusters[i];

                distanceMatrix[c1] = new Dictionary<Cluster, double>();

                for (int j = i + 1; j < CurrentClusters.Count; ++j)
                {
                    Cluster c2 = CurrentClusters[j];
                    double dist;
                    dist = distanceMatrix[c1][c2] = ClusterDistance(CurrentClusters[i], CurrentClusters[j], distanceMethod, weights);                 
                    if (dist < minDist)
                    {
                        minDist = dist;
                        minClust1 = CurrentClusters[i];
                        minClust2 = CurrentClusters[j];
                    }
                }
            }

            //если расстояние между ближайшими кластерами больше пороговой величины, то останавливаем кластеризацию
            if (minDist > threshold)
                flagThreshold = false;

            //создаем слитый кластер
            Cluster newCluster = new Cluster(minClust1, minClust2);

            //удаляем слитые кластеры
            CurrentClusters.Remove(minClust1);
            CurrentClusters.Remove(minClust2);

            //обновляем матрицу расстояний и добавляем новый кластер
            distanceMatrix.Remove(minClust1);
            distanceMatrix.Remove(minClust2);
            CurrentClusters.Add(newCluster);
        }
            
        public double ClusterDistance(Cluster c1, Cluster c2, string distanceMethod, double[] weights)
        {
            //используем алгоритм Колмогорова для нахождения расстояния между кластерами           
            double  dist = 0;
            int m1 = c1.Points.Count;
            int m2 = c2.Points.Count;

            foreach (Point p1 in c1.Points)
            {
                foreach (Point p2 in c2.Points)
                {
                    double distP = distPoints(p1, p2, distanceMethod, weights);
                    distP = Math.Pow(distP, lambda);
                    dist += distP;
                }
            };
            dist /= (m1*m2);
            double clustDist = Math.Pow((double)dist, (double) 1 / lambda);
            return clustDist;
        }

        private static double distPoints(Point p1, Point p2, string distanceMethod, double[] weights)
        {
            double distPoints;
            if (distanceMethod.Equals("Манхэттенское"))
                distPoints = ManhattanDistance(p1, p2, weights);
            else distPoints = DomDistance(p1, p2, weights);
            return distPoints;
        }

        // вычисление Манхэттенского расстояния
        private static double ManhattanDistance(Point p1, Point p2, double[] weights)
        {
            double distance = 0;
            distance = weights[0]*Math.Abs(p2.x - p1.x) + weights[1] * Math.Abs(p2.y - p1.y) + weights[2] * Math.Abs(p2.z - p1.z);
            return distance;
        }

        // вычисление расстояния доминирования
        private static double DomDistance(Point p1, Point p2, double[] weights)
        {
            double distance = 0;
            distance = Math.Max(weights[0] * Math.Abs(p2.x - p1.x), weights[1] * Math.Abs(p2.y - p1.y));
            distance = Math.Max(distance, weights[2] * Math.Abs(p2.z - p1.z));
            return distance;
        }
    }
}

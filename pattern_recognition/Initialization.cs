using System;
using System.Collections.Generic;
using System.Text;

namespace pattern_recognition
{
    class Initialization
    {
        private static double[,] dataSet = new double[,]
        {
                {1,2,15},
                {5,18,4},
                {-10,-3,6},
                {18,15,14},
                {2,19,-13},
                {-3,5,-33},
                {13,5,12},
                {6,11, 1},
                {-7,-2,2},
                {14,13,11},
                {4,20,-11},
                {-5,8,-21},
                {-5,-3,2},
                {16,19,20},
                {11,5,19},
                {9,10,4},
                {-7,0,3},
                {13,14,15},
                {12,19,-17},
                {-5,9,-30},
                {20,7,20},
                {0,19,9},
                {-6,-1, 2},
                {13,11,18},
                {0,15,-20},
                {-2,6,-24},
                {11,4,15},
                {3,15,6},
                {0,0,9},
                {7,8,0}
        };

        public static List<Cluster> ReturnClusterList(bool? normalize)
        {
            double maxX, maxY, maxZ;
            maxX=maxY=maxZ = double.MinValue;
            double minX, minY, minZ;
            minX=minY=minZ = double.MaxValue;

            // находим максимальные и минимальные значения для нормализации методом минимакса
            for (int i = 0; i < dataSet.GetLength(0); i++)
            {
                if (dataSet[i, 0] > maxX)
                    maxX = dataSet[i, 0];
                if (dataSet[i, 0] < minX)
                    minX = dataSet[i, 0];
                if (dataSet[i, 1] > maxY)
                    maxY = dataSet[i, 1];
                if (dataSet[i, 1] < minY)
                    minY = dataSet[i, 1];
                if (dataSet[i, 2] > maxZ)
                    maxZ= dataSet[i, 2];
                if (dataSet[i, 2] < minZ)
                    minZ = dataSet[i, 2];
            }


            //подгружаем исходные данные в лист кластеров
            List<Cluster> initialClusters = new List<Cluster>();
            for (int i = 0; i < dataSet.GetLength(0); i++)
            {
                Cluster cluster = new Cluster();
                List<Point> points = new List<Point>();               

                Point point = new Point()
                {
                    x = dataSet[i, 0],
                    y = dataSet[i, 1],
                    z = dataSet[i, 2],
                };
                
                // нормализация данных
                if (normalize == true)
                {
                    point.x = (point.x - minX) / (maxX - minX);
                    point.y = (point.y - minY) / (maxY - minY);
                    point.z = (point.z - minZ) / (maxZ - minZ);
                };

                points.Add(point);
                cluster.Points = points;
                initialClusters.Add(cluster);
            }
            return initialClusters;
        }  
    }
}

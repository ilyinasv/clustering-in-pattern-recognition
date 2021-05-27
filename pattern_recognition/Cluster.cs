using System;
using System.Collections.Generic;
using System.Text;

namespace pattern_recognition
{
    class Cluster
    {
        public List<Point> Points;

        public Cluster()
        {
            Points = new List<Point>();
        }

        public Cluster(Cluster c1, Cluster c2)
        {
            Points = new List<Point>();

            foreach (Point p in c1.Points)
                Points.Add(p);

            foreach (Point p in c2.Points)
                Points.Add(p);
        }
    }
}

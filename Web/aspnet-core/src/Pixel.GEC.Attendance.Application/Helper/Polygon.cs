using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Pixel.GEC.Attendance.Helper
{

    public class Polygon
    {
        #region Properties

        public List<Point> myPts = new List<Point>();

        public string SiteCode { set; get; }

        public int minSiteID { set; get; }

        public string SiteDesc { set; get; }

        #endregion

        #region Constructors

        public Polygon()
        {
        }
        public Polygon(List<Point> points)
        {
            foreach (Point p in points)
            {
                this.myPts.Add(p);
            }
        }

        #endregion

        #region Methods

        public void Add(Point p)
        {
            this.myPts.Add(p);
        }
        public int Count()
        {
            return myPts.Count;
        }


        public bool IsPointInPolygon(Point point)
        {
            Point[] polygon = this.myPts.ToArray();
            bool isInside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (((polygon[i].Longitude > point.Longitude) != (polygon[j].Longitude > point.Longitude)) &&
                (point.Latitude < (polygon[j].Latitude - polygon[i].Latitude) * (point.Longitude - polygon[i].Longitude) / (polygon[j].Longitude - polygon[i].Longitude) + polygon[i].Latitude))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }

        public bool IsPointInPolygon(double Latitude, double Longitude)
        {
            return IsPointInPolygon(new Point(Latitude, Longitude));
        }
        #endregion
    }
    public class Point
    {
        public double Latitude { set; get; }
        public double Longitude { set; get; }

        public Point(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }
    }
}

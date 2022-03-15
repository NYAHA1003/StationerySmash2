using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public struct ColideData
    {
        public Vector2[] originpoints;
        public Vector2[] points;

        public ColideData(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
        {
            points = new Vector2[4];
            originpoints = new Vector2[4];
            originpoints[0] = point1;
            originpoints[1] = point2;
            originpoints[2] = point3;
            originpoints[3] = point4;
        }

        public void Set_Pos(Vector2 trm_Pos)
        {
            points[0] = trm_Pos + originpoints[0];
            points[1] = trm_Pos + originpoints[1];
            points[2] = trm_Pos + originpoints[2];
            points[3] = trm_Pos + originpoints[3];
        }
    }

    public class Collider
    {
        private static float Check_Dir(float dir1, float dir2)
        {
            return Mathf.Min(dir1, dir2);
        }

        /// <summary>
        /// 두 선분 사이의 거리
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="close1"></param>
        /// <param name="close2"></param>
        /// <returns></returns>
        private static float FindDistanceBetweenSegments(ColideData colideData1, ColideData colideData2)
        {
            // 가장 짧은 거리 찾기
            float best_dist = float.MaxValue;
            float test_dist = 0f;

            for(int i = 0; i < 4; i++)
            {
                test_dist = FindDistanceToSegment(colideData1.points[i], colideData2.points[0], colideData2.points[1]);
                best_dist = Check_Dir(best_dist, test_dist);

                test_dist = FindDistanceToSegment(colideData1.points[i], colideData2.points[0], colideData2.points[2]);
                best_dist = Check_Dir(best_dist, test_dist);

                test_dist = FindDistanceToSegment(colideData1.points[i], colideData2.points[1], colideData2.points[3]);
                best_dist = Check_Dir(best_dist, test_dist);

                test_dist = FindDistanceToSegment(colideData1.points[i], colideData2.points[2], colideData2.points[3]);
                best_dist = Check_Dir(best_dist, test_dist);
            }

            return best_dist;
        }

        /// <summary>
        /// 점과 선분의 최단거리
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static float FindDistanceToSegment(Vector2 pt, Vector2 p1, Vector2 p2)
        {
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            if ((dx == 0) && (dy == 0))
            {
                // It's a point not a line segment.
                dx = pt.x - p1.x;
                dy = pt.y - p1.y;
                return Mathf.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.x - p1.x) * dx + (pt.y - p1.y) * dy) /
                (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                dx = pt.x - p1.x;
                dy = pt.y - p1.y;
            }
            else if (t > 1)
            {
                dx = pt.x - p2.x;
                dy = pt.y - p2.y;
            }
            else
            {
                dx = pt.x - (p1.x + t * dx);
                dy = pt.y - (p1.y + t * dy);
            }

            return Mathf.Sqrt(dx * dx + dy * dy);
        }
    }
}

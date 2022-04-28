using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{
    [System.Serializable]
    public struct CollideData
    {
        public Vector2[] originpoints;
        private Vector2[] points;

        public void SetPos(Vector2 trm_Pos)
        {
            Vector2[] points = new Vector2[4];
            points[0] = trm_Pos + originpoints[0];
            points[1] = trm_Pos + originpoints[1];
            points[2] = trm_Pos + originpoints[2];
            points[3] = trm_Pos + originpoints[3];
            this.points = points;
        }
        public Vector2[] GetPoint(Vector2 pos)
        {
            SetPos(pos);
            return points;
        }
    }

    public static class Collider
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
        public static float FindDistanceBetweenSegments(Vector2[] points1, Vector2[] points2)
        {
            // 가장 짧은 거리 찾기
            float best_dist = float.MaxValue;
            float test_dist = float.MaxValue;

            for(int i = 0; i < 4; i++)
            {
                test_dist = FindDistanceToSegment(points1[i], points2[0], points2[1]);
                best_dist = Check_Dir(best_dist, test_dist);

                test_dist = FindDistanceToSegment(points1[i], points2[0], points2[2]);
                best_dist = Check_Dir(best_dist, test_dist);

                test_dist = FindDistanceToSegment(points1[i], points2[0], points2[3]);
                best_dist = Check_Dir(best_dist, test_dist);

                test_dist = FindDistanceToSegment(points1[i], points2[0], points2[3]);
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
            float t = ((pt.x - p1.x) * dx + (pt.y - p1.y) * dy) / (dx * dx + dy * dy);

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

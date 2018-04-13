using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Collection of basic computational geometry algorithms (distance, intersection, closest point, etc.)
    /// </summary>
    public static partial class Geometry
    {
        public const float Epsilon = 0.00001f;
        public const float EpsilonSqr = Epsilon*Epsilon;

        #region Point-Line

        /// <summary>
        /// Returns a distance to the closest point on the line
        /// </summary>
        public static float DistanceToLine(Vector2 point, Line2 line)
        {
            return Vector2.Distance(point, ClosestPointOnLine(point, line));
        }

        /// <summary>
        /// Returns a distance to the closest point on the line defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the line</param>
        public static float DistanceToLine(Vector2 point, Vector2 origin, Vector2 direction)
        {
            return Vector2.Distance(point, ClosestPointOnLine(point, origin, direction));
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        public static Vector2 ClosestPointOnLine(Vector2 point, Line2 line)
        {
            float projectedX;
            return ClosestPointOnLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line
        /// </summary>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector2 ClosestPointOnLine(Vector2 point, Line2 line, out float projectedX)
        {
            return ClosestPointOnLine(point, line.origin, line.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the line</param>
        public static Vector2 ClosestPointOnLine(Vector2 point, Vector2 origin, Vector2 direction)
        {
            float projectedX;
            return ClosestPointOnLine(point, origin, direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the line</param>
        /// <param name="projectedX">Position of the projected point on the line relative to the origin</param>
        public static Vector2 ClosestPointOnLine(Vector2 point, Vector2 origin, Vector2 direction, out float projectedX)
        {
            Vector2 toPoint = point - origin;

            float dotDirection = Vector2.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid line definition. origin: " + origin + " direction: " + direction);
                projectedX = 0;
                return origin;
            }

            projectedX = Vector2.Dot(toPoint, direction)/dotDirection;
            return origin + direction*projectedX;
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        public static bool IntersectPointLine(Vector2 point, Line2 line)
        {
            return IntersectPointLine(point, line.origin, line.direction);
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the line,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the line
        /// </param>
        public static bool IntersectPointLine(Vector2 point, Line2 line, out int side)
        {
            return IntersectPointLine(point, line.origin, line.direction, out side);
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        public static bool IntersectPointLine(Vector2 point, Vector2 origin, Vector2 direction)
        {
            float perpDot = VectorE.PerpDot(point - origin, direction);
            return -Epsilon < perpDot && perpDot < Epsilon;
        }

        /// <summary>
        /// Tests if the point lies on the line
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the line,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the line
        /// </param>
        public static bool IntersectPointLine(Vector2 point, Vector2 origin, Vector2 direction, out int side)
        {
            float perpDot = VectorE.PerpDot(point - origin, direction);
            if (perpDot < -Epsilon)
            {
                side = -1;
                return false;
            }
            if (perpDot > Epsilon)
            {
                side = 1;
                return false;
            }
            side = 0;
            return true;
        }

        #endregion Point-Line

        #region Point-Ray

        /// <summary>
        /// Returns a distance to the closest point on the ray
        /// </summary>
        public static float DistanceToRay(Vector2 point, Ray2D ray)
        {
            return Vector2.Distance(point, ClosestPointOnRay(point, ray));
        }

        /// <summary>
        /// Returns a distance to the closest point on the ray defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the ray</param>
        public static float DistanceToRay(Vector2 point, Vector2 origin, Vector2 direction)
        {
            return Vector2.Distance(point, ClosestPointOnRay(point, origin, direction));
        }

        /// <summary>
        /// Projects the point onto the ray
        /// </summary>
        public static Vector2 ClosestPointOnRay(Vector2 point, Ray2D ray)
        {
            float projectedX;
            return ClosestPointOnRay(point, ray.origin, ray.direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the ray</param>
        public static Vector2 ClosestPointOnRay(Vector2 point, Vector2 origin, Vector2 direction)
        {
            float projectedX;
            return ClosestPointOnRay(point, origin, direction, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the ray defined by <paramref name="origin"/> and <paramref name="direction"/>
        /// </summary>
        /// <param name="direction">Normalized direction of the ray</param>
        /// <param name="projectedX">Position of the projected point on the ray relative to the origin</param>
        public static Vector2 ClosestPointOnRay(Vector2 point, Vector2 origin, Vector2 direction, out float projectedX)
        {
            Vector2 toPoint = point - origin;

            float dotDirection = Vector2.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid ray definition. origin: " + origin + " direction: " + direction);
                projectedX = 0;
                return origin;
            }

            float dotToPoint = Vector2.Dot(toPoint, direction);
            if (dotToPoint <= 0)
            {
                projectedX = 0;
                return origin;
            }

            projectedX = dotToPoint/dotDirection;
            return origin + direction*projectedX;
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        public static bool IntersectPointRay(Vector2 point, Ray2D ray)
        {
            return IntersectPointRay(point, ray.origin, ray.direction);
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the ray,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the ray
        /// </param>
        public static bool IntersectPointRay(Vector2 point, Ray2D ray, out int side)
        {
            return IntersectPointRay(point, ray.origin, ray.direction, out side);
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        public static bool IntersectPointRay(Vector2 point, Vector2 origin, Vector2 direction)
        {
            Vector2 toPoint = point - origin;
            float perpDot = VectorE.PerpDot(toPoint, direction);
            return -Epsilon < perpDot && perpDot < Epsilon &&
                   Vector2.Dot(toPoint, direction) > -Epsilon;
        }

        /// <summary>
        /// Tests if the point lies on the ray
        /// </summary>
        /// <param name="side">
        /// -1 if the point is to the left of the ray,
        /// 0 if it is on the line,
        /// 1 if it is to the right of the ray
        /// </param>
        public static bool IntersectPointRay(Vector2 point, Vector2 origin, Vector2 direction, out int side)
        {
            Vector2 toPoint = point - origin;
            float perpDot = VectorE.PerpDot(toPoint, direction);
            if (perpDot < -Epsilon)
            {
                side = -1;
                return false;
            }
            if (perpDot > Epsilon)
            {
                side = 1;
                return false;
            }
            side = 0;
            return Vector2.Dot(toPoint, direction) > -Epsilon;
        }

        #endregion Point-Ray

        #region Point-Segment

        /// <summary>
        /// Returns a distance to the closest point on the line segment
        /// </summary>
        public static float DistanceToSegment(Vector2 point, Segment2 segment)
        {
            return Vector2.Distance(point, ClosestPointOnSegment(point, segment));
        }

        /// <summary>
        /// Returns a distance to the closest point on the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static float DistanceToSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            return Vector2.Distance(point, ClosestPointOnSegment(point, segmentA, segmentB));
        }

        /// <summary>
        /// Projects the point onto the line segment
        /// </summary>
        public static Vector2 ClosestPointOnSegment(Vector2 point, Segment2 segment)
        {
            float projectedX;
            return ClosestPointOnSegment(point, segment.a, segment.b, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line segment
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the line segment. 
        /// Value of zero means that the projected point coincides with segment.a. 
        /// Value of one means that the projected point coincides with segment.b.</param>
        public static Vector2 ClosestPointOnSegment(Vector2 point, Segment2 segment, out float projectedX)
        {
            return ClosestPointOnSegment(point, segment.a, segment.b, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        public static Vector2 ClosestPointOnSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB)
        {
            float projectedX;
            return ClosestPointOnSegment(point, segmentA, segmentB, out projectedX);
        }

        /// <summary>
        /// Projects the point onto the line segment defined by <paramref name="segmentA"/> and <paramref name="segmentB"/>
        /// </summary>
        /// <param name="projectedX">Normalized position of the projected point on the line segment. 
        /// Value of zero means that the projected point coincides with <paramref name="segmentA"/>. 
        /// Value of one means that the projected point coincides with <paramref name="segmentB"/>.</param>
        public static Vector2 ClosestPointOnSegment(Vector2 point, Vector2 segmentA, Vector2 segmentB, out float projectedX)
        {
            Vector2 direction = segmentB - segmentA;
            Vector2 toPoint = point - segmentA;

            float dotDirection = Vector2.Dot(direction, direction);
            if (dotDirection < Epsilon)
            {
                Debug.LogError("Invalid segment definition. segmentA: " + segmentA + " segmentB: " + segmentB);
                projectedX = 0;
                return segmentA;
            }

            float dotToPoint = Vector2.Dot(toPoint, direction);
            if (dotToPoint <= 0)
            {
                projectedX = 0;
                return segmentA;
            }

            if (dotToPoint >= dotDirection)
            {
                projectedX = 1;
                return segmentB;
            }

            projectedX = dotToPoint/dotDirection;
            return segmentA + direction*projectedX;
        }

        #endregion Point-Segment

        #region Point-Circle

        /// <summary>
        /// Returns a distance to the closest point on the circle
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float DistanceToCircle(Vector2 point, Circle circle)
        {
            return DistanceToCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        /// Returns a distance to the closest point on the circle defined by <paramref name="center"/> and <paramref name="radius"/>
        /// </summary>
        /// <returns>Positive value if the point is outside, negative otherwise</returns>
        public static float DistanceToCircle(Vector2 point, Vector2 center, float radius)
        {
            return (center - point).magnitude - radius;
        }

        /// <summary>
        /// Projects the point onto the circle
        /// </summary>
        public static Vector2 ClosestPointOnCircle(Vector2 point, Circle circle)
        {
            return ClosestPointOnCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        /// Projects the point onto the circle defined by <paramref name="center"/> and <paramref name="radius"/>
        /// </summary>
        public static Vector2 ClosestPointOnCircle(Vector2 point, Vector2 center, float radius)
        {
            return (point - center).normalized*radius;
        }

        /// <summary>
        /// Tests if the point is inside the circle
        /// </summary>
        public static bool IntersectPointCircle(Vector2 point, Circle circle)
        {
            return IntersectPointCircle(point, circle.center, circle.radius);
        }

        /// <summary>
        /// Tests if the point is inside the circle
        /// </summary>
        public static bool IntersectPointCircle(Vector2 point, Vector2 center, float radius)
        {
            return (point - center).sqrMagnitude < radius*radius;
        }

        #endregion Point-Circle

        #region Line-Line

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool IntersectLineLine(Line2 lineA, Line2 lineB, out Vector2 intersection)
        {
            return IntersectLineLine(lineA.origin, lineA.direction, lineB.origin, lineB.direction, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the lines
        /// </summary>
        public static bool IntersectLineLine(Vector2 originA, Vector2 directionA, Vector2 originB, Vector2 directionB, out Vector2 intersection)
        {
            float denominator = VectorE.PerpDot(directionA, directionB);
            Vector2 originBToA = originA - originB;
            float a = VectorE.PerpDot(directionA, originBToA);
            float b = VectorE.PerpDot(directionB, originBToA);

            if (Mathf.Abs(denominator) < Epsilon)
            {
                // Parallel
                if (Mathf.Abs(a) > Epsilon || Mathf.Abs(b) > Epsilon)
                {
                    // Not collinear
                    intersection = Vector2.zero;
                    return false;
                }
                // Collinear
                intersection = originA;
                return true;
            }

            float distanceA = b/denominator;
            intersection = originA + distanceA*directionA;
            return true;
        }

        #endregion Line-Line

        #region Line-Circle

        /// <summary>
        /// Computes an intersection of the line and the circle
        /// </summary>
        public static bool IntersectLineCircle(Line2 line, Circle circle, out Vector2 pointA, out Vector2 pointB)
        {
            return IntersectLineCircle(line.origin, line.direction, circle.center, circle.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the line and the circle
        /// </summary>
        public static bool IntersectLineCircle(Vector2 origin, Vector2 direction, Vector2 center, float radius, out Vector2 pointA,
            out Vector2 pointB)
        {
            Vector2 toCenter = center - origin;
            float toCenterOnLine = Vector2.Dot(toCenter, direction);
            float sqrDistanceToLine = toCenter.sqrMagnitude - toCenterOnLine*toCenterOnLine;

            float sqrRadius = radius*radius;
            if (sqrDistanceToLine > sqrRadius)
            {
                pointA = Vector2.zero;
                pointB = Vector2.zero;
                return false;
            }
            float fromClosestPointToIntersection = Mathf.Sqrt(sqrRadius - sqrDistanceToLine);
            float intersectionA = toCenterOnLine - fromClosestPointToIntersection;
            float intersectionB = toCenterOnLine + fromClosestPointToIntersection;

            if (intersectionA > intersectionB)
            {
                PTUtils.Swap(ref intersectionA, ref intersectionB);
            }

            pointA = origin + intersectionA*direction;
            pointB = origin + intersectionB*direction;
            return true;
        }

        #endregion Line-Circle

        #region Ray-Circle

        /// <summary>
        /// Computes an intersection of the ray and the circle
        /// </summary>
        public static bool IntersectRayCircle(Ray2D ray, Circle circle, out Vector2 pointA, out Vector2 pointB)
        {
            return IntersectRayCircle(ray.origin, ray.direction, circle.center, circle.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Computes an intersection of the ray and the circle
        /// </summary>
        public static bool IntersectRayCircle(Vector2 origin, Vector2 direction, Vector2 center, float radius, out Vector2 pointA, out Vector2 pointB)
        {
            Vector2 toCenter = center - origin;
            float toCenterOnLine = Vector2.Dot(toCenter, direction);
            float sqrDistanceToLine = toCenter.sqrMagnitude - toCenterOnLine*toCenterOnLine;

            float sqrRadius = radius*radius;
            if (sqrDistanceToLine > sqrRadius)
            {
                pointA = Vector2.zero;
                pointB = Vector2.zero;
                return false;
            }
            float fromClosestPointToIntersection = Mathf.Sqrt(sqrRadius - sqrDistanceToLine);
            float intersectionA = toCenterOnLine - fromClosestPointToIntersection;
            float intersectionB = toCenterOnLine + fromClosestPointToIntersection;

            if (intersectionA > intersectionB)
            {
                PTUtils.Swap(ref intersectionA, ref intersectionB);
            }

            if (intersectionA < 0)
            {
                intersectionA = intersectionB;
                if (intersectionA < 0)
                {
                    pointA = Vector2.zero;
                    pointB = Vector2.zero;
                    return false;
                }
            }

            pointA = origin + intersectionA*direction;
            pointB = origin + intersectionB*direction;
            return true;
        }

        #endregion Ray-Circle

        #region Circle-Circle

        /// <summary>
        /// Returns a distance between the closest points on the circles
        /// </summary>
        /// <returns>
        /// Positive value if the circles do not intersect, negative otherwise.
        /// Negative value can be interpreted as depth of penetration.
        /// </returns>
        public static float DistanceToCircle(Circle circleA, Circle circleB)
        {
            return DistanceToCircle(circleA.center, circleA.radius, circleB.center, circleB.radius);
        }

        /// <summary>
        /// Returns a distance between the closest points on the circles
        /// </summary>
        /// <returns>
        /// Positive value if the circles do not intersect, negative otherwise.
        /// Negative value can be interpreted as depth of penetration.
        /// </returns>
        public static float DistanceToCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
        {
            return Vector2.Distance(centerA, centerB) - radiusA - radiusB;
        }

        /// <summary>
        /// Finds closest points on circles
        /// </summary>
        public static void ClosestPointsOnCircles(Circle circleA, Circle circleB, out Vector2 pointA, out Vector2 pointB)
        {
            ClosestPointsOnCircles(circleA.center, circleA.radius, circleB.center, circleB.radius, out pointA, out pointB);
        }

        /// <summary>
        /// Finds closest points on circles
        /// </summary>
        public static void ClosestPointsOnCircles(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB,
            out Vector2 pointA, out Vector2 pointB)
        {
            Vector2 aToB = (centerB - centerA).normalized;
            pointA = centerA + aToB*radiusA;
            pointB = centerB - aToB*radiusB;
        }

        /// <summary>
        /// Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool IntersectCircleCircle(Circle circleA, Circle circleB, out IntersectionCircleCircle intersection)
        {
            return IntersectCircleCircle(circleA.center, circleA.radius, circleB.center, circleB.radius, out intersection);
        }

        /// <summary>
        /// Computes an intersection of the circles
        /// </summary>
        /// <returns>True if the circles intersect or one circle is contained within the other</returns>
        public static bool IntersectCircleCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB,
            out IntersectionCircleCircle intersection)
        {
            intersection = new IntersectionCircleCircle();

            Vector2 fromBtoA = centerA - centerB;
            float distanceFromBtoASqr = fromBtoA.sqrMagnitude;
            if (distanceFromBtoASqr < EpsilonSqr)
            {
                if (Mathf.Abs(radiusA - radiusB) < Epsilon)
                {
                    // Circles are coincident
                    intersection.type = IntersectionType.Circle;
                    return true;
                }
                // One circle is inside the other
                intersection.type = IntersectionType.None;
                return true;
            }

            float sumOfRadii = radiusA + radiusB;
            float sumOfRadiiSqr = sumOfRadii*sumOfRadii;
            if (distanceFromBtoASqr > sumOfRadiiSqr)
            {
                // No intersections, circles are separate
                intersection.type = IntersectionType.None;
                return false;
            }
            if (Mathf.Abs(distanceFromBtoASqr - sumOfRadiiSqr) < EpsilonSqr)
            {
                // One intersection outside
                intersection.type = IntersectionType.Point;
                intersection.pointA = centerB + fromBtoA*(radiusB/sumOfRadii);
                return true;
            }

            float differenceOfRadii = radiusA - radiusB;
            float differenceOfRadiiSqr = differenceOfRadii*differenceOfRadii;
            if (distanceFromBtoASqr < differenceOfRadiiSqr)
            {
                // One circle is contained within the other
                intersection.type = IntersectionType.None;
                return true;
            }
            if (Mathf.Abs(distanceFromBtoASqr - differenceOfRadiiSqr) < EpsilonSqr)
            {
                // One intersection inside
                intersection.type = IntersectionType.Point;
                intersection.pointA = centerB - fromBtoA*(radiusB/differenceOfRadii);
                return true;
            }

            // Two intersections
            intersection.type = IntersectionType.TwoPoints;

            float radiusASqr = radiusA*radiusA;
            float distanceToMiddle = 0.5f*(radiusASqr - radiusB*radiusB)/distanceFromBtoASqr + 0.5f;
            Vector2 middle = centerA - fromBtoA*distanceToMiddle;

            float discriminant = radiusASqr/distanceFromBtoASqr - distanceToMiddle*distanceToMiddle;
            if (discriminant < 0)
            {
                discriminant = 0;
            }
            Vector2 offset = fromBtoA.Perp()*Mathf.Sqrt(discriminant);

            intersection.pointA = middle + offset;
            intersection.pointB = middle - offset;
            return true;
        }

        #endregion Circle-Circle
    }
}
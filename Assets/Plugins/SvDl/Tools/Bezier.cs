using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SvDl.Tools
{
    /// <summary>
    /// Bezier
    /// </summary>
    public struct Bezier
    {
        /// <summary>
        /// Start
        /// </summary>
        private Vector2 _start;

        /// <summary>
        /// Curve
        /// </summary>
        private float _ax;
        private float _bx;
        private float _cx;
        private float _ay;
        private float _by;
        private float _cy;

        //-------------------------------------------------------
        /// <summary>
        /// Private Constructor
        /// </summary>
        public Bezier(Vector2 start, Vector2 end, Vector2 startTangent, Vector2 endTangent)
        {
            //Memorize
            this._start = start;

            //Build Curve
            this._cx = 3 * (startTangent.x - start.x);
            this._bx = 3 * (endTangent.x - startTangent.x) - this._cx;
            this._ax = end.x - start.x - this._cx - this._bx;

            this._cy = 3 * (startTangent.y - start.y);
            this._by = 3 * (endTangent.y - startTangent.y) - this._cy;
            this._ay = end.y - start.y - this._cy - this._by;
        }

        //-------------------------------------------------------
        /// <summary>
        /// Get Value
        /// </summary>
        /// <returns></returns>
        public float GetX(float t)
        {
            //Clamp t
            t = Mathf.Clamp01(t);

            //Calculate X
            return (this._ax * t * t * t) + (this._bx * t * t) + (this._cx * t) + this._start.x;
        }

        //-------------------------------------------------------
        /// <summary>
        /// Get Value
        /// </summary>
        /// <returns></returns>
        public float GetY(float t)
        {
            //Clamp t
            t = Mathf.Clamp01(t);

            //Calculate X
            return (this._ay * t * t * t) + (this._by * t * t) + (this._cy * t) + this._start.y;
        }

        //-------------------------------------------------------
        /// <summary>
        /// Get Value
        /// </summary>
        /// <returns></returns>
        public Vector2 GetValue(float t)
        {
            //Clamp t
            t = Mathf.Clamp01(t);

            //Calc Point
            float x = this.GetX(t);
            float y = this.GetY(t);

            //Return value
            return new Vector2(x, y);
        }

        //----------------------------------------------------------
        /// <summary>
        /// Build Single curve with startPoint at (0,0) and endPoint at (1,1)
        /// </summary>
        /// <param name="startTangent"></param>
        /// <param name="endTangent"></param>
        /// <returns></returns>
        public static Bezier IncreasingCurve(Vector2 startTangent, Vector2 endTangent)
        {
            Bezier bezier = new Bezier(Vector2.zero, Vector2.one, startTangent, endTangent);
            return bezier;
        }

        //----------------------------------------------------------
        /// <summary>
        /// Build Single curve with startPoint at (0,1) and endPoint at (1,0)
        /// </summary>
        /// <param name="startTangent"></param>
        /// <param name="endTangent"></param>
        /// <returns></returns>
        public static Bezier DecreasingCurve(Vector2 startTangent, Vector2 endTangent)
        {
            Bezier bezier = new Bezier(new Vector2(0, 1), new Vector2(1, 0), startTangent, endTangent);
            return bezier;
        }
    }
}
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace SvDl.Tools
{
    public class BezierTimed
    {
        /// <summary>
        /// Key Frames
        /// </summary>
        private List<Keyframe> _keyframes;

        /// <summary>
        /// Start Time
        /// </summary>
        public float startTime { get { return this._keyframes[0].time; } }

        /// <summary>
        /// End Time
        /// </summary>
        public float endTime { get { return this._keyframes[this._keyframes.Count - 1].time; } }

        //---------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        /// <returns></returns>
        private BezierTimed()
        {
            this._keyframes = new List<Keyframe>();
        }

        //---------------------------------------------------
        /// <summary>
        /// Add Keyframe
        /// </summary>
        /// <param name="keyframe"></param>
        public void Add(Keyframe keyframe)
        {
            this._keyframes.Add(keyframe);
            this._keyframes = this._keyframes.OrderBy(o => o.time).ToList();
        }

        //---------------------------------------------------
        /// <summary>
        /// Get Value
        /// </summary>
        /// <param name="keyframe"></param>
        public float GetValue(float time)
        {
            //Check interval
            if (this._keyframes == null || this._keyframes.Count == 0) return 0;
            if (this._keyframes.Count == 1) return this._keyframes[0].value;
            if (time < this.startTime + 0.001f) return this._keyframes[0].value;
            if (time > this.endTime - 0.001f) return this._keyframes[this._keyframes.Count - 1].value;

            //Get Index
            int index = 0;
            for(int k=0; k<this._keyframes.Count; k++)
            {
                if (time < this._keyframes[k].time) break;
                index = k;
            }

            //Eval T
            float t = Mathf.Clamp01((time - this._keyframes[index].time) / (this._keyframes[index + 1].time - this._keyframes[index].time));

            //Build Curve
            float lv = this._keyframes[index].value + (this._keyframes[index + 1].value - this._keyframes[index].value) * this._keyframes[index].right.y;
            float rv = this._keyframes[index + 1].value - (this._keyframes[index + 1].value - this._keyframes[index].value) * this._keyframes[index + 1].left.y;

            float cv = 3 * (lv - this._keyframes[index].value);
            float bv = 3 * (rv - lv) - cv;
            float av = (this._keyframes[index + 1].value - this._keyframes[index].value) - cv - bv;

            //Return value
            return (av * (t * t * t)) + (bv * (t * t)) + (cv * t) + this._keyframes[index].value;
        }

        //---------------------------------------------------
        /// <summary>
        /// Factory
        /// </summary>
        /// <returns></returns>
        public static BezierTimed Create(Keyframe key1, Keyframe key2, params Keyframe[] keys)
        {
            BezierTimed bc = new BezierTimed();
            bc.Add(key1);
            bc.Add(key2);
            for(int k=0; k<keys.Length; k++)
            {
                bc.Add(keys[k]);
            }
            return bc;
        }

        //---------------------------------------------------
        /// <summary>
        /// Factory
        /// </summary>
        /// <returns></returns>
        public static BezierTimed Create(Keyframe[] keys)
        {
            BezierTimed bc = new BezierTimed();
            for (int k = 0; k < keys.Length; k++)
            {
                bc.Add(keys[k]);
            }
            return bc;
        }

        //-------------------------------------------------------------
        /// <summary>
        /// Key Frame Struct
        /// </summary>
        public struct Keyframe
        {
            public float time;
            public float value;
            public Vector2 left;
            public Vector2 right;

            //--------------------------------------------------
            /// <summary>
            /// Keyframe Constructor
            /// </summary>
            /// <param name="time"></param>
            /// <param name="value"></param>
            /// <param name="handle"></param>
            public Keyframe(float time, float value)
            {
                this.time = time;
                this.value = value;
                this.left = Vector2.zero;
                this.right = Vector2.zero;
            }
        }
    }

    /*
    //-------------------------------------------
    /// <summary>
    /// Bezier Point Structure
    /// </summary>
    private struct BezierCurve
    {
        public BezierPoint start { get; private set; }
        public BezierPoint end { get; private set; }

        private float ax;
        private float bx;
        private float cx;

        private float ay;
        private float by;
        private float cy;

        //------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handle"></param>
        public BezierCurve(BezierPoint start, BezierPoint end)
        {
            this.start = start;
            this.end = end;

            this.cx = 3 * (this.start.handle.x - this.start.point.x);
            this.bx = 3 * (this.end.handle.x - this.start.handle.x) - this.cx;
            this.ax = (this.end.point.x - this.start.point.x) - this.cx - this.bx;

            this.cy = 3 * (this.start.handle.y - this.start.point.y);
            this.by = 3 * (this.end.handle.y - this.start.handle.y) - this.cy;
            this.ay = (this.end.point.y - this.start.point.y) - this.cy - this.by;
        }

        //------------------------------------------------
        /// <summary>
        /// Get Value
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector2 GetValue(float t)
        {
            t = Mathf.Clamp01(t);
            float tCubed = t * t * t;
            float tSquared = t * t;
            float x = (this.ax * tCubed) + (this.bx * tSquared) + (this.cx * t) + this.start.point.x;
            float y = (this.ay * tCubed) + (this.by * tSquared) + (this.cy * t) + this.start.point.y;
            return new Vector2(x, y);
        }
    }*/
}
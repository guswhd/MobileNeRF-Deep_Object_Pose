using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JsonDatas
{
    [System.Serializable]
    public class camera_look_at
    {
        public float[] at = new float[3];
        public float[] eye = new float[3];
        public float[] up = new float[3];
    }

    [System.Serializable]
    public class intrinsics
    {

        public float cx;
        public float cy;
        public float fx;
        public float fy;


    }
    [System.Serializable]
    public class jsonMaxtrix
    {

        public float[][] Mat;
    }

}

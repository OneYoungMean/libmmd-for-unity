using UnityEngine;

namespace LibMMD.Model
{
    public class Vertex
    {
        //OYM：看不懂...
        //OYM：算了反正不重要ε=ε=┏( >_<)┛
        public Vector3 Coordinate { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 UvCoordinate { get; set; }
        public Vector4[] ExtraUvCoordinate { get; set; }
        public SkinningOperator SkinningOperator { get; set; }
        public float EdgeScale { get; set; }
    }
}
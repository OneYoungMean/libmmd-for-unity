using UnityEngine;

namespace LibMMD.Model
{
    public class Vertex
    {
        //OYM：看不懂...
        //OYM：算了反正不重要ε=ε=┏( >_<)┛
        /// <summary>
        /// 顶点坐标
        /// </summary>
        public Vector3 Coordinate { get; set; }
        /// <summary>
        /// 法线
        /// </summary>
        public Vector3 Normal { get; set; }
        /// <summary>
        /// uv坐标
        /// </summary>
        public Vector2 UvCoordinate { get; set; }
        /// <summary>
        /// 额外的uv坐标
        /// </summary>
        public Vector4[] ExtraUvCoordinate { get; set; }
        /// <summary>
        /// 跟皮肤操作相关的吧...
        /// </summary>
        public SkinningOperator SkinningOperator { get; set; }
        /// <summary>
        /// 额外的大小?向外的大小?
        /// </summary>
        public float EdgeScale { get; set; }
    }
}
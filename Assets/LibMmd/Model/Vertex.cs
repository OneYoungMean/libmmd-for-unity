using UnityEngine;

namespace LibMMD.Model
{
    public class Vertex
    {
        //OYM��������...
        //OYM�����˷�������Ҫ��=��=��( >_<)��
        /// <summary>
        /// ��������
        /// </summary>
        public Vector3 Coordinate { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public Vector3 Normal { get; set; }
        /// <summary>
        /// uv����
        /// </summary>
        public Vector2 UvCoordinate { get; set; }
        /// <summary>
        /// �����uv����
        /// </summary>
        public Vector4[] ExtraUvCoordinate { get; set; }
        /// <summary>
        /// ��Ƥ��������صİ�...
        /// </summary>
        public SkinningOperator SkinningOperator { get; set; }
        /// <summary>
        /// ����Ĵ�С?����Ĵ�С?
        /// </summary>
        public float EdgeScale { get; set; }
    }
}
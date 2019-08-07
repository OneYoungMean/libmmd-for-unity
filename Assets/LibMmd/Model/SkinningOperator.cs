using UnityEngine;

namespace LibMMD.Model
{
    /// <summary>
    /// ��Ƥ�Ĳ���,�������ƶ��������Щ�����ƶ�
    /// </summary>
    public class SkinningOperator
    {
        /// <summary>
        /// ��Ƥ����
        /// </summary>
        public enum SkinningType : byte
        {
            /// <summary>
            /// ����������
            /// </summary>
            SkinningBdef1 = 0,
            /// <summary>
            /// ������������
            /// </summary>
            SkinningBdef2 = 1,
            /// <summary>
            /// �ĸ���������
            /// </summary>
            SkinningBdef4 = 2,
            /// <summary>
            /// 
            /// </summary>
            SkinningSdef = 3
        }

        public abstract class SkinningParam
        {
        }

        public class Bdef1 : SkinningParam
        {
            public int BoneId { get; set; }
        }

        public class Bdef2 : SkinningParam
        {
            public Bdef2()
            {
                BoneId = new int[2];
            }

            public int[] BoneId { get; set; }
            public float BoneWeight { get; set; }
        }

        public class Bdef4 : SkinningParam
        {
            public Bdef4()
            {
                BoneId = new int[4];
                BoneWeight = new float[4];
            }

            public int[] BoneId { get; set; }
            public float[] BoneWeight { get; set; }
        }

        public class Sdef : SkinningParam
        {
            public Sdef()
            {
                BoneId = new int[2];
            }
            /// <summary>
            /// ����ID
            /// </summary>
            public int[] BoneId { get; set; }
            /// <summary>
            /// ����Ȩ��
            /// </summary>
            public float BoneWeight { get; set; }
            /// <summary>
            /// Բ��?
            /// </summary>
            public Vector3 C { get; set; }
            /// <summary>
            /// �뾶a?
            /// </summary>
            public Vector3 R0 { get; set; }
            /// <summary>
            /// �뾶B?
            /// </summary>
            public Vector3 R1 { get; set; }
        }

        public SkinningType Type { get; set; }
        public SkinningParam Param { get; set; }
    }
}
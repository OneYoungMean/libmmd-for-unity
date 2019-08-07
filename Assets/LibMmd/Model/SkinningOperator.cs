using UnityEngine;

namespace LibMMD.Model
{
    /// <summary>
    /// 蒙皮的操作,用来控制顶点跟随哪些骨骼移动
    /// </summary>
    public class SkinningOperator
    {
        /// <summary>
        /// 蒙皮种类
        /// </summary>
        public enum SkinningType : byte
        {
            /// <summary>
            /// 单骨骼控制
            /// </summary>
            SkinningBdef1 = 0,
            /// <summary>
            /// 两根骨骼控制
            /// </summary>
            SkinningBdef2 = 1,
            /// <summary>
            /// 四根骨骼控制
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
            /// 骨骼ID
            /// </summary>
            public int[] BoneId { get; set; }
            /// <summary>
            /// 骨骼权重
            /// </summary>
            public float BoneWeight { get; set; }
            /// <summary>
            /// 圆心?
            /// </summary>
            public Vector3 C { get; set; }
            /// <summary>
            /// 半径a?
            /// </summary>
            public Vector3 R0 { get; set; }
            /// <summary>
            /// 半径B?
            /// </summary>
            public Vector3 R1 { get; set; }
        }

        public SkinningType Type { get; set; }
        public SkinningParam Param { get; set; }
    }
}
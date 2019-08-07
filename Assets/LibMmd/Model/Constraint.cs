using UnityEngine;

namespace LibMMD.Model
{
    /// <summary>
    /// 约束
    /// </summary>
    public class Constraint
    {
        /// <summary>
        /// 构造一个约束
        /// </summary>
        public Constraint()
        {
            //OYM：为什么只有两个
            AssociatedRigidBodyIndex = new int[2];
        }
        /// <summary>
        /// 约束名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 约束英文
        /// </summary>
        public string NameEn { get; set; }
        /// <summary>
        /// 约束相关的骨骼
        /// </summary>
        public int[] AssociatedRigidBodyIndex { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// 旋转
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// 最小位置
        /// </summary>
        public Vector3 PositionLowLimit { get; set; }
        /// <summary>
        /// 最大位置,等下这个是干啥用的?
        /// </summary>
        public Vector3 PositionHiLimit { get; set; }
        /// <summary>
        /// 旋转约束
        /// </summary>
        public Vector3 RotationLowLimit { get; set; }
        /// <summary>
        /// 这两个参数我硬是没看懂怎么用的
        /// </summary>
        public Vector3 RotationHiLimit { get; set; }
        /// <summary>
        /// 暂时不知道
        /// </summary>
        public Vector3 SpringTranslate { get; set; }
        /// <summary>
        /// 暂时不知道*2
        /// </summary>
        public Vector3 SpringRotate { get; set; }
    }
}
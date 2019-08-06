using System;

namespace LibMMD.Model
{
    public class MmdModel
    {
        public string Name { get; set; }//OYM：姓名
        public string NameEn { get; set; }//OYM：英文名
        public string Description { get; set; }//OYM：定义
        public string DescriptionEn { get; set; }//OYM：定义英文
        public Vertex[] Vertices { get; set; }//OYM：顶点
        public int ExtraUvNumber { get; set; }//OYM：额外UV
        public int[] TriangleIndexes { get; set; }//OYM：三角形?
        public Part[] Parts { get; set; }//OYM：暂时不知道
        public Bone[] Bones { get; set; }//OYM：骨骼
        public Morph[] Morphs { get; set; }//OYM：顶点变形
        public MmdRigidBody[] Rigidbodies { get; set; }//OYM：刚体
        public Constraint[] Constraints { get; set; }//OYM：这个我忘了

        public void Normalize()
        {
            foreach (var vertex in Vertices)
            {
                switch (vertex.SkinningOperator.Type)
                {
                    case SkinningOperator.SkinningType.SkinningBdef2:
                    {
                        var oldBdef2 = (SkinningOperator.Bdef2) vertex.SkinningOperator.Param;
                        var weight = oldBdef2.BoneWeight;
                        if (Math.Abs(weight) < 0.000001f)
                        {
                            var bdef1 = new SkinningOperator.Bdef1 {BoneId = oldBdef2.BoneId[1]};
                            vertex.SkinningOperator.Param = bdef1;
                            vertex.SkinningOperator.Type = SkinningOperator.SkinningType.SkinningBdef1;
                        }
                        else if (Math.Abs(weight - 1.0f) < 0.00001f)
                        {
                            var bdef1 = new SkinningOperator.Bdef1 {BoneId = oldBdef2.BoneId[0]};
                            vertex.SkinningOperator.Param = bdef1;
                            vertex.SkinningOperator.Type = SkinningOperator.SkinningType.SkinningBdef1;
                        }
                        break;
                    }
                    case SkinningOperator.SkinningType.SkinningSdef:
                    {
                        var oldSdef = (SkinningOperator.Sdef) vertex.SkinningOperator.Param;
                        var bone0 = oldSdef.BoneId[0];
                        var bone1 = oldSdef.BoneId[1];
                        var weight = oldSdef.BoneWeight;
                        if (
                            Bones[bone0].ParentIndex != bone1 &&
                            Bones[bone1].ParentIndex != bone0
                        )
                        {
                            if (Math.Abs(weight) < 0.000001f)
                            {
                                var bdef1 = new SkinningOperator.Bdef1 {BoneId = bone1};
                                vertex.SkinningOperator.Param = bdef1;
                                vertex.SkinningOperator.Type = SkinningOperator.SkinningType.SkinningBdef1;
                            }
                            else if (Math.Abs(weight - 1.0f) < 0.00001f)
                            {
                                var bdef1 = new SkinningOperator.Bdef1 {BoneId = bone0};
                                vertex.SkinningOperator.Param = bdef1;
                                vertex.SkinningOperator.Type = SkinningOperator.SkinningType.SkinningBdef1;
                            }
                            else
                            {
                                var bdef2 = new SkinningOperator.Bdef2();
                                bdef2.BoneId[0] = bone0;
                                bdef2.BoneId[1] = bone1;
                                bdef2.BoneWeight = weight;
                                vertex.SkinningOperator.Param = bdef2;
                                vertex.SkinningOperator.Type = SkinningOperator.SkinningType.SkinningBdef2;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
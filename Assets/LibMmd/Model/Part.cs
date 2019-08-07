namespace LibMMD.Model
{
    public class Part
    {
        /// <summary>
        /// materrial种类
        /// </summary>
        public Material.MmdMaterial Material { get; set; }
        /// <summary>
        /// 作者都说了是顶点数,就当做顶点数目吧...
        /// </summary>
        public int BaseShift { get; set; } //注意这个和libmmd里的含义不同，这里是三角形顶点数
        /// <summary>
        /// 看不懂,告辞
        /// </summary>
        public int TriangleIndexNum { get; set; }
    }
}
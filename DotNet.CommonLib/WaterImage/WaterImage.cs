namespace DotNet.CommonLib.WaterImage
{
    /// <summary>
    /// 装载水印图片的相关信息
    /// </summary>
    public class WaterImage
    {
        public WaterImage() { }

        private string m_sourcePicture;
        /// <summary>
        /// 源图片地址名字(带后缀)

        /// </summary>
        public string SourcePicture
        {
            get { return m_sourcePicture; }
            set { m_sourcePicture = value; }
        }

        private string m_waterImager;
        /// <summary>
        /// 水印图片名字(带后缀)
        /// </summary>
        public string WaterPicture
        {
            get { return m_waterImager; }
            set { m_waterImager = value; }
        }

        private float m_alpha;
        /// <summary>
        /// 水印图片文字的透明度
        /// </summary>
        public float Alpha
        {
            get { return m_alpha; }
            set { m_alpha = value; }
        }

        private ImagePosition m_postition;
        /// <summary>
        /// 水印图片或文字在图片中的位置
        /// </summary>
        public ImagePosition Position
        {
            get { return m_postition; }
            set { m_postition = value; }
        }

        private string m_words;
        /// <summary>
        /// 水印文字的内容
        /// </summary>
        public string Words
        {
            get { return m_words; }
            set { m_words = value; }
        }

    }
}
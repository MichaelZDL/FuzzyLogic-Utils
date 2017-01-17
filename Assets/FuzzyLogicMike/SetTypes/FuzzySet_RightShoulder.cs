/*-----------------------------------------------------------------------------

 * 名称：FuzzySet_RightShoulder “右肩形”隶属函数类
 * 作者：zhdelong@foxmail.com
 * 日期：2016.5.4

-----------------------------------------------------------------------------*/
namespace FuzzyLogicMike {
    public class FuzzySet_RightShoulder : FuzzySet {
        /*-----------------------------------------------------------------------------
         * 定义该模糊变量隶属函数形状的值
        -----------------------------------------------------------------------------*/
        private double m_dPeakPoint;
        private double m_dLeftOffset;
        private double m_dRightOffset;

        /*-----------------------------------------------------------------------------
         * 构造方法
        -----------------------------------------------------------------------------*/
        public FuzzySet_RightShoulder(double peak,double LeftOffset,double RightOffset)
            : base(((peak + RightOffset) + peak) / 2) {
            m_dPeakPoint = peak;
            m_dLeftOffset = LeftOffset;
            m_dRightOffset = RightOffset;
        }
        /*-----------------------------------------------------------------------------
         * 用这种方法计算一个特定值val的隶属度，这里val是横坐标的值
        -----------------------------------------------------------------------------*/
        public override double CalculateDOM(double val) {
            //检查偏移量为0的情况
            if (((m_dRightOffset == 0) && (m_dPeakPoint == val)) ||
                ((m_dLeftOffset == 0) && (m_dPeakPoint == val))) {
                return 1.0;
            }
            //若在中间的左边，计算隶属度
            else if ((val <= m_dPeakPoint) && (val > (m_dPeakPoint - m_dLeftOffset))) {
                double grad = 1.0 / m_dLeftOffset;
                return grad * (val - (m_dPeakPoint - m_dLeftOffset));
            } 
            //若在中间的右边，计算隶属度
            else if ((val > m_dPeakPoint) && (val <= m_dPeakPoint + m_dRightOffset)) {
                return 1.0;
            } 
            //若在这个模糊语言变量的范围之外，返回值为0
            else {
                return 0;
            }
        }
    }
}

/*-----------------------------------------------------------------------------

 * 名称：FuzzySet_Singleton “常数形”隶属函数类
 * 作者：zhdelong@foxmail.com
 * 日期：2016.5.4

-----------------------------------------------------------------------------*/
namespace FuzzyLogicMike {
    public class FuzzySet_Singleton : FuzzySet {
        /*-----------------------------------------------------------------------------
         * 定义该模糊变量隶属函数形状的值
        -----------------------------------------------------------------------------*/
        private double m_dMidPoint;
        private double m_dLeftOffset;
        private double m_dRightOffset;

        /*-----------------------------------------------------------------------------
         * 构造方法
        -----------------------------------------------------------------------------*/
        public FuzzySet_Singleton(double mid,double lft,double rgt)
            : base(mid) {
            m_dMidPoint = mid;
            m_dLeftOffset = lft;
            m_dRightOffset = rgt;
        }
        /*-----------------------------------------------------------------------------
         * 用这种方法计算一个特定值val的隶属度，这里val是横坐标的值
        -----------------------------------------------------------------------------*/
        public override double CalculateDOM(double val) {
            //在范围内，就等于1
            if ((val >= m_dMidPoint - m_dLeftOffset)
                 && (val <= m_dMidPoint + m_dRightOffset)) {
                return 1.0;
            } 
            //不在范围内，就等于0
            else {
                return 0.0;
            }
        }
    }
}

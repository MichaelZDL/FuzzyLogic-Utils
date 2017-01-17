/*-----------------------------------------------------------------------------

 * 名称：FuzzySet 各种形状隶属函数类的基类
 * 作者：zhdelong@foxmail.com
 * 日期：2016.5.4

-----------------------------------------------------------------------------*/
using UnityEngine;
namespace FuzzyLogicMike {
    public abstract class FuzzySet {
        /*-----------------------------------------------------------------------------
         * 构造函数
        -----------------------------------------------------------------------------*/
        public FuzzySet(double RepVal) {
            m_dDOM = 0.0;
            m_dRepresentativeValue = RepVal;
        }
        /*-----------------------------------------------------------------------------
         * m_dDOM：隶属度
         * m_dRepresentativeValue：隶属度最大值对应的横坐标
         * GetRepresentativeVal()：得到m_dRepresentativeValue
        -----------------------------------------------------------------------------*/
        protected double m_dDOM;
        protected double m_dRepresentativeValue;
        public double GetRepresentativeVal() {
            return m_dRepresentativeValue;
        }
        /*-----------------------------------------------------------------------------
         * CalculateDOM：要求子类重写该方法，用于计算特定值val在该隶属函数下的隶属度
        -----------------------------------------------------------------------------*/
        public abstract double CalculateDOM(double val);
        /*-----------------------------------------------------------------------------
         * ORwithDOM：拿书中的那个例子来说，ORwithDOM的作用就是
                      当9个规则算出有两个Undesirable时，或运算一下，取最大的那个0.33
        -----------------------------------------------------------------------------*/
        public void ORwithDOM(double val) {
            if (val > m_dDOM) {
                m_dDOM = val;
            }
        }
        /*---------------------------------------------------------------------------*/
        public void ClearDOM() {
            m_dDOM = 0.0;
        }
        public double GetDOM() {
            return m_dDOM;
        }
        public void SetDOM(double val) {
            Debug.Assert((val <= 1) && (val >= 0), "<FuzzySet.SetDOM>: invalid value");
            m_dDOM = val;
        }
    }
}

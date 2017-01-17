/*-----------------------------------------------------------------------------

 * 名称：用于规则复合运算元素的“或”运算
 * 作者：zhdelong@foxmail.com
 * 日期：2016.5.4

-----------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace FuzzyLogicMike {
    public class FzOR : FuzzyTerm {
        /*-----------------------------------------------------------------------------
         * 把“或”运算中的所有元素存到这个List中
        -----------------------------------------------------------------------------*/
        private List<FuzzyTerm> m_Terms = new List<FuzzyTerm>(4);
        /*-----------------------------------------------------------------------------
         * 二值构造方法
        -----------------------------------------------------------------------------*/
        public FzOR(FuzzyTerm op1, FuzzyTerm op2) {
            m_Terms.Add(op1.Clone());
            m_Terms.Add(op2.Clone());
        }
        /*-----------------------------------------------------------------------------
         * 三值构造方法
        -----------------------------------------------------------------------------*/
        public FzOR(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3) {
            m_Terms.Add(op1.Clone());
            m_Terms.Add(op2.Clone());
            m_Terms.Add(op3.Clone());
        }
        /*-----------------------------------------------------------------------------
         * 四值构造方法
        -----------------------------------------------------------------------------*/
        public FzOR(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3, FuzzyTerm op4) {
            m_Terms.Add(op1.Clone());
            m_Terms.Add(op2.Clone());
            m_Terms.Add(op3.Clone());
            m_Terms.Add(op4.Clone());
        }
        /*-----------------------------------------------------------------------------
         * 拷贝自身,即建一个新FzOR,把原FzOR的m_Terms全部复制到新FzOR中
        -----------------------------------------------------------------------------*/
        public override FuzzyTerm Clone() {
            return (FuzzyTerm)new FzOR(this);
        }

        public FzOR(FzOR fa) {
            foreach (FuzzyTerm ft in fa.m_Terms) {
                m_Terms.Add(ft.Clone());
            }
        }
        /*-----------------------------------------------------------------------------
         * 重写的GetDOM()是进行与运算的核心，即对m_Terms所有元素取最小值
         * 其余的ClearDOM，ORwithDOM都是对复合运算中的每一个元素进行相应操作
        -----------------------------------------------------------------------------*/
        public override double GetDOM() {
            double largest = 0.0;
            foreach (FuzzyTerm ft in m_Terms) {
                if (ft.GetDOM() > largest) {
                    largest = ft.GetDOM();
                }
            }
            return largest;
        }
        public override void ClearDOM() {
            foreach (FuzzyTerm ft in m_Terms) {
                ft.ClearDOM();
            }
        }
        public override void ORwithDOM(double val) {
            foreach (FuzzyTerm ft in m_Terms) {
                ft.ORwithDOM(val);
            }
        }
    }
}

/*-----------------------------------------------------------------------------

 * 名称：FuzzyTerm 运算元素的基类（用于“前提-结果”规则运算的）
 * 作者：zhdelong@foxmail.com
 * 日期：2016.5.4

-----------------------------------------------------------------------------*/
namespace FuzzyLogicMike {
    public abstract class FuzzyTerm {
        /*-----------------------------------------------------------------------------
         * 定义了几个抽象方法，用作所有规则运算元素的公共接口
        -----------------------------------------------------------------------------*/
        public abstract FuzzyTerm Clone();
        public abstract double GetDOM();
        public abstract void ClearDOM();
        public abstract void ORwithDOM(double val);
    }
}

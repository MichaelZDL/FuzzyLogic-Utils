/*-----------------------------------------------------------------------------

 * 名称：FuzzyRule 模糊逻辑规则类
 * 作者：zhdelong@foxmail.com
 * 日期：2016.5.4

-----------------------------------------------------------------------------*/
using UnityEngine;
namespace FuzzyLogicMike {
    public class FuzzyRule {
        /*-----------------------------------------------------------------------------
         * 构造方法
        -----------------------------------------------------------------------------*/
        public FuzzyRule(FuzzyTerm ant, FuzzyTerm con) {
            m_pAntecedent = ant.Clone();
            m_pConsequence = con.Clone();
        }
        /*-----------------------------------------------------------------------------
         * 不允许复制，原书中的private FuzzyRule& operator=(const FuzzyRule&);
        -----------------------------------------------------------------------------*/
        public FuzzyRule(FuzzyRule fr) {
            Debug.Assert(false, "Unsupported operation"); 
        }
        /*-----------------------------------------------------------------------------
         * m_pAntecedent：条件元素
         * m_pConsequence：结果元素
        -----------------------------------------------------------------------------*/
        public FuzzyTerm m_pAntecedent;
        public FuzzyTerm m_pConsequence;
        /*-----------------------------------------------------------------------------
         * 清零结果元素的DOM
        -----------------------------------------------------------------------------*/
        public void SetConfidenceOfConsequentToZero() {
            m_pConsequence.ClearDOM();
        }
        /*-----------------------------------------------------------------------------
         * 将条件元素DOM带入规则中，计算出结果元素DOM
        -----------------------------------------------------------------------------*/
        public void Calculate() {
            m_pConsequence.ORwithDOM(m_pAntecedent.GetDOM());
        }
    }
}

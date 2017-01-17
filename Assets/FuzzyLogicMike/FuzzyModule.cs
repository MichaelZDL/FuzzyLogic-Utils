/*-----------------------------------------------------------------------------

 * 名称：FuzzyModule 模糊逻辑通用模块
 * 作者：zhdelong@foxmail.com
 * 日期：2016.5.4

-----------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FuzzyLogicMike {
    public class FuzzyModule {

        /*-----------------------------------------------------------------------------
         * m_Variables：所有“模糊语言变量”存到Dictionary m_Variables中
         * m_Rules：所有“规则集”存到List m_Rules中
        -----------------------------------------------------------------------------*/
        private Dictionary<string, FuzzyVariable> m_Variables = new Dictionary<string, FuzzyVariable>();
        private List<FuzzyRule> m_Rules = new List<FuzzyRule>();

        /*-----------------------------------------------------------------------------
         * DefuzzifyMethod：用于设置“去模糊化的方式”
                            如：中心法（centroid）和最大值平均（max_av）
         * NumSamples：用“中心法”时，设置抽样点的个数NumSamples
        -----------------------------------------------------------------------------*/
        public enum DefuzzifyMethod {
            max_av, 
            centroid
        };
        public const int NumSamples = 10;

        /*-----------------------------------------------------------------------------
         * CreateFLV()：往m_Variables中添加“空的”模糊语言变量
         * AddRule()：往m_Rules中添加规则
        -----------------------------------------------------------------------------*/
        public FuzzyVariable CreateFLV(string VarName) {
            m_Variables.Add(VarName, new FuzzyVariable());
            return m_Variables[VarName];
        }
        public void AddRule(FuzzyTerm antecedent, FuzzyTerm consequence) {
            m_Rules.Add(new FuzzyRule(antecedent, consequence));
        }

        /*-----------------------------------------------------------------------------
         * Fuzzify：模糊化，为某个模糊语言变量计算在特定值val下的DOM的过程叫Fuzzify
        -----------------------------------------------------------------------------*/
        public void Fuzzify(string NameOfFLV, double val) {
            //首先确保键值非空
            Debug.Assert(m_Variables[NameOfFLV] != null, 
                "<FuzzyModule.Fuzzify()>:m_Variables[NameOfFLV] is NULL");

            m_Variables[NameOfFLV].Fuzzify(val);
        }

        /*-----------------------------------------------------------------------------
         * DeFuzzify：去模糊化，将模糊化算出的DOM放入规则中进行一系列运算，
                      最后算出“结果模糊语言变量”值（期望值）的过程叫DeFuzzify
         * NameOfFLV：“结果模糊语言变量”即书中例子中的期望值
         * DefuzzifyMethod：最后求期望值时用“中心法”还是“最大值平均”
        -----------------------------------------------------------------------------*/
        public double DeFuzzify(string  NameOfFLV, DefuzzifyMethod method) {
            //首先确保键值非空
            Debug.Assert(m_Variables[NameOfFLV] != null, "<FuzzyModule.DeFuzzify>:m_Variables[NameOfFLV] is NULL");
                

            SetConfidencesOfConsequentsToZero();
            int i = 0;
            foreach (FuzzyRule fr in m_Rules) {
                fr.Calculate();

                Debug.Log("Rule" + (++i).ToString() + " Consequence " + ": " + fr.m_pAntecedent.GetDOM().ToString("f4"));
            }

		    //中心法还是最大值平均法
            switch (method) {
                case DefuzzifyMethod.centroid:
                    return m_Variables[NameOfFLV].DeFuzzifyCentroid(NumSamples);
                case DefuzzifyMethod.max_av:
                    return m_Variables[NameOfFLV].DeFuzzifyMaxAv();
            }

            return 0.0;
        }
        /*-----------------------------------------------------------------------------
         * SetConfidencesOfConsequentsToZero：结果元素的DOM全部清零
        -----------------------------------------------------------------------------*/
        private void SetConfidencesOfConsequentsToZero() {
            foreach (FuzzyRule fr in m_Rules) {
                fr.SetConfidenceOfConsequentToZero();
            }
        }
        //结束时销毁两个容器的代码未写
        

    }

}


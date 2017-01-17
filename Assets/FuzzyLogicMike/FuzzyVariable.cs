/*-----------------------------------------------------------------------------

 * 名称：FuzzyVariable 模糊语言变量类
 * 作者：zhdelong@foxmail.com
 * 日期：2016.5.4

-----------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace FuzzyLogicMike {
    public class FuzzyVariable {
        /*-----------------------------------------------------------------------------
         * 构造方法
        -----------------------------------------------------------------------------*/
        public FuzzyVariable() {
            m_dMinRange = 0.0;
            m_dMaxRange = 0.0;
        }

        /*-----------------------------------------------------------------------------
         * 不允许复制，原书中的private FuzzyVariable& operator=(const FuzzyVariable&);
        -----------------------------------------------------------------------------*/
        private FuzzyVariable(FuzzyVariable fv) {
            Debug.Assert(false, "Unsupported operation"); 
        }

        /*-----------------------------------------------------------------------------
         * m_MemberSets：将模糊语言变量中的每个“隶属函数”放入m_MemberSets中
        -----------------------------------------------------------------------------*/
        private Dictionary<string, FuzzySet> m_MemberSets = new Dictionary<string, FuzzySet>();

        /*-----------------------------------------------------------------------------
         * m_dMinRange和m_dMaxRange：该模糊变量的取值范围
         * AdjustRangeToFit()：用于实时更新这个取值范围
        -----------------------------------------------------------------------------*/
        private double m_dMinRange;
        private double m_dMaxRange;
        private void AdjustRangeToFit(double minBound, double maxBound) {
            if (minBound < m_dMinRange) {
                m_dMinRange = minBound;
            }
            if (maxBound > m_dMaxRange) {
                m_dMaxRange = maxBound;
            }
        }

        /*-----------------------------------------------------------------------------
         * AddLeftShoulderSet：为该模糊变量添加一个“左肩形”隶属函数
        -----------------------------------------------------------------------------*/
        public FzSet AddLeftShoulderSet(string name,
            double minBound,
            double peak,
            double maxBound) {
            m_MemberSets.Add(name, new FuzzySet_LeftShoulder(peak, peak - minBound, maxBound - peak));

            //更新整个隶属函数的定义域
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(m_MemberSets[name]);//就是这里，让模糊语言变量容器中的FuzzySet和FzSet联系了起来，以至于FuzzySet和FuzzyTerm联系了起来
        }
        /*-----------------------------------------------------------------------------
         * AddRightShoulderSet：为该模糊变量添加一个“右肩形”隶属函数
        -----------------------------------------------------------------------------*/
        public FzSet AddRightShoulderSet(string name,
            double minBound,
            double peak,
            double maxBound) {
            m_MemberSets.Add(name, new FuzzySet_RightShoulder(peak, peak - minBound, maxBound - peak));

            //更新整个隶属函数的定义域
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(m_MemberSets[name]);
        }
        /*-----------------------------------------------------------------------------
         * AddTriangularSet：为该模糊变量添加一个“三角形”隶属函数
        -----------------------------------------------------------------------------*/
        public FzSet AddTriangularSet(string name,
            double minBound,
            double peak,
            double maxBound) {
            m_MemberSets.Add(name, new FuzzySet_Triangle(peak,
                    peak - minBound,
                    maxBound - peak));
            //更新整个隶属函数的定义域
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(m_MemberSets[name]);//就是这里，让模糊语言变量容器中的FuzzySet和FzSet联系了起来，以至于FuzzySet和FuzzyTerm联系了起来
        }
        /*-----------------------------------------------------------------------------
         * AddSingletonSet：为该模糊变量添加一个“常数形”隶属函数
        -----------------------------------------------------------------------------*/
        public FzSet AddSingletonSet(string name,
            double minBound,
            double peak,
            double maxBound) {
                m_MemberSets.Add(name, new FuzzySet_Singleton(peak,
                    peak - minBound,
                    maxBound - peak));

            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(m_MemberSets[name]);//就是这里，让模糊语言变量容器中的FuzzySet和FzSet联系了起来，以至于FuzzySet和FuzzyTerm联系了起来
        }
        /*-----------------------------------------------------------------------------
         * Fuzzify：模糊化，得到一个特定值val，计算它在每个隶属函数中的隶属值
        -----------------------------------------------------------------------------*/
        public void Fuzzify(double val) {
            //确保所给的特定值是在隶属函数定义域内
            Debug.Assert((val >= m_dMinRange) && (val <= m_dMaxRange), "<FuzzyVariable.Fuzzify>: value out of range");
            //遍历m_MemberSets中的隶属函数，把给定的val放入计算DOM，即为模糊化
            foreach (KeyValuePair<string, FuzzySet> kv in m_MemberSets) {
                kv.Value.SetDOM(kv.Value.CalculateDOM(val));
            }
        }
        /*-----------------------------------------------------------------------------
         * DeFuzzifyCentroid：中心法去模糊化
        -----------------------------------------------------------------------------*/
        public double DeFuzzifyCentroid(int NumSamples) {
            //计算抽样点间隔
            double StepSize = (m_dMaxRange - m_dMinRange) / (double)NumSamples;

            double TotalArea = 0.0;
            double SumOfMoments = 0.0;

            for (int samp = 1; samp <= NumSamples; ++samp) {
                //计算抽样点在隶属函数中的隶属度，及与自身的中心法计算 
                foreach (KeyValuePair<string, FuzzySet> kv in m_MemberSets) {
                    //这一步就是将结果的DOM修剪成置信水平，即通过规则算出的DOM“与”上原图
                    double contribution =
                            (kv.Value.CalculateDOM(m_dMinRange + samp * StepSize) > kv.Value.GetDOM())? 
                                kv.Value.GetDOM() :
                                kv.Value.CalculateDOM(m_dMinRange + samp * StepSize);

                    TotalArea += contribution;

                    SumOfMoments += (m_dMinRange + samp * StepSize) * contribution;
                }
            }

            //防止除数为0
            if (TotalArea == 0) {
                return 0.0;
            }

            return (SumOfMoments / TotalArea);
        }
        /*-----------------------------------------------------------------------------
         * DeFuzzifyMaxAv：最大值中心去模糊化
        -----------------------------------------------------------------------------*/
        public double DeFuzzifyMaxAv() {
            double bottom = 0.0;
            double top = 0.0;

            foreach (KeyValuePair<string, FuzzySet> kv in m_MemberSets) {
                bottom += kv.Value.GetDOM();
                top += kv.Value.GetRepresentativeVal() * kv.Value.GetDOM();
            }
            //防止除数为0
            if (bottom == 0) {
                return 0.0;
            }
            return top / bottom;
        }
    }
}

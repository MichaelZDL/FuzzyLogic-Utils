/*-----------------------------------------------------------------------------

 * 名称：FuzzyLogicMike（main()） 测试程序
 * 作者：zhdelong@foxmail.com
 * 日期：2016.5.4

-----------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;
using FuzzyLogicMike;

public class TEST_SAMPLE : MonoBehaviour {


	void Start () {
        /*-----------------------------------------------------------------------------
        案例：要求在一堆武器中选取合适的武器，则首先为每种武器添加模糊逻辑模块
        -----------------------------------------------------------------------------*/
        FuzzyModule Rocket_Weapon = new FuzzyModule();
        /*-----------------------------------------------------------------------------
        Step1(a) 设计“目标的距离”的模糊语言变量：{近，中等，远} 及其隶属函数图
        -----------------------------------------------------------------------------*/
        FuzzyVariable Distance_FLV = Rocket_Weapon.CreateFLV("Distance");
        FzSet DstClose_FzSet = Distance_FLV.AddLeftShoulderSet("LeftShSetOfDist", 0, 25, 150);
        FzSet DstMedium_FzSet = Distance_FLV.AddTriangularSet("TriSetOfDist", 25, 150, 300);
        FzSet DstFar_FzSet = Distance_FLV.AddRightShoulderSet("RightShSetOfDist", 150, 300, 400);
        /*-----------------------------------------------------------------------------
        Step1(b) 设计“武器的弹药量”的模糊语言变量：{（弹药量）低，合适，超载}及其隶属函数图
        -----------------------------------------------------------------------------*/
        FuzzyVariable Bullet_FLV = Rocket_Weapon.CreateFLV("Bullet");
        FzSet BulletLow_FzSet = Bullet_FLV.AddLeftShoulderSet("LeftShSetOfBul", 0, 0, 10);
        FzSet BulletMedium_FzSet = Bullet_FLV.AddTriangularSet("TriSetOfBul", 0, 10, 30);
        FzSet BulletOver_FzSet = Bullet_FLV.AddRightShoulderSet("RightShSetOfBul", 10, 30, 40);
        /*-----------------------------------------------------------------------------
        Step1(c) 设计“期望值”的模糊语言变量：{不期望，期望，非常期望}及其隶属函数图
        -----------------------------------------------------------------------------*/
        FuzzyVariable DesirableValue_FLV = Rocket_Weapon.CreateFLV("DesirableValue");
        FzSet Undesirable_FzSet = DesirableValue_FLV.AddLeftShoulderSet("LeftShSetOfDsr", 0, 25, 50);
        FzSet Desirable_FzSet = DesirableValue_FLV.AddTriangularSet("TriSetOfDsr", 25, 50, 75);
        FzSet VeryDesirable_FzSet = DesirableValue_FLV.AddRightShoulderSet("RightShSetOfDsr", 50, 75, 100);
        /*-----------------------------------------------------------------------------
        Step2: 设计规则集，根据“目标的距离”和“武器的弹药量”的各三个模糊语言变量，
               可得出9种情况下的规则：
               如规则1：如果目标距离远与弹药量超载，则不期望，规则2：如果…(详见P325)
         * 行后注释为step3计算出隶属度之后，再进行规则计算的结果预测
        -----------------------------------------------------------------------------*/
        Rocket_Weapon.AddRule(new FzAND(DstFar_FzSet, BulletOver_FzSet), Desirable_FzSet);//该条规则在模糊联想矩阵对应的值为0.0000，下同
        Rocket_Weapon.AddRule(new FzAND(DstFar_FzSet, BulletMedium_FzSet), Undesirable_FzSet);//0.3333
        Rocket_Weapon.AddRule(new FzAND(DstFar_FzSet, BulletLow_FzSet), Undesirable_FzSet);//0.2000
        Rocket_Weapon.AddRule(new FzAND(DstMedium_FzSet, BulletOver_FzSet), VeryDesirable_FzSet);//0.0000
        Rocket_Weapon.AddRule(new FzAND(DstMedium_FzSet, BulletMedium_FzSet), VeryDesirable_FzSet);//0.6667
        Rocket_Weapon.AddRule(new FzAND(DstMedium_FzSet, BulletLow_FzSet), Desirable_FzSet);//0.2000
        Rocket_Weapon.AddRule(new FzAND(DstClose_FzSet, BulletOver_FzSet), Undesirable_FzSet);//0.0000
        Rocket_Weapon.AddRule(new FzAND(DstClose_FzSet, BulletMedium_FzSet), Undesirable_FzSet);//0.0000
        Rocket_Weapon.AddRule(new FzAND(DstClose_FzSet, BulletLow_FzSet), Undesirable_FzSet);//0.0000
        /*-----------------------------------------------------------------------------
        Step3: 假设当前条件为目标距离：200像素远，弹药量：8枚。
               接下来我们对火箭发射器进行基于模糊逻辑的“选取期望值”计算。
        
         * 行后注释为结果预测
        -----------------------------------------------------------------------------*/
        //计算每个模糊语言变量的各段隶属函数（FuzzySet）在特定值下的DOM，并存入对于的FuzzySet类中
        Rocket_Weapon.Fuzzify("Distance", 200);
        Debug.Log("Close :" + DstClose_FzSet.GetDOM().ToString("f4"));//0.0000
        Debug.Log("Medium :" + DstMedium_FzSet.GetDOM().ToString("f4"));//0.6667
        Debug.Log("Far :" + DstFar_FzSet.GetDOM().ToString("f4"));//0.3333
        Rocket_Weapon.Fuzzify("Bullet", 8);
        Debug.Log("BulletLow :" + BulletLow_FzSet.GetDOM().ToString("f4")); //0.2000
        Debug.Log("Medium :" + BulletMedium_FzSet.GetDOM().ToString("f4"));//0.8000
        Debug.Log("Over :" + BulletOver_FzSet.GetDOM().ToString("f4"));//0.0000
        /*-----------------------------------------------------------------------------
        Step4: 根据Step3.3得到的置信度模糊形，有三种方法进行“去模糊化”：
               最大值均值法(MOM)，中心法，最大值平均（MaxAv）。
               选取一种方法，最终得到火箭发射器在当前条件通过模糊逻辑计算后的期望值。
        
        结果预测: 结果应该是Undesirable_FzSet = 0.3333, Desirable_FzSet = 0.2, VeryDesirable_FzSet = 0.6667
                  中心法去模糊化，抽样点为{10.20.30....100},则期望值计算公式：
                （10*0.333333+20*0.333333+30*0.533333+40*0.533333+50*0.2+60*0.6+70*0.866667+80*0.666667+90*0.666667+100*0.666667）/
                 (0.333333+0.333333+0.533333+0.533333+0.2+0.6+0.866667+0.666667+0.666667+0.666667)=334.00008/5.4=61.8518
        -----------------------------------------------------------------------------*/
        double DValue = Rocket_Weapon.DeFuzzify("DesirableValue", FuzzyModule.DefuzzifyMethod.centroid);
        Debug.Log("DesirableValue :" + DValue.ToString("f8"));
	}
}
/*-----------------------------------------------------------------------------

                    * 《游戏人工智能编程案例精粹》案例说明：

        要求在一堆武器中选取合适的武器。
        其中合适与否取决于(1)目标的距离（待射击的目标）；(2)（待选取的）武器的弹药量。
        现用模糊逻辑的方法来帮助我们进行选择。

Step1: (a) 设计“目标的距离”的模糊语言变量：{近，中等，远} 及其隶属函数图；
       (b) 设计“武器的弹药量”的模糊语言变量：{（弹药量）低，合适，超载}及其隶属函数图；
       (c) 设计“期望值”的模糊语言变量：{不期望，期望，非常期望}及其隶属函数图；

Step2: 设计规则集，根据“目标的距离”和“武器的弹药量”的各三个模糊语言变量，
       可得出9种情况下的规则：如规则1：如果目标距离远与弹药量超载，则不期望，规则2：如果…(P325)

Step3: 假设当前条件为目标距离：200像素远，弹药量：8枚。
       接下来我们对火箭发射器进行基于模糊逻辑的“选取期望值”计算。

Step3.1先将当前条件放入规则1中，
       计算当前条件在“目标距离远”和“弹药量超载”这两个模糊语言变量下的隶属值，
       并进行“模糊逻辑与运算”，得到了规则1的结果——不期望的隶属度。

Step3.2然后像Step3.1这样依次将当前条件放入规则2,规则3, … ,规则9中，
       分别得到当前条件在各个规则下，对应规则结果的隶属度，汇总成“模糊联想矩阵（FAM）”。

Step3.3将这些隶属度看成置信度，其中当出现多重置信度时，使用一种取最大值的方法。
       这样，当前条件（目标距离：200像素远，弹药量：8枚）通过9个规则计算后，
       它对应的“期望值”的模糊语言变量：{不期望，期望，非常期望}就可以得到相应的置信度，
       从而合成一个单一的关于置信度的“模糊形”。

Step4: 根据Step3.3得到的置信度模糊形，有三种方法进行“去模糊化”：
       最大值均值法(MOM)，中心法，最大值平均（MaxAv）。
       选取一种方法，最终得到火箭发射器在当前条件通过模糊逻辑计算后的期望值。

Step5: 再用相同的方式，计算其他武器在对应条件下的期望值，
       最后选取期望值最大的武器，完成一次完整的模糊逻辑判断。

-----------------------------------------------------------------------------*/
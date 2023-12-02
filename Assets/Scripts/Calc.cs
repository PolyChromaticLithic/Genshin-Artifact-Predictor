using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using XCharts;
using XCharts.Runtime;

public class Calc : MonoBehaviour
{
    double[] critRate = new double[4] { 5.4, 6.2, 7.0, 7.8 };
    double[] critDMG = new double[4] { 5.4, 6.2, 7.0, 7.8 };
    double[] ATK = new double[4] { 4.1, 4.7, 5.3, 5.8 };
    [SerializeField] private TMP_Dropdown critRate_dropdown;
    [SerializeField] private TMP_Dropdown critDMG_dropdown;
    [SerializeField] private TMP_Dropdown ATK_dropdown;
    [SerializeField] private Toggle critRate_toggle;
    [SerializeField] private Toggle critDMG_toggle;
    [SerializeField] private Toggle ATK_toggle;
    [SerializeField] private Toggle fourOptions_toggle;
    [SerializeField] private BarChart barChart;

    [SerializeField] private TextMeshProUGUI mid_text;
    [SerializeField] private TextMeshProUGUI avg_text;
    [SerializeField] private TextMeshProUGUI max_text;

    int[] scores = new int[61];
    int[] options = new int[4];
    bool fourOptions = true;

    int count = 0;

    double mid = 0;

    // Start is called before the first frame update
    void Start()
    {
        barChart.ClearData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Run()
    {
        barChart.ClearData();
        options = new int[4] { critRate_toggle.isOn ? 1 : 0, critDMG_toggle.isOn ? 1 : 0, ATK_toggle.isOn ? 1 : 0, 0 };
        scores = new int[61];
        fourOptions = fourOptions_toggle.isOn;
        count = 0;
        mid = 0;
        double currentScore = 0;
        currentScore += critRate[critRate_dropdown.value] * options[0];
        currentScore += critDMG[critDMG_dropdown.value] * options[1];
        currentScore += ATK[ATK_dropdown.value] * options[2];
        if (fourOptions)
        {
            CalcScore(5, currentScore);
        }
        else
        {
            CalcScore(4, currentScore);
        }
        mid_text.text = "Mid:" + mid.ToString("F2");
        avg_text.text = "Avg:" + CalcAvg().ToString("F2");
        max_text.text = "Max:" + CalcMax(currentScore).ToString("F2");
        barChart.enabled = false;
        for (int i = 0; i < 61; i++)
        {
            barChart.AddData(0, i, scores[i]);
        }
        barChart.enabled = true;
    }

    void CalcScore(int hierarchy, double currentScore)
    {
        if (hierarchy == 0)
        { 
            scores[Convert.ToInt32(Math.Floor(currentScore))]++;
            count++;
            if (fourOptions)
            {
                if (count == 524288)
                {
                    mid += currentScore;
                }
                else if (count == 524289)
                {
                    mid += currentScore;
                    mid /= 2;
                }
            }
            else
            {
                if (count == 32768)
                {
                    mid += currentScore;
                }
                else if (count == 32769)
                {
                    mid += currentScore;
                    mid /= 2;
                }
            }
            
        
        }
        else
        {
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < 4; n++)
                {
                    switch (m)
                    {
                        case 0:
                            //クリティカル率
                            CalcScore(hierarchy - 1, currentScore + critRate[n] * options[0]);
                            break;
                        case 1:
                            //クリティカルダメージ
                            CalcScore(hierarchy - 1, currentScore + critDMG[n] * options[1]);
                            break;
                        case 2:
                            //攻撃力
                            CalcScore(hierarchy - 1, currentScore + ATK[n] * options[2]);
                            break;
                        case 3:
                            CalcScore(hierarchy - 1, currentScore);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    double CalcAvg()
    {
        double avg = 0;
        for (int i = 0; i < 61; i++)
        {
            avg += scores[i] * i;
        }
        return avg / count;
    }

    double CalcMid()
    {
        int mid = 0;
        int sum = 0;
        for (int i = 0; i < 61; i++)
        {
            sum += scores[i];
            if (sum >= count / 2)
            {
                mid = i;
                break;
            }
        }
        return mid;
    }

    double CalcMax(double currentScore)
    {
        if (critRate_toggle.isOn || critDMG_toggle.isOn)
        {
            if (fourOptions_toggle.isOn)
            {
                return currentScore + critRate[3] * 5;
            }
            else
            {
                return currentScore + critRate[3] * 4;
            }
        }
        else if (ATK_toggle.isOn) 
        {
            if (fourOptions_toggle.isOn)
            {
                return currentScore + ATK[3] * 5;
            }
            else
            {
                return currentScore + ATK[3] * 4;
            }
        }
        else
        {
            return currentScore;
        }
    }
}

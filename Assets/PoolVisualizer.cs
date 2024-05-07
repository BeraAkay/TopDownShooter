using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolVisualizer : MonoBehaviour
{
    public Bar[] bars;



    void Start()
    {
        
        foreach (Bar bar in bars)
        {
            bar.UpdateBar();
        }
        
        StartCoroutine(UpdateBars());
    }

    void Update()
    {
    }

    IEnumerator UpdateBars()
    {
        for(; ; )
        {
            foreach(Bar bar in bars)
            {
                bar.UpdateBar();
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    [System.Serializable]
    public class Bar
    {
        public ObjectPooler.Poolable poolID;
        public Slider slider;

        public void UpdateBar()
        {
            slider.value = GameManager.Pooler.poolDict[poolID].Fullness();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatSheetDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject statDisplayerPrefab;

    [SerializeField]
    RectTransform titleTransform;

    RectTransform sheetSpaceTransform;

    float usableHeight, perStatHeight, titleOffset;

    //StatDisplay[] stats;

    List<StatDisplay> stats;

    StatDisplay currentHealth;

    /*
     * universalStats 0: HP 1: MTG 2: DMG Mult 3: RCV Mult 4:MS
     * uniqueStats 0:regen, 1: crit, 2: pickupRange, 3: xpMultiplier, 4: extra lives, 5: curse 
     */

    public void CreateSheet()
    {
        
        if(stats != null)
        {
            foreach(StatDisplay disp in stats)
            {
                Destroy(disp.gameObject);
            }
        }
        
        stats = new List<StatDisplay>();
        statDisplayerPrefab.SetActive(true);

        sheetSpaceTransform = titleTransform.parent.GetComponent<RectTransform>();

        usableHeight = sheetSpaceTransform.sizeDelta.y - titleTransform.sizeDelta.y;
        titleOffset = titleTransform.sizeDelta.y / 2.0f;
        perStatHeight = usableHeight / PlayerInteractions.instance.stats.Count;


        foreach(Stats.Types key in PlayerInteractions.instance.stats.Keys)
        {
            stats.Add(Instantiate(statDisplayerPrefab, sheetSpaceTransform).GetComponent<StatDisplay>());

            stats[stats.Count - 1].SetDisplay(key);
            //do the positioning of each statdisplay
            //use the perstat height and the titleoffset to calculate each position
            RectTransform rectTransform = stats[stats.Count - 1].GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, perStatHeight);
            Vector3 pos = titleTransform.localPosition;
            pos.y -= titleOffset + (perStatHeight * (stats.Count-1)) + (perStatHeight / 2.0f);
            rectTransform.localPosition = pos;


            if(key == Stats.Types.CurrentHealth)
            {
                currentHealth = stats[stats.Count - 1];
            }
        }
        
        //StartCoroutine(UpdateHP());

        statDisplayerPrefab.SetActive(false);
    }

    public void UpdateHP()
    {
        currentHealth.UpdateDisplay();
    }

    private void OnEnable()
    {
        UpdateHP();
    }

    /*
    public IEnumerator UpdateHP()
    {
        for(; ; )
        {
            currentHealth.UpdateDisplay();

            yield return new WaitForSeconds(.1f);
        }
    }
    */

    public void UpdateStats()
    {
        foreach (StatDisplay stat in stats)
        {
            stat.UpdateDisplay();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
public class RangeIndicator : MonoBehaviour
{
    public float range = 3;
    public float indicatorHeight;
    public BaseTower twr;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, twr.transform.position.y + indicatorHeight, transform.position.z);
        transform.localScale = new Vector3(range * 2, indicatorHeight, range * 2);
        if (twr.enabled)
        {
            if (twr != null) range = twr.range * twr.rangeMultiplier;
        }
        else range = twr.transform.GetComponent<TowerInfo>().range;
    }
}

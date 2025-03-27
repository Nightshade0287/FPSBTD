using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visuals : MonoBehaviour
{
    public List<BloonEffects> effects;
    public List<BloonTypes> types;
    public Material bloonMaterial;
    public Texture2D camo;
    public void Update()
    {
        List<BloonEffects> effects = gameObject.GetComponent<Health>().bloonEffects;
        List<BloonTypes> types = gameObject.GetComponent<Health>().bloonTypes;
        if(types.Contains(BloonTypes.Camo))
        {
            bloonMaterial.SetTexture("_MainTex", camo);
        }
        else
        {
            bloonMaterial.SetTexture("_MainTex", null);
        }
    }
}

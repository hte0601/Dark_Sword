using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour
{
    public float fps = 30.0f;
    public Texture2D[] frames;

    private MeshRenderer rendererMy;

    void Start()
    {
        rendererMy = GetComponent<MeshRenderer>();
        StartCoroutine("NextFrame");
    }

    IEnumerator NextFrame()
    {
        for(int i = 0; i < 4; i++)
        {
            Debug.Log("count");
            rendererMy.sharedMaterial.SetTexture("_MainTex", frames[i]);
            yield return new WaitForSeconds(1 / fps);
        }
    }
}
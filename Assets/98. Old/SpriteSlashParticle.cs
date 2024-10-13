using UnityEngine;
using System.Collections;

namespace SpeedMode
{
    public class SpriteSlashParticle : MonoBehaviour
    {
        public float fps = 30.0f;
        public Texture[] frames;

        private MeshRenderer rendererMy;

        void Awake()
        {
            rendererMy = GetComponent<MeshRenderer>();
            rendererMy.sortingOrder = 2;
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            StartCoroutine("NextFrame");
        }

        IEnumerator NextFrame()
        {
            for (int i = 0; i < frames.Length; i++)
            {
                rendererMy.sharedMaterial.SetTexture("_MainTex", frames[i]);
                yield return new WaitForSeconds(1 / fps);
            }
            //여운?주기
            yield return new WaitForSeconds(3 / fps);
            gameObject.SetActive(false);
        }
    }
}

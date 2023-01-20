using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject flahgHolder;
    public Sprite[] flashSprites;
    public SpriteRenderer[] spriteRenderers;

    public float flashTime;

    // Start is called before the first frame update
    void Start()
    {
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        flahgHolder.SetActive(true);

        int flashSpriteIndex = Random.Range(0, flashSprites.Length);
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = flashSprites[flashSpriteIndex];
        }

        Invoke("Deactivate", flashTime);
    }

    void Deactivate()
    {
        flahgHolder.SetActive(false);
    }
}

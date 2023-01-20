using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shell : MonoBehaviour
{
    public Rigidbody myRigidbody;
    public float forceMin;
    public float forceMax;

    float lifeTime = 4f;
    float fadeTime = 2f;


    // Start is called before the first frame update
    void Start()
    {
        float force = Random.Range(forceMin, forceMax);
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.AddForce(Vector3.right * force);
        myRigidbody.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifeTime);

        float percent = 0f;
        float fadeSpeed = 1 / fadeTime;
        Material mat = GetComponent<Renderer>().material;
        Color initColor = mat.color;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initColor, Color.clear, percent);
            yield return null;
        }

        Destroy(gameObject);
    }
}

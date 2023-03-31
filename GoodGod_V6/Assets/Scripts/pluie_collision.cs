using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pluie_collision : MonoBehaviour
{
    public ParticleSystem part, feu;
    public List<ParticleCollisionEvent> collisionEvents;
    public float EmFeu;
    public Transform Coco;
    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        EmFeu = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
      


       
        if (other.tag == "feu")
        {
            feu = other.GetComponent<ParticleSystem>();
            StartCoroutine(eteindreFeu());
            Debug.Log("eteint feu");
        }else StopCoroutine(eteindreFeu());

        if (other.tag == "Tree")
        {
          Coco =  other.transform.Find("Coco");
            Coco.gameObject.SetActive(true);
        }

    }

    private IEnumerator eteindreFeu()
    {
        var em = feu.emission;
        while (EmFeu > 0)
        {
            EmFeu--;
          
            em.rateOverTime = EmFeu;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

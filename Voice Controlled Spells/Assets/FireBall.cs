using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;
using UnityEngine;
using System;

public class FireBall : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> spells = new Dictionary<string, System.Action>();

    public GameObject ThisOb;
    public Rigidbody Player;
    public TrailRenderer Trail;
    public SphereCollider Col;

    void Start()
    {
        Col = ThisOb.GetComponent<SphereCollider>();

        //Disables trail so it is not visible while moving
        Trail.enabled = false;

        //If the objects name is not SpellCast then add a Rigidbody component, disable gravity, set forward velocity to 10m/s and enable the trail renderer
        if (this.name != "SpellCast")
        {
            Rigidbody Rg = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            Rg.useGravity = false;
            Rg.velocity = new Vector3(10, 0, 0);
            Trail.enabled = true;
        }

        if (this.name == "SpellCast")
        {
            spells.Add("Fire ball", Fireball);

            keywordRecognizer = new KeywordRecognizer(spells.Keys.ToArray());
            keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
            keywordRecognizer.Start();

            Debug.Log("Keywords added");
        }
    }

    private void Fireball()
    {
        GameObject ob = GameObject.Instantiate(ThisOb, Player.transform);
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        spells[speech.text].Invoke();
    }

    void Update()
    {
        //If they space key is pressed and the objects name is SpellCast then copy this object with the same position the player has
        if (Input.GetKeyDown(KeyCode.Space) && this.name == "SpellCast")
        {
            Fireball();
        }        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Player" && collision.gameObject.name != "SpellCast")
        {
            Destroy(ThisOb);
        }
    }
}

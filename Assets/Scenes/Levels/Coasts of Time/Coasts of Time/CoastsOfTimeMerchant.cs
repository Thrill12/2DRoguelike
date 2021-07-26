using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoastsOfTimeMerchant : BaseInteractable
{
    public DropTable table;
    public float soulmarkPrice;
    public float speed;
    public GameObject purchaseDeniedObj;

    public List<string> messages;

    public Transform messageSpawnTransform;
    public GameObject messageObject;
    private AudioSource source;
    private TMP_Text messageText;
    private GameObject obj;

    public override void Start()
    {
        base.Start();
        source = GetComponent<AudioSource>();
        StartCoroutine(ShowMessage());
    }

    public override void Interact()
    {
        base.Interact();

        if(GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>().soulMarks >= soulmarkPrice)
        {
            GameObject.FindGameObjectWithTag("GeneralManager").GetComponent<GeneralManager>().soulMarks -= soulmarkPrice;
            GameObject item = Instantiate(table.GetRandomItem(), transform.position, Quaternion.identity);
            item.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
        }
        else
        {
            Instantiate(purchaseDeniedObj, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
        }
    }

    public IEnumerator ShowMessage()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2, 15));

            

            yield return new WaitForSeconds(5.1f);

                   
        }       
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if(collision.transform.tag == "Player")
        {
            obj = Instantiate(messageObject, new Vector2(messageSpawnTransform.position.x, messageSpawnTransform.position.y), Quaternion.identity);
            messageText = obj.gameObject.GetComponentInChildren<TMP_Text>();
            messageText.text = messages[Random.Range(0, messages.Count)];

            source.Play();
        }       
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if(collision.transform.tag == "Player")
        {
            obj.GetComponentInChildren<Animator>().SetTrigger("Exit");

            Destroy(obj, 0.5f);
        }
        
    }
}

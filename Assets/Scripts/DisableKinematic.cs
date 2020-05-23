using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableKinematic : MonoBehaviour
{
    [SerializeField]
    private float _fallTimer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collider Found");
            Debug.Log("mass" + GetComponent<Rigidbody2D>().mass);
            StartCoroutine(FallAfterTime(_fallTimer));
            
        }
    }
    private IEnumerator FallAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;        
    }
}

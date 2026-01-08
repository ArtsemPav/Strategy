using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAttack : MonoBehaviour
{
    [NonSerialized] public int carHealth = 100;
    public float radius = 50f;
    public GameObject bullet;
    private Coroutine _corotine = null;

    private void Update()
    {
        DetectCollision();
    }

    private void DetectCollision()
    {
       Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        if (hitColliders.Length == 0 && _corotine != null)
        {
            StopCoroutine(_corotine);
            _corotine = null;
            if (gameObject.CompareTag("Enemy"))
                GetComponent<NavMeshAgent>().SetDestination(gameObject.transform.position);
        }

        foreach(var element in hitColliders)
        {
            if ((gameObject.CompareTag("Player") && element.gameObject.CompareTag("Enemy")) || (gameObject.CompareTag("Enemy") && element.gameObject.CompareTag("Player")))
            {
                if (gameObject.CompareTag("Enemy"))
                    GetComponent<NavMeshAgent>().SetDestination(element.transform.position);
                if(_corotine == null)
                _corotine = StartCoroutine(StartAttack(element));
            }
        }
    }

    IEnumerator StartAttack(Collider enemy)
    {
        GameObject obj = Instantiate(bullet, transform.GetChild(1).position, Quaternion.identity);
        obj.GetComponent<BulletController>().position = enemy.transform.position;
        yield return new WaitForSeconds(1f);
        StopCoroutine(_corotine);
        _corotine = null;
    }
}

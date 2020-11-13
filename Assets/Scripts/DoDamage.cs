using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    public class DoDamage : MonoBehaviour
    {
        public GameObject weapon;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Hit Enemy");
                EnemyAI enemyAIScript = other.gameObject.GetComponent<EnemyAI>();
                enemyAIScript.TakeDamage(30);
            }
        }
    }
}

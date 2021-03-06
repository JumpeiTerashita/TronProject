﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTB
{
    public class EnemyMover : MonoBehaviour
    {
        [System.NonSerialized]
        public GameObject InstEffect;

        [SerializeField]
        float Speed = 0.01f;

        public int Id;

        bool IsDead = false;

        public GameObject BoidsController;

        [SerializeField]
        int destroyScore = 1;

        private void Awake()
        {
            InstEffect = PrefabHolder.Instance.Inst;
           
        }

        // Use this for initialization
        void Start()
        {
            StartCoroutine(InstPhase());
            //GetComponent<DestinationHolder>().SetDestination();
        }

        // Update is called once per frame
        void Update()
        {
            //FlyToDestination();
        }

        void FlyToDestination(float _speedMagnitude = 1.0f, float _turnMagnitude = 1.0f)
        {
            // 設定した目的地にだんだん向く
            //Vector3 TargetPos = GetComponent<DestinationHolder>().GetDestination();
            //Quaternion targetRotation = Quaternion.LookRotation(TargetPos - transform.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _turnMagnitude);

            // 向いている方向に飛ぶ
            transform.Translate(new Vector3(0, 0, Speed * _speedMagnitude));
            return;
        }

        /// <summary>
        /// 破壊されたとき
        /// </summary>
        void Destroy()
        {
            if (!IsDead)
            {
                IsDead = true;
                this.GetComponent<Rigidbody>().detectCollisions = false;
                InGameManager.Instance.Score += destroyScore;
                StartCoroutine(ExplosionPhase());
            }
        }

        IEnumerator InstPhase()
        {
            yield return null;
            var EnemyInst = Instantiate(InstEffect);
            EnemyInst.transform.position = transform.position;
            yield break;
        }

        IEnumerator ExplosionPhase()
        {
            int RandomNum = Random.Range(0,5);
            var Explosion = PrefabHolder.Instance.Explode[RandomNum];
            yield return null;
            var Explode = Instantiate(Explosion);
           
            Explode.transform.position = transform.position;
            Explode.transform.SetParent(this.gameObject.transform);
            Explode.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            float ExplodeTime = Explosion.GetComponent<AutoDestroy>().GetDestroyLimit();
            yield return null;

            yield return new WaitForSeconds(ExplodeTime);
            Debug.Log("Destroy -- Enemy " + Id);
            BoidsController.SendMessage("Delete", Id);
            Destroy(gameObject);
            yield break;
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTB
{
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField]
        public GameObject InstEffect;

        [SerializeField]
        float Speed = 0.01f;

        public int Id;

        bool IsDead = false;

        public GameObject BoidsController;

        // Use this for initialization
        void Start()
        {
            var EnemyInst = Instantiate(InstEffect);
            EnemyInst.transform.position = transform.position;
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
                Debug.Log("Destroy -- Enemy " + Id);
                BoidsController.SendMessage("Delete",Id);
                Destroy(gameObject);
            }
            
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace KTB
{
    [RequireComponent(typeof(DestinationHolder))]
    public class MissileBehavior : MonoBehaviour
    {
        [SerializeField]
        float ArriveLength = 0.25f;

        [SerializeField]
        float Speed = 0.01f;

        bool IsDead = false;

        public bool IsTutorial; 

        private Subject<Collision> onCollision = new Subject<Collision>();

        // Use this for initialization
        void Start()
        {
            GameObject cameraObj = GameObject.Find("MixedRealityCameraParent");
            IsDead = false;
            

            GetComponent<DestinationHolder>().SetDestination(new Vector3( cameraObj.transform.position.x,transform.position.y,cameraObj.transform.position.z));

            
            OnCollision().Subscribe(__ =>
            {
                Destroy(gameObject);
                Debug.Log("Missile Break!!");
            });
            //Debug.Log(GetComponent<DestinationHolder>().GetDestination());
        }

        // Update is called once per frame
        void Update()
        {
            if (IsTutorial) return;

            // 向いて移動
            FlyToDestination();

            // 目的地に到着済みか確認
            //ArriveCheck();

        }

        void FlyToDestination(float _speedMagnitude = 1.0f, float _turnMagnitude = 1.0f)
        {
            // 設定した目的地にだんだん向く
            Vector3 TargetPos = GetComponent<DestinationHolder>().GetDestination();
            Quaternion targetRotation = Quaternion.LookRotation(TargetPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _turnMagnitude);

            // 向いている方向に飛ぶ
            transform.Translate(new Vector3(0, 0, Speed * _speedMagnitude));

            
            return;
        }

        // FIX : なんかうまくとれない　Colliderでやるべき
        //void ArriveCheck()
        //{
        //    var Destination = GetComponent<DestinationHolder>().GetDestination();
        //    Vector3 Distance = Destination - transform.position;
        //    if (MyMath.IsShortLength(Destination, ArriveLength))
        //    {
        //        hasArrived = true;
        //    }
        //}

        private void OnCollisionEnter(Collision collision)
        {
            onCollision.OnNext(collision);
        }

        public IObservable<Collision> OnCollision()
        {
            return onCollision;
        }
    }
}
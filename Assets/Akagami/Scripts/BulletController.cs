﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gami
{
    public class BulletController : MonoBehaviour
    {
        Vector3 speed;
        float mag = .1f;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            this.transform.position += speed * mag;
        }
        public GameObject CreateBullet(Vector3 _pos)
        {
            GameObject instance = Instantiate(this.gameObject);
            instance.transform.position = _pos;
            return instance;
        }
        public void SetSpeed(Vector3 _speed)
        {
            speed = _speed;
        }
    }
}
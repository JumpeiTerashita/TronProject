﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if WINDOWS_UWP
using Windows.Gaming.Input;
#endif
namespace gami
{
    public class PillarFactory : MonoBehaviour
    {
#if WINDOWS_UWP
        public GamepadReading reading;
#endif

        /// <summary>
        /// 柱の寿命 デフォルト1.0f
        /// </summary>
        [SerializeField]
        float everyPillarLifeLimit = 1.0f;

        /// <summary>
        /// 柱の大きさ デフォルト5
        /// </summary>
        [SerializeField]
        float pillarHeightParamt = 5;

        [SerializeField]
        GameObject pillar;

        [SerializeField]
        Camera camera;

        GameObject OpeningSceneManager;

        /// <summary>
        /// 放出方向
        /// </summary>
        private bool isSideUnder;

        private GameObject player;

        bool IsStarted;

        private void Awake()
        {
            player = GameObject.Find("Player");
            OpeningSceneManager = GameObject.Find("OpeningSceneManager");
            IsStarted = false;
        }

        // Use this for initialization
        void Start()
        { 
            isSideUnder = false;
            StartCoroutine(StandByPhase());
        }


        // Update is called once per frame
        void Update()
        {
            if (!IsStarted) return;
            // Yボタン入力でフラグを管理
            bool sideChange = false;
#if WINDOWS_UWP
            reading = 
            ControlEventGetter.Instance.reading;
            if(reading.Buttons.HasFlag(GamepadButtons.Y))
            {
                sideChange = true;
            }
#else
            if (Input.GetButtonDown("Attack_SideChange"))sideChange = true;
#endif
            if (sideChange)
            {
                isSideUnder = !isSideUnder;
            }
            // プレイヤー位置を取得、その場に実態生成
            Vector3 playerPos = player.transform.position;
            GameObject instPillar = Instantiate(pillar);
            //　スケールY値に変数の値を加算
            instPillar.transform.localScale = 
                new Vector3(instPillar.transform.localScale.x,
                pillarHeightParamt ,
                instPillar.transform.localScale.z);
            // ポジション変更のための変数用意
            float pillarPos = (pillarHeightParamt / 2);
            if (isSideUnder)
            {
                // 下方向ならポジションをマイナスするため*=-1
                pillarPos *= -1;
            }
            // 変数を加算した値にポジション変更
            instPillar.transform.position = new Vector3(playerPos.x, playerPos.y + pillarPos, playerPos.z);
            // どの方向に出されたかを子オブジェクトに持たせる
            instPillar.transform.GetComponent<Pillar>().SetSideUnderFlag(isSideUnder);
            // 親オブジェクトにthisオブジェクトを登録
            instPillar.transform.SetParent(this.gameObject.transform);
            // 死に時間設定
            instPillar.GetComponent<KTB.AutoDestroy>().SetDestroyLimit(everyPillarLifeLimit);
            // カメラの方向に向きを合わせるを向かせる
            if (camera == null) return;
            instPillar.transform.eulerAngles += new Vector3(0, camera.transform.eulerAngles.y, 0);
            // カメラの参照をPillarに！ by KTB
            //instPillar.GetComponent<Pillar>().Camera = OpeningSceneManager.GetComponent<gami.OpeningCameraMover>().mainCamera;
        }

        IEnumerator StandByPhase()
        {
            //pillar = KTB.PrefabHolder.Instance.Pillar;
            yield return null;
            IsStarted = true;
        }
    }
}
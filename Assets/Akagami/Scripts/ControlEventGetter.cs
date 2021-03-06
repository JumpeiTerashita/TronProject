﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTB;

#if WINDOWS_UWP
using Windows.Gaming.Input;
#endif
namespace gami
{
    public class ControlEventGetter : SingleTon<ControlEventGetter>
    {
        private int notInputCount;
#if WINDOWS_UWP
        public Gamepad controller;
        public GamepadReading reading;
        //public static GamepadReading oldButton;
        // Use this for initialization
        void Start()
        {
            // Gamepadを探す
            if(Gamepad.Gamepads.Count > 0) {
                //Debug.Log("Gamepad found.");
                //controller = Gamepad.Gamepads.First();
            } else
            {
                Debug.Log("Gamepad not found.");
            }
            // ゲームパッド追加時イベント処理を追加
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
        }

        // Update is called once per frame
        void Update()
        {
            if(controller != null)
            {
                //oldButton = reading;
                reading = controller.GetCurrentReading();
            }
        }
        // ゲームパッド追加時のイベント処理
        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            controller = e;
        }
#else

#endif
    }
}
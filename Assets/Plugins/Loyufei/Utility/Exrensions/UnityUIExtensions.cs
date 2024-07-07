using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Loyufei
{
    public static class UnityUIExtensions
    {
        public static void AddListener(this Button self, UnityAction action) 
        {
            self.onClick.AddListener(action);
        }
        
        public static void SetSprite(this Image self, Sprite sprite, Color color) 
        {
            self.sprite = sprite;
            self.color  = color;
        }
    }

}

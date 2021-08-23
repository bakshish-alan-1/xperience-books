using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BookSystem {
    public class Utility : MonoBehaviour
    {
        //Converter


       

        public static void Texture2DToSprite(Texture2D texture,Action<Sprite> img = null)
        {
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            img?.Invoke(sprite);
        }




      
    
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelGame;

// Physical Pixels
public class Pixel : MonoBehaviour, IPixelObject
{
    public Image Image;
    public PixelCollider Collider;
    public PixelSprite Sprite;
}
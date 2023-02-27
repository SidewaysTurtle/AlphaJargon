using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelGame
{
    public abstract class PixelComponent : MonoBehaviour, ISpawnable
    {
        public abstract void Create(PixelGameObject parent);
    }
}
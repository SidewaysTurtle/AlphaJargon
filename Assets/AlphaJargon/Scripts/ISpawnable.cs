using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelGame
{
    public interface ISpawnable
    {
        public void Create(PixelGameObject parent);
    }
}
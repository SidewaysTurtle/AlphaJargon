using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BuildingBlocks.DataTypes;

using PixelGame;

// this file makes me want to die inside
// please.. please dont touch it...

// Eventually convert to using Sutherland-Hodgman algo to check
// for polygon on polygon clipping (not Griner-Hormann cuz slow and not really needed for squares)

public class PixelScreenManager
{
    public static PixelScreenManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        PixelScreen.onPixelScreenCreateEvent += AddToPixelScreen;
        PixelScreen.onPixelScreenDeleteEvent += RemoveFromPixelScreen;
    }

    void OnDisable()
    {
        PixelScreen.onPixelScreenCreateEvent -= AddToPixelScreen;
        PixelScreen.onPixelScreenDeleteEvent -= RemoveFromPixelScreen;
    }

    public List<KeyValuePair<PixelGameObject, Dictionary<int, Pixel>>> Layers = new List<KeyValuePair<PixelGameObject, Dictionary<int, Pixel>>>();

    void AddToPixelScreen(PixelGameObject parent, PixelScreen pixelScreen)
    {
        Layers.Add(new KeyValuePair<PixelGameObject, Dictionary<int, Pixel>>(parent, pixelScreen.grid.ToDictionary()));
    }
    void RemoveFromPixelScreen(PixelGameObject parent, PixelScreen pixelScreen)
    {
        Layers.Remove(new KeyValuePair<PixelGameObject, Dictionary<int, Pixel>>(parent, pixelScreen.grid.ToDictionary()));
    }

    //
    public List<KeyValuePair<PixelPosition, Pixel>> GetPixelsWithColliderOtherThan(PixelGameObject pgo)
    {
        List<KeyValuePair<PixelPosition, Pixel>> pixels = new List<KeyValuePair<PixelPosition, Pixel>>();

        foreach (KeyValuePair<PixelGameObject, Dictionary<int, Pixel>> layer in Layers)
        {
            if (!layer.Key.Equals(pgo))
            {
                foreach (KeyValuePair<int, Pixel> pixel in layer.Value)
                {
                    if (pixel.Value.Collider != null)
                    {
                        pixels.Add(new KeyValuePair<PixelPosition, Pixel>(pixel.Key + layer.Key.position, pixel.Value));
                    }
                }
            }
        }
        return pixels;
    }

    public List<KeyValuePair<PixelPosition, Pixel>> GetPixelsWithCollider(PixelGameObject pgo, PixelPosition translation)
    {
        List<KeyValuePair<PixelPosition, Pixel>> pixels = new List<KeyValuePair<PixelPosition, Pixel>>();
        
        foreach (KeyValuePair<PixelGameObject, Dictionary<int, Pixel>> layer in Layers)
        {
            if (layer.Key.Equals(pgo))
            {
                foreach (KeyValuePair<int, Pixel> pixel in layer.Value)
                {
                    if (pixel.Value.Collider != null)
                    {
                        pixels.Add(new KeyValuePair<PixelPosition, Pixel>(pixel.Key + pgo.position + translation, pixel.Value));
                    }
                }
            }
        }
        
        return pixels;
    }
    public List<Pixel> GetSpritePixelsAtPosition(PixelPosition translation)
    {
        List<Pixel> pixels = new List<Pixel>();

        foreach(KeyValuePair<PixelGameObject, Dictionary<int, Pixel>> layer in Layers)
        {
            foreach(KeyValuePair<int, Pixel> pixel in layer.Value)
            {
                if ((layer.Key.position + pixel.Key) == (translation))
                {
                    pixels.Add(pixel.Value);
                }
            }
        }

        return pixels;
    }

}
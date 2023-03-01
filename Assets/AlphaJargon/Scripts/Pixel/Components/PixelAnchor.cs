using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelGame
{
    [System.Obsolete("Dont use these")]
    public class PixelAnchor : PixelComponent
    {
        public override PixelGameObject parent{get;set;}
        public PixelPosition position;
        public void add(PixelSprite ps, PixelPosition pp)
        {
            position = pp;
        }
        public override void Create(PixelGameObject parent)
        {
            this.parent = parent;
            position = new PixelPosition(0,0);
        }
        public override void Remove()
        {
            Destroy(this);
        }
        public static PixelPosition GetPixelAnchorForCollider(PolygonCollider2D collider)
        {
            // Get the bounds of the collider
            Bounds bounds = collider.bounds;

            // Get the center of the bounds
            Vector3 center = bounds.center;

            // Convert center to pixel coordinates
            float cellSize = PixelScreen.CellSize;
            float gridSideSize = PixelScreen.GridSideSize;
            int x = Mathf.RoundToInt((center.x + (gridSideSize * cellSize) / 2f) / cellSize);
            int y = Mathf.RoundToInt(((gridSideSize * cellSize) / 2f - center.y) / cellSize);

            // Create a new PixelPosition using the rounded pixel coordinates
            return new PixelPosition(x, y);
        }
    }
}
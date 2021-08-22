using System.Collections.Generic;
using UnityEngine;

namespace TNOB.Scripts
{
    public class RGB : MonoBehaviour
    {
        public Color[] colors = new Color[] {
            new Color(0.5f, 0, 0),
            new Color(0, 0.5f, 0),
            new Color(0, 0, 0.5f),
        };

        public int currentIndex = 0;
        private int nextIndex = 1;

        public float changeColourTime = 2.0f;

        //private float lastChange = 0.0f;
        private float timer = 0.0f;
        public List<Renderer> Renderers;

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > changeColourTime)
            {
                currentIndex = (currentIndex + 1) % colors.Length;
                nextIndex = (currentIndex + 1) % colors.Length;
                timer = 0.0f;

            }

            foreach (var renderer in Renderers)
            {
                renderer.material.color = Color.Lerp(colors[currentIndex], colors[nextIndex], timer / changeColourTime);
            }
        }
    }
}

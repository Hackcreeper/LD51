using System;
using UnityEngine;
using Util;

namespace Ui
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PopupManager : MonoBehaviour
    {
        public static PopupManager Instance { get; private set; }

        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            Instance = this;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ShowAt(Vector3 position, string message, Sprite icon)
        {
            _spriteRenderer.enabled = true;
            transform.position = position + new Vector3(0, 1f, .35f);
            
            GetComponent<Wobble>()?.SetBasePosition(transform.position);
        }

        public void Hide()
        {
            _spriteRenderer.enabled = false;
        }
    }
}
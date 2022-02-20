using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SA
{
    public class ResourceGeneratorOverlay : MonoBehaviour
    {
        [Header("Overlay Refs (Drops).")]
        public SpriteRenderer overlayIconSpriteRenderer;
        public TMP_Text overlayText;
        public Transform overlayBarTransform;
    }
}
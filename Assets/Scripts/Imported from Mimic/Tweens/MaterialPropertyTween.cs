using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Serialization;

public enum MaterialRendererType { IMAGE, RAW_IMAGE, MESH_RENDERER, DIRECT }
public enum PropertyParameterType { FLOAT, COLOR }

namespace Mimic.LolFramework
{
    public class MaterialPropertyTween : GeneralTween
    {
        [Header("Material Tween"), SerializeField]
        private MaterialRendererType rendererType;

        [SerializeField, ConditionalField("rendererType", false, new object[] { "DIRECT" })]
        private Material directMaterial;

        [SerializeField, ConditionalField("rendererType", false, new object[] { "IMAGE" })]
        private Image image;

        [SerializeField, ConditionalField("rendererType", false, new object[] { "RAW_IMAGE" })]
        private RawImage rawImage;

        [SerializeField, ConditionalField("rendererType", false, new object[] { "MESH_RENDERER" })]
        private MeshRenderer meshRenderer;

        private Material mat;
        private Material Mat {
            get {
                if (mat == null) {
                    switch (rendererType) {
                        case MaterialRendererType.IMAGE:
                            if (cloneMaterial) {
                                mat = Instantiate(image.material);
                                image.material = mat;
                            } else {
                                mat = image.material;
                            }
                            break;
                        case MaterialRendererType.RAW_IMAGE:
                            if (cloneMaterial) {
                                mat = Instantiate(rawImage.material);
                                rawImage.material = mat;
                            } else {
                                mat = rawImage.material;
                            }
                            break;
                        case MaterialRendererType.MESH_RENDERER:
                            if (cloneMaterial) {
                                mat = Instantiate(meshRenderer.material);
                                meshRenderer.material = mat;
                            } else {
                                mat = meshRenderer.material;
                            }
                            break;
                        default:
                            if (cloneMaterial) {
                                mat = Instantiate(directMaterial);
                            } else {
                                mat = directMaterial;
                            }
                            break;
                    }
                }
                return mat;
            }
        }

        [SerializeField]
        private string propertyName;

        [SerializeField]
        private PropertyParameterType parameterType;

        [SerializeField]
        private bool setStartingValue = false;

        [SerializeField, ConditionalField("parameterType", false, new object[] { "FLOAT" }), FormerlySerializedAs("startingValue")]
        private float startingValueFloat;

        [SerializeField, ConditionalField("parameterType", false, new object[] { "COLOR" })]
        private Color startingValueColor = Color.white;

        [SerializeField, ConditionalField("parameterType", false, new object[] { "FLOAT" }), FormerlySerializedAs("targetValue")]
        private float targetValueFloat;

        [SerializeField, ConditionalField("parameterType", false, new object[] { "COLOR" })]
        private Color targetValueColor = Color.white;

        [SerializeField]
        private bool cloneMaterial = true;

        public override event OnCompleteTween OnCompleteListeners;

        protected override Tween StartTween() {
            if (setStartingValue) {
                switch (parameterType) {
                    case PropertyParameterType.FLOAT:
                        Mat.SetFloat(propertyName, startingValueFloat);
                        break;
                    case PropertyParameterType.COLOR:
                        Mat.SetColor(propertyName, startingValueColor);
                        break;
                }
            }
            switch (parameterType) {
                case PropertyParameterType.FLOAT:
                    return Mat.DOFloat(targetValueFloat, propertyName, duration).
                        OnComplete(() => OnCompleteListeners?.Invoke());
                case PropertyParameterType.COLOR:
                    return Mat.DOColor(targetValueColor, propertyName, duration).
                        OnComplete(() => OnCompleteListeners?.Invoke());
            }
            return null;
        }
    }
}
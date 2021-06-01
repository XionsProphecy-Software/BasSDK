using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad.Plugins
{
    [CreateAssetMenu(menuName = "RainyReignGames/Reveal Material Data Asset")]
    public class RevealMaterialData : ScriptableObject
    {
        [System.Serializable]
        public struct FloatProperty
        {
            public string name;
            public int id;
            public float value;

            public int GetShaderID()
            {
                if (id == -1)
                {
                    id = Shader.PropertyToID(name);
                    return id;
                }
                return id;
            }
        }
        [System.Serializable]
        public struct ColorProperty
        {
            public string name;
            public int id;
            [ColorUsage(false, true)]
            public Color value;

            public int GetShaderID()
            {
                if (id == -1)
                {
                    id = Shader.PropertyToID(name);
                    return id;
                }
                return id;
            }
        }
        [System.Serializable]
        public struct TextureProperty
        {
            public string name;
            public int id;
            public Texture value;

            public int GetShaderID()
            {
                if (id == -1)
                {
                    id = Shader.PropertyToID(name);
                    return id;
                }
                return id;
            }
        }
        [System.Serializable]
        public struct Vector4Property
        {
            public string name;
            public int id;
            public Vector4 value;

            public int GetShaderID()
            {
                if (id == -1)
                {
                    id = Shader.PropertyToID(name);
                    return id;
                }
                return id;
            }
        }

        public Shader shader;
        public FloatProperty[] revealFloatProperties = new FloatProperty[0];
        public ColorProperty[] revealColorProperties = new ColorProperty[0];
        public TextureProperty[] revealTextureProperties = new TextureProperty[0];
        public Vector4Property[] revealVector4Properties = new Vector4Property[0];

        private void OnValidate()
        {
            for (int i = 0; i < revealFloatProperties.Length; i++)
            {
                revealFloatProperties[i].id = -1;
            }
            for (int i = 0; i < revealColorProperties.Length; i++)
            {
                revealColorProperties[i].id = -1;
            }
            for (int i = 0; i < revealTextureProperties.Length; i++)
            {
                revealTextureProperties[i].id = -1;
            }
            for (int i = 0; i < revealVector4Properties.Length; i++)
            {
                revealVector4Properties[i].id = -1;
            }
        }

        public void SetPropertiesOnMaterial(Material material)
        {
            if (material.shader != null)
            {
                material.shader = shader;
            }
            for(int i = 0; i < revealFloatProperties.Length; i++)
            {
                material.SetFloat(revealFloatProperties[i].GetShaderID(), revealFloatProperties[i].value);
            }
            for(int i = 0; i < revealColorProperties.Length; i++)
            {
                material.SetColor(revealColorProperties[i].GetShaderID(), revealColorProperties[i].value);
            }
            for(int i = 0; i < revealTextureProperties.Length; i++)
            {
                material.SetTexture(revealTextureProperties[i].GetShaderID(), revealTextureProperties[i].value);
            }
            for(int i = 0; i < revealVector4Properties.Length; i++)
            {
                material.SetVector(revealVector4Properties[i].GetShaderID(), revealVector4Properties[i].value);
            }
        }

        public void GetPropertyValuesFromMaterial(Material material)
        {
            for (int i = 0; i < revealFloatProperties.Length; i++)
            {
                if (material.HasProperty(revealFloatProperties[i].name))
                {
                    revealFloatProperties[i].value = material.GetFloat(revealFloatProperties[i].name);
                }
            }
            for (int i = 0; i < revealColorProperties.Length; i++)
            {
                if (material.HasProperty(revealColorProperties[i].name))
                {
                    revealColorProperties[i].value = material.GetColor(revealColorProperties[i].name);
                }
            }
            for (int i = 0; i < revealVector4Properties.Length; i++)
            {
                if (material.HasProperty(revealVector4Properties[i].name))
                {
                    revealVector4Properties[i].value = material.GetVector(revealVector4Properties[i].name);
                }
            }
            for (int i = 0; i < revealTextureProperties.Length; i++)
            {
                if (material.HasProperty(revealTextureProperties[i].name))
                {
                    revealTextureProperties[i].value = material.GetTexture(revealTextureProperties[i].name);
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if FULLGAME
using Sirenix.OdinInspector;
#endif

namespace BS
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemDefinition : MonoBehaviour
    {
#if FULLGAME
        [ValueDropdown("GetAllItemID")]
#endif
        public string itemId;
        public Transform holderPoint;
        public Transform parryPoint;
        public HandleDefinition mainHandleRight;
        public HandleDefinition mainHandleLeft;
        public Transform flyDirRef;
        public Preview preview;
        public List<CustomReference> customReferences;
        public bool initialized { get; protected set; }

        [NonSerialized]
        public List<Renderer> renderers;
        [NonSerialized]
        public List<ColliderGroup> colliderGroups;
        [NonSerialized]
        public List<WhooshPoint> whooshPoints;


#if FULLGAME
        public delegate void InitializedDelegate(Item interactiveObject);
        public event InitializedDelegate Initialized;

        public List<ValueDropdownItem<string>> GetAllItemID()
        {
            return Catalog.current.GetDropdownAllID(Catalog.Category.Item);
        }
#endif

        public Transform GetCustomReference(string name)
        {
            CustomReference customReference = customReferences.Find(cr => cr.name == name);
            if (customReference != null)
            {
                return customReference.transform;
            }
            else
            {
                Debug.LogError("[" + itemId + "] Cannot find item definition custom reference " + name);
                return null;
            }
        }

        protected virtual void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy) return;
            holderPoint = this.transform.Find("HolderPoint");
            if (!holderPoint)
            {
                holderPoint = new GameObject("HolderPoint").transform;
                holderPoint.SetParent(this.transform, false);
            }
            parryPoint = this.transform.Find("ParryPoint");
            if (!parryPoint)
            {
                parryPoint = new GameObject("ParryPoint").transform;
                parryPoint.SetParent(this.transform, false);
            }
            preview = this.GetComponentInChildren<Preview>();
            if (!preview && this.transform.Find("Preview")) preview = this.transform.Find("Preview").gameObject.AddComponent<Preview>();
            if (!preview)
            {
                preview = new GameObject("Preview").AddComponent<Preview>();
                preview.transform.SetParent(this.transform, false);
            }
            Transform whoosh = this.transform.Find("Whoosh");
            if (whoosh && !whoosh.GetComponent<WhooshPoint>())
            {
                whoosh.gameObject.AddComponent<WhooshPoint>();
            }
            
            if (!mainHandleRight)
            {
                foreach (HandleDefinition handleDefinition in this.GetComponentsInChildren<HandleDefinition>())
                {
                    if (handleDefinition.IsAllowed(Side.Right))
                    {
                        mainHandleRight = handleDefinition;
                        break;
                    }
                }
            }
            if (!mainHandleLeft)
            {
                foreach (HandleDefinition handleDefinition in this.GetComponentsInChildren<HandleDefinition>())
                {
                    if (handleDefinition.IsAllowed(Side.Left))
                    {
                        mainHandleLeft = handleDefinition;
                        break;
                    }
                }
            }
            if (!mainHandleRight)
            {
                mainHandleRight = this.GetComponentInChildren<HandleDefinition>();
            }
        }

#if FULLGAME
        protected virtual void Awake()
        {
            renderers = new List<Renderer>(this.GetComponentsInChildren<Renderer>());
            colliderGroups = new List<ColliderGroup>(this.GetComponentsInChildren<ColliderGroup>());
            whooshPoints = new List<WhooshPoint>(this.GetComponentsInChildren<WhooshPoint>());
        }

        protected virtual void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            if (!initialized && itemId != null && itemId != "" && itemId != "None")
            {
                Init(Catalog.current.GetData<ItemData>(itemId));
            }
        }

        public virtual Item Init(ItemData item)
        { 
            foreach (Item existingInteractiveObject in this.gameObject.GetComponents<Item>())
            {
                Destroy(existingInteractiveObject);
            }
            itemId = item.id;
            initialized = true;
            Item interactiveObject = item.CreateComponent(this.gameObject);
            if (Initialized != null) Initialized.Invoke(interactiveObject);
            return interactiveObject;
        }
#endif

        public static void DrawGizmoArrow(Vector3 pos, Vector3 direction, Vector3 upwards, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            Vector3 right = Quaternion.LookRotation(direction, upwards) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction, upwards) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(this.transform.TransformPoint(this.GetComponent<Rigidbody>().centerOfMass), 0.01f);
            Gizmos.matrix = holderPoint.transform.localToWorldMatrix;
            DrawGizmoArrow(Vector3.zero, Vector3.forward * 0.1f, Vector3.up, Common.HueColourValue(HueColorNames.Purple), 0.1f, 10);
            DrawGizmoArrow(Vector3.zero, Vector3.up * 0.05f, Vector3.up, Common.HueColourValue(HueColorNames.Green), 0.05f);
        }
    }
}

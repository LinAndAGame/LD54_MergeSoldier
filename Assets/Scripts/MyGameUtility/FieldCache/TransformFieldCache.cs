using System;
using UnityEngine;

namespace MyGameUtility.FieldCache {
    public class TransformFieldCache : FieldCache<Transform> {
        public TransformFieldCache(Component target,string relativePath) : base(()=> target.transform.Find(relativePath)) { }
    }
}
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace MyGameUtility {
    public interface ICanUsePool{
        IDisposable PooledObject { get; set; }
    }
}
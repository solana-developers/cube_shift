using System;
using System.Collections;
using ToolBox.Pools;
using UnityEngine;

namespace Cubeshift
{
    public class PooledFx : MonoBehaviour, IPoolable
    {
        public float ReleaseTime;

        public void OnEnable()
        {
            StartCoroutine(ReturnToPoolDelayed());
        }

        public void OnReuse()
        {
        }

        private IEnumerator ReturnToPoolDelayed()
        {
            yield return new WaitForSeconds(ReleaseTime);
            this.gameObject.Release();
        }

        public void OnRelease()
        {
            
        }
    }
}
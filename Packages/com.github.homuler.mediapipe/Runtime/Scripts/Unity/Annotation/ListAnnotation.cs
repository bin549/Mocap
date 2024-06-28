using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public abstract class ListAnnotation<T> : HierarchicalAnnotation where T : HierarchicalAnnotation
  {
    [SerializeField] private GameObject _annotationPrefab;

    private List<T> _children;
    protected List<T> children
    {
      get
      {
        if (_children == null)
        {
          _children = new List<T>();
        }
        return _children;
      }
    }

    public T this[int index] => children[index];

    public int count => children.Count;

    public void Fill(int count)
    {
      while (children.Count < count)
      {
        children.Add(InstantiateChild(false));
      }
    }

    public void Add(T element)
    {
      children.Add(element);
    }

    public override bool isMirrored
    {
      set
      {
        foreach (var child in children)
        {
          child.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        foreach (var child in children)
        {
          child.rotationAngle = value;
        }
        base.rotationAngle = value;
      }
    }

    protected virtual void Destroy()
    {
      foreach (var child in children)
      {
        Destroy(child);
      }
      _children = null;
    }

    protected virtual T InstantiateChild(bool isActive = true)
    {
      var annotation = InstantiateChild<T>(_annotationPrefab);
      annotation.SetActive(isActive);
      return annotation;
    }

    protected void CallActionForAll<TArg>(IReadOnlyList<TArg> argumentList, Action<T, TArg> action)
    {
      for (var i = 0; i < Mathf.Max(children.Count, argumentList.Count); i++)
      {
        if (i >= argumentList.Count)
        {
          // children.Count > argumentList.Count
          action(children[i], default);
          continue;
        }

        // reset annotations
        if (i >= children.Count)
        {
          // children.Count < argumentList.Count
          children.Add(InstantiateChild());
        }
        else if (children[i] == null)
        {
          // child is not initialized yet
          children[i] = InstantiateChild();
        }
        action(children[i], argumentList[i]);
      }
    }
  }
}

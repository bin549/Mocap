using UnityEngine;
using System.Collections.Generic;
using Mediapipe;

namespace Mediapipe.Unity.Sample.PoseTracking
{ 
    public class Mediapipe2UnitySkeletonController : MonoBehaviour
    {
        [SerializeField] private HumanJointFactory jointFactory;
        [SerializeField] private HashSet<HumanJointCalculator> calculators;
        [SerializeField, Range(0f, 1f)] private float landmarkSmoothing = 0.5f;

        private List<Vector3> _smoothedPoints;
        private bool _hasSmoothed;

        private Animator _anim;

        private void Start()
        {
            _anim = GetComponent<Animator>();
            jointFactory = new HumanJointFactory(_anim);
            calculators = jointFactory.Generate();
        }

        private void Update()
        {
            foreach (var calculator in calculators)
            {
                calculator.Calc();
            }
        }

        public void Refresh(LandmarkList target)
        {
            if (target == null || target.Landmark == null || target.Landmark.Count == 0)
            {
                return;
            }

            // 初始化或尺寸变化时重置平滑缓存
            if (_smoothedPoints == null || _smoothedPoints.Count != target.Landmark.Count)
            {
                _smoothedPoints = new List<Vector3>(target.Landmark.Count);
                for (int i = 0; i < target.Landmark.Count; i++)
                {
                    var lm = target.Landmark[i];
                    _smoothedPoints.Add(new Vector3(lm.X, lm.Y, lm.Z));
                }
                _hasSmoothed = true;
            }

            var alpha = Mathf.Clamp01(landmarkSmoothing);
            var smoothedList = new LandmarkList();
            for (int i = 0; i < target.Landmark.Count; i++)
            {
                var src = target.Landmark[i];
                var curr = new Vector3(src.X, src.Y, src.Z);
                var prev = _smoothedPoints[i];
                var sm = Vector3.Lerp(prev, curr, alpha);
                _smoothedPoints[i] = sm;

                var dst = new Landmark
                {
                    X = sm.x,
                    Y = sm.y,
                    Z = sm.z,
                    Visibility = src.Visibility,
                    Presence = src.Presence
                };
                smoothedList.Landmark.Add(dst);
            }

            foreach (var calculator in calculators)
            {
                calculator.Refresh(smoothedList);
            }
        }
    }
}

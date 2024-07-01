using UnityEngine;
using System.Collections.Generic;

namespace Mediapipe.Unity.Sample.PoseTracking
{
    internal class Mediapipe2UnitySkeletonController : MonoBehaviour
    {
        [SerializeField] private HumanJointFactory jointFactory;
        [SerializeField] private HashSet<HumanJointCalculator> calculators;

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
            foreach (var calculator in calculators)
            {
                calculator.Refresh(target);
            }
        }
    }
}

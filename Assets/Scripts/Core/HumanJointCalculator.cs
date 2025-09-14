using UnityEngine;

namespace Mediapipe.Unity.Sample.PoseTracking
{
    public class HumanJointCalculator
    {
        public Transform obj;
        public LandmarkList _landmarkList;
        private readonly Quaternion _initialWorldRotation;

        public HumanJointCalculator(Transform t)
        {
            obj = t;
            _initialWorldRotation = t.rotation;
        }

        public void Refresh(LandmarkList landmarkList)
        {
            _landmarkList = landmarkList;
        }

        public virtual void Calc()
        {
        }

        protected void ResetToInitialRotation()
        {
            obj.rotation = _initialWorldRotation;
        }
    }
};

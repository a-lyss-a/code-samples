public class NetworkPuppeteering : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    Dictionary<int, float[]> kinectRotationsMap; //changing JointId to int for serialization AND QUATERNION TO FLOAT[]
    [SerializeField]
    Vector3 hipPosition;
    private void mapBonesFromKinect()
    {
        // chainging it from JointId to int and Quaternion to Vector3 for serialization purposes
        Dictionary<int, float[]> kinectRotations = new Dictionary<int, float[]>();
        for (int j = 0; j < (int)JointId.Count; j++)
        {
            if (MapKinectJoint((JointId)j) != HumanBodyBones.LastBone && absoluteOffsetMap.ContainsKey((JointId)j))
            {
                // get the absolute offset and calculate new angle
                Quaternion absOffset = absoluteOffsetMap[(JointId)j];
                Quaternion rot = Y_90_ROT * absOffset * Quaternion.Inverse(absOffset) * KinectDevice.absoluteJointRotations[j] * absOffset;
                // convert to a serialisable structure and add to dictionary
                float[] cursedQuaternion = new float[4] { rot.x, rot.y, rot.z, rot.w };
                kinectRotations[j] = cursedQuaternion; //changing JointId to int because JointIds can't be serialized
                // get the transform of the hbb corresponding to the kinect joint in question and overwrite rotation
                Transform finalJoint = PuppetAnimator.GetBoneTransform(MapKinectJoint((JointId)j));
                finalJoint.rotation = rot;
                if (j == 0)
                {
                    finalJoint.position = CharacterRootTransform.position + new Vector3(RootPosition.transform.localPosition.x, RootPosition.transform.localPosition.y + OffsetY, RootPosition.transform.localPosition.z - OffsetZ);
                    hipPosition = finalJoint.position;
                }
                kinectRotationsMap = kinectRotations;
            }        }    }    }

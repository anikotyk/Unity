using Lofelt.NiceVibrations;
using System.Collections;
using UnityEngine;

public class RagdollMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed;
    [SerializeField] private Wall _firstWall;
    [SerializeField] private Wall _targetWall;
    [SerializeField] private ConfigurableJoint[] _set1Spring;
    [SerializeField] private Rigidbody[] _turnOnGravityRigibdody;
    [SerializeField] private ConfigurableJoint[] _set100Spring;
    
    [SerializeField] private float _delayBeforeParticles;
    [SerializeField] private float _delayBeforeEndLevel;

    [SerializeField] private Transform direction;

    private bool flag = true;
    private bool flag2 = true;

    private bool isMove = false;


    public void StartMove(Wall targetWall)
    {
        _targetWall = targetWall;
        isMove = true;
    }

    private void FixedUpdate()
    {
        if (!isMove)
        {
            return;
        }

        if (_rb.position.z <= _firstWall.WallPositionPoint.position.z + 3 && flag)
        {
            CinemachineController.Instance.SetCameraEnd();
            Time.timeScale = 0.4f;
            flag = false;
        }
        else if (_rb.position.z <= _firstWall.WallPositionPoint.position.z-3 && flag2)
        {
            Time.timeScale = 1f;
            flag2 = false;
        }
        else if (_rb.position.z <= _targetWall.WallPositionPoint.position.z + 1)
        {
            _rb.velocity = Vector3.zero;
            _rb.Sleep();
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            _rb.isKinematic = true;
            GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Free;
            _rb.constraints = RigidbodyConstraints.FreezePositionZ;
            _rb.isKinematic = false;
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            foreach (ConfigurableJoint joint in _set1Spring)
            {
                JointDrive drive = joint.angularYZDrive;
                drive.positionSpring = 1;
                joint.angularYZDrive = drive;

                drive = joint.angularXDrive;
                drive.positionSpring = 1;
                joint.angularXDrive = drive;
            }

            foreach (ConfigurableJoint joint in _set100Spring)
            {
                JointDrive drive = joint.angularYZDrive;
                drive.positionSpring = 100;
                joint.angularYZDrive = drive;

                drive = joint.angularXDrive;
                drive.positionSpring = 100;
                joint.angularXDrive = drive;
            }

            foreach (Rigidbody rb in _turnOnGravityRigibdody)
            {
                rb.useGravity = true;
            }

            _targetWall.HideCoefText();

            enabled = false;
            CinemachineController.Instance.SetCameraDie();
            StartCoroutine(ShowWallParticles());
        }
        else
        {
            _rb.velocity = direction.forward * _speed;
        }
    }

    private IEnumerator ShowWallParticles()
    {
        yield return new WaitForSeconds(_delayBeforeParticles);
        SoundsController.Instance.LevelWin();
        _targetWall.PlayParticles();
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
        yield return new WaitForSeconds(_delayBeforeEndLevel);

        GameController.Instance.EndLevel();
    }
}

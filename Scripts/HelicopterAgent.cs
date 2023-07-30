using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;

public class HelicopterAgent : Agent
{
    public PhantomController controller;
    public Text endText;

    public PhantomFlightComputer computer;
    /* Features:
     * Camera infomation (84, 84) * stack (4)
     */

    /* Action:
     * 1. Pitch
     * 2. Roll
     * 3. Rudder
     */

    /* Rewards:
     * 1. closer distance to target
     * 2. achieved target
     */

    public int behaviorType;
    public int currentTarget;
    public Transform[] targets;
    
    private Rigidbody rBody;
    private float currentTime;
    private bool firstTime = true;
    private Vector3 initPos;

    private bool ending;

    private int epoch;

    void Start()
    {
        epoch = 0;
        behaviorType = 0;
        initPos = transform.localPosition;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
    }

    public Transform Target;
    public override void OnEpisodeBegin()
    {
        epoch++;
        currentTarget = 0;
        rBody = GetComponent<Rigidbody>();
        transform.localPosition = initPos;
        transform.eulerAngles = Vector3.zero;
        rBody.angularVelocity = Vector3.zero;
        rBody.velocity = Vector3.zero;
        Input.ResetInputAxes();
        if (!firstTime)
        {
            controller.HelicopterReset();
        }
        SetResetParameters();
        controller.fail = false;
        firstTime = false;
        endText.gameObject.SetActive(false);
        ending = false;
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 3
        controller.pitchInput = actionBuffers.ContinuousActions[0];
        controller.rollInput = actionBuffers.ContinuousActions[1];
        controller.yawInput = actionBuffers.ContinuousActions[2];
        AddReward(1 / (float)(Vector3.Distance(transform.localPosition, targets[currentTarget].localPosition) + 1e-9));
        AddReward(currentTarget * 0.01f);
        // Fell off platform or time exceeds
        if (controller.fail || StepCount > 50000f)
        {
            AddReward(-1.0f);
            EndEpisode();
        }
        if (currentTarget >= 7 && !ending)
        {
            ending = true;
            AddReward(1.0f);
            endText.gameObject.SetActive(true);
            StartCoroutine(Ending());
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        behaviorType = 1;
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Pitch");
        continuousActionsOut[1] = Input.GetAxis("Roll");
        continuousActionsOut[2] = Input.GetAxis("Rudder");
    }

    IEnumerator Ending()
    {
        yield return new WaitForSeconds(5f);
        EndEpisode();
    }

    public void SetResetParameters()
    {
        currentTime = 0;
        currentTarget = 0;
    }
}

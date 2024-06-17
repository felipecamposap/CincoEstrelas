using UnityEngine;

public class Accelerate : MonoBehaviour, IState
{
    private Npc npc;
    private float acceleration = 0.090f;
    private float steeringPower = 0.006f;
    private float maxSpeed = 8;
    private float bkSpeed;

    // ------
    private Vector3 steeringTotal = Vector3.zero;
    private float SteeringForce = 0;
    private Vector3 velocity = Vector3.zero;
    private float currentSpeed;
    private Vector3 desiredVelocity;



    public void Enter(Npc _npc)
    {
        npc = _npc;
        maxSpeed = npc.speed;
        bkSpeed = maxSpeed;
        currentSpeed = 0;
        SteeringForce = 0;
        //throw new System.NotImplementedException();
    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }

    void IState.Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(npc.frontRay.position, npc.frontRay.forward, out hit, 5, npc.layerRay))
        {
            RaycastBehaviour(hit);
        }
        Debug.DrawRay(npc.frontRay.position, npc.frontRay.forward * 5, Color.white);

        SteeringCalculate();
        if ((npc.targetPosition - npc.transform.position).magnitude < 2f){
            npc.GetNextDestination();
        }
        var modifierSteering = Mathf.Max(currentSpeed / (maxSpeed * 80), 0);

        currentSpeed += currentSpeed < maxSpeed ? acceleration : 0;
        SteeringForce += SteeringForce < maxSpeed ? steeringPower +  modifierSteering : 0;

        // Define a velocidade do Rigidbody
        npc.rb.velocity = npc.transform.forward * currentSpeed;
    }

    private void RaycastBehaviour(RaycastHit _hit)
    {
        try
        {
            var tl = _hit.transform.GetComponent<TrafficLight>();
            var index = 0;
            for (var i = 0; i < tl.streets.Length; i++)
            {
                index = i;
                if (npc.currentStreet == tl.streets[i])
                {
                    break;
                }
            }
            if (index >= tl.streets.Length)
                index = tl.streets.Length - 1;
            if (tl.isRed[index])
                npc.ChangeState(new BreakSpeed());
        }
        catch 
        {
            npc.ChangeState(new BreakSpeed());
        }
    }

    private void SteeringCalculate()
    {

        // Calcula a dire��o desejada para o pr�ximo destino
        desiredVelocity = (npc.targetPosition - npc.transform.position).normalized;

        // Calcula a for�a de dire��o
        steeringTotal = (desiredVelocity - velocity) * SteeringForce;


        // Atualiza a velocidade
        velocity = npc.transform.forward + steeringTotal * Time.deltaTime;


        // Rotaciona para olhar na dire��o do movimento
        if (velocity.magnitude > 0.1f) // Verifica se h� movimento significativo
        {
            npc.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    public void ExitTrigger()
    {
        maxSpeed = bkSpeed / 1.5f;
        SteeringForce = 0.0065f;
        //Debug.Log("Exit");
    }

    public void TouchTrigger() 
    {
        maxSpeed = bkSpeed;
        SteeringForce = 0.0065f;
        //Debug.Log("Touch");
    }


   
}

using UnityEngine;

public class Accelerate : MonoBehaviour, IState
{
    Npc npc;
    float acceleration = 0.090f;
    float steeringPower = 0.006f;
    float maxSpeed = 8;
    float bkSpeed;

    // ------
    Vector3 steeringTotal = Vector3.zero;
    float SteeringForce = 0;
    Vector3 velocity = Vector3.zero;
    float currentSpeed;
    Vector3 desiredVelocity;



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
        float modifierSteering = Mathf.Max(currentSpeed / (maxSpeed * 80), 0);

        currentSpeed += currentSpeed < maxSpeed ? acceleration : 0;
        SteeringForce += SteeringForce < maxSpeed ? steeringPower +  modifierSteering : 0;

        // Define a velocidade do Rigidbody
        npc.rb.velocity = npc.transform.forward * currentSpeed;
    }

    private void RaycastBehaviour(RaycastHit _hit)
    {
        try
        {
            TrafficLight tl = _hit.transform.GetComponent<TrafficLight>();
            int index = 0;
            for (int i = 0; i < tl.streets.Length; i++)
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

    void SteeringCalculate()
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

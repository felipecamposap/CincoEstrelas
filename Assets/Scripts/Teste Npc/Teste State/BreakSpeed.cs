using UnityEngine;

public class BreakSpeed : IState
{
    Npc npc;
    float breakPower = 0.30f;
    float steeringPower = 0.006f;
    float maxSpeed = 8;

    // ------
    Vector3 steeringTotal = Vector3.zero;
    float SteeringForce = 0;
    Vector3 velocity = Vector3.zero;
    float currentSpeed;
    Vector3 desiredVelocity;

    public void Enter(Npc _npc)
    {
        npc = _npc;
        maxSpeed = npc.rb.velocity.magnitude;
        currentSpeed = maxSpeed;
        //Debug.Log("Break");
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
        else
        {
            npc.ChangeState(new Accelerate());
        }
        Debug.DrawRay(npc.frontRay.position, npc.frontRay.forward * 5, Color.red);

        SteeringCalculate();
        if ((npc.targetPosition - npc.transform.position).magnitude < 1.5f)
            npc.GetNextDestination();

        if (currentSpeed > 0)
            currentSpeed -= breakPower;
        else
            currentSpeed = 0;
        SteeringForce -= SteeringForce > 0 ? steeringPower : 0;

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
            if (!tl.isRed[index])
                npc.ChangeState(new Accelerate());
        }
        catch { }
    }

    void SteeringCalculate()
    {

        // Calcula a direção desejada para o próximo destino
        desiredVelocity = (npc.targetPosition - npc.transform.position).normalized;

        // Calcula a força de direção
        steeringTotal = (desiredVelocity - velocity) * SteeringForce;


        // Atualiza a velocidade
        velocity = npc.transform.forward + steeringTotal * Time.deltaTime;//Vector3.ClampMagnitude(velocity + steeringForce * Time.deltaTime, maxSpeed);


        // Rotaciona para olhar na direção do movimento
        if (velocity.magnitude > 0.1f) // Verifica se há movimento significativo
        {
            npc.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    public void TouchTrigger()
    {
        //throw new System.NotImplementedException();
    }

    public void ExitTrigger()
    {
        //throw new System.NotImplementedException();
    }
}

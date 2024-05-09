public interface IState
{
    void Enter(Npc _npc);
    void Update();
    void Exit();
    public void TouchTrigger();
    public void ExitTrigger();
}

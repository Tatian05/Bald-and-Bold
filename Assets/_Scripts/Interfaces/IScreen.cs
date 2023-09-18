public interface IScreen
{
    void Activate();
    void Deactivate();
    void Free();
    void PauseObjectsInCinematic();
    void UnpauseObjectsInCinematic();
}

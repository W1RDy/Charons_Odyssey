using System.Threading;

public class PauseToken
{
    public bool IsCancellationRequested {get; private set;}
    private CancellationTokenSource _tokenSouce = new CancellationTokenSource();
    public CancellationToken CancellationToken { get; private set; }

    public PauseToken()
    {
        CancellationToken = _tokenSouce.Token;
    }

    public void Pause()
    {
        IsCancellationRequested = true;
        _tokenSouce.Cancel();
    }

    public void Unpause()
    {
        IsCancellationRequested = false;
        _tokenSouce.Dispose();
        _tokenSouce = new CancellationTokenSource();
        CancellationToken = _tokenSouce.Token;
    }
}

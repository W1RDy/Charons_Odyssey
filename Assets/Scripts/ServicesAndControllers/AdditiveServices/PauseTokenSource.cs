using System.Collections.Generic;

public class PauseTokenSource
{
    private HashSet<PauseToken> _tokens = new HashSet<PauseToken>();
    public PauseToken Token
    {
        get
        {
            var token = new PauseToken();
            _tokens.Add(token);
            return token;
        }
    }

    public void Pause()
    {
        var hashSet = new HashSet<PauseToken>(_tokens);
        foreach (var token in hashSet)
        {
            if (token == null) _tokens.Remove(token);
            else token.Pause();
        }
    }

    public void Unpause()
    {
        var hashSet = new HashSet<PauseToken>(_tokens);
        foreach (var token in hashSet)
        {
            if (token == null) _tokens.Remove(token);
            else token.Unpause();
        }
    }
}

public class LoopingSoundsPlayer : AudioPlayer
{
    public override void PlayAudio(string index)
    {
        var audio = _audioService.GetSound(index);
        PlayAudio(audio);
    }
}

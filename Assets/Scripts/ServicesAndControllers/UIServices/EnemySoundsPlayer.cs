public class EnemySoundsPlayer : LoopingSoundsPlayer
{
    public override void PlayAudio(string index)
    {
        var audio = _audioService.GetSound(index);
        if (audio.GroupIndex == "looping sound") base.PlayAudio(audio);
        else _audioMaster.PlaySound(index);
    }
}

    public enum VoiceType
    {
        None,
        把相同的东西找出来吧,
    }
    public enum SoundType
    {
        None,
        出场可爱的嚎叫的声音,
    }
    public enum SoundID
    {
        None,
        背景,
        语音,
        音效,
    }
    public class SoundController : Singleton<SoundController>
    {
        public string BackGoundSound = "春节背景";
        public float SoundVolume = 0.6f;
        public void IsPlayBackGroundSound(bool isPlay)
        {
            //if (isPlay)
            // PlaySound(BackGoundSound, false).SetLoop(true).SetID(GetSoundID(SoundID.背景)).SetVolume(0.6f);
            //else
            //DestroySound(BackGoundSound, false);
        }
        string GetSoundName(VoiceType type)
        {
            switch (type)
            {
                case VoiceType.None:
                    return "";
                case VoiceType.把相同的东西找出来吧:
                    return "01001把相同的东西找出来吧~";
            }
            return "";
        }
        string GetSoundName(SoundType type)
        {
            switch (type)
            {
                case SoundType.None:
                    return "";
                case SoundType.出场可爱的嚎叫的声音:
                    return "出场可爱的嚎叫的声音";
            }
            return "";
        }
        public string GetSoundID(SoundID id)
        {
            switch (id)
            {
                case SoundID.None:
                    return "";
                case SoundID.背景:
                    return "背景";
                case SoundID.语音:
                    return "语音";
                case SoundID.音效:
                    return "音效";
                default:
                    return "";
            }
        }
    }
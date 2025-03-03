using FMOD;

/// <summary>
/// 간단한 사운드매니저
/// 배경음은 Track으로, 효과음은 Snippet으로
/// </summary>
class SoundManager
{
    // 싱글턴
    private static SoundManager instance = null!;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SoundManager();
            }
            return instance;
        }
    }

    // FMOD 시스템
    static FMOD.System system;

    // 상대경로
    static string resourcesPath = "..\\..\\..\\..\\Resources\\Sounds\\";

    // 사운드파일당 한번식만 로드되도록 관리
    Dictionary<string, Sound> loadedSounds;

    // (배경음악용) 재생되는 트랙 관리
    Dictionary<string, Track> managedTracks;

    // 페이드 속도: 클수록 빨리 페이드됨
    static float fadeSpeed = 0.02F;

    SoundManager()
    {
        loadedSounds = new Dictionary<string, Sound>();
        managedTracks = new Dictionary<string, Track>();

        // FMOD 시스템 초기화
        system = new FMOD.System();

        RESULT r = Factory.System_Create(out system);
        //if (r != RESULT.OK) Console.WriteLine("에러: System_Create " + r);

        r = system.init(32, INITFLAGS.NORMAL, IntPtr.Zero);
        //if (r != RESULT.OK) Console.WriteLine("에러: system.init " + r);
    }

    ~SoundManager()
    {
        // 지금까지 로드했던 사운드 릴리즈
        if (loadedSounds != null)
        {
            if (loadedSounds.Count > 0)
            {
                foreach (Sound sound in loadedSounds.Values) { sound.release(); }
            }
            loadedSounds.Clear();
        }

        // FMOD 시스템 정리
        system.close();
        system.release();
    }

    /// <summary>
    /// 짧은 소리 (효과음용)
    /// </summary>
    class Snippet
    {
        // FMOD 채널과 채널그룹
        protected Sound sound;
        protected Channel channel;
        protected ChannelGroup channelGroup;

        protected float volumeMultiplier;     // 외부에서 설정한 볼륨값

        public Snippet(Sound sound, float volumeMultiplier)
        {
            this.sound = sound;
            channel = new Channel();
            channelGroup = new ChannelGroup();
            this.volumeMultiplier = volumeMultiplier;
        }

        public virtual void Play()
        {
            RESULT r = system.playSound(sound, channelGroup, false, out channel);
            //if (r != RESULT.OK) Console.WriteLine("에러: playSound " + r);

            if (volumeMultiplier > 1.0F) channel.setVolume(1.0F);
            else if (volumeMultiplier < 0.0F) channel.setVolume(0.0F);
            else channel.setVolume(volumeMultiplier);
        }

        ~Snippet()
        {
            channel.stop();
            channelGroup.release();
        }
    }

    /// <summary>
    /// 트랙: 긴 파일을 반복재생, 자동 페이드 적용 (배경음용)
    /// </summary>
    class Track : Snippet
    {
        // 생성된 트랙 상태관리용 리스트
        static List<Track> tracks = new List<Track>();

        // track에서 제거되기를 기다리고 있는지
        bool waitingRemoval;

        float internalVolume = 0.0F;    // 트랙의 페이드용 내부변수, 0과 1을 오가며 volumeMultiplier를 곱해서 최종 적용
        float InternalVolume
        {
            get { return internalVolume; }
            set
            {
                if (value < 0.0F) internalVolume = 0.0F;
                else if (value > 1.0F) internalVolume = 1.0F;
                else internalVolume = value;

                channel.setVolume(internalVolume * volumeMultiplier);
            }
        }

        // 볼륨 변화상태
        VolumeState volumeState;
        public enum VolumeState
        {
            STATIC,                 // 볼륨 변화 없음
            INCREASING,             // 볼륨 증가
            DECREASING_TO_PAUSE,    // 정지를 위한 볼륨 감소
            DECREASING_TO_STOP      // 일시정지를 위한 볼륨 감소
        }

        public Track(Sound sound, float volumeMultiplier)
            : base(sound, volumeMultiplier)
        {
            waitingRemoval = false;

            InternalVolume = 0.0F;
            volumeState = VolumeState.INCREASING;

            // 첫 생성시 관리용 리스트에 추가
            tracks.Add(this);
        }

        void VolumeUpdate()
        {
            switch (volumeState)
            {
                case VolumeState.INCREASING:
                    InternalVolume = InternalVolume + fadeSpeed;
                    if (InternalVolume >= 1.0F)
                    {
                        //Console.WriteLine("Fade In End");
                        volumeState = VolumeState.STATIC;
                    }
                    break;
                case VolumeState.DECREASING_TO_PAUSE:
                    InternalVolume = InternalVolume - fadeSpeed;
                    if (InternalVolume <= 0.0F)
                    {
                        channel.setPaused(true);
                        volumeState = VolumeState.STATIC;
                    }
                    break;
                case VolumeState.DECREASING_TO_STOP:
                    InternalVolume = InternalVolume - fadeSpeed;
                    if (InternalVolume <= 0.0F)
                    {
                        channel.stop();
                        volumeState = VolumeState.STATIC;
                        waitingRemoval = true;
                    }
                    break;
            }
        }

        public override void Play()
        {
            RESULT r = system.playSound(sound, channelGroup, false, out channel);
            //if (r != RESULT.OK) Console.WriteLine("에러: playSound " + r);
        }

        public void StopTrack()
        {
            volumeState = VolumeState.DECREASING_TO_STOP;
        }

        public void PauseTrack()
        {
            volumeState = VolumeState.DECREASING_TO_PAUSE;
        }

        public void ResumeTrack()
        {
            channel.setPaused(false);
            volumeState = VolumeState.INCREASING;
        }

        public static void AllTracksUpdate()
        {
            for (int i = tracks.Count - 1; i >= 0; i--)
            {
                if (tracks[i].waitingRemoval)
                {
                    tracks.RemoveAt(i);
                }
                else
                {
                    tracks[i].VolumeUpdate();
                }
            }
        }
    }

    /// <summary>
    /// 트랙 재생
    /// </summary>
    /// <param name="name">파일 이름(확장명까지 작성)</param>
    /// <param name="trackKey">등록할 트랙 키(단어 형태로 등록)</param>
    /// <param name="volume">트랙 볼륨(0과 1 사이)</param>
    public void PlayTrack(string fileName, string trackKey, float volume = 1.0F)
    {
        LoadSound(fileName, true);

        if (loadedSounds.ContainsKey(fileName))
        {
            if (managedTracks.ContainsKey(trackKey))
            {
                managedTracks[trackKey].StopTrack();
            }

            Track newTrack = new Track(loadedSounds[fileName], volume);
            newTrack.Play();
            managedTracks[trackKey] = newTrack;
        }
    }

    /// <summary>
    /// 짧은 소리(스니펫) 재생
    /// </summary>
    /// <param name="name"></param>
    /// <param name="volume"></param>
    public void PlaySnippet(string name, float volume = 1.0F)
    {
        LoadSound(name, false);

        // 스니펫은 일회용 - 따로 관리 안한다
        Snippet newSnippet = new Snippet(loadedSounds[name], volume);
        newSnippet.Play();
        //Console.WriteLine("played " + name);
    }

    /// <summary>
    /// 재생 시 등록했던 키로 트랙 정지
    /// </summary>
    /// <param name="trackKey"></param>
    public void StopTrack(string trackKey)
    {
        if (managedTracks.TryGetValue(trackKey, out Track? track))
        {
            track.StopTrack();
        }
    }

    /// <summary>
    /// 재생 시 등록했던 키로 트랙 일시정지
    /// </summary>
    /// <param name="trackKey"></param>
    public void PauseTrack(string trackKey)
    {
        if (managedTracks.TryGetValue(trackKey, out Track? track))
        {
            track.PauseTrack();
        }
    }

    /// <summary>
    /// 재생 시 등록했던 키로 트랙 재개
    /// </summary>
    /// <param name="trackKey"></param>
    public bool ResumeTrack(string trackKey)
    {
        if (managedTracks.TryGetValue(trackKey, out Track? track))
        {
            track.ResumeTrack();
            return true;
        }
        else return false;
    }

    void LoadSound(string name, bool loop)
    {
        if (loadedSounds == null)
        {
            loadedSounds = new Dictionary<string, Sound>();
        }

        if (!loadedSounds.ContainsKey(name))
        {
            Sound newSound = new Sound();

            MODE mode = loop ? MODE.LOOP_NORMAL : MODE.DEFAULT;
            RESULT r = system.createSound(resourcesPath + name, mode, out newSound);
            //if (r != RESULT.OK) Console.WriteLine("에러: createSound " + r);

            if (r == RESULT.OK)
            {
                loadedSounds.Add(name, newSound);
            }
        }
    }

    public void FixedUpdate()
    {
        Track.AllTracksUpdate();
    }
}